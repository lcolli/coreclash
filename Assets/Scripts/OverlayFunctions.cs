using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayFunctions : MonoBehaviour {

    private CoreClash game;
    public Sprite menuOverlay,pauseOverlay,victoryOverlay;
    public Image thisImage;


    private void Start()
    {
        thisImage = GetComponent<Image>();
        GameObject mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        game = mainCam.GetComponent<CoreClash>();
    }


    //theoretically house how to press the buttons
    private void Update()
    {
        if(game.State==gamestate.menu)
        {
            if(Input.anyKeyDown)
            {
                game.GameStart(true);
            }
        }
    }

    public void DisplayMenu(bool isShown)
    {
        //print(thisImage.name);
        thisImage.sprite = menuOverlay;
        thisImage.enabled = isShown;
        //place buttons
    }

    public void DisplayPause(bool isShown)
    {
        thisImage.sprite = pauseOverlay;
        thisImage.enabled = isShown;
        //place buttons
    }

    public void DisplayVictory(bool isShown,Sprite winner)
    {
        thisImage.sprite = victoryOverlay;
        thisImage.enabled = isShown;
        //place buttons
    }
}
