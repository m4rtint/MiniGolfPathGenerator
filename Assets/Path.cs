using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path:MonoBehaviour {

    readonly Vector3 SetupVector = new Vector3(1.5f,0,1.5f);

    public Direction[] m_EntryPoints;
    public Vector3 point;


    void Awake() {
        if (gameObject.transform.childCount > 0)
        {
            gameObject.transform.GetChild(0).Translate(SetupVector);
        }
    }

    public void RotatePath(bool clockwise){
        //Rotate axis
        float isClockWise = clockwise ? 1 : -1;
        Vector3 rotateCW = new Vector3(0, isClockWise * 90, 0);
        transform.Rotate(rotateCW);

        //Change Direction
        for (int i = 0; i < m_EntryPoints.Length; i++)
        {
            Direction dir = m_EntryPoints[i];
            m_EntryPoints[i] = clockwise ? RotationManager.instance.RotateCW(dir) : RotationManager.instance.RotateACW(dir);
        }
    }

}
