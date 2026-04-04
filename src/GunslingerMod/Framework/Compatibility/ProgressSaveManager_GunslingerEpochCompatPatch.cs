using GunslingerMod.Models.Characters;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Saves.Managers;

namespace GunslingerMod.Framework.Compatibility;

[HarmonyPatch(typeof(ProgressSaveManager))]
internal static class ProgressSaveManager_GunslingerEpochCompatPatch
{
    [HarmonyPatch("CheckFifteenElitesDefeatedEpoch")]
    [HarmonyPrefix]
    private static bool SkipEliteEpochCheckForGunslinger(Player localPlayer)
    {
        return localPlayer.Character is not Gunslinger;
    }

    [HarmonyPatch("CheckFifteenBossesDefeatedEpoch")]
    [HarmonyPrefix]
    private static bool SkipBossEpochCheckForGunslinger(Player localPlayer)
    {
        return localPlayer.Character is not Gunslinger;
    }

    [HarmonyPatch("ObtainCharUnlockEpoch")]
    [HarmonyPrefix]
    private static bool SkipCharacterEpochUnlockForGunslinger(Player localPlayer)
    {
        return localPlayer.Character is not Gunslinger;
    }
}
