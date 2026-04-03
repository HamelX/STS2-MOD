using GunslingerMod.Models.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace GunslingerMod.Models.Cards;

public sealed class SealRampage() : CardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cylinder = Owner.Creature.GetPower<CylinderPower>();
        if (cylinder == null)
            return;

        var sealIndex = SealShotHelper.FindHighestLevelSealIndex(cylinder);
        if (sealIndex < 0)
            return;

        var spend = (byte)(IsUpgraded ? 1 : 2);
        var current = cylinder.GetSealLevel(sealIndex);
        if (current < spend)
            spend = current;

        if (spend > 0)
            cylinder.ReduceSealLevel(sealIndex, spend);

        await PowerCmd.SetAmount<CylinderPower>(Owner.Creature, cylinder.CountLoaded(), Owner.Creature, this);

        await PlayerCmd.GainEnergy(1, Owner);

        if (IsUpgraded)
            await CardPileCmd.Draw(choiceContext, 1, Owner);
    }
}
