using System;

namespace TtfLoader.Tables.Head
{
    [Flags]
    public enum HeadFlags
    {
        YZeroSpecifiesBaseline = 1 << 0,
        XPositionLeftmostBlackBitIsLsb = 1 << 1,
        ScaledPointSizesDiffer = 1 << 2,
        UseIntegerScaling = 1 << 3,
        MicrosoftTtfScaler = 1 << 4,
        VerticalBaseline = 1 << 5,
        ReservedZero = 1 << 6,
        RequiresLinguisticLayout = 1 << 7,
        MetamorphasisEffectsByDefault = 1 << 8,
        ContainsStrongLeftToRightGlyphs = 1 << 9,
        ContainsIndicStyleRearrangementEffects = 1 << 10,
        AdobeBit11 = 1 << 11,
        AdobeBit12 = 1 << 12,
        AdobeBit13 = 1 << 13,
        GlyphsAreGenericSymbols = 1 << 14
    }
}