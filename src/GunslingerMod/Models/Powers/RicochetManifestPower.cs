using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace GunslingerMod.Models.Powers;

public sealed class RicochetManifestPower : PowerModel
{
    public decimal DamagePerTrigger { get; private set; } = 5m;

    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool ShouldReceiveCombatHooks => true;

    public void Configure(decimal damagePerTrigger)
    {
        if (damagePerTrigger > 0)
            DamagePerTrigger = damagePerTrigger;
    }

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (Owner == null || Owner.Side != side || Amount <= 0 || !Owner.IsAlive || Owner.CurrentHp <= 0)
            return;

        var owner = Owner;
        await CreatureCmd.Damage(null!, owner, DamagePerTrigger, ValueProp.Move, owner, null);
        await PowerCmd.Decrement(this);
    }
}
