using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReturn : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "PlayerBall")
        {
            PlayStateManager.Instance.KillBall(collision.gameObject);
            //PlayStateManager.Instance.BallReturn(collision.gameObject);
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
