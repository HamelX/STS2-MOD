using System.Collections.Generic;
using System.Linq;
using GunslingerMod.Models.CardPools;
using GunslingerMod.Models.Characters;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace GunslingerMod.Framework.Compatibility;

[HarmonyPatch(typeof(ModelDb))]
internal static class ModelDbCardLibraryFallbackPatch
{
    [HarmonyPatch("get_AllCharacterCardPools")]
    [HarmonyPostfix]
    private static IEnumerable<CardPoolModel> EnsureGunslingerCharacterPool(IEnumerable<CardPoolModel> __result)
    {
        var pools = __result.ToList();
        var gunslingerPool = ModelDb.CardPool<GunslingerCardPool>();

        if (pools.All(pool => pool.Id != gunslingerPool.Id))
        {
            pools.Add(gunslingerPool);
        }

        return pools;
    }

    [HarmonyPatch("get_AllCardPools")]
    [HarmonyPostfix]
    private static IEnumerable<CardPoolModel> EnsureGunslingerPoolInAllPools(IEnumerable<CardPoolModel> __result)
    {
        var pools = __result.ToList();
        var gunslingerPool = ModelDb.CardPool<GunslingerCardPool>();

        if (pools.All(pool => pool.Id != gunslingerPool.Id))
        {
            pools.Add(gunslingerPool);
        }

        return pools;
    }

    [HarmonyPatch("get_AllCards")]
    [HarmonyPostfix]
    private static IEnumerable<CardModel> EnsureGunslingerCardsVisible(IEnumerable<CardModel> __result)
    {
        var cards = __result.ToList();
        var gunslingerPool = ModelDb.CardPool<GunslingerCardPool>();
        var startingDeck = ModelDb.Character<Gunslinger>().StartingDeck;

        foreach (var card in gunslingerPool.AllCards.Concat(startingDeck).Distinct())
        {
            if (cards.All(existing => existing.Id != card.Id))
            {
                cards.Add(card);
            }
        }

        return cards;
    }
}
