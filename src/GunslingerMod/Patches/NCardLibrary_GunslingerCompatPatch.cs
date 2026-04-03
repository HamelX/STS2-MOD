using System;
using System.Collections.Generic;
using System.Linq;
using GunslingerMod.Models.CardPools;
using GunslingerMod.Models.Characters;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using Godot;

namespace GunslingerMod.Patches;

[HarmonyPatch(typeof(NCardLibrary), "_Ready")]
internal static class NCardLibrary_GunslingerCompatPatch
{
    [HarmonyPostfix]
    private static void EnsureGunslingerCompendiumFilter(NCardLibrary __instance)
    {
        var cardPoolFilters = AccessTools
            .Field(typeof(NCardLibrary), "_cardPoolFilters")
            .GetValue(__instance) as Dictionary<CharacterModel, NCardPoolFilter>;
        var poolFilters = AccessTools
            .Field(typeof(NCardLibrary), "_poolFilters")
            .GetValue(__instance) as Dictionary<NCardPoolFilter, Func<CardModel, bool>>;
        var miscPoolFilter = AccessTools
            .Field(typeof(NCardLibrary), "_miscPoolFilter")
            .GetValue(__instance) as NCardPoolFilter;

        if (cardPoolFilters == null || poolFilters == null || miscPoolFilter == null)
        {
            return;
        }

        var gunslinger = ModelDb.Character<Gunslinger>();
        var gunslingerCardIds = gunslinger.CardPool.AllCards
            .Concat(gunslinger.StartingDeck)
            .Select(card => card.Id)
            .ToHashSet();
        cardPoolFilters[gunslinger] = miscPoolFilter;

        miscPoolFilter.Visible = true;
        miscPoolFilter.Loc = gunslinger.Title;
        if (miscPoolFilter.GetNodeOrNull<Control>("Image") is TextureRect image)
        {
            image.Texture = ResourceLoader.Load<Texture2D>(
                ImageHelper.GetImagePath("ui/top_panel/character_icon_gunslinger.png"));
        }

        poolFilters[miscPoolFilter] = card => gunslingerCardIds.Contains(card.Id);
    }
}
