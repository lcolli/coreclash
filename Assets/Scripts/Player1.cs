using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : Player {

    public Player1 S;


    private new void Update()
    {
        if (Input.GetKeyDown(usePU))
        {
           
            UsePowerup();
        }
        base.Update();
    }

    private void Awake()
    {
        this.gameObject.name = "Player 1";
        S = this;
    }


    public new void UsePowerup()
    {
        
        powerup.use(this);

    }
}
