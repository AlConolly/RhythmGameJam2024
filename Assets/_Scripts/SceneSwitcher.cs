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
        SceneManager.LoadScene("Main");
    }
    public void levelSelect()
    {
        LoadLevel("level Select");
    }
    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
/// <summary>
/// switch to level with given name ex. LoadLevel("Level 1");
/// </summary>
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
