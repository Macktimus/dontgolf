using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PlayStateManager : MonoBehaviour
{
    public enum PlayStates { Starting = 0, Loading, Play, SpawnNewCourse, Stay, Pause, Summary};
    public PlayStates m_CurrentPlayState = PlayStates.Starting;

    [Header("UI References and Managers")]
    public BallLivesUI BALL_LIVES_UI;
    public GameObject m_GameplayUI;
    public GameObject m_LoadScreenUI;
    public GameObject m_EndGameUI;

    [Header("Stuff")]
    public GameObject BALL_PREFAB;
    public GameObject HOLE_PREFAB;
    public GameObject COIN_PREFAB;
    
    public TextMeshProUGUI m_ObjectiveField;
    

    List<BallControl> m_PlayerBall = new List<BallControl>();
    public CourseManager m_CurrentCourseManager;
    CoursePrefab m_CurrentCourse;

    public TextMeshProUGUI m_PuttCountLabel;
    public Slider m_PowerSlider;
    int m_PuttCounter;
    int m_CourseCounter;
    float m_LoadingScreenCounter = 5f;

    int m_PlayerLives;
    bool m_SafetyActivated = false;

    public UnityEvent PUTT_MADE = new UnityEvent();

    private static PlayStateManager _instance;
    public static PlayStateManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PlayStateManager>();
            }
            return _instance;
        }
    }

    public void StartLevels()
    {
        ResetData();
        m_LoadScreenUI.SetActive(true);
        m_GameplayUI.SetActive(true);
        //spawn course
        SpawnFirstCourse();
        //spawn ball
        SpawnBall();
        //SpawnNextCourse();
    }

    void SpawnBall()
    {
        m_PlayerBall.Add(Instantiate(BALL_PREFAB, m_CurrentCourse.GetBallSpawn().position, Quaternion.identity).GetComponent<BallControl>());
    }

    void ResetData()
    {
        m_PuttCountLabel.text = "0";
        m_PuttCounter = 0;
        m_CourseCounter = 0;
        m_LoadingScreenCounter = 1f;
        ResetObjectiveField();
    }

    void CleanUpScene()
    {
        ResetData();
        m_GameplayUI.SetActive(false);
        if( m_CurrentCourse )
        {
            Destroy(m_CurrentCourse.gameObject);
        }
        if( m_PlayerBall.Count > 0 )
        {
            KillBallSilently();
        }
        
    }

    public bool CheckBallSleeping()
    {
        bool _CheckAllBalls = true;
        foreach( BallControl b in m_PlayerBall )
        {
            _CheckAllBalls = b.CheckBallSleeping();
        }
        return _CheckAllBalls;
    }

    void TogglePlayerBallRigidbody()
    {
        foreach (BallControl b in m_PlayerBall)
        {
            b.ToggleBallRigidbody();
        }
    }

    private void Update()
    {
        if( m_CurrentPlayState == PlayStates.SpawnNewCourse && CheckBallSleeping() )
        {
            Debug.Log("Spawn new course good.");
            SpawnNextCourse();
        }

        if( m_CurrentPlayState == PlayStates.Loading )
        {
            m_LoadingScreenCounter -= Time.deltaTime;
            if( m_LoadingScreenCounter <= 0f )
            {
                m_CurrentPlayState = PlayStates.Play;
                m_LoadScreenUI.SetActive(false);
            }
        }
    }

    public void KillBall (GameObject playerBall)
    {
        m_PlayerBall.Remove(playerBall.GetComponent<BallControl>());
        Destroy(playerBall);
        
        if( m_SafetyActivated )//&& m_PlayerBall.Count <= 0)
        {
            SpawnBall();
        }
        else if(m_PlayerBall.Count <= 0 )
        {
            m_PlayerLives = BALL_LIVES_UI.SubtractLife();            
            if( m_PlayerLives < 0 )
            {
                m_GameplayUI.SetActive(false);
                m_CurrentPlayState = PlayStates.Summary;
                //trigger end state UI
                m_EndGameUI.SetActive(true);
            }
            else
            {
                SpawnBall();
            }
        }
    }

    void KillBallSilently()
    {
        foreach (BallControl b in m_PlayerBall)
        {
            Destroy(b.gameObject);
        }
        m_PlayerBall.RemoveRange(0, m_PlayerBall.Count);
    }

    public void ReturnToMainMenu()
    {
        m_EndGameUI.SetActive(false);
        BALL_LIVES_UI.ClearLives();
        CleanUpScene();
        GameStateManager.Instance.LoadMainMenu();
    }



    public void IncrementPutts(float puttForce)
    {
        PUTT_MADE.Invoke();
        m_PuttCounter++;
        m_PuttCountLabel.text = m_PuttCounter.ToString();
        m_CurrentCourse.IncrementPutts(puttForce);
    }

    public void EnterCourseSpawnState()
    {
        //Debug.LogWarning("Enter course spawn state");
        m_CurrentPlayState = PlayStates.SpawnNewCourse;
        //m_CourseCounter++;
    }

    void SpawnFirstCourse()
    {
        //m_CurrentCourse = Instantiate(COURSE_LIST[0]).GetComponent<CoursePrefab>();
        m_CurrentCourse = Instantiate(m_CurrentCourseManager.GetCourse(m_CourseCounter)).GetComponent<CoursePrefab>();
        m_PlayerLives = m_CurrentCourseManager.GetLives();
        m_CourseCounter++;
        BALL_LIVES_UI.SetupLives(m_PlayerLives);
    }

    public void SpawnNextCourse()
    {   
        if (m_CurrentCourse != null)
        {
            Destroy(m_CurrentCourse.gameObject);
            KillBallSilently();
        }
        //Debug.Log("course counter: " + m_CourseCounter + " course list length: " + COURSE_LIST.Length);
        if( m_CourseCounter <= m_CurrentCourseManager.GetCourseLength() )
        {
            m_CurrentCourse = Instantiate(m_CurrentCourseManager.GetCourse(m_CourseCounter)).GetComponent<CoursePrefab>();
            m_CurrentPlayState = PlayStates.Play;
            //m_PlayerLives = m_CurrentCourse.m_PlayerLives;
            
            if( m_PlayerLives > 0 )
            {
                m_SafetyActivated = false;
                BALL_LIVES_UI.SetupLives(m_PlayerLives);
            }
            else
            {
                m_SafetyActivated = true;
                BALL_LIVES_UI.SetupLives(0);
            }
            SpawnBall();
            //TogglePlayerBallRigidbody();
        }
        else
        {
            ReturnToMainMenu();
        }
        m_CourseCounter++;
    }

    public void SetObjectiveText( string objective )
    {
        m_ObjectiveField.text = objective;
    }

    void ResetObjectiveField()
    {
        m_ObjectiveField.text = "";
    }

    public void CoinAcquired(GameObject coin)
    {
        m_CurrentCourse.RemoveCoin(coin);
    }

    public void BallReturn(GameObject ball)
    {
        ball.transform.position = m_CurrentCourse.m_SafetySpawn.position;
    }

}
