using System.Collections.Generic;
using System.Linq;
using GunslingerMod.Models.Characters;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace GunslingerMod.Framework.Compatibility;

[HarmonyPatch(typeof(ModelDb), "get_AllCharacters")]
internal static class ModelDbAllCharactersFallbackPatch
{
    [HarmonyPostfix]
    private static IEnumerable<CharacterModel> EnsureGunslingerVisible(IEnumerable<CharacterModel> __result)
    {
        var characters = __result.ToList();
        var gunslingerId = ModelDb.GetId<Gunslinger>();

        if (characters.All(c => c.Id != gunslingerId))
        {
            characters.Add(ModelDb.Character<Gunslinger>());
        }

        return characters;
    }
}
