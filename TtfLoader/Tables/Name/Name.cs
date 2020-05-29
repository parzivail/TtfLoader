using System.IO;
using System.Text;
using TtfLoader.Data;

namespace TtfLoader.Tables.Name
{
    internal struct Name : ITable, IBinarySerializable
    {
        public ushort Format;
        public ushort Count;
        public ushort StringOffset;
        public NameRecord[] NameRecords;

        /// <inheritdoc />
        public void Read(BinaryReader r)
        {
            Format = r.ReadUInt16();
            Count = r.ReadUInt16();
            StringOffset = r.ReadUInt16();
            NameRecords = r.ReadMany<NameRecord>(Count);

            var pos = r.BaseStream.Position;

            for (var i = 0; i < NameRecords.Length; i++)
            {
                var nameRecord = NameRecords[i];

                r.BaseStream.Seek(nameRecord.Offset, SeekOrigin.Current);
                var chars = r.ReadBytes(nameRecord.Length);
                r.BaseStream.Seek(pos, SeekOrigin.Begin);

                switch (nameRecord.PlatformId)
                {
                    case PlatformIdentifier.Microsoft:
                    case PlatformIdentifier.Unicode:
                    {
                        var platId = (PlatformEncodingUnicode)nameRecord.PlatformSpecificId; // ???
                        nameRecord.Value = Encoding.BigEndianUnicode.GetString(chars);
                        break;
                    }
                    case PlatformIdentifier.Macintosh:
                    {
                        var platId = (PlatformEncodingMacintosh)nameRecord.PlatformSpecificId;

                        if (platId == PlatformEncodingMacintosh.Roman)
                            nameRecord.Value = Encoding.ASCII.GetString(chars);

                        break;
                    }
                }

                NameRecords[i] = nameRecord;
            }
        }
    }
}