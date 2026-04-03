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

        var loadedCount = 0;
        for (var i = 0; i < CylinderPower.MaxRounds; i++)
        {
            if (cylinder.IsLoaded(i))
                loadedCount++;
        }

        if (loadedCount <= 0)
            return;

        var blockAmount = (decimal)(IsUpgraded ? loadedCount + 1 : loadedCount);
        if (blockAmount > 0)
            await CreatureCmd.GainBlock(Owner.Creature, blockAmount, ValueProp.Move, cardPlay);

        cylinder.ClearAll();
        await PowerCmd.SetAmount<CylinderPower>(Owner.Creature, 0, Owner.Creature, this);

        var drawAmount = (loadedCount / 2) + (IsUpgraded ? 1 : 0);
        var energyGain = loadedCount >= (IsUpgraded ? 3 : 4) ? 1 : 0;
        if (energyGain > 0)
            await PlayerCmd.GainEnergy(energyGain, Owner);

        if (drawAmount > 0)
            await CardPileCmd.Draw(choiceContext, drawAmount, Owner);
    }

    protected override void OnUpgrade()
    {
    }
}
