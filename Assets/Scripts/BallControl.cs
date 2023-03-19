using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BallControl : MonoBehaviour
{
    public float m_MaxPower;
    public float m_AngleSpeedModifier;
    public float m_LineLength;
    

    private Rigidbody m_BallRB;
    private float m_Angle;
    private LineRenderer m_AimLine;
    float m_PowerUpTime;
    float m_Power;
    

    private void Awake()
    {
        m_BallRB = GetComponent<Rigidbody>();
        m_BallRB.maxAngularVelocity = 1000;
        m_AimLine = GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if( PlayStateManager.Instance.m_CurrentPlayState == PlayStateManager.PlayStates.Play )
        if ( m_BallRB.IsSleeping() )//&& PlayStateManager.Instance.m_CurrentPlayState == PlayStateManager.PlayStates.Play)
        {
            m_AimLine.enabled = true;
            if (Input.GetKey(KeyCode.A))
            {
                m_Angle -= Time.deltaTime * m_AngleSpeedModifier;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                m_Angle += Time.deltaTime * m_AngleSpeedModifier;
            }

            UpdateAimLine();

            if (Input.GetKeyUp(KeyCode.Space))
            {
                Putt();
                m_PowerUpTime = 0f;
                m_Power = 0f;
                PlayStateManager.Instance.m_PowerSlider.value = 0f;
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                PowerUp();
            }
        }
        else
        {
            m_AimLine.enabled = false;
        }
        
    }

    void UpdateAimLine()
    {
        m_AimLine.SetPosition(0, transform.position);
        m_AimLine.SetPosition(1, transform.position + (Quaternion.Euler(0, m_Angle, 0) * Vector3.forward * m_LineLength));
    }

    void Putt()
    {
        float _PuttForce = m_MaxPower * m_Power;
        m_BallRB.AddForce(Quaternion.Euler(0, m_Angle, 0) * Vector3.forward * _PuttForce, ForceMode.Impulse);
        PlayStateManager.Instance.IncrementPutts(_PuttForce);
    }


    void PowerUp()
    {
        m_PowerUpTime += Time.deltaTime;
        m_Power = Mathf.PingPong(m_PowerUpTime, 1);
        PlayStateManager.Instance.m_PowerSlider.value = m_Power;
    }

    public void PauseBall()
    {
        m_BallRB.Sleep();
    }

    public bool CheckBallSleeping()
    {
        return m_BallRB.IsSleeping();
    }

    public void ToggleBallRigidbody()
    {
        if (!m_BallRB.IsSleeping())
        {
            m_BallRB.Sleep();
        }
        else
        {
            m_BallRB.WakeUp();
        }
    }
}
