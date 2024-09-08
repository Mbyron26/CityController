using CS2Shared.Common;
using Game;
using Game.City;
using Game.Input;
using Game.SceneFlow;
using Game.Simulation;

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
        if (componentData.m_Unlimited) {
            componentData.m_Unlimited = false;
            EntityManager.SetComponentData(citySystem.City, componentData);
            Logger.Info($"Set PlayerMoney to Limited Money {componentData.m_Unlimited}{componentData.money}");
        }
    }

    public void OnSubtractMoney() => ModifyMoney(ModifyMoneyType.Subtract, Setting.Instance.ManualMoneyAmount);

    public void OnAddMoney() => ModifyMoney(ModifyMoneyType.Add, Setting.Instance.ManualMoneyAmount);

    private void ModifyMoney(ModifyMoneyType modifyMoneyType, int money, bool auto = false) {
        if (GameManager.instance.gameMode != GameMode.Game || citySystem is null || modifyMoneyType == ModifyMoneyType.None)
            return;
        PlayerMoney componentData = EntityManager.GetComponentData<PlayerMoney>(citySystem.City);
        if (modifyMoneyType == ModifyMoneyType.Add) {
            componentData.Add(money);
        }
        else if (modifyMoneyType == ModifyMoneyType.Subtract) {
            componentData.Subtract(money);
        }
        var prefix = auto ? "Auto " : "";
        Logger.Info($"{prefix}{modifyMoneyType} money {money} to {componentData.money} ");
        EntityManager.SetComponentData(citySystem.City, componentData);
    }

    public enum ModifyMoneyType {
        Add,
        Subtract,
        None
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
                ModifyMoney(ModifyMoneyType.Add, Setting.Instance.AutomaticAddMoneyAmount, true);
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