using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayStateManager : MonoBehaviour
{
    public enum PlayStates { Starting = 0, Loading, Play, SpawnNewCourse, Stay, Pause, Summary};
    public PlayStates m_CurrentPlayState = PlayStates.Starting;

    public GameObject BALL_PREFAB;
    public GameObject HOLE_PREFAB;
    public GameObject COIN_PREFAB;
    public GameObject[] COURSE_LIST;
    public GameObject m_GameplayUI;
    public GameObject m_LoadScreenUI;
    public TextMeshProUGUI m_ObjectiveField;
    public GameObject m_EndGameUI;

    BallControl m_PlayerBall;
    CoursePrefab m_CurrentCourse;

    public TextMeshProUGUI m_PuttCountLabel;
    public Slider m_PowerSlider;
    int m_PuttCounter;
    int m_CourseCounter;
    float m_LoadingScreenCounter = 5f;

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
        m_PlayerBall = Instantiate(BALL_PREFAB, m_CurrentCourse.GetBallSpawn().position, Quaternion.identity).GetComponent<BallControl>();
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
        if( m_PlayerBall )
        {
            Destroy(m_PlayerBall);
        }
        
    }

    public bool CheckBallSleeping()
    {
        return m_PlayerBall.CheckBallSleeping();
    }

    private void Update()
    {
        if( m_CurrentPlayState == PlayStates.SpawnNewCourse && CheckBallSleeping() )
        {
            m_PlayerBall.ToggleBallRigidbody();
            SpawnNextCourse();
            m_PlayerBall.ToggleBallRigidbody();
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
        Destroy(playerBall);
        m_GameplayUI.SetActive(false);
        m_CurrentPlayState = PlayStates.Summary;
        //trigger end state UI
        m_EndGameUI.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        m_EndGameUI.SetActive(false);
        CleanUpScene();
        GameStateManager.Instance.LoadMainMenu();
    }



    public void IncrementPutts(float puttForce)
    {
        m_PuttCounter++;
        m_PuttCountLabel.text = m_PuttCounter.ToString();
        m_CurrentCourse.IncrementPutts(puttForce);
    }

    public void EnterCourseSpawnState()
    {
        m_CurrentPlayState = PlayStates.SpawnNewCourse;
        //m_CourseCounter++;
    }

    void SpawnFirstCourse()
    {
        m_CurrentCourse = Instantiate(COURSE_LIST[0]).GetComponent<CoursePrefab>();
    }

    public void SpawnNextCourse()
    {
        m_CourseCounter++;
        if (m_CurrentCourse != null)
        {
            Destroy(m_CurrentCourse.gameObject);
            Destroy(m_PlayerBall.gameObject);
        }
        Debug.Log("course counter: " + m_CourseCounter + " course list length: " + COURSE_LIST.Length);
        if( m_CourseCounter < COURSE_LIST.Length )
        {
            m_CurrentCourse = Instantiate(COURSE_LIST[m_CourseCounter]).GetComponent<CoursePrefab>();
            m_CurrentPlayState = PlayStates.Play;
            m_PlayerBall = Instantiate(BALL_PREFAB, m_CurrentCourse.GetBallSpawn().position, BALL_PREFAB.transform.rotation).GetComponent<BallControl>();
            m_PlayerBall.ToggleBallRigidbody();
        }
        else
        {
            ReturnToMainMenu();
        }
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

    public void BallReturn()
    {
        m_PlayerBall.gameObject.transform.position = m_CurrentCourse.m_SafetySpawn.position;
    }

}
