using System;
using System.Linq;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using GunslingerMod.Models.Powers;

namespace GunslingerMod.Models.Cards;

public sealed class RicochetShot() : CardModel(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(9m, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        var source = Owner?.Creature;
        if (source == null)
            return;

        await CreatureCmd.Damage(choiceContext, cardPlay.Target, DynamicVars.Damage.BaseValue, ValueProp.Move, source, this);

        var ricochet = source.GetPower<RicochetPower>();
        if (ricochet == null || ricochet.Amount < 1)
            return;

        await PowerCmd.Apply<RicochetPower>(source, -1, source, this);

        var opponents = source.CombatState?.GetOpponentsOf(source)
            .Where(c => c.IsAlive && c.CurrentHp > 0)
            .ToList();
        if (opponents == null || opponents.Count == 0)
            return;

        var bounceDamage = IsUpgraded ? 10m : 7m;
        var rng = Owner?.RunState?.Rng?.CombatTargets;
        for (var i = 0; i < 2; i++)
        {
            var target = rng?.NextItem(opponents) ?? opponents[0];
            if (!target.IsAlive || target.CurrentHp <= 0)
                continue;

            await CreatureCmd.Damage(choiceContext, target, bounceDamage, ValueProp.Move, source, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
