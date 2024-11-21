using CS2Shared.Settings;
using System;

namespace CityController.Settings;

public partial class Setting {
    public SettingHidden Hidden { get; set; } = new();

    public class SettingHidden : SettingChildClassBase {
        public bool ControlPanelDraggable { get; set; }

        public override void SetDefaults() {
            ControlPanelDraggable = false;
        }
    }
}
