using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using GunslingerMod.Models.Powers;

namespace GunslingerMod.Models.Cards;

public sealed class Evasion() : CardModel(1, CardType.Skill, CardRarity.Common, TargetType.None)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(
            Owner.Creature,
            IsUpgraded ? 10m : 8m,
            MegaCrit.Sts2.Core.ValueProps.ValueProp.Move,
            cardPlay);
        await PowerCmd.Apply<EvasionPower>(Owner.Creature, IsUpgraded ? 14 : 10, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}
