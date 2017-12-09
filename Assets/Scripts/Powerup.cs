using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Powerup : MonoBehaviour {
    [Header("Set in Inspector")]
    public float empoverheat = 3f;
    public int overclockTime = 250;
    public float overclockdowntime = 3f;
    //the list of powerups
     string[] PUList = new string[] {"Diamond","Dynamite","EMP",
                                    "Grapple","Overclock","Shield" };
    AudioClip[] PUAudios;

    public Sprite[] image;
    public Image PowerupDisplay;

    [Header("Set Dynamically")]
    //the name of this powerup
    public string Name="none";
    public Player1 p1;
    public Player2 p2;
    private AudioClip currentAudio;

    public void Start()
    {
        GameObject[] players =GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject pl in players)
        {
            
            if (pl.name == "Player 1")
                p1 = pl.GetComponent<Player1>();
            else if (pl.name == "Player 2")
                p2 = pl.GetComponent<Player2>();

        }
    }
    //picks a random power up from the list
    public void getPowerup()
    {
        int num = Random.Range(0,PUList.Length-1);
        
        Name = PUList[num];
        PowerupDisplay.sprite = image[num];
        currentAudio = PUAudios[num];
        
	}   

    //uses the function based on the power up, this is if player1 uses it
    public void use(Player1 player)
    {
       
        switch (Name)
        {
            case "Diamond":
                player.drill.diamond = true;
                Reset();
                break;
            case "Dynamite":
               
                player.DynamiteBlast();
                Reset();
                break;
            case "EMP":
                if(!p2.drill.useShield())
                  StartCoroutine(p2.drill.overheat(empoverheat));
                Reset();
                break;
            case "Grapple":
                if (!p2.drill.useShield())
                {
                    Name = p2.powerup.Name;
                    p2.powerup.Name = "none";
                }
                else
                {
                    Reset();                        
                }
                break;
            case "Overclock":
                player.drill.Overclock(overclockTime,overclockdowntime);
                Reset();
                break;
            case "Shield":
                Reset();
                player.drill.shielded = false;
                break;
        }
        player.source.PlayOneShot(currentAudio,1);
    }   

    public void use(Player2 player)
    {
        switch (Name)
        {
            case "Diamond":
                player.drill.diamond = true;
                Reset();
                break;
            case "Dynamite":
                player.DynamiteBlast();
                Reset();
                break;
            case "EMP":
                if (!p1.drill.useShield())
                    StartCoroutine(p1.drill.overheat(empoverheat));
                Reset();
                break;
            case "Grapple":
                if (!p1.drill.useShield())
                {
                    Name = p1.powerup.Name;
                    p1.powerup.Name = "none";
                }
                else
                {
                    Reset();

                }
                break;
            case "Overclock":
                player.drill.Overclock(overclockTime, overclockdowntime);
                Reset(); 
                break;
            case "Shield":
                player.drill.shielded = false;
                Reset();
                break;
        }
        player.source.PlayOneShot(currentAudio, 1);
    }

    public void Reset()
    {
        //PowerupDisplay.sprite = image[6];
        Name = "none";
        currentAudio = null;
    }



}
