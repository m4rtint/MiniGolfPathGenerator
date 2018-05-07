using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramp : Path {

    [Header("Ramp Property")]
    [SerializeField]
    Direction m_High;
    [SerializeField]
    Direction m_Low;
    [SerializeField]
    int Height;

    public Direction Get_HighDirection(){
        return m_High;
    }

    public Direction Get_LowDirection(){
        return m_Low;
    }

    public override void RotatePath(bool clockwise)
    {
        base.RotatePath(clockwise);

        if (clockwise) {
            m_High = RotationManager.instance.RotateCW(m_High);
            m_Low = RotationManager.instance.RotateCW(m_Low);
        } else {
            m_High = RotationManager.instance.RotateACW(m_High);
            m_Low = RotationManager.instance.RotateACW(m_Low);
        }

    }
}

