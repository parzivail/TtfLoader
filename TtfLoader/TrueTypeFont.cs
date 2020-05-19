using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;

namespace TtfLoader
{
    public class TrueTypeFont
    {
        private static readonly Dictionary<string, Type> DirTableTypeMap = new Dictionary<string, Type>
        {
            { "head", typeof(Head) },
            { "hhea", typeof(Hhea) }
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

                // name
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

    internal interface IBinarySerializable
    {
        void Read(BinaryReader r);
    }

    public interface ITable
    {

    }

    internal struct Head : ITable, IBinarySerializable
    {
        public Fixed Version;
        public Fixed FontRevision;
        public uint ChecksumAdjustment;
        public ushort Flags;
        public ushort UnitsPerEm;
        public ulong Created;
        public ulong Modified;
        public short MinX;
        public short MinY;
        public short MaxX;
        public short MaxY;
        public ushort MacStyle;
        public ushort LowestRecPpem;
        public short FontDirectionHint;
        public short IndexToLocFormat;
        public short GlyphDataFormat;

        /// <inheritdoc />
        public void Read(BinaryReader r)
        {
            Version = r.Read<Fixed>();
            FontRevision = r.Read<Fixed>();
            ChecksumAdjustment = r.ReadUInt32();

            var magic = r.ReadUInt32();
            if (magic != 0x5F0F3CF5)
                throw new ArgumentException($"TTF 'head' table magic invalid, expected 0x5F0F3CF5, got 0x{magic:X}");

            Flags = r.ReadUInt16();
            UnitsPerEm = r.ReadUInt16();
            Created = r.ReadUInt64();
            Modified = r.ReadUInt64();
            MinX = r.ReadInt16();
            MinY = r.ReadInt16();
            MaxX = r.ReadInt16();
            MaxY = r.ReadInt16();
            MacStyle = r.ReadUInt16();
            LowestRecPpem = r.ReadUInt16();
            FontDirectionHint = r.ReadInt16();
            IndexToLocFormat = r.ReadInt16();
            GlyphDataFormat = r.ReadInt16();
        }

        private bool CheckFixedBytes(byte[] test, params byte[] known)
        {
            return test.Length == known.Length && !test.Where((t, i) => t != known[i]).Any();
        }
    }

    internal struct Hhea : ITable, IBinarySerializable
    {
        public Fixed Version;
        public short Ascender;
        public short Descender;
        public short LineGap;
        public short AdvanceWidthMax;
        public short MinLeftSideBearing;
        public short MinRightSideBearing;
        public short XMaxExtend;
        public short CaretSlopeRise;
        public short CaretSlopeRun;
        public short MetricDataFormat;
        public short NumberOfHmetrics;

        /// <inheritdoc />
        public void Read(BinaryReader r)
        {
            Version = r.Read<Fixed>();
            Ascender = r.ReadInt16();
            Descender = r.ReadInt16();
            LineGap = r.ReadInt16();
            AdvanceWidthMax = r.ReadInt16();
            MinLeftSideBearing = r.ReadInt16();
            MinRightSideBearing = r.ReadInt16();
            XMaxExtend = r.ReadInt16();
            CaretSlopeRise = r.ReadInt16();
            CaretSlopeRun = r.ReadInt16();

            // reserved
            var reserved = r.ReadBytes(10);
            Debug.WriteLineIf(reserved.Any(b => b != 0), "Unexpected data in TTF 'hhea' table reserved area");

            MetricDataFormat = r.ReadInt16();
            NumberOfHmetrics = r.ReadInt16();
        }
    }

    public struct DirTableEntry : IBinarySerializable
    {
        public string Tag;
        public uint Checksum;
        public uint Offset;
        public uint Length;

        public void Read(BinaryReader r)
        {
            Tag = new string(r.ReadChars(4));
            Checksum = r.ReadUInt32();
            Offset = r.ReadUInt32();
            Length = r.ReadUInt32();
        }
    }

    public struct OffsetTable : IBinarySerializable
    {
        public Fixed SfntVersion;
        public ushort NumTables;
        public ushort SearchRange;
        public ushort EntrySelector;
        public ushort RangeShift;

        public void Read(BinaryReader r)
        {
            SfntVersion = r.Read<Fixed>();
            NumTables = r.ReadUInt16();
            SearchRange = r.ReadUInt16();
            EntrySelector = r.ReadUInt16();
            RangeShift = r.ReadUInt16();
        }
    }

    public struct Fixed : IBinarySerializable
    {
        public ushort Major;
        public ushort Minor;

        public void Read(BinaryReader r)
        {
            Major = r.ReadUInt16();
            Minor = r.ReadUInt16();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{nameof(Major)}: {Major}, {nameof(Minor)}: {Minor}";
        }
    }
}
