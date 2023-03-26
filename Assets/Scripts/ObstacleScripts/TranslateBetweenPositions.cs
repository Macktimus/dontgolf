using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateBetweenPositions : MonoBehaviour
{
    public enum MovementInterval { Constant = 0, Stroke};
    public MovementInterval m_MovementType = MovementInterval.Constant;

    Vector3 m_StartPoint;
    public Vector3 m_EndPoint;
    Vector3 m_DifferenceVector;

    public float m_MoveSpeed = 1f;

    public float m_TimeToMove = 1f;

    // Start is called before the first frame update
    void Start()
    {
        m_DifferenceVector = m_EndPoint - m_StartPoint;
    }

    // Update is called once per frame
    void Update()
    {
        if( m_MovementType == MovementInterval.Constant )
        {
            /*m_PowerUpTime += Time.deltaTime;
        m_Power = Mathf.PingPong(m_PowerUpTime, 1);
        PlayStateManager.Instance.m_PowerSlider.value = m_Power;*/
            
        }
    }
}
