using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { 
NORTH,
EAST,
SOUTH,
WEST,
}

//Clockwise
//North 0+1 %4
//West 3+1 %4

//ANticlockwise
//North 
public class PathGenerator : MonoBehaviour
{

    readonly float PathSize = 3f;

    Path m_StartPath;
    Path m_EndPath;
    Path[] m_ListOfPaths;

    Vector3 m_StartPoint;
    Vector3 m_CurrentPoint;

    Direction m_CurrentDirection;

    GameObject paths;

    public PathGenerator(Path start, Path end, Vector3 startCoordinates, Path[] LoP)
    {
        m_StartPath = start;
        m_StartPoint = startCoordinates;
        m_ListOfPaths = LoP;
        m_EndPath = end;
        m_CurrentPoint = m_StartPoint;
        m_CurrentDirection = m_StartPath.GetComponent<Path>().m_EntryPoints[0];
    }

    void ResetProperties(){
        m_CurrentPoint = m_StartPoint;
        m_CurrentDirection = m_StartPath.GetComponent<Path>().m_EntryPoints[0];
    }

    public void GeneratePath(int maxPaths = 10)
    {
        ResetProperties();
        paths = new GameObject("Path");

        //First Path
        InstantiatePath(m_StartPath, m_StartPoint);

        // Middle Section
        for (int i = 0; i < maxPaths; i++)
        {
            Path nextPath = RandomlyChoosePath();
            Vector3 nextPosition = NextPosition();
            GameObject nextPathGameObject = InstantiatePath(nextPath, nextPosition);
            SetChosenPath(nextPathGameObject.GetComponent<Path>());

        }

        //End Path
        NextPosition();
        GameObject m_NextEndPath = InstantiatePath(m_EndPath, m_CurrentPoint);
        SetEndPath(m_NextEndPath.GetComponent<Path>());

    }



    GameObject InstantiatePath(Path path, Vector3 point)
    {
        GameObject pathNew = Instantiate(path.gameObject, point, Quaternion.identity);
        pathNew.GetComponent<Path>().point = point;
        pathNew.transform.parent = paths.transform;
        return pathNew;
    }

    #region EndPath
    void SetEndPath(Path end) {
        while (!CheckPathDirectionFit(end)) {
            RotatePath(end,false);
        }
    }
    #endregion

    #region DirectionSpawning
    Vector3 NextPosition(){
        Vector3 placement = Vector3.zero;
        switch(m_CurrentDirection){
            case Direction.EAST:
                placement = new Vector3(0, 0, PathSize);
                break;
            case Direction.WEST:
                placement = new Vector3(0, 0, -PathSize);
                break;
            case Direction.NORTH:
                placement = new Vector3(-PathSize, 0, 0);
                break;
            case Direction.SOUTH:
                placement = new Vector3(PathSize, 0, 0);
                break;
        }

        m_CurrentPoint = m_CurrentPoint + placement;
        return m_CurrentPoint;
    }
#endregion


    #region PathChoosing
    Path RandomlyChoosePath(){
        int index = Random.Range(0, m_ListOfPaths.Length);
        return m_ListOfPaths[index];
    }

    Path SetChosenPath(Path NextPath)
    {
        bool randomDir = Random.Range(0, 10) % 2 == 0;
        while (!CheckPathDirectionFit(NextPath))
        {
            //Rotate Until Fit
            RotatePath(NextPath,randomDir);
        }

        //Change Current Direction if needed
        ChangeCurrentDirection(NextPath);

        return NextPath;
    }

    bool CheckPathDirectionFit(Path path)
    {
        bool IsGoodFit = false;
        foreach (Direction dir in path.m_EntryPoints)
        {
            if (dir == GetOppositeDirection())
            {
                IsGoodFit = true;
            }
        }
        return IsGoodFit;
    }


    Direction GetOppositeDirection() {
        int curDir = (int)m_CurrentDirection;
        return (Direction)((curDir + 2) % 4);
    }

    void RotatePath(Path path, bool clockwise){
        //Rotate axis
        float isClockWise = clockwise ? 1 : -1;
        Vector3 rotateCW = new Vector3(0, isClockWise*90, 0);
        path.gameObject.transform.Rotate(rotateCW);

        //Change Direction
        for (int i = 0; i < path.m_EntryPoints.Length; i++){
            Direction dir = path.m_EntryPoints[i];
            path.m_EntryPoints[i] = clockwise ? RotateCW(dir) : RotateACW(dir);
        }
    }

    void ChangeCurrentDirection(Path path){
        foreach(Direction dir in path.m_EntryPoints){
            if (dir != GetOppositeDirection()){
                m_CurrentDirection = dir;
                return;
            }
        }
    }

    Direction RotateCW(Direction dir) {
        int curDir = (int)dir;
        return (Direction)((curDir + 1) % 4);
    }

    Direction RotateACW(Direction dir)
    {
        int curDir = (int)dir;
        curDir = ((curDir - 1) % 4);

        if (curDir < 0)
        {
            curDir = 3;
        }
        return (Direction)curDir;

    }



    #endregion








}
