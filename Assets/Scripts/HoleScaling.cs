using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleScaling : MonoBehaviour
{
    public float m_MaxScale = 1.5f;
    public float m_MinScale = 1f;
    public float m_ScaleSpeed = 0.5f;

    float m_CurrentScale = 1f;
    float m_ScalePong = 0f;

    // Update is called once per frame
    void Update()
    {
        m_ScalePong += Time.deltaTime * m_ScaleSpeed;
        m_CurrentScale = Mathf.PingPong(m_ScalePong, m_MaxScale);
        Vector3 _baseVector = new Vector3(m_MinScale+m_CurrentScale, gameObject.transform.localScale.y, m_MinScale+m_CurrentScale);
        gameObject.transform.localScale = _baseVector;
    }
}
