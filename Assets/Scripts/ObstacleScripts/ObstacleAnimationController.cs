using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAnimationController : MonoBehaviour
{
    public enum MovementTrigger { Constantly = 0, PuttBased};
    public MovementTrigger m_MovementTrigger = MovementTrigger.PuttBased;

    public bool m_Fallen = false;
    bool m_ConstantAnimation = false;

    

    private void Awake()
    {
        if( m_MovementTrigger == MovementTrigger.PuttBased )
        {
            PlayStateManager.Instance.PUTT_MADE.AddListener(doSomething);
            gameObject.GetComponent<Animator>().SetBool("Fallen", m_Fallen);
        }
        else
        {
            m_ConstantAnimation = true;
            gameObject.GetComponent<Animator>().SetBool("ConstantAnimation", m_ConstantAnimation);
        }
    }

    void doSomething()
    {
        m_Fallen = !m_Fallen;
        gameObject.GetComponent<Animator>().SetBool("Fallen", m_Fallen);
    }
}
