using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoBehaviour {

	public readonly float RampHeight = 0.44f;
    public static RotationManager instance = null;

    void Awake(){
        instance = this;
    }

    public Direction RotateCW(Direction dir)
    {
        int curDir = (int)dir;
        return (Direction)((curDir + 1) % 4);
    }

    public Direction RotateACW(Direction dir)
    {
        int curDir = (int)dir;
        curDir = ((curDir - 1) % 4);

        if (curDir < 0)
        {
            curDir = 3;
        }
        return (Direction)curDir;

    }

    public Direction GetOppositeDirection(Direction dir)
    {
        int curDir = (int)dir;
        return (Direction)((curDir + 2) % 4);
    }

	public bool PathsMatches(Direction dir, Direction opposite) {
		return dir == GetOppositeDirection (opposite);
	}
}
