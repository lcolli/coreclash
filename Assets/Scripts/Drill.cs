using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour {

    public Drill S; // Singleton instance

    [Header("Set in inspector")]
    public float drillACC = .1f;        //acceleration of the drills angular momentum
    public float drillDmgRamp = .1f;    //how fast damage builds per fixed update  
    public float overheatdmg = 4f;      //variable that determines when the rig overheats
    public Color defaultCockpitColor;   //new Color(114, 162, 242, 255);
    public Color defaultDrillColor = new Color(200, 200, 200, 255); //normal color of drill
    public Color buildUp = new Color(200, 200, 200, 255);      //color of build up
    public Color ideal = new Color(200, 200, 200, 255);         //final color before overheat
    public Color overHeatColor = new Color(255, 150, 150, 255); //overheated color

    
    public float overheatTime = 3f;     //the standard downtime when you overheat

    public MeshRenderer[] rendDrill;
    public MeshRenderer rendCockpit;
    public Material drillState;
    public Material cockpitState;
    

    
    [Header("Set Dynamically")]
    
    public GameObject target;                       //the block the drill is facint
    public float damage;                            //the damage charge of the 
    public bool isPlayer1;  
    public GameObject playerGO;                     //the player object attached to this game
    public bool pointingDown, shielded;             //if the drill is pointing down and if it currently has a shield
    public GameObject left, Right, Down, Up;        //the blocks in the specified direction
    public KeyCode drilluse=KeyCode.Space;          // the key that is used to operate the drill
    public bool overheated;                         //wheter or not the drill is overheated


    //finds the target in front of the drill
    public void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.GetComponent<BoxCollider>().isTrigger)
            target = other.gameObject;
    }

    //if the drill is no longer facing a game object it will set the target to null
    public void OnTriggerExit(Collider other)
    {
        target = null;
    }
    
    //if the player is shielded it will use the shield and return true else it will return false
    public bool useShield()
    {
        if (!shielded)
            return false;
        else
        {
            if (isPlayer1)
            {
                playerGO.GetComponent<Player1>().usePowerup();                
            }
            else
            {
                playerGO.GetComponent<Player2>().usePowerup();
            }
            return true;
        }
    }


    
    void Start ()
    {
           //finds the player object this drill is attached to    
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (GameObject player in players)
        {
            if (this.transform.IsChildOf(player.gameObject.transform))
            {
                playerGO = player;
                if (player.gameObject.name == "Player 1") 
                    isPlayer1 = true;                
                else                
                    isPlayer1 = false;
                
            }
        }

        resetDrillState();
        S = this;
        shielded = false;
        
    }

    //gets the power up. just a middle function between block and the player
    public void addPowerUp()
    {
        if (isPlayer1)
            playerGO.GetComponent<Player1>().getPowerup();
        else
            playerGO.GetComponent<Player2>().getPowerup();
    }

    
	
	// Update is called once per frame
	void Update () {

       

        //will attack when you release the spacebar
        if (Input.GetKeyUp(drilluse) && !overheated)
        {           
            if (target != null)
            {               
                if (target.GetComponent<Block>().attack(damage,this) && pointingDown)
                {                    
                        if (isPlayer1)
                            playerGO.GetComponent<Player1>().destroyedBelow();
                        else
                            playerGO.GetComponent<Player2>().destroyedBelow();                                   
                   
                }
                target = null;
            }
            
            resetDrillState ();
        }
        
                
    }

    void FixedUpdate()
    {

        //will charge as long as you're holding the correct button. increases the damage 
        if(Input.GetKey(drilluse) && !overheated)
        {
            damage += drillDmgRamp;          

            /*if (damage < 0.5f)
                resetDrillState ();
            else*/ if (damage > 0.5f && damage < 2f)
                drillState.color = buildUp;
            else if (damage > 1.25f && damage < 3f)
                drillState.color = ideal;
            else if (damage > 4f)
                StartCoroutine(overheat(overheatTime));

            Material newMaterial = new Material (Shader.Find ("Specular"));
            newMaterial.color = overHeatColor;

            foreach (MeshRenderer mr in rendDrill) {
                mr.material = drillState;
            }
        }
        

        

    }

    //the next set of functions change the way the drill is facing
    public void PointDown()
    {
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, 0));
        pointingDown = true;
    }
    public void PointLeft()
    {
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, -90));
        pointingDown = false;
    }

    public void PointRight()
    {
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, 90));
        pointingDown = false;
    }
    public void PointUp()
    {
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, 180));
        pointingDown = false;
    }



    //finds the targets in each direction and sets them to the appropriate variable
    public void getTargets()
    {
       
        Collider[] cols = Physics.OverlapSphere(transform.position, 1.5f);
        List<GameObject> gos = new List<GameObject>();
        foreach(Collider collide in cols)
        {
            if(collide.gameObject.layer==LayerMask.NameToLayer("Playing field"))
            {
                gos.Add(collide.gameObject);               
            }
        }
        left = null;
        Right = null;
        Up = null;
        Down = null;
        foreach(GameObject GO in gos)
        {
            if (GO.transform.position.x < transform.position.x - .5)
                left = GO;
            else if (GO.transform.position.x > transform.position.x + .5)
                Right = GO;
            else if (GO.transform.position.y > transform.position.y + .5)
                Up = GO;
            else if (GO.transform.position.y < transform.position.y - .5)
                Down = GO;
        }
    }
       

    //the enumerator function overheats the drill then waits until the overheat is done then goes back to idle
    public IEnumerator overheat(float overheatTime){
        
        overheated = true;
        drillState.color = overHeatColor;
        cockpitState.color = overHeatColor;
        if(isPlayer1)
            playerGO.GetComponent<Player1>().S.State = state.overheat;
        else
            playerGO.GetComponent<Player2>().S.State = state.overheat;

        yield return new WaitForSeconds(overheatTime);

        if (isPlayer1)
            playerGO.GetComponent<Player1>().S.State = state.idle;
        else
            playerGO.GetComponent<Player2>().S.State = state.idle;
        overheated = false;
        resetDrillState();
        
    }

    //resets the drill back to 0 damage finds the new targets and changes its color back
    void resetDrillState(){
        drillState.color = defaultDrillColor;
        cockpitState.color = defaultCockpitColor;
        damage = 0;
        getTargets();
    }
    
}
