using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using GunslingerMod.Models.Powers;

namespace GunslingerMod.Models.Cards;

public sealed class SealOpen() : CardModel(1, CardType.Skill, CardRarity.Rare, TargetType.None)
{
    protected override bool IsPlayable
    {
        get
        {
            var cylinder = Owner?.Creature?.GetPower<CylinderPower>();
            return cylinder != null && cylinder.CountSealLoaded() > 0;
        }
    }

    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cylinder = Owner.Creature.GetPower<CylinderPower>();
        if (cylinder == null)
            return Task.CompletedTask;

        var sealIndex = SealShotHelper.FindHighestLevelSealIndex(cylinder);
        if (sealIndex < 0)
            return Task.CompletedTask;

        if (sealIndex != cylinder.ChamberIndex)
            cylinder.SwapChambers(sealIndex, cylinder.ChamberIndex);

        if (IsUpgraded)
            cylinder.IncrementSealLevels(2);

        return Task.CompletedTask;
    }
}
