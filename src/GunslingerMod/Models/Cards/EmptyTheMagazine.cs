using System;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using GunslingerMod.Models.Powers;

namespace GunslingerMod.Models.Cards;

public sealed class EmptyTheMagazine() : CardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cylinder = Owner.Creature.GetPower<CylinderPower>();
        if (cylinder == null)
            return;

        var normalCount = 0;
        var tracerCount = 0;
        var sealLevelTotal = 0;
        for (var i = 0; i < CylinderPower.MaxRounds; i++)
        {
            switch (cylinder.GetAmmoType(i))
            {
                case CylinderPower.AmmoType.Normal:
                    normalCount++;
                    break;
                case CylinderPower.AmmoType.Tracer:
                    tracerCount++;
                    break;
                case CylinderPower.AmmoType.Seal:
                    sealLevelTotal += cylinder.GetSealLevel(i);
                    break;
            }
        }

        var blockAmount = IsUpgraded
            ? (decimal)Math.Ceiling(sealLevelTotal * 1.5m)
            : sealLevelTotal;
        if (blockAmount > 0)
            await CreatureCmd.GainBlock(Owner.Creature, blockAmount, ValueProp.Move, cardPlay);

        cylinder.ClearAll();
        await PowerCmd.SetAmount<CylinderPower>(Owner.Creature, 0, Owner.Creature, this);

        var drawAmount = IsUpgraded ? normalCount : normalCount / 2;
        if (drawAmount > 0)
            await CardPileCmd.Draw(choiceContext, drawAmount, Owner);

        var energyGain = tracerCount / 3;
        if (energyGain > 0)
            await PlayerCmd.GainEnergy(energyGain, Owner);
    }

    protected override void OnUpgrade()
    {
    }
}
