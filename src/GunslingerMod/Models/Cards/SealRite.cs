using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using GunslingerMod.Models.Powers;

namespace GunslingerMod.Models.Cards;

public sealed class SealRite() : CardModel(2, CardType.Power, CardRarity.Uncommon, TargetType.None)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cylinder = await PowerCmd.Apply<CylinderPower>(Owner.Creature, 1, Owner.Creature, this);
        if (cylinder == null)
            return;

        var loadedNewSeal = false;
        if (cylinder.CountSealLoaded() <= 0)
            loadedNewSeal = cylinder.TryLoadNext(CylinderPower.AmmoType.Seal);

        await PowerCmd.Apply<SealRitePower>(Owner.Creature, 1, Owner.Creature, this);

        if (loadedNewSeal)
            await SealShotHelper.GrantTemporaryToHand(choiceContext, this);

        await PowerCmd.SetAmount<CylinderPower>(Owner.Creature, cylinder.CountLoaded(), Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
