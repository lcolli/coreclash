using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {
    [Header("Set in Inspector")]
    public float empoverheat = 3f;
    //the list of powerups
     string[] PUList = new string[] {"Daimond Drill","Dynamite","EMP",
                                    "Grapple","Overclock","Shield" };

    [Header("Set Dynamically")]
    //the name of this powerup
    public string Name="none";
    

    //picks a random power up from the list
	public void getPowerup()
    {
        int num = Random.Range(0,PUList.Length-1    );
        Name = PUList[num];
        
	}   

    //uses the function based on the power up, this is if player1 uses it
    public void use(Player1 player)
    {
        switch (Name)
        {
            case "Daimond Drill":
                break;
            case "Dynamite":
                break;
            case "EMP":
                if(!GetComponent<Player2>().S.drill.useShield())
                    GetComponent<Player2>().S.drill.overheat(empoverheat);
                break;
            case "Grapple":
                break;
            case "Overclock":
                break;
        }
    }

    public void use(Player2 player)
    {

    }
}
