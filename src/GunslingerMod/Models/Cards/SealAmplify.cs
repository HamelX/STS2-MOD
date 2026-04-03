using GunslingerMod.Models.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace GunslingerMod.Models.Cards;

public sealed class SealAmplify() : CardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cylinder = await PowerCmd.Apply<CylinderPower>(Owner.Creature, 1, Owner.Creature, this);
        if (cylinder == null)
            return;

        cylinder.IncrementSealLevels((byte)(IsUpgraded ? 2 : 1));
        await PowerCmd.SetAmount<CylinderPower>(Owner.Creature, cylinder.CountLoaded(), Owner.Creature, this);
    }
}
