using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleKill : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if( other.tag == "PlayerBall" )
        {
            PlayStateManager.Instance.KillBall(other.gameObject);
        }
    }
}
