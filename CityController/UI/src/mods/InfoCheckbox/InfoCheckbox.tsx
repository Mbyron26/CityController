import { Icon } from "cs2/ui";
import { CSSProperties } from "react";
import { Checkbox } from "../Checkbox/Checkbox";
import styles from "./InfoCheckbox.module.scss";

interface InfoCheckboxProps {
    image: string;
    label: string | null;
    count?: number;
    isChecked: boolean;
    onToggle: (newVal: boolean) => void;
    className?: string;
    style?: CSSProperties;
}

export const InfoCheckbox = ({ image, label, count, isChecked, onToggle, className, style }: InfoCheckboxProps) => {
    return (
        <div className={styles.subPanel + " " + className} style={{ ...style, opacity: isChecked ? 1 : 0.5 }} onClick={() => onToggle(!isChecked)}>
            <div className={styles.iconLabelSection}>
                <Icon src={image} className={styles.icon} />
                <span className={styles.label}>{label}</span>
            </div>
            <div className={styles.labelCheckboxSection}>
                {count !== undefined && count !== null && (
                    <span className={styles.label}>{"Count" + ": " + count}</span>
                )}
                <Checkbox isChecked={isChecked} onValueToggle={(value) => { }} />
            </div>
        </div>
    )
};