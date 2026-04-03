using GunslingerMod.Models.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace GunslingerMod.Models.Cards;

public sealed class SealSearch() : CardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cylinder = await PowerCmd.Apply<CylinderPower>(Owner.Creature, 1, Owner.Creature, this);
        if (cylinder == null)
            return;

        var sealIndex = SealShotHelper.FindHighestLevelSealIndex(cylinder);
        if (sealIndex >= 0 && sealIndex != cylinder.ChamberIndex)
            cylinder.SwapChambers(sealIndex, cylinder.ChamberIndex);

        await PowerCmd.SetAmount<CylinderPower>(Owner.Creature, cylinder.CountLoaded(), Owner.Creature, this);
        await CardPileCmd.Draw(choiceContext, IsUpgraded ? 2 : 1, Owner);
    }
}
