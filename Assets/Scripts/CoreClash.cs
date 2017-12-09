using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public enum gamestate
{
    playing,
    pause,
    menu,
    victory,
    countdown
}

public class CoreClash : MonoBehaviour {

    
    

    [Header("Set in Inspector")]
    public GameObject player1GO;
    public GameObject player2GO;
    public Vector3 p1Start, p2Start;
    public GameObject course;
    public Camera cam1,cam2;
    public Image p1PUSprite, p2PUSprite,countdownImg;
    public AudioClip mainTheme,victTheme;
    public AudioClip[] countdownAudio=new AudioClip[2];
    public Sprite[] cdSprites;
    public float musicVlmScale=.624f;
    public float CntdwnVlmScale=.8f;
    public Sprite[] victorySprites;
    public GameObject menuOverlay;
    public float zoomTime = 1.5f;

    [Header ("Set Dynamically")]
    public gamestate State;
    public GameObject player1, player2;
    private AudioSource source;
    private Image overlayIMG;
    private OverlayFunctions overlayClass;

    
    private void Start()
    {
        overlayIMG=menuOverlay.GetComponent<Image>();
        overlayClass = menuOverlay.GetComponent<OverlayFunctions>();
        State = gamestate.menu;
        overlayClass.DisplayMenu(true);
    }

    //probably doesn't need to be here but just in case
    private void Update()
    {
        switch(State)
        {
            case gamestate.menu:                
                break;
            case gamestate.playing:
                break;
            case gamestate.pause:
                break;
            case gamestate.victory:
                break;
            case gamestate.countdown:
                break;
        }
    }

    // Use this for initialization
    public void GameStart ()
    {
        overlayClass.DisplayMenu(false);
        countdownImg.enabled = false;
        source = GetComponent<AudioSource>();
        source.loop = false;
        
        State = gamestate.countdown;

        course.GetComponent<PlayingField>().GameStart();
        CreatePlayers();
        setCameras();
        StartCoroutine(CountDown());

    }    

    public IEnumerator CountDown()
    {
        //3
        countdownImg.sprite = cdSprites[0];
        countdownImg.enabled = true;
        source.PlayOneShot(countdownAudio[0], CntdwnVlmScale);       
        yield return new WaitForSeconds(1);
        //2
        countdownImg.sprite = cdSprites[1];
        source.PlayOneShot(countdownAudio[0], CntdwnVlmScale);
        yield return new WaitForSeconds(1);
        //1
        countdownImg.sprite = cdSprites[2];
        source.PlayOneShot(countdownAudio[0], CntdwnVlmScale);
        yield return new WaitForSeconds(1);
        //go 
        countdownImg.sprite = cdSprites[3];
        source.PlayOneShot(countdownAudio[1], CntdwnVlmScale);
        State = gamestate.playing;
       
        yield return new WaitForSeconds(.5f);
        countdownImg.enabled = false;
        source.volume = musicVlmScale;
        source.clip = mainTheme;
        source.Play();
        source.loop = true;

    }
    
    void Fade()
    {

    }
    public void setCameras()
    {
        
        player1.GetComponent<Player1>().playerCam = cam1;
        player2.GetComponent<Player2>().playerCam = cam2;
        
    }


    public void CreatePlayers()
    {
        player1=Instantiate(player1GO);
        player2 = Instantiate(player2GO);
        player1.transform.position = p1Start;
        player2.transform.position = p2Start;
        player1.GetComponent<Powerup>().PowerupDisplay = p1PUSprite;
        player2.GetComponent<Powerup>().PowerupDisplay = p2PUSprite;  

    }
    public void PauseGame()
    {
        if (State!=gamestate.pause)
        {
            Time.timeScale = 0;
            State = gamestate.pause;
            //pull up pause menu
            overlayClass.DisplayPause(true);
        }
        else
        {
            Time.timeScale = 1;
            State = gamestate.playing;
            //put away pause menu
            overlayClass.DisplayPause(false);
        }
    }

    public void Victory(int winningPlayer)
    {
        State = gamestate.victory;
        Time.timeScale = 0;
        Camera winningCam = null,losingCam=null;
        
        switch(winningPlayer)
        {
            case 0:
                Draw();                
                break;
            case 1:
                winningCam = cam1;
                losingCam = cam2;
                
                break;
            case 2:
                winningCam = cam2;
                losingCam = cam2;
                
                break;               
        }

        PlayerVictory(victorySprites[winningPlayer], winningCam,losingCam);
    }


    //not reachable as of right now
    private void Draw()
    {

    }


    private void PlayerVictory(Sprite winner, Camera winningCam, Camera losingCam)
    {
        source.Stop();
        source.clip = victTheme;
        source.Play();
        player1.GetComponent<Player>().source.Stop();
        player2.GetComponent<Player>().source.Stop();
        source.loop = true;
        losingCam.rect = new Rect(0, 0, 0, 0);
        winningCam.rect = new Rect(0, 0, 1, 1);
        cam1.orthographicSize = 2;
        p1PUSprite.enabled = false;
        p2PUSprite.enabled = false;
        overlayClass.DisplayVictory(true,winner);
        //victory overlay
    }


    
}
