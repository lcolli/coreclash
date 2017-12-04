using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandFall : MonoBehaviour {


    GameObject below;
    GameObject player;
    public bool isRightTrack = true;

    private void Start()
    {
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
        
        if(FindBelow())
        {
            Fall();
        }
        	  	
	}
   

    public void PlayerBelow()
    {
        player.GetComponent<Player>().Shift();
    }      

    private void Fall()
    {
        player.GetComponent<Player>().State = state.overheat;
        transform.position += new Vector3(0, -2, 0);
        player.GetComponent<Player>().drill.getTargets();
        player.GetComponent<Player>().State = state.idle;
    }    

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

        return (below == null);           


    }

    private void OnDestroy()
    {
        below.GetComponent<Block>().SetSand(null);
    }
}
