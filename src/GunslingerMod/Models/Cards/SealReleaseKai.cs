using System;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using GunslingerMod.Models.Combat;
using GunslingerMod.Models.Powers;

namespace GunslingerMod.Models.Cards;

public sealed class SealReleaseKai() : CardModel(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

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

        var sealsToFire = new List<(CylinderPower.AmmoType AmmoType, byte SealLevel)>();
        for (var i = 0; i < CylinderPower.MaxRounds; i++)
        {
            if (cylinder.GetAmmoType(i) != CylinderPower.AmmoType.Seal)
                continue;

            sealsToFire.Add((CylinderPower.AmmoType.Seal, cylinder.GetSealLevel(i)));
        }

        if (sealsToFire.Count == 0)
            return;

        for (var i = 0; i < CylinderPower.MaxRounds; i++)
        {
            if (cylinder.GetAmmoType(i) == CylinderPower.AmmoType.Seal)
                cylinder.ClearChamberAt(i);
        }
        await PowerCmd.SetAmount<CylinderPower>(Owner.Creature, cylinder.CountLoaded(), Owner.Creature, this);

        var combatState = Owner.Creature.CombatState;
        Creature? target = cardPlay.Target;
        foreach (var (ammoType, sealLevel) in sealsToFire)
        {
            if (!BulletResolver.ShouldContinueFiring(combatState))
                return;

            target = GetNextAliveTarget(combatState, Owner.Creature, target);
            if (target == null)
                return;

            var wasAlive = target.IsAlive;
            var baseDamage = BulletResolver.GetBaseDamage(ammoType, sealLevel);
            await BulletResolver.FireAtTarget(choiceContext, Owner.Creature, target, this, ammoType, sealLevel, baseDamage);

            if (wasAlive && !target.IsAlive)
            {
                cylinder.TryLoadOrIncrementSeal((byte)(IsUpgraded ? 2 : 1));
                await PowerCmd.SetAmount<CylinderPower>(Owner.Creature, cylinder.CountLoaded(), Owner.Creature, this);
            }

            if (!BulletResolver.ShouldContinueFiring(combatState))
                return;
        }
    }

    private static Creature? GetNextAliveTarget(CombatState? combatState, Creature owner, Creature? currentTarget)
    {
        if (currentTarget?.IsAlive == true)
            return currentTarget;

        var enemies = combatState?.GetOpponentsOf(owner);
        if (enemies == null)
            return null;

        foreach (var enemy in enemies)
        {
            if (enemy.IsAlive)
                return enemy;
        }
        return null;
    }
}
