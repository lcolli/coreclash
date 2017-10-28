using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour {

    public Drill S; // Singleton instance

    //set in game engine
    public float drillACC = .1f;        //acceleration of the drills angular momentum
    public float drillDmgMult = .1f;    //how fast damage builds per fixed update    
    public Color defaultColor = new Color(200, 200, 200, 255); //normal color of drill
    public Color buildUp = new Color(200, 200, 200, 255);      //color of build up
    public Color ideal = new Color(200, 200, 200, 255);         //final color before overheat
    public Color overHeatColor = new Color(255, 150, 150, 255); //overheated color

    public bool __________________________________________________________________________;
    //values set dynamically
    public GameObject[] leftDrills,rightDrills;
    public Block target;
    

    // Use this for initialization
    void Start ()
    {
        S = this;

        //so the drills rotate in opposite direction
        leftDrills = GameObject.FindGameObjectsWithTag("Left");
        rightDrills = GameObject.FindGameObjectsWithTag("Right");
        
    }
	
	// Update is called once per frame
	void Update () {

        foreach (GameObject drill in leftDrills)
        {
            drill.GetComponent<Rigidbody>().angularVelocity.Set(0, drill.GetComponent<Rigidbody>().angularVelocity.magnitude + drillACC * Time.deltaTime, 0);
        }
        //increase angular velocity of right 
        foreach (GameObject drill in rightDrills)
        {
            drill.GetComponent<Rigidbody>().angularVelocity.Set(0, drill.GetComponent<Rigidbody>().angularVelocity.magnitude - drillACC * Time.deltaTime, 0);
        }

    }

    void FixedUpdate()
    {
        if(Input.GetButtonUp("Down Arrow"))
        {
            //attack the block
            //reset damage back to zero
            //move piece
        }
       if(Input.GetButton("Down Arrow"))
        {
           //increase angular if of left drills
           //increase angular velocity of right drills
           //change colors incrementally
        }
    }

    public void gravitySwitch()
    {
        if(S.GetComponent<Rigidbody>().useGravity)
            S.GetComponent<Rigidbody>().useGravity = false;
        else
            S.GetComponent<Rigidbody>().useGravity = true;
    }

    public void kinematicSwitch()
    {
        if (S.GetComponent<Rigidbody>().isKinematic)
            S.GetComponent<Rigidbody>().isKinematic = false;
        else
            S.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        target=other.gameObject.GetComponent<Block>();
    }

    public void OnTriggerExit(Collider other)
    {
        target = null;
    }
}
