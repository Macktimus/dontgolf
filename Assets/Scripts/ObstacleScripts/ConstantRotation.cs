using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    public float m_Speed = 1f;
    public Vector3 m_PivotAxis = Vector3.up;

    void Update()
    {
        gameObject.transform.Rotate(m_PivotAxis, m_Speed * Time.deltaTime);
    }
}
