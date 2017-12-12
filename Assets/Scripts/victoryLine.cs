using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class victoryLine : MonoBehaviour {

    [Header("Set Dynamically")]
   public CoreClash game;

	// Use this for initialization
	void Start () {
       GameObject mainCam = GameObject.FindGameObjectWithTag("MainCamera");
       game = mainCam.GetComponent<CoreClash>();
    }

    private void OnTriggerEnter(Collider other)
    {
        

        if(other.gameObject.name=="Player 1")
        {
            game.Victory(1);
        }
        else 
        {
            game.Victory(2);
        }
        
        
    }
}
