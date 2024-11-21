using System.Collections.Generic;
using CityController.Systems;
using Colossal.IO.AssetDatabase;
using CS2Shared.Settings;
using CS2Shared.Tools;
using Game.Input;
using Game.Modding;
using Game.Settings;
using Game.UI;
using Game.UI.Widgets;
using Unity.Entities;

namespace CityController.Settings;

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
public partial class Setting : ModSettingBase {
    internal static Setting Instance { get; set; }

    public const string AddMoneyAction = nameof(AddMoneyAction);
    public const string SubtractMoneyAction = nameof(SubtractMoneyAction);

    public Setting(IMod mod) : base(mod) { }

    internal const string Achievements = nameof(Achievements);
    internal const string Money = nameof(Money);
    internal const string Milestone = nameof(Milestone);

    [SettingsUISection(General, Achievements)]
    [SettingsUIHideByCondition(typeof(Setting), nameof(IsAchievementEnablerIncluded))]
    [SettingsUISetter(typeof(Setting), nameof(OnAchievementsOptionChanged))]
    public bool AchievementsEnabled { get; set; }

    private bool IsAchievementEnablerIncluded() => ModTools.IsModInclusive("AchievementEnabler");

    private void OnAchievementsOptionChanged(bool value) => World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<AchievementsControllerSystem>()?.SetAchievements(value);

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

    [SettingsUIDropdown(typeof(Setting), nameof(GetInitialMoneyItems))]
    [SettingsUISection(General, Money)]
    [SettingsUIDisableByCondition(typeof(Setting), nameof(IsInGame))]
    public int InitialMoney { get; set; }

    public DropdownItem<int>[] GetInitialMoneyItems() {
        var items = new List<DropdownItem<int>> {
            new () {
                value = 0,
                displayName = GetOptionLocaleID("GameDefault"),
            },
            new () {
                value = 100000,
                displayName = 100000.ToString("N0"),
            },
            new () {
                value = 500000,
                displayName = 500000.ToString("N0"),
            },
            new () {
                value = 5000000,
                displayName = 5000000.ToString("N0"),
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

    public void ResetInitialMoney() => InitialMoney = 0;

    [SettingsUIButton]
    [SettingsUIConfirmation]
    [SettingsUISection(General, Money)]
    [SettingsUIDisableByCondition(typeof(Setting), nameof(NotInGame))]
    public bool MoneyTransfer {
        set {
            if (NotInGame)
                return;
            World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<MoneyControllerSystem>()?.SetUnlimitedMoneyToLimitedMoney();
        }
    }

    #region CustomMilestone
    [SettingsUISection(General, Milestone)]
    [SettingsUIDisableByCondition(typeof(Setting), nameof(IsInGame))]
    public bool CustomMilestone { get; set; }
    #endregion

    #region MilestoneLevel
    [SettingsUIDropdown(typeof(Setting), nameof(GetMilestoneLevelItems))]
    [SettingsUISection(General, Milestone)]
    [SettingsUIDisableByCondition(typeof(Setting), nameof(GetMilestoneLevelStatus))]
    public int MilestoneLevel { get; set; }

    private bool GetMilestoneLevelStatus() => InGame || !CustomMilestone;

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

#if DEBUG
    [SettingsUIKeyboardBinding(BindingKeyboard.T, "DebugAction", ctrl: true, shift: true)]
    [SettingsUISection(KeyBindings, Money)]
    public ProxyBinding DebugKeyboardBinding { get; set; }
#endif

    [SettingsUIKeyboardBinding(BindingKeyboard.M, AddMoneyAction, ctrl: true, shift: true)]
    [SettingsUISection(KeyBindings, Money)]
    public ProxyBinding AddMoneyKeyboardBinding { get; set; }

    [SettingsUIKeyboardBinding(BindingKeyboard.N, SubtractMoneyAction, ctrl: true, shift: true)]
    [SettingsUISection(KeyBindings, Money)]
    public ProxyBinding SubtractMoneyKeyboardBinding { get; set; }

    public override void SetDefaults() {
        base.SetDefaults();
        AchievementsEnabled = true;
        ManualMoneyAmount = 1000000;
        AutomaticAddMoney = false;
        AutomaticAddMoneyThreshold = 1000000;
        AutomaticAddMoneyAmount = 1000000;
        InitialMoney = 0;
        CustomMilestone = false;
        MilestoneLevel = 19;
        Notification.SetDefaults();
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

    
}