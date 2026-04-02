using BaseLib.Patches.Content;
using Godot;
using GunslingerMod.Models.CardPools;
using GunslingerMod.Models.RelicPools;
using MegaCrit.Sts2.Core.Modding;

namespace GunslingerMod.Framework.Registration;

internal static class GunslingerRegistration
{
    public static void RegisterAll()
    {
        RegisterPoolContent(typeof(GunslingerCardPool), GunslingerContentSpec.ActiveCardTypes);
        RegisterDetachedContent(GunslingerContentSpec.SupportOnlyCardTypes);

        RegisterPoolContent(typeof(GunslingerRelicPool), GunslingerContentSpec.ActiveRelicTypes);
        RegisterDetachedContent(GunslingerContentSpec.SupportOnlyRelicTypes);
    }

    private static void RegisterPoolContent(Type poolType, IEnumerable<Type> modelTypes)
    {
        var registered = 0;

        foreach (var modelType in modelTypes)
        {
            if (CustomContentDictionary.RegisterType(modelType))
            {
                registered++;
            }

            ModHelper.AddModelToPool(poolType, modelType);
        }

        GD.Print($"[Gunslinger] Registered {registered} direct types for pool {poolType.Name}");
    }

    private static void RegisterDetachedContent(IEnumerable<Type> modelTypes)
    {
        var registered = 0;

        foreach (var modelType in modelTypes)
        {
            if (CustomContentDictionary.RegisterType(modelType))
            {
                registered++;
            }
        }

        GD.Print($"[Gunslinger] Registered {registered} support-only model types");
    }
}
