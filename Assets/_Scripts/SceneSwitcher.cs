using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneSwitcher : MonoBehaviour
{ 
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextScene() // Used to move to next level
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OptionsMenu()
    {
        LoadLevel("Options");
    }

    public void BackToMenu()
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
    }
}
