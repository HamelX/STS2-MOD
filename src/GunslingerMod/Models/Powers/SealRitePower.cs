using System.Threading.Tasks;
using System;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace GunslingerMod.Models.Powers;

public sealed class SealRitePower : PowerModel
{
    public int PendingSealLevelBonus { get; private set; }

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override bool ShouldReceiveCombatHooks => true;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (Owner == null || Owner.Side != side)
            return;

        var cylinder = Owner.GetPower<CylinderPower>();
        if (cylinder == null)
        {
            cylinder = await PowerCmd.Apply<CylinderPower>(Owner, 1, Owner, null);
            if (cylinder == null)
                return;
        }

        var loadLevel = (byte)Math.Clamp(1 + PendingSealLevelBonus, 1, 255);
        cylinder.TryLoadOrIncrementSeal(loadLevel);
        PendingSealLevelBonus = 0;
    }

    public void QueueNextSealLevelBonus(int amount = 1)
    {
        if (amount <= 0)
            return;

        PendingSealLevelBonus += amount;
    }
}
