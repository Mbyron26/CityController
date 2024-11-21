using CityController.Settings;
using CityController.Systems;
using CS2Shared.Common;
using CS2Shared.Tools;
using Game;
using Game.Modding;
using System;

namespace CityController;

public class Mod : ModBase, IMod {
    public override bool BetaVersion => true;
    public override DateTime VersionDate => new(2024, 11, 21);

    protected override void CreateSetting() {
        Setting = Settings.Setting.Instance = new Setting(this);
        LoadSetting(new Setting(this));
    }

    protected override void CreateSystem(UpdateSystem updateSystem) {
        base.CreateSystem(updateSystem);
        if (!ModTools.IsModInclusive("AchievementEnabler"))
            updateSystem.UpdateAfter<AchievementsControllerSystem>(SystemUpdatePhase.Deserialize);
        updateSystem.UpdateAt<MoneyControllerSystem>(SystemUpdatePhase.ModificationEnd);
        updateSystem.UpdateAt<UnlockMilestonesSystem>(SystemUpdatePhase.ModificationEnd);
        updateSystem.UpdateAt<CityControllerUISystem>(SystemUpdatePhase.UIUpdate);
        updateSystem.UpdateAt<NotificationControllerSystem>(SystemUpdatePhase.ModificationEnd);
    }
}