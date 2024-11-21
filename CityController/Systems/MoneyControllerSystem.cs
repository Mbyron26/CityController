using CityController.Settings;
using Colossal.Serialization.Entities;
using CS2Shared.Common;
using Game;
using Game.City;
using Game.Input;
using Game.SceneFlow;
using Game.Simulation;
using System.Reflection;
using System.Text;
using Unity.Entities;

namespace CityController.Systems;

public partial class MoneyControllerSystem : GameSystemBaseExtension {
    private CitySystem citySystem;
    private CityConfigurationSystem cityConfigurationSystem;
    private ProxyAction addMoneyAction;
    private ProxyAction subtractMoneyAction;

    public void ExportCurrentCityConfigurationInformation() {
        StringBuilder stringBuilder = new();
        stringBuilder.AppendLine($"---Current City Configuration Information---");
        stringBuilder.AppendLine($"{nameof(cityConfigurationSystem.cityName)}: {cityConfigurationSystem.cityName}");
        stringBuilder.AppendLine($"{nameof(cityConfigurationSystem.overrideCityName)}: {cityConfigurationSystem.overrideCityName}");
        stringBuilder.AppendLine($"{nameof(cityConfigurationSystem.leftHandTraffic)}: {cityConfigurationSystem.leftHandTraffic}");
        stringBuilder.AppendLine($"{nameof(cityConfigurationSystem.overrideLeftHandTraffic)}: {cityConfigurationSystem.overrideLeftHandTraffic}");
        stringBuilder.AppendLine($"{nameof(cityConfigurationSystem.naturalDisasters)}: {cityConfigurationSystem.naturalDisasters}");
        stringBuilder.AppendLine($"{nameof(cityConfigurationSystem.overrideNaturalDisasters)}: {cityConfigurationSystem.overrideNaturalDisasters}");
        stringBuilder.AppendLine($"{nameof(cityConfigurationSystem.unlockAll)}: {cityConfigurationSystem.unlockAll}");
        stringBuilder.AppendLine($"{nameof(cityConfigurationSystem.overrideUnlockAll)}: {cityConfigurationSystem.overrideUnlockAll}");
        stringBuilder.AppendLine($"{nameof(cityConfigurationSystem.unlimitedMoney)}: {cityConfigurationSystem.unlimitedMoney}");
        stringBuilder.AppendLine($"{nameof(cityConfigurationSystem.overrideUnlimitedMoney)}: {cityConfigurationSystem.overrideUnlimitedMoney}");
        stringBuilder.AppendLine($"{nameof(cityConfigurationSystem.unlockMapTiles)}: {cityConfigurationSystem.unlockMapTiles}");
        stringBuilder.AppendLine($"{nameof(cityConfigurationSystem.overrideUnlockMapTiles)}: {cityConfigurationSystem.overrideUnlockMapTiles}");
        stringBuilder.AppendLine($"{nameof(cityConfigurationSystem.overrideLoadedOptions)}: {cityConfigurationSystem.overrideLoadedOptions}");
        stringBuilder.AppendLine($"{string.Join(", ", cityConfigurationSystem.usedMods)}");
        stringBuilder.AppendLine($"---End Current City Configuration Information---");
        Logger.Info(stringBuilder.ToString());
    }

    public void SetUnlimitedMoneyToLimitedMoney() {
        if (citySystem is null || cityConfigurationSystem is null)
            return;
        Logger.Info($"Starting set unlimited money to limited money to limited money, PlayerMoney.m_Unlimited: {EntityManager.GetComponentData<PlayerMoney>(citySystem.City).m_Unlimited}, PlayerMoney.money: {EntityManager.GetComponentData<PlayerMoney>(citySystem.City).money}, CityConfigurationSystem.unlimitedMoney: {cityConfigurationSystem.unlimitedMoney}, CityConfigurationSystem.overrideUnlimitedMoney: {cityConfigurationSystem.overrideUnlimitedMoney}");
        cityConfigurationSystem.unlimitedMoney = false;
        cityConfigurationSystem.overrideUnlimitedMoney = false;
        var componentData = EntityManager.GetComponentData<PlayerMoney>(citySystem.City);
        componentData.m_Unlimited = false;
        EntityManager.SetComponentData(citySystem.City, componentData);
        var field = typeof(CityConfigurationSystem).GetField("m_LoadedUnlimitedMoney", BindingFlags.NonPublic | BindingFlags.Instance);
        if (field is null)
            Logger.Info("m_LoadedUnlimitedMoney is null");
        else
            field.SetValue(cityConfigurationSystem, false);
        Logger.Info($"Set unlimited money to limited money to limited money completed, PlayerMoney.m_Unlimited: {componentData.m_Unlimited}, PlayerMoney.money: {componentData.money}, CityConfigurationSystem.unlimitedMoney: {cityConfigurationSystem.unlimitedMoney}, CityConfigurationSystem.overrideUnlimitedMoney: {cityConfigurationSystem.overrideUnlimitedMoney}");
    }

    public void OnSubtractMoney() => ModifyMoney(ModifyMoneyType.ManualSubtract, Setting.Instance.ManualMoneyAmount);

    public void OnAddMoney() => ModifyMoney(ModifyMoneyType.ManualAdd, Setting.Instance.ManualMoneyAmount);

    private void ModifyMoney(ModifyMoneyType modifyMoneyType, int money) {
        if (GameManager.instance.gameMode != GameMode.Game || citySystem is null || modifyMoneyType == ModifyMoneyType.None)
            return;
        PlayerMoney componentData = EntityManager.GetComponentData<PlayerMoney>(citySystem.City);
        if (modifyMoneyType == ModifyMoneyType.AutoAdd || modifyMoneyType == ModifyMoneyType.ManualAdd) {
            componentData.Add(money);
        }
        else if (modifyMoneyType == ModifyMoneyType.AutoSubtract || modifyMoneyType == ModifyMoneyType.ManualSubtract) {
            componentData.Subtract(money);
        }
        Logger.Info($"{modifyMoneyType} money {money} to {componentData.money} ");
        EntityManager.SetComponentData(citySystem.City, componentData);
    }

    public enum ModifyMoneyType {
        AutoAdd,
        ManualAdd,
        AutoSubtract,
        ManualSubtract,
        None
    }

    protected override void OnGameLoaded(Context serializationContext) {
        base.OnGameLoaded(serializationContext);
        if ((serializationContext.purpose == Purpose.NewGame || serializationContext.purpose == Purpose.LoadGame) && Setting.Instance.InitialMoney != 0) {
            var componentData = EntityManager.GetComponentData<PlayerMoney>(citySystem.City);
            if (!componentData.m_Unlimited) {
                var raw = componentData.money;
                Logger.Info($"Setting initial money, default money: {raw}");
                ModifyMoney(ModifyMoneyType.AutoSubtract, raw);
                ModifyMoney(ModifyMoneyType.AutoAdd, Setting.Instance.InitialMoney);
                Setting.Instance.ResetInitialMoney();
                Logger.Info($"Set initial money completed, money: {componentData.money}");
            }
        }
    }

    protected override void OnCreate() {
        base.OnCreate();
        citySystem = World.GetOrCreateSystemManaged<CitySystem>();
        cityConfigurationSystem = World?.GetOrCreateSystemManaged<CityConfigurationSystem>();
        addMoneyAction = Setting.Instance.GetAction(Setting.AddMoneyAction);
        addMoneyAction.shouldBeEnabled = true;
        subtractMoneyAction = Setting.Instance.GetAction(Setting.SubtractMoneyAction);
        subtractMoneyAction.shouldBeEnabled = true;
    }

    protected override void OnUpdate() {
        if (Setting.Instance.AutomaticAddMoney && InGame) {
            PlayerMoney componentData = EntityManager.GetComponentData<PlayerMoney>(citySystem.City);
            if (componentData.money < Setting.Instance.AutomaticAddMoneyThreshold) {
                Logger.Info($"{componentData.money} < {Setting.Instance.AutomaticAddMoneyThreshold}, automatically add money");
                ModifyMoney(ModifyMoneyType.AutoAdd, Setting.Instance.AutomaticAddMoneyAmount);
            }
        }
        if (InGame && addMoneyAction.WasPerformedThisFrame()) {
            OnAddMoney();
        }
        if (InGame && subtractMoneyAction.WasPerformedThisFrame()) {
            OnSubtractMoney();
        }
    }
}