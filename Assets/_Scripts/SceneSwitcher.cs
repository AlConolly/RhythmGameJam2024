using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneSwitcher : MonoBehaviour
{
    public static bool optsWithPause = false;
    public AudioClip acceptClip;
    public AudioClip declineClip;
    private AudioSystem au;
    void Start()
    {
        au = GameObject.Find("Audio System").GetComponent<AudioSystem>();
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextScene() // Used to move to next level
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OptionsMenu(string whoRanThis)
    {
        if (whoRanThis == "pausemenu")
        {
            optsWithPause = true;
        }
        SceneManager.LoadScene("Options", LoadSceneMode.Additive);
        
    }

    /// <summary>
    /// Used in the options menu
    /// </summary>
    public void GoBACK()
    {
        if (optsWithPause)
        {
            // The options menu was opened via the pause menu
            optsWithPause = false;
        }
        SceneManager.UnloadSceneAsync("Options");
    }

    public void ReturnToMenu()
    {
        LoadLevel("Mainmenu");
    }
    
    /// <summary>
    /// Suprisingly this function closes the application.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
    /// <summary>
    /// switch to a scene with given name ex. LoadLevel("Level 1");
    /// </summary>
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
        optsWithPause = false;
    }

    public void PlayAcceptSound()
    {
        au.PlaySound(acceptClip);
    }
    public void PlayDeclineSound()
    {
        au.PlaySound(declineClip);
    }
}
