using GunslingerMod.Models.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace GunslingerMod.Models.Cards;

public sealed class SealInsight() : CardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cylinder = Owner.Creature.GetPower<CylinderPower>();
        var sealIndex = cylinder == null ? -1 : SealShotHelper.FindHighestLevelSealIndex(cylinder);
        var draw = 1;

        if (sealIndex >= 0)
        {
            draw += 1;
            if (cylinder!.GetSealLevel(sealIndex) >= 3)
                draw += IsUpgraded ? 2 : 1;
        }

        await CardPileCmd.Draw(choiceContext, draw, Owner);
    }
}
