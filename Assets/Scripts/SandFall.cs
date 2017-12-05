using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is the code to add gravity to the sand and has code to deal with the falling on the game board

public class SandFall : MonoBehaviour {


    GameObject below;                   //the gameobject below this one
    GameObject player;                  //the player object on this track
    public bool isRightTrack = true;    //determines which track its on

    private void Start()
    {


        //figures out which side of the track its on and assigns the player variable
        if(transform.position.x<0)
        {
            isRightTrack = false;
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject GO in players)
        {
            if (isRightTrack && GO.transform.position.x>0)
            {
                player = GO;
            }
            if (!isRightTrack && GO.transform.position.x < 0)
            {
                player = GO;
            }

        }
        
    }

    void Update ()
    {
        
        if(!FindBelow()) // returns true if there is nothing belowe the object
        {
            Fall(); 
        }
        	  	
	}
   
    //if there's a player below this object it will shift the player out of the way
    public void PlayerBelow()
    {
        player.GetComponent<Player>().Shift();
    }      

    //will move to the position below this block
    private void Fall()
    {
        player.GetComponent<Player>().State = state.overheat;
        transform.position += new Vector3(0, -2, 0);
        player.GetComponent<Player>().drill.getTargets();
        player.GetComponent<Player>().State = state.idle;
    }    


    //searches the space below it and returns false if there's nothing and true if there is a block. will also put a link
    //to this object in the block if there is one
    private bool FindBelow()
    {
        
        Vector3 belowPos = transform.position + new Vector3(0, -1f, .5f);

        Collider[] cols = Physics.OverlapCapsule(transform.position+new Vector3(0,-.5f,0), belowPos, .1f);
        List<GameObject> gos = new List<GameObject>();
        foreach (Collider collide in cols)
        {
            if (collide.gameObject.layer == LayerMask.NameToLayer("Playing field"))
            {
                
                gos.Add(collide.gameObject);
            }
        }

        below = null;
        foreach (GameObject GO in gos)
        {
            
            if(GO!=this.gameObject)
            {
               
                below = GO;
            }
        }

        if (below != null)
            below.GetComponent<Block>().SetSand(this.gameObject);

        return (below != null);           


    }

    //destroys the link to this object that it created
    private void OnDestroy()
    {
        below.GetComponent<Block>().SetSand(null);
    }
}
