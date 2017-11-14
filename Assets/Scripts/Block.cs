using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    public int health; //the ammount of damage a block can take before it breaks

    // Use this for initialization
    void Start()
    {
        health = 1;
        //takes the name of the game object and applies its functions and stats
        switch (this.tag)
        {
            case "Dirt":                
                break;

            case "Diamond":
                //daimond stats
                health += 2;
                break;

            case "Gold":
                //gold stats
                health += 1;
                break;

            case "Magma":
                //magma stats
                health += 1;
                break;

            case "Metal":
                //metal stats
                health += 2;
                break;

            case "Sand":
                this.gameObject.GetComponent<Rigidbody>().useGravity = true;
                break;

            case "Stone":
                //stone stats
                health += 1;
                break;

            case "Treasure":
                //treasure stats
                health += 1;
                break;

            case "Water":
                //water stats
                break;

            default:
                break;

        }
    }


    //takes the damage from the drill and destroys this gameobject if the damage is more than health
    public bool attack(float damage)
    {
        if (damage > health)
        {
            blockFunction();
            Destroy(this.gameObject);            
            return true;
        }
        return false;
    }

    public void blockFunction()
    {
        switch (this.tag)
        {
          
            case "Diamond":
                //diamond stats
                break;

            case "Gold":
                //gold stats
                break;

            case "Magma":
                //magma stats
                break;             
            case "Treasure":
                //treasure stats
                break;
            case "Water":
                //water stats
                break;

            default:
                break;

        }
    }
   
}
