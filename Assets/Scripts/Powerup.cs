using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {
    [Header("Set in Inspector")]
    public float empoverheat = 3f;
    public float overclockTime = 5f;
    public float overclockdowntime = 3f;
    //the list of powerups
     string[] PUList = new string[] {"Diamond","Dynamite","EMP",
                                    "Grapple","Overclock","Shield" };

    [Header("Set Dynamically")]
    //the name of this powerup
    public string Name="none";
    public Player1 p1;
    public Player2 p2;

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
        
	}   

    //uses the function based on the power up, this is if player1 uses it
    public void use(Player1 player)
    {
        switch (Name)
        {
            case "Diamond":
                player.drill.diamond = true;
                Name = "none";
                break;
            case "Dynamite":
                player.DynamiteBlast();
                Name = "none";
                break;
            case "EMP":
                if(!p2.drill.useShield())
                  StartCoroutine(p2.drill.overheat(empoverheat));
                Name = "none";
                break;
            case "Grapple":
                if (!p2.drill.useShield())
                {
                    Name = p2.powerup.Name;
                    p2.powerup.Name = "none";
                }
                else
                {
                    Name = "none";
                        
                }
                break;
            case "Overclock":
                StartCoroutine(Overclock(player.drill));
                Name = "none";
                break;
            case "Shield":
                Name = "none";
                player.drill.shielded = false;
                break;
        }
    }   

    public void use(Player2 player)
    {
        switch (Name)
        {
            case "Diamond":
                player.drill.diamond = true;
                Name = "none";
                break;
            case "Dynamite":
                player.DynamiteBlast();
                Name = "none";
                break;
            case "EMP":
                if (!p1.drill.useShield())
                    StartCoroutine(p1.drill.overheat(empoverheat));
                Name = "none";
                break;
            case "Grapple":
                if (!p1.drill.useShield())
                {
                    Name = p1.powerup.Name;
                    p1.powerup.Name = "none";
                }
                else
                {
                    Name = "none";

                }
                break;
            case "Overclock":
                StartCoroutine(Overclock(player.drill));
                Name = "none";
                break;
            case "Shield":
                player.drill.shielded = false;
                Name = "none";
                break;
        }
    }


    public IEnumerator Overclock(Drill drill)
    {
        drill.overclocked = true;
        float temp = drill.drillDmgRamp;
        drill.drillDmgRamp *= 2;
        yield return new WaitForSeconds(overclockTime);
        drill.drillDmgRamp = temp;
        drill.overclocked = false;
        drill.overheat(overclockdowntime);
    }
}
