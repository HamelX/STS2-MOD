using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using GunslingerMod.Models.Powers;

namespace GunslingerMod.Models.Cards;

public sealed class SealAssimilation() : CardModel(2, CardType.Skill, CardRarity.Rare, TargetType.None)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override bool IsPlayable
    {
        get
        {
            var cylinder = Owner?.Creature?.GetPower<CylinderPower>();
            return cylinder != null && cylinder.CountSealLoaded() > 0;
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cylinder = Owner.Creature.GetPower<CylinderPower>();
        if (cylinder == null)
            return;

        var bestLvl = -1;

        for (var i = 0; i < CylinderPower.MaxRounds; i++)
        {
            if (cylinder.GetAmmoType(i) != CylinderPower.AmmoType.Seal)
                continue;

            var lvl = cylinder.GetSealLevel(i);
            if (lvl > bestLvl)
            {
                bestLvl = lvl;
            }
        }

        if (bestLvl <= 0)
            return;

        for (var i = 0; i < CylinderPower.MaxRounds; i++)
        {
            if (cylinder.GetAmmoType(i) != CylinderPower.AmmoType.Seal)
                continue;

            var current = cylinder.GetSealLevel(i);
            if (current < bestLvl)
                cylinder.IncrementSealLevel(i, (byte)(bestLvl - current));
        }

        await PowerCmd.SetAmount<CylinderPower>(Owner.Creature, cylinder.CountLoaded(), Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}
