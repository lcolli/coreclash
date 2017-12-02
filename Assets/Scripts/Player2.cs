using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : Player {
    public Player2 S;

    


    private void Awake()
    {
        this.gameObject.name = "Player 2";
        S = this;
    }

    private new void Update()
    {
        if (Input.GetKeyDown(usePU))
        {
           
            UsePowerup();
        }
        base.Update();
    }



    //uses the power up that is attached to this player if it has one
    public new void UsePowerup()
    {
        powerup.use(this);
        
    }

    
}

