using System;
using UnityEngine;


/// <summary>
/// Nice, easy to understand enum-based game manager. For larger and more complex games, look into
/// state machines. But this will serve just fine for most games.
/// </summary>
public class ExampleGameManager : StaticInstance<ExampleGameManager> {
    public static event Action<GameState> OnBeforeStateChanged = delegate { };
    public static event Action<GameState> OnAfterStateChanged = delegate { };
    private GameObject pauseMenu;

    public GameState State { get; private set; }

    // Kick the game off with the first state
    void Start()
    {
        ChangeState(GameState.Starting);
        
    }
    private void OnEnable()
    {
        OnBeforeStateChanged += unPause;
        pauseMenu = GameObject.FindGameObjectWithTag("pauseMenu");
        pauseMenu?.SetActive(false);
    }
    private void OnDisable()
    {
        OnBeforeStateChanged -= unPause;
    }

    private void Update()
    {
        checkPauseGame();
    }

    public void ChangeState(GameState newState) {
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState) {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.Playing:
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
            case GameState.Paused:
                pauseMenu?.SetActive(true);
                Time.timeScale = 0; //sets the movement of time to 0 in the game
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);
        
        Debug.Log($"New state: {newState}");
    }

    public void ChangeState(string newState)
    {
        switch(newState)
        {
            case "Starting":
                ChangeState(GameState.Starting);
                break;
            case "Playing":
                ChangeState(GameState.Playing);
                break;
            case "Win":
                ChangeState(GameState.Win);
                break;
            case "Lose":
                ChangeState(GameState.Lose);
                break;
            case "Paused":
                ChangeState(GameState.Paused);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    private void HandleStarting() {
        // Do some start setup, could be environment, cinematics etc

        // Eventually call ChangeState again with your next state
        
        ChangeState(GameState.Playing);
    }
    private void checkPauseGame()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) //if you didn't press escape do nothing
        { return; }

        if (State != GameState.Paused)
            ChangeState(GameState.Paused);
        else if (State == GameState.Paused)
            ChangeState(GameState.Playing);
    }
    private void unPause(GameState gameState)
    {
        if (gameState != GameState.Paused)
        {
            Time.timeScale = 1;
            pauseMenu?.SetActive(false);
        }
    }

}

[Serializable]
public enum GameState {
    Starting = 0,
    Playing = 1,
    Win = 2,
    Lose = 3,
    Paused = 4,
}