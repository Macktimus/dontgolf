using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseManager : MonoBehaviour
{
    public GameObject[] COURSE_LIST;
    public int m_PlayerLives = 0;

    //int m_CurrentCourseIndex = 0;
    // Start is called before the first frame update
    

    public CoursePrefab GetCourse(int index)
    {
        Debug.LogWarning("Get Course Index " + index);
        Debug.LogWarning("Get Course Name " + COURSE_LIST[index].name);
        return COURSE_LIST[index].GetComponent<CoursePrefab>();
    }

    public int GetCourseLength()
    {
        Debug.LogWarning("Get Course Length " + COURSE_LIST.Length);
        return COURSE_LIST.Length;
    }

    public int GetLives()
    {
        return m_PlayerLives;
    }
}
