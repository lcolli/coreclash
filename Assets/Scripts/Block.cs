using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("Set in Inspector")]
    float waterOverheatTime = 3f;   //the time that a water block will overheat the rig
    float lavaOverheatTime = 5f;    //time that the lava block will overheat the rig
    [Header("Set Dynamically")]
    public int health;              //the ammount of damage a block can take before it breaks

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
    public bool attack(float damage, Drill drill)
    {
        if (this.tag=="Stone")
        {
            if (damage >= 2)
            {                
                Destroy(this.gameObject);
                return true;
            }
            else if(damage>=1)
            {
                turntodirt();
            }
        }
        if (damage > health)
        {
            blockFunction(drill);
            Destroy(this.gameObject);            
            return true;
        }
        return false;
    }


    //stone can turn to dirt if you don't do full damage
    void turntodirt()
    {

    }


    //the functions of the blocks depending on the type of this block
    public void blockFunction(Drill drill)
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
                if (!drill.useShield())
                    StartCoroutine(drill.overheat(lavaOverheatTime));
                break;             
            case "Treasure":
                //treasure stats
                
                drill.addPowerUp();
                break;
            case "Water":
                //water stats
                if (!drill.useShield())
                    StartCoroutine(drill.overheat(waterOverheatTime));
                break;

            default:
                break;

        }
    }
   
}
