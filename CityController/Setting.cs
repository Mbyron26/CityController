using CityController.Systems;
using Colossal.IO.AssetDatabase;
using CS2Shared.Common;
using CS2Shared.Tools;
using Game.Input;
using Game.Modding;
using Game.Settings;
using Game.UI;
using Game.UI.Widgets;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;

namespace CityController;

[FileLocation($"ModsSettings/{nameof(CityController)}/{nameof(Setting)}")]
#if DEBUG
[SettingsUITabOrder(General, KeyBindings, Advanced, Debug)]
[SettingsUIGroupOrder(ModInfo, Achievements, Money, Reset, Serialize)]
[SettingsUIShowGroupName(ModInfo, Achievements, Money, Reset, Serialize)]
#else
[SettingsUITabOrder(General, KeyBindings, Advanced)]
[SettingsUIGroupOrder(ModInfo, Achievements, Money, Reset)]
[SettingsUIShowGroupName(ModInfo, Achievements, Money, Reset)]
#endif
public class Setting : ModSettingBase {
    private bool achievementsEnabled = true;
    internal static Setting Instance { get; set; }

    public const string AddMoneyAction = nameof(AddMoneyAction);
    public const string SubtractMoneyAction = nameof(SubtractMoneyAction);

    public Setting(IMod mod) : base(mod) { }

    internal const string Achievements = nameof(Achievements);
    internal const string Money = nameof(Money);
    internal const string Milestone = nameof(Milestone);

    [SettingsUISection(General, Achievements)]
    [SettingsUIHideByCondition(typeof(Setting), nameof(IsAchievementEnablerIncluded))]
    public bool AchievementsEnabled {
        get => achievementsEnabled;
        set {
            achievementsEnabled = value;
            World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<AchievementsControllerSystem>()?.SetAchievements(value);
        }
    }

    private bool IsAchievementEnablerIncluded() => ModTools.IsModInclusive("AchievementEnabler");

    [SettingsUISlider(min = 10000, max = 5000000, step = 50000, scalarMultiplier = 1, unit = Unit.kInteger)]
    [SettingsUISection(General, Money)]
    public int ManualMoneyAmount { get; set; }

    [SettingsUISection(General, Money)]
    public bool AutomaticAddMoney { get; set; }

    #region AutomaticAddMoneyThreshold
    [SettingsUIDropdown(typeof(Setting), nameof(GetAutomaticAddMoneyThresholdItems))]
    [SettingsUISection(General, Money)]
    [SettingsUIDisableByCondition(typeof(Setting), nameof(EnsureAutomaticAddMoneyEnabled))]
    public int AutomaticAddMoneyThreshold { get; set; }

    public bool EnsureAutomaticAddMoneyEnabled() => !AutomaticAddMoney;

    public DropdownItem<int>[] GetAutomaticAddMoneyThresholdItems() {
        var items = new List<DropdownItem<int>> {
            new () {
                value = 10000,
                displayName = 10000.ToString("N0"),
            },
            new () {
                value = 100000,
                displayName = 100000.ToString("N0"),
            },
            new () {
                value = 1000000,
                displayName = 1000000.ToString("N0"),
            },
            new () {
                value = 10000000,
                displayName = 10000000.ToString("N0"),
            },
        };
        return items.ToArray();
    }
    #endregion

    #region AutomaticAddMoneyAmount
    [SettingsUIDropdown(typeof(Setting), nameof(GetAutomaticAddMoneyAmountItems))]
    [SettingsUISection(General, Money)]
    [SettingsUIDisableByCondition(typeof(Setting), nameof(EnsureAutomaticAddMoneyEnabled))]
    public int AutomaticAddMoneyAmount { get; set; }

    public DropdownItem<int>[] GetAutomaticAddMoneyAmountItems() {
        var items = new List<DropdownItem<int>> {
            new () {
                value = 10000,
                displayName = 10000.ToString("N0"),
            },
            new () {
                value = 100000,
                displayName = 100000.ToString("N0"),
            },
            new () {
                value = 1000000,
                displayName = 1000000.ToString("N0"),
            },
            new () {
                value = 10000000,
                displayName = 10000000.ToString("N0"),
            },
            new () {
                value = 100000000,
                displayName = 100000000.ToString("N0"),
            },
        };
        return items.ToArray();
    }
    #endregion

    [SettingsUIButton]
    [SettingsUIConfirmation]
    [SettingsUISection(General, Money)]
    [SettingsUIDisableByCondition(typeof(Setting), nameof(NotInGame))]
    public bool MoneyTransfer {
        set {
            if (NotInGame())
                return;
            World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<MoneyControllerSystem>()?.SetUnlimitedMoneyToLimitedMoney();
        }
    }

    #region CustomMilestone
    [SettingsUISection(General, Milestone)]
    [SettingsUIDisableByCondition(typeof(Setting), nameof(InGame))]
    public bool CustomMilestone { get; set; }
    #endregion

    #region MilestoneLevel
    [SettingsUIDropdown(typeof(Setting), nameof(GetMilestoneLevelItems))]
    [SettingsUISection(General, Milestone)]
    [SettingsUIDisableByCondition(typeof(Setting), nameof(GetMilestoneLevelStatus))]
    public int MilestoneLevel { get; set; }

    private bool GetMilestoneLevelStatus() => InGame() || !CustomMilestone;

    private DropdownItem<int>[] GetMilestoneLevelItems() {
        var items = new List<DropdownItem<int>>();
        for (int i = 0; i < 20; i++) {
            items.Add(new DropdownItem<int>() {
                value = i,
                displayName = GetOptionLocaleID(Milestones[i])
            });
        }
        return items.ToArray();
    }
    #endregion

    private bool InGame() => !NotInGame();
    private bool NotInGame() => Mode != Game.GameMode.Game;

    [SettingsUIKeyboardBinding(BindingKeyboard.M, AddMoneyAction, ctrl: true, shift: true)]
    [SettingsUISection(KeyBindings, Money)]
    public ProxyBinding AddMoneyKeyboardBinding { get; set; }

    [SettingsUIKeyboardBinding(BindingKeyboard.N, SubtractMoneyAction, ctrl: true, shift: true)]
    [SettingsUISection(KeyBindings, Money)]
    public ProxyBinding SubtractMoneyKeyboardBinding { get; set; }

    public override void SetDefaults() {
        base.SetDefaults();
        ManualMoneyAmount = 1000000;
        AutomaticAddMoney = false;
        AutomaticAddMoneyThreshold = 1000000;
        AutomaticAddMoneyAmount = 1000000;
        CustomMilestone = false;
        MilestoneLevel = 19;
    }

    public string[] Milestones { get; } = new string[] {
        "TinyVillage",
        "SmallVillage",
        "LargeVillage",
        "GrandVillage",
        "TinyTown",
        "BoomTown",
        "BusyTown",
        "BigTown",
        "GreatTown",
        "SmallCity",
        "BigCity",
        "LargeCity",
        "HugeCity",
        "GrandCity",
        "Metropolis",
        "ThrivingMetropolis",
        "FlourishingMetropolis",
        "ExpansiveMetropolis",
        "MassiveMetropolis",
        "Megalopolis",
    };

    protected override void CreateLocaleSource() {
        base.CreateLocaleSource();
        AddLocaleSource(GetSettingsLocaleID(), "City Controller");
        AddLocaleSource(GetOptionGroupLocaleID(Achievements), "Achievements");
        AddLocaleSource(GetOptionGroupLocaleID(Money), "Money");
        AddLocaleSource(GetOptionGroupLocaleID(Milestone), "Milestone");
        AddLocaleSource(GetOptionLabelLocaleID(nameof(AchievementsEnabled)), "Enable Achievements");
        AddLocaleSource(GetOptionDescLocaleID(nameof(AchievementsEnabled)), "Allows Achievements system to be activated even when Mods ars enabled, Unlock All or Unlimited Money is enabled.");
        AddLocaleSource(GetOptionLabelLocaleID(nameof(AutomaticAddMoney)), "Automatic Add Money");
        AddLocaleSource(GetOptionDescLocaleID(nameof(AutomaticAddMoney)), $"Select to enable automatic add money mode.");
        AddLocaleSource(GetOptionLabelLocaleID(nameof(ManualMoneyAmount)), "Manual Money Amount");
        AddLocaleSource(GetOptionDescLocaleID(nameof(ManualMoneyAmount)), $"Set this value to manually add/subtract money amounts.");
        AddLocaleSource(GetOptionLabelLocaleID(nameof(AutomaticAddMoneyThreshold)), "Automatic Add Money Threshold");
        AddLocaleSource(GetOptionDescLocaleID(nameof(AutomaticAddMoneyThreshold)), $"Set this value to automatically add money when money falls below this set threshold.");
        AddLocaleSource(GetOptionLabelLocaleID(nameof(AutomaticAddMoneyAmount)), "Automatic Add Money Amount");
        AddLocaleSource(GetOptionDescLocaleID(nameof(AutomaticAddMoneyAmount)), $"Set the amount of money that will be automatically added when money falls below the threshold.");
        AddLocaleSource(GetOptionLabelLocaleID(nameof(MoneyTransfer)), "Money Transfer");
        AddLocaleSource(GetOptionDescLocaleID(nameof(MoneyTransfer)), $"Allows you to switch Unlimited Money to Limited Money in the game, this options only available in-game");
        AddLocaleSource(GetOptionWarningLocaleID(nameof(MoneyTransfer)), "Are you sure you want to convert Unlimited Money to Limited Money? This operation is not reversible!");
        AddLocaleSource(GetOptionLabelLocaleID(nameof(CustomMilestone)), "Custom Milestone");
        AddLocaleSource(GetOptionDescLocaleID(nameof(CustomMilestone)), "Enable this option to customize the start milestone, and needs to be seton the main menu page before entering the game.");
        AddLocaleSource(new Dictionary<string, string>() {
            { GetOptionLabelLocaleID(nameof(MilestoneLevel)), "Milestone" },
            { GetOptionDescLocaleID(nameof(MilestoneLevel)), "Select any milestone level to unlock before starting game, and needs to be set on the main menu page before entering the game." },
            { GetOptionLabelLocaleID(nameof(AddMoneyKeyboardBinding)), "Add Money" },
            { GetOptionDescLocaleID(nameof(AddMoneyKeyboardBinding)), $"Hotkeys for adding money within the game." },
            { GetBindingKeyLocaleID(AddMoneyAction), "Add Money" },
            { GetOptionLabelLocaleID(nameof(SubtractMoneyKeyboardBinding)), "Subtract Money" },
            { GetOptionDescLocaleID(nameof(SubtractMoneyKeyboardBinding)), $"Hotkeys for subtracting money within the game." },
            { GetBindingKeyLocaleID(SubtractMoneyAction), "Subtract Money" },
        });
        AddLocaleSource(Milestones.ToDictionary(milestone => GetOptionLocaleID(milestone), milestone => milestone));
    }
}