using System.IO;
using TtfLoader.Data;
using TtfLoader.Types;

namespace TtfLoader.Header
{
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
}