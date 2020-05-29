using System.IO;
using TtfLoader.Data;

namespace TtfLoader.Header
{
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
}