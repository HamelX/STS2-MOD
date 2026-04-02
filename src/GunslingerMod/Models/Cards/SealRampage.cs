using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using GunslingerMod.Models.Powers;

namespace GunslingerMod.Models.Cards;

public sealed class SealRampage() : CardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
{
    protected override bool IsPlayable
    {
        get
        {
            var cylinder = Owner?.Creature?.GetPower<CylinderPower>();
            return cylinder != null && FindHighestLevelSealIndex(cylinder) >= 0;
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cylinder = Owner.Creature.GetPower<CylinderPower>();
        if (cylinder == null)
            return;

        var sealIndex = FindHighestLevelSealIndex(cylinder);
        if (sealIndex < 0)
            return;

        var sealLevel = cylinder.GetSealLevel(sealIndex);
        if (sealLevel <= 0)
            return;

        var energyGain = Math.Min(IsUpgraded ? 3 : 2, sealLevel);
        cylinder.ReduceSealLevel(sealIndex, (byte)Math.Max(1, sealLevel / 2));
        await PowerCmd.SetAmount<CylinderPower>(Owner.Creature, cylinder.CountLoaded(), Owner.Creature, this);
        if (energyGain > 0)
            await PlayerCmd.GainEnergy(energyGain, Owner);
    }

    private static int FindHighestLevelSealIndex(CylinderPower cylinder)
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
}
