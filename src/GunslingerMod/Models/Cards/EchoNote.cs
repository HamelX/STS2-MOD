using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using GunslingerMod.Models.Powers;

namespace GunslingerMod.Models.Cards;

public sealed class EchoNote() : CardModel(0, CardType.Skill, CardRarity.Common, TargetType.None)
{
    protected override bool IsPlayable
    {
        get
        {
            if (IsUpgraded)
                return true;

            var imprint = Owner?.Creature?.GetPower<ImprintPower>();
            return imprint != null && imprint.Amount >= 1;
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var imprint = Owner.Creature.GetPower<ImprintPower>();
        if (imprint != null && imprint.Amount >= 1)
        {
            await PowerCmd.Apply<ImprintPower>(Owner.Creature, -1, Owner.Creature, this);
            await CardPileCmd.Draw(choiceContext, 2, Owner);
            return;
        }

        if (IsUpgraded)
            await CardPileCmd.Draw(choiceContext, 1, Owner);
    }
}
