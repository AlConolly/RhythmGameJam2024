using System;
using UnityEngine;


namespace RhythmEngine
{

    /// <summary>
    /// Nice, easy to understand enum-based game manager. For larger and more complex games, look into
    /// state machines. But this will serve just fine for most games.
    /// </summary>
    public class ExampleGameManager : StaticInstance<ExampleGameManager>
    {
        public static int score = 0;
        public static float health = 100;
        public static event Action<GameState> OnBeforeStateChanged = delegate { };
        public static event Action<GameState> OnAfterStateChanged = delegate { };
        private GameObject pauseMenu;
        private RhythmEngineCore rhythmEngine;
        public GameState State { get; private set; }

        // Kick the game off with the first state
        void Start()
        {
            rhythmEngine = GameObject.FindGameObjectWithTag("RhythmEngine").GetComponent<RhythmEngineCore>();
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
                    onWin();
                    break;
                case GameState.Lose:
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

        public void onWin()
        {
            
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