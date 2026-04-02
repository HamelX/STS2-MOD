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
        typeof(ImprintSqueeze),
        typeof(ImprintCompression),
        typeof(FanTheBrand),
        typeof(RicochetShot),
        typeof(ReboundNet),
        typeof(ImprintManifestRicochet),
        typeof(TracerConversion),
        typeof(BallisticCompiler),
        typeof(ChainBurst),
        typeof(WalkingFire),
        typeof(BlankFire),
        typeof(SealSearch),
        typeof(SealAmplify),
        typeof(SealResonance),
        typeof(EmptyTheMagazine),
        typeof(OverclockDrum),
        typeof(OverclockCharge),
        typeof(ExecutionShot),
        typeof(ImprintIgnition),
        typeof(SealRite),
        typeof(SealOpen),
        typeof(SealReleaseKai),
        typeof(SealRampage),
        typeof(SealInsight),
        typeof(SealBarrier)
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


