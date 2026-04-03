using GunslingerMod.Models.Cards;
using GunslingerMod.Relics;

namespace GunslingerMod.Framework.Registration;

internal static class GunslingerContentSpec
{
    public static readonly Type[] ActiveCardTypes =
    [
        typeof(Shoot),
        typeof(DefendGunslinger),
        typeof(Reload),
        typeof(TakeCover),
        typeof(Evasion),
        typeof(EchoNote),
        typeof(QuickRack),
        typeof(HotChamber),
        typeof(Panning),
        typeof(SprayFire),
        typeof(SealLoad),
        typeof(SigilGuard),
        typeof(EtchedTracer),
        typeof(RicochetSqueeze),
        typeof(RicochetCompression),
        typeof(TracerConversion),
        typeof(BallisticCompiler),
        typeof(ChainBurst),
        typeof(WalkingFire),
        typeof(CrossfireRhythm),
        typeof(EmptyTheMagazine),
        typeof(OverclockDrum),
        typeof(OverclockCharge),
        typeof(ExecutionShot),
        typeof(SealSearch),
        typeof(SealRite),
        typeof(SealOpen),
        typeof(SealAmplify),
        typeof(SealResonance),
        typeof(GrandRite),
        typeof(SealReleaseKai),
        typeof(SealRampage),
        typeof(SealInsight),
        typeof(SealBarrier),
    ];

    public static readonly Type[] SupportOnlyCardTypes =
    [
        typeof(SealShot)
    ];

    public static readonly Type[] ActiveRelicTypes =
    [
        typeof(CylinderRelic),
        typeof(HotEjectorRelic)
    ];

    public static readonly Type[] SupportOnlyRelicTypes =
    [
    ];
}


