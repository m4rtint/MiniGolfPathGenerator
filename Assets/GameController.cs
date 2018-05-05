using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    PathGenerator m_PathGenerator;

    [Header("Path Objects")]
    [SerializeField]
    GameObject m_StartPathObject;
    [SerializeField]
    GameObject m_EndPathObject;
    [SerializeField]
    GameObject[] m_ListOfPathObjects;
    [SerializeField]
    Vector3 m_StartingPoint;
    [SerializeField]
    int m_MaxPathSize;
    //Path
    Path m_StartPath;
    Path m_EndPath;
    Path[] m_ListOfPaths;
    //Result
    Path[] m_ResultingPath;

    void Awake()
    {
        m_ListOfPaths = SetupPathArray();
        m_StartPath = m_StartPathObject.GetComponent<Path>();
        m_EndPath = m_EndPathObject.GetComponent<Path>();
    }

    Path[] SetupPathArray()
    {
        Path[] paths = new Path[m_ListOfPathObjects.Length];
        for (int i = 0; i < m_ListOfPathObjects.Length; i++)
        {
            paths[i] = m_ListOfPathObjects[i].GetComponent<Path>();
        }
        return paths;
    }

    // Use this for initialization
    void Start()
    {
        m_PathGenerator = new PathGenerator(m_StartPath, m_EndPath, m_StartingPoint, m_ListOfPaths);
        m_PathGenerator.GeneratePath(m_MaxPathSize);
    }

    #region Reset Path
    public void RecreatePath()
    {
        DestroyPath();
        m_PathGenerator.GeneratePath(m_MaxPathSize);
    }

    void DestroyPath()
    {
        Destroy(GameObject.Find("Path"));
    }

    #endregion



}
