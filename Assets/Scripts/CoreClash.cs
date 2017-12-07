using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Image p1PUSprite, p2PUSprite;


    [Header ("Set Dynamically")]
    public gamestate State;
    public GameObject player1, player2;
    

    // Use this for initialization
    void Start ()
    {

        State = gamestate.countdown;

        course.GetComponent<PlayingField>().GameStart();
        CreatePlayers();
        setCameras();

        //initiate countdown to start

	}

    private void Countdown()
    {
        
        //time loops
        //display numbers
        //reactivate scripts
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

    
}
