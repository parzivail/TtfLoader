using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using TtfLoader.Data;
using TtfLoader.Header;
using TtfLoader.Tables;
using TtfLoader.Tables.Head;
using TtfLoader.Tables.Hhea;
using TtfLoader.Tables.Name;

namespace TtfLoader
{
    public class TrueTypeFont
    {
        private static readonly Dictionary<string, Type> DirTableTypeMap = new Dictionary<string, Type>
        {
            { "head", typeof(Head) },
            { "hhea", typeof(Hhea) },
            { "name", typeof(Name) }
        };

        public OffsetTable OffsetTable;
        public DirTableEntry[] DirectoryTable;

        private Dictionary<Type, object> _tables;

        public static TrueTypeFont Load(string filename)
        {
            using (var r = new EndiannessAwareBinaryReader(File.Open(filename, FileMode.Open),
                EndiannessAwareBinaryReader.Endianness.Big))
            {
                var offsetTable = r.Read<OffsetTable>();
                var directoryTable = r.ReadMany<DirTableEntry>(offsetTable.NumTables);

                var tables = new Dictionary<Type, object>();
                foreach (var pair in DirTableTypeMap) tables.Add(pair.Value, ReadTable(r, directoryTable, pair.Key));

                // hmtx
                // maxp
                // loca
                // cmap
                // glyf
                // post

                return new TrueTypeFont
                {
                    _tables = tables,
                    OffsetTable = offsetTable,
                    DirectoryTable = directoryTable
                };
            }
        }

        public T GetTable<T>() where T : ITable
        {
            return (T)_tables[typeof(T)];
        }

        private static object ReadTable(BinaryReader r, DirTableEntry[] directoryTable, string tableTag)
        {
            var tableEntry = directoryTable.First(entry => entry.Tag == tableTag);

            var pos = r.BaseStream.Position;
            r.BaseStream.Seek(tableEntry.Offset, SeekOrigin.Begin);

            var tableType = DirTableTypeMap[tableTag];
            var t = (IBinarySerializable)Activator.CreateInstance(tableType);
            t.Read(r);

            r.BaseStream.Seek(pos, SeekOrigin.Begin);
            return t;
        }
    }
}
