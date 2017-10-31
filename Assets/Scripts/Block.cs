using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    public int health; //the ammount of damage a block can take before it breaks

    // Use this for initialization
    void Start()
    {

        //takes the name of the game object and applies its functions and stats
        switch(this.tag)
        {
            case "Dirt":
                health = 1;
                break;

           default:
                break;
            
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    //takes the damage from the drill and destroys this gameobject if the damage is more than health
    public void attack(float damage)
    {
        if(damage>health)
        {
            Destroy(this.gameObject);
        }
    }
}
