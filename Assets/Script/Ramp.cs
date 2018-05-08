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

	public int Get_Height() {
		return Height;
	}

    public override void RotatePath(bool clockwise = true)
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

	public void ChangeRampHeight () {
		float y_Change = RotationManager.instance.RampHeight * Height;
		Vector3 moveRamp = new Vector3 (0, y_Change, 0);
		transform.Translate (moveRamp);
	}
}

