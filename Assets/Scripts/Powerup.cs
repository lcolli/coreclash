using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {
    [Header("Set in Inspector")]
    public float empoverheat = 3f;
    public float overclockTime = 5f;
    public float overclockdowntime = 3f;
    //the list of powerups
     string[] PUList = new string[] {"Daimond Drill","Dynamite","EMP",
                                    "Grapple","Overclock","Shield" };

    [Header("Set Dynamically")]
    //the name of this powerup
    public string Name="none";
    

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
            case "Daimond Drill":
                player.drill.daimond = true;
                Name = "none";
                break;
            case "Dynamite":
                player.DynamiteBlast();
                Name = "none";
                break;
            case "EMP":
                if(!GetComponent<Player2>().S.drill.useShield())
                    GetComponent<Player2>().S.drill.overheat(empoverheat);
                Name = "none";
                break;
            case "Grapple":
                Name=GetComponent<Player2>().S.powerup.Name;
                GetComponent<Player2>().S.powerup.Name="none";
                break;
            case "Overclock":
                StartCoroutine(Overclock(player.drill));
                Name = "none";
                break;
            case "sheild":
                Name = "none";
                player.drill.shielded = false;
                break;
        }
    }   

    public void use(Player2 player)
    {
        switch (Name)
        {
            case "Daimond Drill":
                player.drill.daimond = true;
                Name = "none";
                break;
            case "Dynamite":
                player.DynamiteBlast();
                Name = "none";
                break;
            case "EMP":
                if (!GetComponent<Player1>().S.drill.useShield())
                    GetComponent<Player1>().S.drill.overheat(empoverheat);
                Name = "none";
                break;
            case "Grapple":
                Name = GetComponent<Player1>().S.powerup.Name;
                GetComponent<Player1>().S.powerup.Name = "none";
                break;
            case "Overclock":
                StartCoroutine(Overclock(player.drill));
                Name = "none";
                break;
            case "sheild":
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
