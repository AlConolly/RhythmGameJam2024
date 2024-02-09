using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneSwitcher : MonoBehaviour
{ 
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextScene() //used to move to next level
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void backToMenu()
    {
        SceneManager.LoadScene("Mainmenu");
    }
    public void levelSelect()
    {
        LoadLevel("level Select");
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
    /// switch to level with given name ex. LoadLevel("Level 1");
    /// VistasFileja - "lowkey this is for lazy mfs"
    /// </summary>
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
