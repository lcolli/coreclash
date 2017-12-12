using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public Transform pauseMenu;
    public Transform gameOverMenu;
    private GameObject cam;
    private CoreClash mainScript;

    void Start(){
        pauseMenu.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(false);
        cam = GameObject.Find("Main Camera");
        mainScript = cam.GetComponent<CoreClash>();
    }

    public void StartGame(string newGame){
        SceneManager.LoadScene(newGame);
    }

    public void ExitGame(){
        Application.Quit();
    }

    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResumeGame(){
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void GoMainMenu(string menu){
        SceneManager.LoadScene(menu);
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)){

            if (pauseMenu.gameObject.activeInHierarchy == false)
            {
                pauseMenu.gameObject.SetActive(true);
                Time.timeScale = 0;
            } else {
                pauseMenu.gameObject.SetActive(false);
                Time.timeScale = 1;
            }

        }

        if(mainScript.State == gamestate.victory){
            gameOverMenu.gameObject.SetActive(true);
        }


    }
}
