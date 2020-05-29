using System.IO;
using TtfLoader.Data;

namespace TtfLoader.Types
{
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
            return $"{Major}.{Minor}";
        }
    }
}