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

}
