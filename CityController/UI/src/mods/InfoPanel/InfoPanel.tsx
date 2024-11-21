import { CSSProperties, ReactNode } from "react";
import styles from "../InfoPanel/InfoPanel.module.scss";

export interface InfoPanelProps {
    className?: string;
    style?: CSSProperties;
    title?: string;
    children?: ReactNode;
}

export const InfoPanel = ({ className = "", style = {}, title, children }: InfoPanelProps) => {
    return (
        <div className={`${styles.infoPanel} ${className}`} style={style}>
            {title && <div className={styles.infoPanelTitle}>{title}</div>}
            {children}
        </div>
    );
};