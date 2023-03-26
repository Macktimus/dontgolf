using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceObject : MonoBehaviour
{
    public SequenceManager m_SequenceManager;

    private void OnCollisionEnter(Collision collision)
    {
        if( collision.gameObject.tag == "PlayerBall" )
        {
            m_SequenceManager.ObjectHit(gameObject);
        }
    }
}
