using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Manager<GameManager>
{
    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED
    }
    // load other persistent systems
    public GameObject[] SystemPrefabs;    // Prefabs we can load through the Editor
    public Events.EventGameState OnGameStateChanged;

    private string _currentSceneName = string.Empty;
    private List<GameObject> _instancedSystemPrefabs;    // Instantiated Prefabs
    private List<AsyncOperation> _loadOperations;
    private GameState _currentGameState = GameState.PREGAME;

    public GameState CurrentGameState
    {
        get => _currentGameState;
        private set => _currentGameState = value;
    }

    private void Start()
    {
        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();

        InstantiateSystemPrefabs();
    }

    public void LoadScene(string sceneName)
    {
        // Replace current scene with a new scene, saving a reference to the async operation
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        if (ao == null)
        {
            Debug.LogErrorFormat("[Game Manager] Unable to load scene {0}", sceneName);
            return;
        }
        // Add a listener to the operation completed event
        ao.completed += OnLoadOperationComplete;
        // Add to our List of load operations
        _loadOperations.Add(ao);
        // Update current scene name
        _currentSceneName = sceneName;
    }

    private void OnLoadOperationComplete(AsyncOperation ao)
    {
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);

            if (_loadOperations.Count == 0)
            {
                UpdateState(GameState.RUNNING);
            }
        }
        Debug.Log("Load Complete.");
    }

    private void UpdateState(GameState state)
    {
        GameState previousGameState = _currentGameState;
        _currentGameState = state;

        switch (_currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1.0f;
                break;
            case GameState.RUNNING:
                Time.timeScale = 1.0f;
                break;
            case GameState.PAUSED:
                Time.timeScale = 0.0f;
                break;
            default:
                break;
        }
        // dispatch messages
        OnGameStateChanged.Invoke(_currentGameState, previousGameState);
    }

    private void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;

        for (int i = 0; i < SystemPrefabs.Length; ++i)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    protected void OnDestroy()
    {
        if (_instancedSystemPrefabs == null)
        {
            return;
        }
        for (int i = 0; i < _instancedSystemPrefabs.Count; ++i)
        {
            Destroy(_instancedSystemPrefabs[i]);
        }
        _instancedSystemPrefabs.Clear();
    }

    public void StartGame()
    {
        LoadScene("Main");
    }

    public void TogglePause()
    {
        UpdateState(_currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
    }

    public void RestartGame()
    {
        UpdateState(GameState.PREGAME);
    }

    public void QuitGame()
    {
        // implement features for quitting (autosave, etc)
        Application.Quit();
    }
}
