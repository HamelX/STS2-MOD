using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using GunslingerMod.Models.Powers;

namespace GunslingerMod.Models.Cards;

public sealed class SealLoad() : CardModel(1, CardType.Skill, CardRarity.Common, TargetType.None)
{
    private const int MaxAmmo = 6;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cylinder = await PowerCmd.Apply<CylinderPower>(Owner.Creature, 1, Owner.Creature, this);
        if (cylinder == null)
            return;

        var loadedNewSeal = false;
        if (cylinder.CountSealLoaded() >= CylinderPower.MaxSealRounds)
            cylinder.IncrementSealLevels(1);
        else
            loadedNewSeal = cylinder.TryLoadNext(CylinderPower.AmmoType.Seal);

        if (loadedNewSeal)
            await SealShotHelper.GrantTemporaryToHand(choiceContext, this);

        await CreatureCmd.GainBlock(Owner.Creature, IsUpgraded ? 6m : 4m, ValueProp.Move, cardPlay);

        var count = cylinder.CountLoaded();
        if (count > MaxAmmo)
            count = MaxAmmo;

        await PowerCmd.SetAmount<CylinderPower>(Owner.Creature, count, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}
