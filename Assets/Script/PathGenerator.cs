using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { 
NORTH,
EAST,
SOUTH,
WEST,
}

public class PathGenerator : MonoBehaviour
{
    //Non changing values
    readonly float PathSize = 3f;

    //Start and End
    Path m_StartPath;
    Path m_EndPath;
    Vector3 m_StartPoint;

    //Current Properties
    Vector3 m_CurrentPoint;
    Direction m_CurrentDirection;
	//Height of current path denoted as levels - everything is multiples of 0.44f
    int m_CurrentHeight;

    //Path Objects
    GameObject paths;
    Path[] m_ListOfPaths;
    Path[] m_ListOfRamps;

    public PathGenerator(Path start, Path end, Vector3 startCoordinates, Path[] LoP, Path[] LoR)
    {
        m_StartPath = start;
        m_StartPoint = startCoordinates;
        m_ListOfPaths = LoP;
        m_ListOfRamps = LoR;
        m_EndPath = end;
        m_CurrentPoint = m_StartPoint;
        m_CurrentDirection = m_StartPath.GetComponent<Path>().m_EntryPoints[0];
        m_CurrentHeight = 0;

    }

    void ResetProperties(){
        m_CurrentHeight = 0;
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
			//BUG Next Position issues.
			GameObject nextPathGameObject = InstantiatePath(RandomlyChoosePath(), NextPosition());
			Path nextPath = nextPathGameObject.GetComponent<Path> ();
			Ramp nextRamp = nextPathGameObject.GetComponent<Ramp> ();

			if (nextRamp != null) {
				SetChosenRamp (nextRamp);
				nextRamp = null;
			} else {
				SetChosenPath (nextPath);
			}
        }

        //End Path
        NextPosition();
        GameObject m_NextEndPath = InstantiatePath(m_EndPath, m_CurrentPoint);
        SetEndPath(m_NextEndPath.GetComponent<Path>());
    }
		
    GameObject InstantiatePath(Path path, Vector3 point)
    {
        GameObject pathNew = Instantiate(path.gameObject, point, Quaternion.identity);
        pathNew.GetComponent<Path>().SetPoint(point);
        pathNew.transform.parent = paths.transform;
        return pathNew;
    }

	#region RampPath
	void SetChosenRamp(Ramp ramp) {
		bool doGoHigher = Random.Range(0,10) % 2 == 0;
		while (!CheckRampDirectionFitting (ramp, doGoHigher)) {
			ramp.RotatePath ();
		}

		if (doGoHigher) {
			//change path height
			ramp.ChangeRampHeight ();
		}

		//Change Current Height
		ChangeCurrentHeight(ramp, doGoHigher);
	}

	bool CheckRampDirectionFitting(Ramp ramp, bool GoHigh) {
		if (GoHigh) {
			return RotationManager.instance.PathsMatches (m_CurrentDirection, ramp.Get_LowDirection());
		} else {
			return RotationManager.instance.PathsMatches (m_CurrentDirection, ramp.Get_HighDirection());
		}
	}

	void ChangeCurrentHeight (Ramp ramp, bool goHigher) {
		//Change Current Height
		int UpOrDown = goHigher ? 1 : -1;
		//Update Height
		m_CurrentHeight += (UpOrDown * ramp.Get_Height());
		Debug.Log ("Current Height" + m_CurrentHeight);
		//Update Point
		m_CurrentPoint += new Vector3 (0, m_CurrentHeight * RotationManager.instance.RampHeight, 0);
	}

	#endregion


    #region EndPath
    void SetEndPath(Path end) {
        while (!CheckPathDirectionFit(end)) {
            end.RotatePath(false);
        }
    }
    #endregion

    #region DirectionSpawning
    Vector3 NextPosition(){
		Vector3 placement = Vector3.zero;

        switch(m_CurrentDirection){
            case Direction.EAST:
			placement += new Vector3(0, 0, PathSize);
                break;
            case Direction.WEST:
			placement += new Vector3(0, 0, -PathSize);
                break;
            case Direction.NORTH:
			placement += new Vector3(-PathSize, 0, 0);
                break;
            case Direction.SOUTH:
			placement += new Vector3(PathSize, 0, 0);
                break;
        }

        m_CurrentPoint = m_CurrentPoint + placement;
        return m_CurrentPoint;
    }

	#endregion


    #region PathChoosing
	//TODO - Randomly choose the path Needs changing
    Path RandomlyChoosePath(){
		int ChanceToChooseRamp = Random.Range (0, 10);
		if (ChanceToChooseRamp < 5) {
			int index = Random.Range (0, m_ListOfPaths.Length);
			return m_ListOfPaths [index];
		} else {
			int index = Random.Range(0, m_ListOfRamps.Length);
			return m_ListOfRamps[index];
		}
        
    }

    void SetChosenPath(Path NextPath)
    {
        bool randomDir = Random.Range(0, 10) % 2 == 0;
        while (!CheckPathDirectionFit(NextPath))
        {
            //Rotate Until Fit
            NextPath.RotatePath(randomDir);
        }

        //Change Current Direction if needed
        ChangeCurrentDirection(NextPath);
    }

    bool CheckPathDirectionFit(Path path)
    {
        bool IsGoodFit = false;
        foreach (Direction dir in path.m_EntryPoints)
        {
			if (RotationManager.instance.PathsMatches(dir,m_CurrentDirection))
            {
                IsGoodFit = true;
            }
        }
        return IsGoodFit;
    }

    void ChangeCurrentDirection(Path path){
        foreach(Direction dir in path.m_EntryPoints){
			if(!RotationManager.instance.PathsMatches(dir,m_CurrentDirection)){
                m_CurrentDirection = dir;
                return;
            }
        }
    }


    #endregion








}
