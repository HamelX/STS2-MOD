using System;
using GunslingerMod.Models.Combat;
using GunslingerMod.Models.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace GunslingerMod.Models.Cards;

public sealed class SealResonance() : CardModel(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        var cylinder = Owner.Creature.GetPower<CylinderPower>();
        if (cylinder == null || !BulletResolver.HasAliveOpponents(Owner.Creature))
            return;

        var sealIndex = SealShotHelper.FindHighestLevelSealIndex(cylinder);
        if (sealIndex < 0)
            return;

        if (sealIndex != cylinder.ChamberIndex)
            cylinder.SwapChambers(sealIndex, cylinder.ChamberIndex);

        var target = BulletResolver.ResolveAliveTarget(Owner.Creature, cardPlay.Target);
        if (target == null)
            return;

        var sealLevel = cylinder.GetSealLevel(cylinder.ChamberIndex);
        var didFire = BulletResolver.TryConsumeCurrentWithSealSkip(cylinder, this, out var ammoType, out var consumedSealLevel);
        await PowerCmd.SetAmount<CylinderPower>(Owner.Creature, cylinder.CountLoaded(), Owner.Creature, this);
        if (!didFire)
            return;

        var damage = Math.Max(0m, BulletResolver.GetBaseDamage(ammoType, consumedSealLevel));
        await BulletResolver.FireAtTarget(choiceContext, Owner.Creature, target, this, ammoType, consumedSealLevel, damage);

        var bonus = sealLevel * (IsUpgraded ? 3 : 2);
        if (bonus > 0 && target.IsAlive && target.CurrentHp > 0)
            await CreatureCmd.Damage(choiceContext, target, bonus, ValueProp.Move, Owner.Creature, this);
    }
}
