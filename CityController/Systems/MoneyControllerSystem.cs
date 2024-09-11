using Colossal.Serialization.Entities;
using CS2Shared.Common;
using Game;
using Game.City;
using Game.Input;
using Game.SceneFlow;
using Game.Simulation;
using Unity.Entities;

namespace CityController.Systems;

public partial class MoneyControllerSystem : GameSystemExtensionBase {
    private CitySystem citySystem;
    private ProxyAction addMoneyAction;
    private ProxyAction subtractMoneyAction;

    public void SetUnlimitedMoneyToLimitedMoney() {
        if (citySystem is null)
            return;
        var cityConfigurationSystem = World?.GetOrCreateSystemManaged<CityConfigurationSystem>();
        if (cityConfigurationSystem is null)
            return;
        cityConfigurationSystem.unlimitedMoney = false;
        cityConfigurationSystem.overrideUnlimitedMoney = false;
        Logger.Info($"Set CityConfigurationSystem to Limited Money");
        PlayerMoney componentData = EntityManager.GetComponentData<PlayerMoney>(citySystem.City);
        componentData.m_Unlimited = false;
        ModifyMoney(ModifyMoneyType.AutoAdd, 1);
        ModifyMoney(ModifyMoneyType.AutoSubtract, 1);
        EntityManager.SetComponentData(citySystem.City, componentData);
        Logger.Info($"Set PlayerMoney to Limited Money {componentData.m_Unlimited}{componentData.money}");
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
        addMoneyAction = Setting.Instance.GetAction(Setting.AddMoneyAction);
        addMoneyAction.shouldBeEnabled = true;
        subtractMoneyAction = Setting.Instance.GetAction(Setting.SubtractMoneyAction);
        subtractMoneyAction.shouldBeEnabled = true;
    }

    protected override void OnUpdate() {
        if (Setting.Instance.AutomaticAddMoney && IsInGame) {
            PlayerMoney componentData = EntityManager.GetComponentData<PlayerMoney>(citySystem.City);
            if (componentData.money < Setting.Instance.AutomaticAddMoneyThreshold) {
                Logger.Info($"{componentData.money} < {Setting.Instance.AutomaticAddMoneyThreshold}, automatically add money");
                ModifyMoney(ModifyMoneyType.AutoAdd, Setting.Instance.AutomaticAddMoneyAmount);
            }
        }
        if (IsInGame && addMoneyAction.WasPerformedThisFrame()) {
            OnAddMoney();
        }
        if (IsInGame && subtractMoneyAction.WasPerformedThisFrame()) {
            OnSubtractMoney();
        }
    }
}