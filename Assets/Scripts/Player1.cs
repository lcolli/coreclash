using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : Player {

    public Player1 S;       

    private void Awake()
    {
        this.gameObject.name = "Player 1";
        S = this;
    }


    public void usePowerup()
    {
        powerup.use(this);

    }
}
