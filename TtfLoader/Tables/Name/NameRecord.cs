using System.IO;
using TtfLoader.Data;

namespace TtfLoader.Tables.Name
{
    internal struct NameRecord : IBinarySerializable
    {
        public PlatformIdentifier PlatformId;
        public ushort PlatformSpecificId;
        public ushort LanguageId;
        public ushort NameId;
        public ushort Length;
        public ushort Offset;

        public string Value;

        /// <inheritdoc />
        public void Read(BinaryReader r)
        {
            PlatformId = (PlatformIdentifier)r.ReadUInt16();
            PlatformSpecificId = r.ReadUInt16();
            LanguageId = r.ReadUInt16();
            NameId = r.ReadUInt16();
            Length = r.ReadUInt16();
            Offset = r.ReadUInt16();
        }

        public NameIdentifier GetNameIdentifier() => (NameIdentifier)NameId;

        /// <inheritdoc />
        public override string ToString()
        {
            var platId = $"{PlatformSpecificId}";

            switch (PlatformId)
            {
                case PlatformIdentifier.Unicode:
                    platId = $"{(PlatformEncodingUnicode)PlatformSpecificId}";
                    break;
                case PlatformIdentifier.Macintosh:
                    platId = $"{(PlatformEncodingMacintosh)PlatformSpecificId}";
                    break;
            }

            return $"{PlatformId}/{platId} ({GetNameIdentifier()}): {Value}";
        }
    }
}