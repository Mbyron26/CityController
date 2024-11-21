using CityController.Settings;
using Colossal.PSI.Common;
using Colossal.Serialization.Entities;
using CS2Shared.Common;
using Game;

namespace CityController.Systems;

public partial class AchievementsControllerSystem : GameSystemBaseExtension {
    protected override void OnGamePreload(Purpose purpose, GameMode mode) {
        base.OnGamePreload(purpose, mode);
        Logger.Info($"AchievementsControllerSystem OnGamePreload, game mode: {mode}, game/mod achievements status: {PlatformManager.instance.achievementsEnabled} {Setting.Instance.AchievementsEnabled} ");
    }

    protected override void OnGameLoaded(Context serializationContext) {
        base.OnGameLoaded(serializationContext);
        Logger.Info($"AchievementsControllerSystem OnGameLoaded, game/mod achievements status: {PlatformManager.instance.achievementsEnabled} {Setting.Instance.AchievementsEnabled} ");
        SetAchievements(Setting.Instance.AchievementsEnabled);
    }

    public void SetAchievements(bool enabled) {
        if (PlatformManager.instance.achievementsEnabled == enabled)
            return;
        PlatformManager.instance.achievementsEnabled = enabled;
        Logger.Info($"Set achievements: {enabled}");
    }
}
