using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramp : Path {

    [SerializeField]
    Direction m_High;
    [SerializeField]
    Direction m_Low;

    public Direction Get_HighDirection(){
        return m_High;
    }

    public Direction Get_LowDirection(){
        return m_Low;
    }
}

