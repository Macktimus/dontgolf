using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoursePrefab : MonoBehaviour
{
    public List<Transform> m_HoleSpawnPositions = new List<Transform>();
    public List<Transform> m_CoinSpawns = new List<Transform>();
    public int m_PuttsToNextCourse;
    public int m_PuttsBetweenHoleSpawns;
    public int m_PuttsBetweenHoleScales;
    public float m_ScaleFactor;
    

    List<GameObject> m_Holes = new List<GameObject>();
    List<GameObject> m_Coins = new List<GameObject>();
    int m_PuttCounter;

    public Transform m_CameraPosition;
    public Transform m_SafetySpawn;
    public Transform m_BallSpawn;
    float m_CameraPositionSpeed = 0.5f;
    Transform m_StartPos, m_EndPos;
    float m_Fraction;

    public enum LevelTypes { Practice = 0, Coins, Sequence};
    public LevelTypes m_LevelType = LevelTypes.Practice;

    bool m_SpawnHole = false;

    private void Awake()
    {
        if( m_LevelType == LevelTypes.Practice )
        {
            PlayStateManager.Instance.SetObjectiveText("Warm Up Time: " + m_PuttsToNextCourse);
        }
        else if( m_LevelType == LevelTypes.Coins )
        {
            SpawnCoins();
            m_SpawnHole = true;
            PlayStateManager.Instance.SetObjectiveText("Collect " + m_Coins.Count + " Coins");
        }
        else
        {
            m_SpawnHole = true;
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

        if( m_SpawnHole && PlayStateManager.Instance.CheckBallSleeping() )
        {
            m_SpawnHole = false;
            SpawnHole();
        }
    }

    void SpawnHole()
    {
        int SpawnLocation = Random.Range(1, m_HoleSpawnPositions.Count)-1;
        GameObject _hole = m_HoleSpawnPositions[SpawnLocation].gameObject; //Instantiate(PlayStateManager.Instance.HOLE_PREFAB, m_HoleSpawnPositions[SpawnLocation]);
        m_Holes.Add(_hole);
        _hole.GetComponent<AssignedHole>().ActivateHole();
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
                    //_t.localScale += new Vector3(_HoleScalar, 0, _HoleScalar);
                }
            }

            if (m_PuttsBetweenHoleSpawns > 0)
            {
                if ((m_PuttCounter % m_PuttsBetweenHoleSpawns) == 0)
                {
                    m_SpawnHole = true;
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
        else if (m_LevelType == LevelTypes.Coins)
        {
            PlayStateManager.Instance.SetObjectiveText("Collect " + m_Coins.Count + " Coins");
        }
    }

    public void RemoveCoin(GameObject coin)
    {
        m_Coins.Remove(coin);
        Destroy(coin);
        PlayStateManager.Instance.SetObjectiveText("Collect " + m_Coins.Count + " Coins");
        if (m_Coins.Count <= 0)
        {
            //LEVEL COMPLETE!
            PlayStateManager.Instance.EnterCourseSpawnState();
        }
    }

    void SpawnCoins()
    {
        for( int x = 0; x < m_CoinSpawns.Count; x++ )
        {
            m_Coins.Add(Instantiate(PlayStateManager.Instance.COIN_PREFAB, m_CoinSpawns[x]));
        }
    }

    public Transform GetBallSpawn()
    {
        return m_BallSpawn;
    }

}
