using System.Diagnostics;
using System.IO;
using System.Linq;
using TtfLoader.Data;
using TtfLoader.Types;

namespace TtfLoader.Tables.Hhea
{
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
}