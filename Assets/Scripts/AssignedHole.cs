using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignedHole : MonoBehaviour
{
    public GameObject m_Hole;

    public void ActivateHole()
    {
        m_Hole.SetActive(true);
    }
}
