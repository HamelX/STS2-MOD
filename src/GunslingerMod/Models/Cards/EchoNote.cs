using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using GunslingerMod.Models.Powers;

namespace GunslingerMod.Models.Cards;

public sealed class EchoNote() : CardModel(0, CardType.Skill, CardRarity.Common, TargetType.None)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var draw = IsUpgraded ? 2 : 1;
        if ((Owner.Creature.GetPower<TracerFiredThisTurnPower>()?.Amount ?? 0) > 0)
            draw += 1;

        await CardPileCmd.Draw(choiceContext, draw, Owner);
    }
}
