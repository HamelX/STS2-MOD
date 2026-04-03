using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using GunslingerMod.Models.Powers;
using Godot;

namespace GunslingerMod.Models.Cards;

internal static class SealShotHelper
{
    public static int FindHighestLevelSealIndex(CylinderPower cylinder)
    {
        var bestIdx = -1;
        var bestLvl = -1;

        for (var i = 0; i < CylinderPower.MaxRounds; i++)
        {
            if (cylinder.GetAmmoType(i) != CylinderPower.AmmoType.Seal)
                continue;

            var lvl = cylinder.GetSealLevel(i);
            if (lvl > bestLvl)
            {
                bestLvl = lvl;
                bestIdx = i;
            }
        }

        return bestIdx;
    }

    public static async Task GrantTemporaryToHand(PlayerChoiceContext choiceContext, CardModel source)
    {
        var owner = source.Owner;
        var combatState = owner?.Creature?.CombatState;
        if (owner?.RunState == null || combatState == null)
        {
            GD.Print("[Gunslinger] SealShot grant skipped: source owner/runstate/combatState missing");
            return;
        }

        var generated = combatState.CreateCard<SealShot>(owner);
        GD.Print($"[Gunslinger] SealShot grant attempt: source={source.Id.Entry}, card={generated.Id.Entry}");
        _ = choiceContext;
        await CardPileCmd.AddGeneratedCardToCombat(generated, PileType.Hand, addedByPlayer: true);
        GD.Print("[Gunslinger] SealShot grant queued to hand");
    }
}
