using System;
using GunslingerMod.Models.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace GunslingerMod.Models.Cards;

public sealed class SealBarrier() : CardModel(2, CardType.Skill, CardRarity.Rare, TargetType.None)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cylinder = await PowerCmd.Apply<CylinderPower>(Owner.Creature, 1, Owner.Creature, this);
        if (cylinder == null)
            return;

        var highestSealLevel = 0;
        for (var i = 0; i < CylinderPower.MaxRounds; i++)
        {
            if (cylinder.GetAmmoType(i) != CylinderPower.AmmoType.Seal)
                continue;

            highestSealLevel = Math.Max(highestSealLevel, cylinder.GetSealLevel(i));
        }

        var block = (IsUpgraded ? 14m : 10m) + (highestSealLevel * 2m);
        await CreatureCmd.GainBlock(Owner.Creature, block, ValueProp.Move, cardPlay);
    }
}
