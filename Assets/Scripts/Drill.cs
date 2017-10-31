using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour {

    public Drill S; // Singleton instance

    //set in game engine
    public float drillACC = .1f;        //acceleration of the drills angular momentum
    public float drillDmgRamp = .1f;    //how fast damage builds per fixed update  
    public float overheatdmg = 2f;      //variable that determines when the rig overheats
    public Color defaultColor = new Color(200, 200, 200, 255); //normal color of drill
    public Color buildUp = new Color(200, 200, 200, 255);      //color of build up
    public Color ideal = new Color(200, 200, 200, 255);         //final color before overheat
    public Color overHeatColor = new Color(255, 150, 150, 255); //overheated color

    public bool __________________________________________________________________________;
    //values set dynamically
    GameObject[] leftDrills,rightDrills;
    public GameObject target;
    public float damage;


    // Use this for initialization
    void Start ()
    {
        S = this;

        //so the drills rotate in opposite direction
        leftDrills = GameObject.FindGameObjectsWithTag("Left");
        rightDrills = GameObject.FindGameObjectsWithTag("Right");
        damage = 0f;
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

        //will charge as long as you're holding spacebar. increases the damage 
        if(Input.GetKey(KeyCode.Space))
        {
            damage += drillDmgRamp;
        }

        //will reset if rig gets too hot
        if (damage >= overheatdmg)
        {
            damage = 0;
        }

        //will attack when you release the spacebar
       if(Input.GetKeyUp(KeyCode.Space))
        {            
            if(target!=null) target.GetComponent<Block>().attack(damage);
           
            
            damage = 0f;
        }

       
    }


    //this will turn the gravity on and off for the drill. will change it from true to false or false to true
    public void gravitySwitch()
    {
        if(S.GetComponent<Rigidbody>().useGravity)
            S.GetComponent<Rigidbody>().useGravity = false;
        else
            S.GetComponent<Rigidbody>().useGravity = true;
    }

    //same as above but with kinematic boolean
    public void kinematicSwitch()
    {
        if (S.GetComponent<Rigidbody>().isKinematic)
            S.GetComponent<Rigidbody>().isKinematic = false;
        else
            S.GetComponent<Rigidbody>().isKinematic = true;
    }


    //takes the sphere collider that's 1m away. will set the gameobject that it touches to target
    public void OnTriggerEnter(Collider other)
    {
        target = other.gameObject;
    }

    //if the drill is no longer facing a game object it will set the target to null
    public void OnTriggerExit(Collider other)
    {
        target = null;
    }
}
