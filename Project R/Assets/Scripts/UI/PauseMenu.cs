using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public bool isPaused = false;
    public GameObject pauseMenuUI;
    


    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.visible = false;
        foreach (AudioSource audio in FindObjectsOfType<AudioSource>())
        {
            audio.UnPause();
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.visible = true;
        foreach (AudioSource audio in FindObjectsOfType<AudioSource>())
        {
            audio.Pause();
        }
    }

    public void MainMenu()
    {
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            Destroy(o);
        }
        Resume();
        Cursor.visible = true;
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
