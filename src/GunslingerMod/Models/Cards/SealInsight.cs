using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using GunslingerMod.Models.DynamicVars;
using GunslingerMod.Models.Powers;

namespace GunslingerMod.Models.Cards;

// 봉인 통찰
public sealed class SealInsight() : CardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new SealInsightDrawVar()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cylinder = Owner.Creature.GetPower<CylinderPower>();
        if (cylinder == null)
            return;

        var drawCount = GetDrawCount(cylinder, IsUpgraded);
        if (drawCount <= 0)
            return;

        await CardPileCmd.Draw(choiceContext, drawCount, Owner);
    }

    internal static int GetDrawCount(CylinderPower cylinder, bool isUpgraded)
    {
        var totalSealLevels = 0;
        for (var i = 0; i < CylinderPower.MaxRounds; i++)
        {
            if (cylinder.GetAmmoType(i) != CylinderPower.AmmoType.Seal)
                continue;

            totalSealLevels += cylinder.GetSealLevel(i);
        }

        var threshold = isUpgraded ? 2 : 4;
        return totalSealLevels >= threshold ? 4 : 2;
    }
}
