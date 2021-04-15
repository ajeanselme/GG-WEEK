using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    static bool _isPaused = false;
    public GameObject pauseMenu;
    public string levelName;

    public void pauseGame()
    {
        if (_isPaused)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            _isPaused = false;
        }
        else
        {
            _isPaused = true;

            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void loadScene()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        _isPaused = false;

        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
