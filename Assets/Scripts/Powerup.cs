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
    public AudioClip[] PUAudios;

    public Sprite[] image;
    public Image PowerupDisplay;

    [Header("Set Dynamically")]
    //the name of this powerup
    public string Name="none";
    public Player1 p1;
    public Player2 p2;
    public AIPlayer AI;
    
    private AudioClip currentAudio;
    private bool vsAI;

    public void Start()
    {
        GameObject[] players =GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject pl in players)
        {
            print(pl.name);
            if (pl.name == "Player 1")
                p1 = pl.GetComponent<Player1>();
            else if (pl.name == "Player 2")
                p2 = pl.GetComponent<Player2>();
            else if (p1.name == "AI Player")
                AI = p1.GetComponent<AIPlayer>();

                
        }
        Reset();
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
        if (currentAudio != null)
            player.source.PlayOneShot(currentAudio, 1);
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
                if (vsAI)
                {
                    if (!AI.drill.useShield())
                        StartCoroutine(AI.drill.overheat(empoverheat));
                }
                else
                {
                    if (!p2.drill.useShield())
                        StartCoroutine(p2.drill.overheat(empoverheat));
                }
                Reset();
                break;
            case "Grapple":
                if(vsAI && !AI.drill.useShield())
                {
                    Name = AI.powerup.Name;
                    currentAudio = AI.powerup.currentAudio;
                    PowerupDisplay.sprite = AI.powerup.PowerupDisplay.sprite;
                    AI.powerup.Reset();
                }
                else if (!p2.drill.useShield())
                {
                    Name = p2.powerup.Name;
                    currentAudio = p2.powerup.currentAudio;
                    PowerupDisplay.sprite = p2.powerup.PowerupDisplay.sprite;
                    p2.powerup.Reset();
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
        
    }   

    public void use(Player2 player)
    {
        if(currentAudio!=null)
            player.source.PlayOneShot(currentAudio, 1);
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
                    currentAudio = p1.powerup.currentAudio;
                    PowerupDisplay.sprite = p1.powerup.PowerupDisplay.sprite;                    
                    p1.powerup.Reset();
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
       
    }

    public void use(AIPlayer player)
    {
        if (currentAudio != null)
            player.source.PlayOneShot(currentAudio, 1);
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
                    currentAudio = p1.powerup.currentAudio;
                    PowerupDisplay.sprite = p1.powerup.PowerupDisplay.sprite;
                    p1.powerup.Reset();
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

    }

    public void Reset()
    {
        PowerupDisplay.sprite = image[6];
        Name = "none";
        currentAudio = PUAudios[6];
    }



}
