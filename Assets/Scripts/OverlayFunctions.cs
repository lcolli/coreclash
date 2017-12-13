using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OverlayFunctions : MonoBehaviour
{
    public Transform startMenu;
    public Transform pauseMenu;
    public Transform gameOverMenu;
    public Transform helpMenu;
    private CoreClash game;
    public Image thisImage;


    private void Start()
    {
        GameObject mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        game = mainCam.GetComponent<CoreClash>();
    }


    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && game.State != gamestate.menu)
        {

            if (pauseMenu.gameObject.activeInHierarchy == false)
            {
                pauseMenu.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                pauseMenu.gameObject.SetActive(false);
                Time.timeScale = 1;
            }

        }

        if (game.State == gamestate.victory)
        {
            gameOverMenu.gameObject.SetActive(true);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResumeGame()
    {
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void GoToMainMenu(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DisplayMenu(bool isShown)
    {
        startMenu.gameObject.SetActive(isShown);
    }

    public void DisplayPause(bool isShown)
    {
        pauseMenu.gameObject.SetActive(isShown);
    }

    public void DisplayVictory(bool isShown, Sprite winner)
    {
        gameOverMenu.gameObject.SetActive(isShown);
    }

    public void DisplayHelp(bool isShown){
        helpMenu.gameObject.SetActive(isShown);
    }
}