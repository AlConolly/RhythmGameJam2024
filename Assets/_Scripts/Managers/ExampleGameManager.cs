using System;
using UnityEngine;
using RhythmEngine;
using TMPro;
using UnityEngine.SceneManagement;
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
    private RhythmEngineCore rhythmEngine;
    public SongStarter songStarter;
    public GameObject pauseMenu;
    public GameObject LoseScreen;
    public GameObject WinScreen;
    public GameObject TutorialScreen;
    public TextMeshProUGUI scoreText;
    public ScoreStorer ScoreStorer;
    public string levelName;
    [HideInInspector] public double songLength;
    [HideInInspector] public double songTime;
    [HideInInspector] public double songProgress;
    public GameState State { get; private set; }

    // Kick the game off with the first state
    void Start()
    {
        //ScoreStorer = GameObject.Find("ScoreStorer").GetComponent<ScoreStorer>();
        rhythmEngine = GameObject.FindGameObjectWithTag("RhythmEngine").GetComponent<RhythmEngineCore>();
        songStarter = rhythmEngine.GetComponent<SongStarter>();
        songLength = rhythmEngine.MusicSource.clip.length;
        ChangeState(GameState.InTutorial);
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
        if (State != GameState.InTutorial)
        {
            songTime = rhythmEngine.GetCurrentAudioTime();
            songProgress = songTime / songLength;
        }
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
            case GameState.InTutorial:
                TutorialScreen.SetActive(true);
                Time.timeScale = 0; // Sets the movement of time to 0 in the game
                rhythmEngine.Pause();
                break;
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
                onWin();
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
        SceneSwitcher.optsWithPause = false;
    }
    public void ExitTutorial()
    {
        ChangeState(GameState.Starting);
    }
    public void onWin()
    {
        int percentScore = (score / (score + missed))*100;
        //ScoreStorer.SaveScore(levelName, percentScore);
    }

    private void HandleStarting()
    {
        // Do some start setup, could be environment, cinematics etc

        // Eventually call ChangeState again with your next state
        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);
        TutorialScreen.SetActive(false);
        health = 100;
        score = 0;
        missed = 0;

        songStarter.StartSong();

        ChangeState(GameState.Playing);
    }

    private void checkPauseGame()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        Debug.Log(State);


        if (SceneSwitcher.optsWithPause)
        {
            SceneSwitcher.optsWithPause = false;
            SceneManager.UnloadSceneAsync("Options");
            return;
        }

        if (State != GameState.Paused) ChangeState(GameState.Paused);
        else unPause(GameState.Playing);
    }
    private void unPause(GameState gs) // Why are we taking an input??
    {
        if (gs == GameState.Paused || gs == GameState.Starting || gs == GameState.InTutorial) return; // Theoretically this can be removed

        

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
    InTutorial = 5
}