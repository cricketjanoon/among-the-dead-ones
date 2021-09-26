using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameState
{
    public string Key = null;
    public string Value = null;
}

public class ApplicationManager : MonoBehaviour
{
    // Inspector Assigned
    // This holds any states you wish set at game startup
    [SerializeField]
    private List<GameState> _startingGameStates = new List<GameState>();

    // Used to store the key/values pairs in the above list in a more efficient dictionary for runtime lookup
    private Dictionary<string, string> _gameStateDictionary = new Dictionary<string, string>();

    private static ApplicationManager _instance = null;

    public static ApplicationManager instance
    {
        get
        {
            if (_instance == null)
                _instance = (ApplicationManager)FindObjectOfType<ApplicationManager>();

            return _instance;
        }
    }

    private void Awake()
    {
        //This object lives for the entire application
        DontDestroyOnLoad(gameObject);

        // Copy starting game states into game state dictionary
        ResetGameStates();
    }

    // ----------------------------------------------------------------------------------------------
    // Name	:	GetGameState
    // Desc	:	Returns the value of a game state
    // ----------------------------------------------------------------------------------------------
    public string GetGameState(string key)
    {
        string result = null;
        _gameStateDictionary.TryGetValue(key, out result);
        return result;
    }

    // ----------------------------------------------------------------------------------------------
    // Name	:	SetGameState
    // Desc	:	Sets a Game State
    // ----------------------------------------------------------------------------------------------
    public bool SetGameState(string key, string value)
    {
        if (key == null || value == null) return false;

        _gameStateDictionary[key] = value;

        return true;
    }

    private void ResetGameStates()
    {
        _gameStateDictionary.Clear();

        // Copy starting game states into game state dictionary
        for (int i = 0; i < _startingGameStates.Count; i++)
        {
            GameState gs = _startingGameStates[i];
            _gameStateDictionary[gs.Key] = gs.Value;
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGame()
    {
        ResetGameStates();
        SceneManager.LoadScene("AbandonedHospital");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
    }
}