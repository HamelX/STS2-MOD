using BaseLib.Abstracts;
using Godot;
using GunslingerMod.Models.Characters;
using MegaCrit.Sts2.Core.Models;

namespace GunslingerMod.Models.CardPools;

public sealed class GunslingerCardPool : CustomCardPoolModel
{
    public override string Title => "gunslinger";

    public override string EnergyColorName => "gunslinger";

    public override string CardFrameMaterialPath => "card_frame_purple";

    public override Color ShaderColor => Gunslinger.Color;

    public override Color DeckEntryCardColor => Gunslinger.Color;

    public override Color EnergyOutlineColor => new("2A2A2A");

    public override bool IsColorless => false;
}
