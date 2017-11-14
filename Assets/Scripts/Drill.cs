using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour {

    public Drill S; // Singleton instance

    //set in game engine
    public float drillACC = .1f;        //acceleration of the drills angular momentum
    public float drillDmgRamp = .1f;    //how fast damage builds per fixed update  
    public float overheatdmg = 4f;      //variable that determines when the rig overheats
    public Color defaultCockpitColor; //= new Color(114, 162, 242, 255);
    public Color defaultDrillColor = new Color(200, 200, 200, 255); //normal color of drill
    public Color buildUp = new Color(200, 200, 200, 255);      //color of build up
    public Color ideal = new Color(200, 200, 200, 255);         //final color before overheat
    public Color overHeatColor = new Color(255, 150, 150, 255); //overheated color

    public float movementSpeed = 2f;
    public float overheatTime = 3f;

    public MeshRenderer[] rendDrill;
    public MeshRenderer rendCockpit;
    public Material drillState;
    public Material cockpitState;

    public bool __________________________________________________________________________;

    //values set dynamically
    GameObject[] leftDrills,rightDrills;
    public GameObject target;
    public float damage;

    public bool overheated;

    // Use this for initialization
    void Start ()
    {
        resetDrillState();
        S = this;

        //so the drills rotate in opposite direction
        leftDrills = GameObject.FindGameObjectsWithTag("Left");
        rightDrills = GameObject.FindGameObjectsWithTag("Right");
        damage = 0f;
        
    }
	
	// Update is called once per frame
	void Update () {

       

        //will attack when you release the spacebar
        if (Input.GetKeyUp(KeyCode.Space) && !overheated)
        {
            if (target != null)
            {

                if (target.GetComponent<Block>().attack(damage))
                {
                    target = null;
                    GetComponent<Rigidbody>().useGravity = true;
                }
            }

            damage = 0f;

            resetDrillState ();
        }
        else if(Input.GetKeyUp(KeyCode.Space) && overheated == true)
            overheat();
        
        if(!overheated){
            if (Input.GetKey (KeyCode.DownArrow)) 
                moveDown ();            
            //if(Input.GetKey(KeyCode.UpArrow))
               // moveUp ();
            if(Input.GetKey(KeyCode.LeftArrow))
                moveLeft ();
           if(Input.GetKey(KeyCode.RightArrow))
                moveRight ();
        }
    }

    void FixedUpdate()
    {

        //will charge as long as you're holding spacebar. increases the damage 
        if(Input.GetKey(KeyCode.Space) && !overheated)
        {
            damage += drillDmgRamp;

            

            if (damage < 0.5f)
                resetDrillState ();
            else if (damage > 0.5f && damage < 1.25f)
                drillState.color = buildUp;
            else if (damage > 1.25f && damage < 2.0f)
                drillState.color = ideal;
            else if (damage > 2.0f)
                StartCoroutine(overheat());

            //Material newMaterial = new Material (Shader.Find ("Specular"));
            //newMaterial.color = overHeatColor;

            foreach (MeshRenderer mr in rendDrill) {
                mr.material = drillState;
            }
        }

        

    }

    void moveDown(){
        // var destination = transform.position + new Vector3 (0, -1, 0);
        //Vector3 movement = Vector3.Lerp (transform.position, destination, movementSpeed * Time.deltaTime);
        //transform.SetPositionAndRotation (movement, Quaternion.Euler(0,0,0));
        transform.rotation = Quaternion.Euler(0, 0, 0);

    }

    void moveUp(){
        var destination = transform.position + new Vector3 (0, 1, 0);
        Vector3 movement = Vector3.Lerp (transform.position, destination, movementSpeed * Time.deltaTime);
        transform.SetPositionAndRotation (movement, Quaternion.Euler(0,0,180));
    }

    void moveLeft(){
        var destination = transform.position + new Vector3 (-1, 0, 0);
        Vector3 movement = Vector3.Lerp (transform.position, destination, movementSpeed * Time.deltaTime);
        transform.SetPositionAndRotation (movement, Quaternion.Euler(0,0,-90));
    }

    void moveRight(){
        var destination = transform.position + new Vector3 (1, 0, 0);
        Vector3 movement = Vector3.Lerp (transform.position, destination, movementSpeed * Time.deltaTime);
        transform.SetPositionAndRotation (movement, Quaternion.Euler(0,0,90));
    }

    IEnumerator overheat(){
        var time = 0f;

        overheated = true;
        drillState.color = overHeatColor;
        cockpitState.color = overHeatColor;

        yield return new WaitForSeconds(3);
        
        overheated = false;
        resetDrillState();
        damage = 0f;
    }

    void resetDrillState(){
        drillState.color = defaultDrillColor;
        cockpitState.color = defaultCockpitColor;
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

    public void OnCollisionEnter(Collision collision)
    {
        GameObject collidedWith = collision.gameObject;
        GetComponent<Rigidbody>().useGravity = false;
    }


    //takes the sphere collider that's 1m away. will set the gameobject that it touches to target
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<BoxCollider>().isTrigger)
            target = other.gameObject;
    }

    //if the drill is no longer facing a game object it will set the target to null
    public void OnTriggerExit(Collider other)
    {
        target = null;
    }
}
