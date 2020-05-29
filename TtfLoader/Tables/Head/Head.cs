using System;
using System.IO;
using TtfLoader.Data;
using TtfLoader.Types;

namespace TtfLoader.Tables.Head
{
    internal struct Head : ITable, IBinarySerializable
    {
        public Fixed Version;
        public Fixed FontRevision;
        public uint ChecksumAdjustment;
        public HeadFlags Flags;
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

            Flags = (HeadFlags)r.ReadUInt16();
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
    }
}