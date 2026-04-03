using System.Runtime.InteropServices;
using BaseLib.Abstracts;
using Godot;
using GunslingerMod.Models.CardPools;
using GunslingerMod.Models.Cards;
using GunslingerMod.Models.RelicPools;
using GunslingerMod.Relics;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.PotionPools;

namespace GunslingerMod.Models.Characters;

public sealed class Gunslinger : CustomCharacterModel
{
    public const string energyColorName = "gunslinger";
    public static readonly Color Color = new("4D4D4D");
    private const string CharacterSelectIconAssetPath = "res://images/packed/character_select/char_select_gunslinger.png";
    private const string CharacterSelectLockedIconAssetPath = "res://images/packed/character_select/char_select_gunslinger_locked.png";
    private const string TopPanelIconPath = "res://images/ui/top_panel/character_icon_gunslinger.png";
    private const string MapMarkerAssetPath = "res://images/packed/map/icons/map_marker_gunslinger.png";
    private const string CreatureVisualsPath = "res://scenes/creature_visuals/gunslinger.tscn";
    private const string TrailAssetPath = "res://scenes/vfx/card_trail_gunslinger.tscn";
    private const string CharacterIconScenePath = "res://scenes/ui/character_icons/gunslinger_icon.tscn";
    private const string RestSiteAnimScenePath = "res://scenes/rest_site/characters/gunslinger_rest_site.tscn";
    private const string MerchantAnimScenePath = "res://scenes/merchant/characters/gunslinger_merchant.tscn";
    private const string CharacterSelectBgScenePath = "res://scenes/screens/char_select/char_select_bg_gunslinger.tscn";
    private const string CharacterSelectTransitionAssetPath = "res://materials/transitions/gunslinger_transition_mat.tres";
    private const string EnergyCounterScenePath = "res://scenes/combat/energy_counters/gunslinger_energy_counter.tscn";

    public override List<(string, string)>? Localization => GetCharacterLocalization();

    public override CharacterGender Gender => CharacterGender.Masculine;

    public override Color NameColor => Color;

    public override string? CustomVisualPath => CreatureVisualsPath;

    public override string? CustomTrailPath => TrailAssetPath;

    public override string? CustomIconTexturePath => TopPanelIconPath;

    public override string? CustomIconPath => CharacterIconScenePath;

    public override string? CustomEnergyCounterPath => EnergyCounterScenePath;

    public override string? CustomRestSiteAnimPath => RestSiteAnimScenePath;

    public override string? CustomMerchantAnimPath => MerchantAnimScenePath;

    public override string? CustomCharacterSelectBg => CharacterSelectBgScenePath;

    public override string? CustomCharacterSelectIconPath => CharacterSelectIconAssetPath;

    public override string? CustomCharacterSelectLockedIconPath => CharacterSelectLockedIconAssetPath;

    public override string? CustomCharacterSelectTransitionPath => CharacterSelectTransitionAssetPath;

    public override string? CustomMapMarkerPath => MapMarkerAssetPath;

    public override int StartingHp => 77;

    public override int StartingGold => 99;

    public override CardPoolModel CardPool => ModelDb.CardPool<GunslingerCardPool>();

    public override PotionPoolModel PotionPool => ModelDb.PotionPool<IroncladPotionPool>();

    public override RelicPoolModel RelicPool => ModelDb.RelicPool<GunslingerRelicPool>();

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<Shoot>(),
        ModelDb.Card<Shoot>(),
        ModelDb.Card<Shoot>(),
        ModelDb.Card<Shoot>(),
        ModelDb.Card<DefendGunslinger>(),
        ModelDb.Card<DefendGunslinger>(),
        ModelDb.Card<DefendGunslinger>(),
        ModelDb.Card<DefendGunslinger>(),
        ModelDb.Card<Reload>(),
        ModelDb.Card<EtchedTracer>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<CylinderRelic>()
    ];

    public override float AttackAnimDelay => 0.15f;

    public override float CastAnimDelay => 0.25f;

    public override Color EnergyLabelOutlineColor => new("1F1F1F");

    public override Color DialogueColor => new("2C2C2C");

    public override Color MapDrawingColor => Color;

    public override Color RemoteTargetingLineColor => new("6B6B6B");

    public override Color RemoteTargetingLineOutline => new("1F1F1F");

    public override List<string> GetArchitectAttackVfx()
    {
        var num = 3;
        var list = new List<string>(num);
        CollectionsMarshal.SetCount(list, num);
        var span = CollectionsMarshal.AsSpan(list);
        var num2 = 0;
        span[num2] = "vfx/vfx_attack_slash";
        num2++;
        span[num2] = "vfx/vfx_attack_blunt";
        num2++;
        span[num2] = "vfx/vfx_heavy_blunt";
        return list;
    }

    private static List<(string, string)> GetCharacterLocalization()
    {
        return LocManager.Instance.Language switch
        {
            "kor" =>
            [
                ("title", "건슬링어"),
                ("titleObject", "건슬링어"),
                ("description", "실린더를 돌리며 추적탄으로 흐름을 만들고, 도탄 폭발로 템포를 이어가며, 봉인탄으로 마무리하는 총잡이입니다."),
                ("cardsModifierTitle", "건슬링어 카드"),
                ("cardsModifierDescription", "건슬링어 카드가 보상과 상점에 등장합니다."),
                ("pronounSubject", "그"),
                ("pronounObject", "그를"),
                ("pronounPossessive", "그의 것"),
                ("possessiveAdjective", "그의"),
                ("aromaPrinciple", "[sine]실린더가 도는 소리. 소원을 누르는 무게.[/sine]"),
                ("goldMonologue", "[sine]금은 강철은 사도, 구원은 사지 못한다.[/sine]"),
                ("eventDeathPrevention", "아직 끝나지 않았다. 소원도, 총성도.")
            ],
            "jpn" =>
            [
                ("title", "ガンスリンガー"),
                ("titleObject", "ガンスリンガー"),
                ("description", "シリンダーを回しながら射撃して刻印を溜め、追跡弾で流れを作り、封印弾で締めるガンマン。\n射撃が命中すると刻印1を得る。"),
                ("cardsModifierTitle", "ガンスリンガーカード"),
                ("cardsModifierDescription", "ガンスリンガーカードが報酬とショップに出現します。"),
                ("pronounSubject", "彼"),
                ("pronounObject", "彼を"),
                ("pronounPossessive", "彼のもの"),
                ("possessiveAdjective", "彼の"),
                ("aromaPrinciple", "[sine]シリンダーの回る音。願いを押し込む重さ。[/sine]"),
                ("goldMonologue", "[sine]金で鋼は買えても、救いまでは買えない。[/sine]"),
                ("eventDeathPrevention", "まだ終わらない。願いも、銃声も。")
            ],
            _ =>
            [
                ("title", "The Gunslinger"),
                ("titleObject", "The Gunslinger"),
                ("description", "A gunslinger who keeps tempo with tracer rounds, converts momentum into ricochet bursts, and finishes with seal rounds."),
                ("cardsModifierTitle", "Gunslinger Cards"),
                ("cardsModifierDescription", "Gunslinger cards will now appear in rewards and shops."),
                ("pronounSubject", "he"),
                ("pronounObject", "him"),
                ("pronounPossessive", "his"),
                ("possessiveAdjective", "his"),
                ("aromaPrinciple", "[sine]The click of a cylinder. The weight of a wish.[/sine]"),
                ("goldMonologue", "[sine]Gold buys steel, but not salvation.[/sine]"),
                ("eventDeathPrevention", "Not yet. The wish and the gunfire are not finished.")
            ]
        };
    }
}
