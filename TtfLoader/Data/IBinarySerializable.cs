using System.IO;

namespace TtfLoader.Data
{
    internal interface IBinarySerializable
    {
        void Read(BinaryReader r);
    }
}