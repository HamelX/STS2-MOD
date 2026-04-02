using System.Linq;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using GunslingerMod.Models.Combat;
using GunslingerMod.Models.Powers;

namespace GunslingerMod.Models.Cards;

public sealed class GrandRite() : CardModel(3, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        var cylinder = Owner.Creature.GetPower<CylinderPower>();
        if (cylinder == null)
            return;

        var sealIndices = Enumerable.Range(0, CylinderPower.MaxRounds)
            .Where(i => cylinder.GetAmmoType(i) == CylinderPower.AmmoType.Seal)
            .ToList();
        if (sealIndices.Count == 0)
            return;

        DistributeSealLevels(cylinder, sealIndices, IsUpgraded ? 3 : 2);

        var sealIndex = FindHighestLevelSealIndex(cylinder);
        if (sealIndex < 0)
            return;

        if (sealIndex != cylinder.ChamberIndex)
            cylinder.SwapChambers(sealIndex, cylinder.ChamberIndex);

        if (cylinder.GetAmmoType(cylinder.ChamberIndex) != CylinderPower.AmmoType.Seal)
            return;

        var target = cardPlay.Target.IsAlive
            ? cardPlay.Target
            : Owner.Creature.CombatState?.HittableEnemies.FirstOrDefault(e => e.IsAlive);
        if (target == null)
        {
            await PowerCmd.SetAmount<CylinderPower>(Owner.Creature, cylinder.CountLoaded(), Owner.Creature, this);
            return;
        }

        var didFire = BulletResolver.TryConsumeCurrentWithSealSkip(cylinder, this, out var ammoType, out var sealLevel);
        await PowerCmd.SetAmount<CylinderPower>(Owner.Creature, cylinder.CountLoaded(), Owner.Creature, this);

        if (!didFire)
            return;

        var damage = Math.Max(0m, BulletResolver.GetBaseDamage(ammoType, sealLevel));
        await BulletResolver.FireAtTarget(choiceContext, Owner.Creature, target, this, ammoType, sealLevel, damage);
    }

    private static void DistributeSealLevels(CylinderPower cylinder, IReadOnlyList<int> sealIndices, int totalBonus)
    {
        for (var i = 0; i < totalBonus; i++)
        {
            var targetIndex = sealIndices
                .OrderBy(idx => cylinder.GetSealLevel(idx))
                .ThenBy(idx => idx == cylinder.ChamberIndex ? 0 : 1)
                .ThenBy(idx => idx)
                .First();
            cylinder.IncrementSealLevel(targetIndex, 1);
        }
    }

    private static int FindHighestLevelSealIndex(CylinderPower cylinder)
    {
        var bestIdx = -1;
        var bestLvl = -1;

        for (var i = 0; i < CylinderPower.MaxRounds; i++)
        {
            if (cylinder.GetAmmoType(i) != CylinderPower.AmmoType.Seal)
                continue;

            var lvl = cylinder.GetSealLevel(i);
            if (lvl > bestLvl)
            {
                bestLvl = lvl;
                bestIdx = i;
            }
        }

        return bestIdx;
    }
}
