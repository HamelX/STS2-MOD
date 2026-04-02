using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using GunslingerMod.Models.DynamicVars;
using GunslingerMod.Models.Powers;

namespace GunslingerMod.Models.Cards;

public sealed class SealLoad() : CardModel(1, CardType.Skill, CardRarity.Common, TargetType.None)
{
    private const int MaxAmmo = 6;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new SealLoadProtectionAmountVar()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cylinder = await PowerCmd.Apply<CylinderPower>(Owner.Creature, 1, Owner.Creature, this);
        if (cylinder == null)
            return;

        if (cylinder.CountSealLoaded() >= CylinderPower.MaxSealRounds)
            cylinder.IncrementSealLevels(1);
        else
            cylinder.TryLoadNext(CylinderPower.AmmoType.Seal);

        await CreatureCmd.GainBlock(Owner.Creature, IsUpgraded ? 8m : 5m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move, cardPlay);

        var count = cylinder.CountLoaded();
        if (count > MaxAmmo)
            count = MaxAmmo;

        await PowerCmd.SetAmount<CylinderPower>(Owner.Creature, count, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}
