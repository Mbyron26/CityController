import { ModRegistrar } from "cs2/modding";
import mod from "../mod.json";
import { NotificationPanel } from "./mods/NotificationPanel/NotificationPanel";
import { EntryButton } from "./mods/EntryButton/EntryButton";
import { VanillaComponentResolver } from "./mods/VanillaComponentResolver/VanillaComponentResolver";

const register: ModRegistrar = (moduleRegistry) => {
    VanillaComponentResolver.setRegistry(moduleRegistry);
    moduleRegistry.append("GameTopLeft", EntryButton);
    moduleRegistry.append("Game", NotificationPanel);
    console.log(`${mod.id} - UI registration completed`);
}

export default register;