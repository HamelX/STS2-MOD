using System;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using GunslingerMod.Models.Powers;

namespace GunslingerMod.Models.Cards;

public sealed class RicochetManifest() : CardModel(1, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override bool IsPlayable
    {
        get
        {
            var ricochet = Owner?.Creature?.GetPower<RicochetPower>();
            return ricochet != null && ricochet.Amount >= 1;
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        if ((Owner.Creature.GetPower<RicochetPower>()?.Amount ?? 0) < 1)
            return;

        await PowerCmd.Apply<RicochetPower>(Owner.Creature, -1, Owner.Creature, this);

        var applied = await PowerCmd.Apply<RicochetManifestPower>(cardPlay.Target, IsUpgraded ? 6 : 4, Owner.Creature, this);
        applied?.Configure(IsUpgraded ? 7m : 5m);
    }
}
