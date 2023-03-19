using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingBall : MonoBehaviour
{
    void Update()
    {
        gameObject.transform.Rotate(Vector3.forward, -25 * Time.deltaTime);
    }
}
