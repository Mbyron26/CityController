import { useValue } from "cs2/api";
import { Button, Icon, Tooltip } from "cs2/ui";
import { OnControlPanelBindingToggle, controlPanelEnabled$ } from "../Bindings/Bindings";

export const EntryButton = () => {
    const showPanel = useValue(controlPanelEnabled$);
    const modIcon = "coui://ui-mods/images/CityControllerIcon_outline.svg";

    return (
        <Tooltip
            tooltip="City Controller">
            <Button
                variant="floating"
                selected={showPanel}
                onSelect={() => { OnControlPanelBindingToggle(!showPanel) }}>
                <Icon tinted={true} src={modIcon} />
            </Button>
        </Tooltip>
    )
}