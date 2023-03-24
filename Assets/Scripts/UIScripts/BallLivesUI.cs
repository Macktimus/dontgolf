using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLivesUI : MonoBehaviour
{
    public List<GameObject> m_Lives = new List<GameObject>();

    int m_LivesRemaining = 0;

    private void Awake()
    {
        ClearLives();
    }

    public void SetupLives(int lives)
    {
        m_LivesRemaining = lives;
        UpdateLives(lives);
    }

    void UpdateLives(int lives)
    {
        ClearLives();
        for (int x = 0; x<lives; x++)
        {
            m_Lives[x].SetActive(true);
        }
    }

    public void ClearLives()
    {
        for (int x = 0; x < m_Lives.Count; x++)
        {
            m_Lives[x].SetActive(false);
        }
    }

    public int SubtractLife()
    {
        m_LivesRemaining--;
        UpdateLives(m_LivesRemaining);
        return m_LivesRemaining;
    }
}
