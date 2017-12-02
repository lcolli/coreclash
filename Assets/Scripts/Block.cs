using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float waterOverheatTime = 3f;   //the time that a water block will overheat the rig
    public float lavaOverheatTime = 5f;    //time that the lava block will overheat the rig
    public GameObject transformPrefab;
    public int GOverclockTime = 250;
    public int DOverclockTime = 450;
    

    [Header("Set Dynamically")]
    public int health;              //the ammount of damage a block can take before it breaks
    GameObject sand;
    bool sandAbove = false;

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
    public bool attack(float damage, Drill drill,string pointing)
    {
        if (this.tag=="Stone")
        {
            if (damage >= 2)
            {
                blockFunction(drill,pointing);
                Destroy(this.gameObject);
                return true;
            }
            else if(damage>=1)
            {
                transformBlock(transformPrefab);
            }
        }
        if (damage >= health)
        {
            blockFunction(drill,pointing);
            Destroy(this.gameObject);            
            return true;
        }
        return false;
    }

    public void SetSand(GameObject sand)
    {
        this.sand =sand;
        sandAbove = sand != null;
    }

    


    //stone can turn to dirt if you don't do full damage
    void transformBlock(GameObject transformPrefab)
    {
        transformPrefab.transform.position = transform.position;
        Destroy(this.gameObject);
        Instantiate(transformPrefab);
    }


    //the functions of the blocks depending on the type of this block
    public void blockFunction(Drill drill, string pointing)
    {
        if(sandAbove && pointing=="up")
        {
            sand.GetComponent<SandFall>().PlayerBelow();
        }

        switch (this.tag)
        {
          
            case "Diamond":
                //diamond stats
                drill.Overclock(DOverclockTime, 0);
                break;

            case "Gold":
                //gold stats
                drill.Overclock(GOverclockTime, 0);
                break;

            case "Magma":
                //magma stats
                if (!drill.useShield())
                    drill.OverheatLink(lavaOverheatTime);
                break;             
            case "Treasure":
                //treasure stats                
                drill.addPowerUp();
                break;
            case "Water":
                //water stats
                if (!drill.useShield())
                    drill.OverheatLink(waterOverheatTime);
                break;
            default:
                break;

        }

    }

    

}
