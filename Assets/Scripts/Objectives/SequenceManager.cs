using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : MonoBehaviour
{
    public List<GameObject> m_SequenceOfObjects = new List<GameObject>();
    int m_CurrentObjectIndex = 0;

    public Material m_Unhit;
    public Material m_Next;
    public Material m_Hit;

    private void Awake()
    {
        ResetObjects();
    }

    public void ObjectHit ( GameObject go )
    {
        if( go == m_SequenceOfObjects[m_CurrentObjectIndex] )
        {
            go.GetComponent<Renderer>().material = m_Hit;
            if( m_CurrentObjectIndex < m_SequenceOfObjects.Count - 1 )
            {
                m_SequenceOfObjects[m_CurrentObjectIndex + 1].GetComponent<Renderer>().material = m_Next;
            }
            else
            {
                PlayStateManager.Instance.EnterCourseSpawnState();
            }
            m_CurrentObjectIndex++;
        }
        else
        {
            ResetObjects();
        }

        /*else if( go == m_SequenceOfObjects[m_CurrentObjectIndex - 1] )
        {
            //do nothing if it's the last object hit, because we're nice
            //might want to make it so you can hit any previous object
        }*/


        /*if (m_CurrentObjectIndex >= m_SequenceOfObjects.Count )
        {
            PlayStateManager.Instance.EnterCourseSpawnState();
        }*/
    }

    void ResetObjects()
    {
        foreach (GameObject go in m_SequenceOfObjects)
        {
            go.GetComponent<Renderer>().material = m_Unhit;
        }
        m_CurrentObjectIndex = 0;
        m_SequenceOfObjects[m_CurrentObjectIndex].GetComponent<Renderer>().material = m_Next;
    }
}
