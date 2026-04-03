using System;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using GunslingerMod.Models.Combat;
using GunslingerMod.Models.Powers;

namespace GunslingerMod.Models.Cards;

public sealed class HotChamber() : CardModel(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        var cylinder = Owner.Creature.GetPower<CylinderPower>();
        if (cylinder == null)
            return;

        var target = BulletResolver.ResolveAliveTarget(Owner.Creature, cardPlay.Target);
        if (target == null)
            return;

        var didFire = BulletResolver.TryConsumeCurrentWithSealSkip(cylinder, this, out var ammoType, out var sealLevel);
        await PowerCmd.SetAmount<CylinderPower>(Owner.Creature, cylinder.CountLoaded(), Owner.Creature, this);
        if (!didFire)
            return;

        var damageBonus = IsUpgraded ? 6m : 4m;
        await BulletResolver.FireAtTarget(
            choiceContext,
            Owner.Creature,
            target,
            this,
            ammoType,
            sealLevel,
            BulletResolver.GetBaseDamage(ammoType, sealLevel) + damageBonus);

        if (ammoType != CylinderPower.AmmoType.Tracer)
            return;

        TryLoadTracerWithFallback(cylinder);
        await PowerCmd.SetAmount<CylinderPower>(Owner.Creature, cylinder.CountLoaded(), Owner.Creature, this);
        await CardPileCmd.Draw(choiceContext, 1, Owner);
    }

    private static bool TryLoadTracerWithFallback(CylinderPower cylinder)
    {
        if (cylinder.TryLoadNext(CylinderPower.AmmoType.Tracer))
            return true;

        var idx = cylinder.ChamberIndex;
        var ammo = cylinder.GetAmmoType(idx);
        if (ammo is CylinderPower.AmmoType.Normal or CylinderPower.AmmoType.Enhanced or CylinderPower.AmmoType.Penetrator)
        {
            cylinder.ClearChamberAt(idx);
            return cylinder.TryLoadInto(idx, CylinderPower.AmmoType.Tracer);
        }

        return false;
    }
}
