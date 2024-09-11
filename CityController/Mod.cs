using CityController.Systems;
using CS2Shared.Common;
using CS2Shared.Tools;
using Game;
using Game.Modding;
using System;

namespace CityController;

public class Mod : ModBase, IMod {
    public override bool BetaVersion => true;
    public override DateTime VersionDate => new(2024, 9, 11);

    protected override void CreateSetting() {
        Setting = CityController.Setting.Instance = new Setting(this);
        LoadSetting(new Setting(this));
    }

    protected override void CreateSystem(UpdateSystem updateSystem) {
        if (!ModTools.IsModInclusive("AchievementEnabler"))
            updateSystem.UpdateAfter<AchievementsControllerSystem>(SystemUpdatePhase.Deserialize);
        updateSystem.UpdateAt<MoneyControllerSystem>(SystemUpdatePhase.ModificationEnd);
        updateSystem.UpdateAt<UnlockMilestonesSystem>(SystemUpdatePhase.ModificationEnd);
    }
}