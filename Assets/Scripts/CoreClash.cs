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
    public AudioClip mainTheme, firstMainTheme;
    public AudioClip[] countdownAudio=new AudioClip[2];
    public Sprite[] cdSprites;
    public float musicVlmScale=1;
    public float CntdwnVlmScale=1;

    [Header ("Set Dynamically")]
    public gamestate State;
    public GameObject player1, player2;
    private AudioSource source;
    


    
    // Use this for initialization
    void Start ()
    {
        source = GetComponent<AudioSource>();       
        source.clip = firstMainTheme;
        //source.PlayOneShot(firstMainTheme,musicVlmScale);
        State = gamestate.countdown;

        course.GetComponent<PlayingField>().GameStart();
        CreatePlayers();
        setCameras();
        StartCoroutine(CountDown());

    }
    public void FixedUpdate()
    {
        if(!source.isPlaying)
        {
            source.clip = mainTheme;
            source.volume = musicVlmScale;
            source.Play();
        }
    }

    public IEnumerator CountDown()
    {
        //3
        //countdownImg.sprite = cdSprites[0];
        source.PlayOneShot(countdownAudio[0], CntdwnVlmScale);
        print("3");
        yield return new WaitForSeconds(1);
        //2
        //countdownImg.sprite = cdSprites[1];
        print("2");
        source.PlayOneShot(countdownAudio[0], CntdwnVlmScale);
        yield return new WaitForSeconds(1);

        //1
        //countdownImg.sprite = cdSprites[1];
        print("1");
        source.PlayOneShot(countdownAudio[0], CntdwnVlmScale);
        yield return new WaitForSeconds(1);
        //go and fade
        //countdownImg.sprite = cdSprites[3];
        print("go");
        source.PlayOneShot(countdownAudio[1], CntdwnVlmScale);
        State = gamestate.playing;
        yield return new WaitForSeconds(.5f);
        ///countdownImg.enabled = false;

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
        }
        else
        {
            Time.timeScale = 1;
            State = gamestate.playing;
        }
    }

    
}
