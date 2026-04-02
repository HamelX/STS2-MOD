using BaseLib.Abstracts;
using Godot;
using GunslingerMod.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Unlocks;

namespace GunslingerMod.Models.RelicPools;

public sealed class GunslingerRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => StsColors.gray;

    public override IEnumerable<RelicModel> GetUnlockedRelics(UnlockState unlockState)
    {
        return AllRelics.Where(r => r is not CylinderRelic).ToList();
    }
}
