using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoursePrefab : MonoBehaviour
{
    public List<Transform> m_HoleSpawnPositions = new List<Transform>();
    public int m_PuttsToNextCourse;
    public int m_PuttsBetweenHoleSpawns;
    public int m_PuttsBetweenHoleScales;
    public float m_ScaleFactor;
    

    List<GameObject> m_Holes = new List<GameObject>();
    int m_PuttCounter;

    public Transform m_CameraPosition;
    float m_CameraPositionSpeed = 0.5f;
    Transform m_StartPos, m_EndPos;
    float m_Fraction;

    public enum LevelTypes { Practice = 0, Coins, Sequence};
    public LevelTypes m_LevelType = LevelTypes.Practice;

    private void Awake()
    {
        if( m_LevelType == LevelTypes.Practice )
        {
            PlayStateManager.Instance.SetObjectiveText("Warm Up Time: " + m_PuttsToNextCourse);
        }
        else
        {
            SpawnHole();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_StartPos = Camera.main.transform;
        m_EndPos = m_CameraPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if( m_Fraction < 1)
        {
            m_Fraction += Time.deltaTime;
            Camera.main.transform.position = Vector3.Lerp(m_StartPos.position, m_EndPos.position, m_Fraction);
            Camera.main.transform.rotation = Quaternion.Lerp(m_StartPos.rotation, m_EndPos.rotation, m_Fraction);
        }
    }

    void SpawnHole()
    {
        int SpawnLocation = Random.Range(1, m_HoleSpawnPositions.Count)-1;
        GameObject _hole = Instantiate(PlayStateManager.Instance.HOLE_PREFAB, m_HoleSpawnPositions[SpawnLocation]);
        m_Holes.Add(_hole);
        m_HoleSpawnPositions.Remove(m_HoleSpawnPositions[SpawnLocation]);
    }

    public void IncrementPutts(float puttForce)
    {
        m_PuttCounter++;
        /*if( m_PuttCounter >= m_PuttsToNextCourse )
        {
            PlayStateManager.Instance.EnterCourseSpawnState();
        }*/
        if( m_LevelType != LevelTypes.Practice )
        {
            if ((m_PuttCounter % m_PuttsBetweenHoleScales) == 0)
            {
                float _HoleScalar = m_ScaleFactor * (1 - (puttForce / 3));
                foreach (GameObject hole in m_Holes)
                {
                    Transform _t = hole.transform;
                    _t.localScale += new Vector3(_HoleScalar, 0, _HoleScalar);
                }
            }

            if (m_PuttsBetweenHoleSpawns > 0)
            {
                if ((m_PuttCounter % m_PuttsBetweenHoleSpawns) == 0)
                {
                    SpawnHole();
                }
            }
        }

        if (m_LevelType == LevelTypes.Practice)
        {
            m_PuttsToNextCourse--;
            if (m_PuttsToNextCourse <= 0)
            {
                //LEVEL COMPLETE!
                PlayStateManager.Instance.EnterCourseSpawnState();
            }
            else
            {
                PlayStateManager.Instance.SetObjectiveText("Warm Up Time: " + m_PuttsToNextCourse);
            }
        }
    }
}
