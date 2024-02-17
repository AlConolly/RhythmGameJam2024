using System;
using UnityEngine;
using RhythmEngine;
using TMPro;
/// <summary>
/// Nice, easy to understand enum-based game manager. For larger and more complex games, look into
/// state machines. But this will serve just fine for most games.
/// </summary>
public class ExampleGameManager : StaticInstance<ExampleGameManager>
{
    public static int score = 0;
    public static int missed = 0;
    public static float health = 100;
    public float damageOnMiss = 10;
    public static event Action<GameState> OnBeforeStateChanged = delegate { };
    public static event Action<GameState> OnAfterStateChanged = delegate { };
    private GameObject pauseMenu;
    private RhythmEngineCore rhythmEngine;
    public GameObject LoseScreen;
    public GameObject WinScreen;
    public TextMeshProUGUI scoreText;
    [HideInInspector] public double songLength;
    [HideInInspector] public double songTime;
    [HideInInspector] public double songProgress;
    public GameState State { get; private set; }

    // Kick the game off with the first state
    void Start()
    {
        rhythmEngine = GameObject.FindGameObjectWithTag("RhythmEngine").GetComponent<RhythmEngineCore>();
        songLength =  rhythmEngine.MusicSource.clip.length;
        ChangeState(GameState.Starting);
    }
    private void OnEnable()
    {
        OnBeforeStateChanged += unPause;
        NoteManager.OnMiss += reduceHealth;
        pauseMenu = GameObject.FindGameObjectWithTag("pauseMenu");
        pauseMenu?.SetActive(false);
    }
    private void OnDisable()
    {
        NoteManager.OnMiss -= reduceHealth;
        OnBeforeStateChanged -= unPause;
    }

    private void Update()
    {
        songTime = rhythmEngine.GetCurrentAudioTime();
        songProgress = songTime / songLength;
        print("songProgress: "+songProgress);
        print("songTime: " + songTime);
        print("songLength: " + songLength);
        checkEndSong();
        checkPauseGame();
        if(health<0)
            ChangeState(GameState.Lose);
    }

    public void ChangeState(GameState newState)
    {
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState)
        {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.Playing:
                break;
            case GameState.Win:
                WinScreen.SetActive(true);
                scoreText.text = "Hit: " + score + "\nMissed: " + missed;
                Time.timeScale = 0; // Sets the movement of time to 0 in the game
                rhythmEngine.Pause();
                break;
            case GameState.Lose:
                print("Lost");
                LoseScreen.SetActive(true);
                Time.timeScale = 0; // Sets the movement of time to 0 in the game
                rhythmEngine.Pause();
                break;
            case GameState.Paused:
                pauseMenu?.SetActive(true);
                Time.timeScale = 0; // Sets the movement of time to 0 in the game
                rhythmEngine.Pause();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);

        Debug.Log($"New state: {newState}");
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    public void ResumeGame()
    {
        ChangeState(GameState.Playing);
    }

    private void HandleStarting()
    {
        // Do some start setup, could be environment, cinematics etc

        // Eventually call ChangeState again with your next state
        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);
        health = 100;
        score = 0;


        ChangeState(GameState.Playing);
    }

    private void checkPauseGame()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        Debug.Log(State);

        if (State != GameState.Paused) ChangeState(GameState.Paused);
        else unPause(GameState.Playing);
    }
    private void unPause(GameState gs) // Why are we taking an input??
    {
        if (gs == GameState.Paused) return; // Theoretically this can be removed

        State = GameState.Playing;
        Time.timeScale = 1;
        pauseMenu?.SetActive(false);
        rhythmEngine.Unpause();
    }
    private void checkEndSong()
    {
        if(songTime>songLength)
        {
            ChangeState(GameState.Win);
        }
    }
    private void reduceHealth()
    {
        health -= damageOnMiss;
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