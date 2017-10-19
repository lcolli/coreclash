using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    public int health;

    // Use this for initialization
    void Start()
    {
        switch(this.name)
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

    public void attack(float damage)
    {
        if(damage>health)
        {
            Destroy(this);
        }
    }
}
