using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public enum GameStates { Menu = 0, Paused, Play};
    GameStates m_CurrentState = GameStates.Menu;

    public GameObject m_MainMenuUI;


    private static GameStateManager _instance;
    public static GameStateManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameStateManager>();
            }
            return _instance;
        }
    }
    
    public void LoadMainMenu()
    {
        m_CurrentState = GameStates.Menu;
        m_MainMenuUI.SetActive(true);
        PlayStateManager.Instance.m_CurrentPlayState = PlayStateManager.PlayStates.Starting;
        
    }

    public void LoadLevels()
    {
        m_CurrentState = GameStates.Play;
        PlayStateManager.Instance.m_CurrentPlayState = PlayStateManager.PlayStates.Loading;
        PlayStateManager.Instance.StartLevels();
        m_MainMenuUI.SetActive(false);
    }

}
