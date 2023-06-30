using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLauncher : MonoBehaviour
{
    public GameObject LevelCompletePanel;

    //Change the scenes
    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
        if (name == "Start")
            FindObjectOfType<GameManager>().ResetGame();
        Resume();

    }


    //Exit from the game
    public void Exit()
    {
        Debug.Log("++Play Time: " + Time.time);
        Application.Quit();

    }

    //Continue the level after display the pannel with options
    public void ContinueLevel()
    {
        LevelCompletePanel.SetActive(false);
        Resume();
        FindObjectOfType<GameManager>().Restart();
    }

    public void DisplayLevelCompletion()
    {
        LevelCompletePanel.SetActive(true);
        Pause();
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Exit();
        }
    }

}
