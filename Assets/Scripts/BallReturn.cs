using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReturn : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "PlayerBall")
        {
            PlayStateManager.Instance.BallReturn();
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
