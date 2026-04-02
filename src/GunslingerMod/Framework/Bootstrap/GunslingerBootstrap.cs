using Godot;
using GunslingerMod.Framework.Registration;
using GunslingerMod.Models.CardPools;
using GunslingerMod.Models.Characters;
using GunslingerMod.Models.RelicPools;

namespace GunslingerMod.Framework.Bootstrap;

internal static class GunslingerBootstrap
{
    private static Gunslinger? _gunslinger;
    private static GunslingerCardPool? _gunslingerCardPool;
    private static GunslingerRelicPool? _gunslingerRelicPool;

    public static void Initialize()
    {
        EnsureCustomModelsInitialized();
        GunslingerRegistration.RegisterAll();
    }

    private static void EnsureCustomModelsInitialized()
    {
        _gunslingerCardPool ??= new GunslingerCardPool();
        _gunslingerRelicPool ??= new GunslingerRelicPool();
        _gunslinger ??= new Gunslinger();

        GD.Print("[Gunslinger] BaseLib custom models initialized");
    }
}
