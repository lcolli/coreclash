using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {


    public Vector3 destination;
    
    
   
    

    private void Start()
    {
        destination = transform.position;
    }

	public void moveCam()
    {
      destination += new Vector3(0, -2, 0);
    }

    private void FixedUpdate()
    {
        
       
            transform.position = Vector3.Lerp(transform.position, destination, .02f);
            
        
    } 

   
}
