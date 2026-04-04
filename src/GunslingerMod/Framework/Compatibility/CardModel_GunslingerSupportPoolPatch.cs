using System;
using GunslingerMod.Framework.Registration;
using GunslingerMod.Models.CardPools;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace GunslingerMod.Framework.Compatibility;

[HarmonyPatch(typeof(CardModel), "get_Pool")]
internal static class CardModel_GunslingerSupportPoolPatch
{
    [HarmonyPostfix]
    private static CardPoolModel EnsureSupportCardsUseGunslingerPool(CardModel __instance, CardPoolModel __result)
    {
        if (__result is not MockCardPool)
        {
            return __result;
        }

        var cardType = __instance.GetType();
        if (!Array.Exists(GunslingerContentSpec.SupportOnlyCardTypes, type => type == cardType))
        {
            return __result;
        }

        return ModelDb.CardPool<GunslingerCardPool>();
    }
}
