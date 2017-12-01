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
   
    

    //uses the power up that is attached to this player if it has one
    public new void usePowerup()
    {
        powerup.use(this);
        
    }

    
}

