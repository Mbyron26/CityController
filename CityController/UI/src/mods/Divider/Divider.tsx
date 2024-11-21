import { CSSProperties } from "react";
import styles from "../Divider/Divider.module.scss";

interface DividerProps {
    className?: string;
    style?: CSSProperties;
    text?: string;
}

export const Divider = ({ className, style, text }: DividerProps) => {
    return (
        <div className={`${styles.divider} ${className || ""}`} style={style}>
            {text && <span className={styles.dividerText}>{text}</span>}
        </div>
    );
};
