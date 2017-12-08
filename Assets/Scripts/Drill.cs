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
    public float overclockMult = 2;
    

    
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
    public bool  shielded;                          //if the drill currently has a shield
    public bool diamond, overclocked;
    public GameObject left, Right, Down, Up;        //the blocks in the specified direction
    public KeyCode drilluse=KeyCode.Space;          // the key that is used to operate the drill
    public bool overheated;                         //wheter or not the drill is overheated
    public int overclockCount=0, overClockLimit=0;  //the overclock mechanic, count is how many frames its been overclocked
                                                    //and limit is maximum fixed frames
    float overclockDown=0f;                         //the amount of time the player overheats after an overclock
    float drillDmgMem;                              //place holder for the drill damage ramp
    string pointing;                                //the direction the drill is pointing
    public CoreClash game;



    //if the player is shielded it will use the shield and return true else it will return false
    public bool useShield()
    {
        if (!shielded)
            return false;
        else
        {
            if (isPlayer1)
            {
                playerGO.GetComponent<Player1>().UsePowerup();                
            }
            else
            {
                
                playerGO.GetComponent<Player2>().UsePowerup();
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
        diamond = false;
        overclocked = false;
        drillDmgMem = drillDmgRamp;
        
    }

    //gets the power up. just a middle function between block and the player
    public void addPowerUp()
    {
        
            playerGO.GetComponent<Player>().getPowerup();
        
    }

    
	
	// Update is called once per frame
	void Update () {
        if (game.State == gamestate.playing)
        {
            getTargets();//figures out the blocks around the player


            //will attack when you release the spacebar
            if (Input.GetKeyUp(drilluse) && !overheated)
            {
                if (target != null)
                {

                    if (diamond)//if the diamond drill is activated it will attack the target and the one behind it
                    {

                        Vector3 start = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                        Vector3 direction = target.transform.position;
                        Vector3 length = (direction - start) * 2;
                        Collider[] cols = Physics.OverlapCapsule(start, start + length, .1f); List<GameObject> gos = new List<GameObject>();
                        foreach (Collider collide in cols)
                        {
                            if (collide.gameObject.layer == LayerMask.NameToLayer("Playing field"))
                            {

                                gos.Add(collide.gameObject);
                            }
                        }

                        foreach (GameObject GO in gos)
                        {
                            if (GO.name != "Rig")
                            {

                                GO.GetComponent<Block>().attack(damage, this, pointing);
                            }
                        }
                        diamond = false;
                    }
                    else if (target.GetComponent<Block>().attack(damage, this, pointing) && pointing == "down")
                    {

                        playerGO.GetComponent<Player>().destroyedBelow();


                    }
                    getTargets();
                    target = null;
                }

                resetDrillState();
            }
        }
                
    }

    void FixedUpdate()
    {
        if (game.State == gamestate.playing)
        {
            //ends the overclock
            if (overclockCount > overClockLimit)
            {
                overClockLimit = 0;
                overclockCount = 0;
                StartCoroutine(overheat(overclockDown));
                overclockDown = 0;
                overclocked = false;
                drillDmgRamp = drillDmgMem;
            }
            //if its overclocked will add 1 more frame tot he overclock count
            if (overclocked)
                overclockCount++;

            //will charge as long as you're holding the correct button. increases the damage 
            if (Input.GetKey(drilluse) && !overheated)
            {
                damage += drillDmgRamp;

                /*if (damage < 0.5f)
                    resetDrillState ();
                else*/
                if (damage > 0.5f && damage < 2f)
                    drillState.color = buildUp;
                else if (damage > 1.25f && damage < 3f)
                    drillState.color = ideal;
                else if (damage > 4f)
                    StartCoroutine(overheat(overheatTime));

                Material newMaterial = new Material(Shader.Find("Specular"));
                newMaterial.color = overHeatColor;

                foreach (MeshRenderer mr in rendDrill)
                {
                    mr.material = drillState;
                }
            }
        }

    }

    //the next set of functions change the way the drill is facing
    public void PointDown()
    {
        target = Down;
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, 0));
        pointing = "down";
        HighlightTarget();
    }
    public void PointLeft()
    {
        target = left ;
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, -90));
        pointing = "left";
        HighlightTarget();
    }

    public void PointRight()
    {
        target = Right;
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, 90));
        pointing = "right";
        HighlightTarget();
    }
    public void PointUp()
    {
        target = Up;
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, 180));
        pointing = "up";
        HighlightTarget();
    }



    //finds the targets in each direction and sets them to the appropriate variable
    public void getTargets()
    {
        left = null;
        Right = null;
        Up = null;
        Down = null;
        Vector3 offset = new Vector3(transform.position.x,transform.position.y,transform.position.z);
        offset += new Vector3(0, -.5f, 0);
        Collider[] cols = Physics.OverlapSphere(offset, 1.1f);
        List<GameObject> gos = new List<GameObject>();
        foreach(Collider collide in cols)
        {
            if(collide.gameObject.layer==LayerMask.NameToLayer("Playing field"))
            {
                gos.Add(collide.gameObject);               
            }
        }
        
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
       
    //a method other objects can call to overheat this player
    public void OverheatLink(float overHeatTime)
    {
        StartCoroutine(overheat(overHeatTime));
    }

    //starts the overclock, takes a time in frames for length and a float in seconds for how long it will overheat after
    //will then add these values to the current limit and downtime
    public void Overclock(int time,float downtime)
    {
        overclockDown += downtime;
        overClockLimit += time;
        if(!overclocked)
            drillDmgRamp *= overclockMult;
        overclocked = true;
    }

    //the enumerator function overheats the drill then waits until the overheat is done then goes back to idle
    public IEnumerator overheat(float overheatTime){
        if (!overclocked)
        {
            S.overheated = true;
            //S.drillState.color = overHeatColor;
            //S.cockpitState.color = overHeatColor;
            HighlightTarget();
            
           
            playerGO.GetComponent<Player>().State = state.overheat;

            yield return new WaitForSeconds(overheatTime);
            
            playerGO.GetComponent<Player>().State = state.idle;
            S.overheated = false;
            resetDrillState();
        }
        
    }

    void HighlightTarget()
    {
        if(target==null || overheated)
        {
            playerGO.GetComponent<Player>().HighlightTarget(new Vector3(0,0,-100),pointing,diamond);
        }
        else
        {
            playerGO.GetComponent<Player>().HighlightTarget(target.transform.position, pointing, diamond);
        }
    }

    
    
   

    //resets the drill back to 0 damage finds the new targets and changes its color back
    void resetDrillState(){
        //drillState.color = defaultDrillColor;
        //cockpitState.color = defaultCockpitColor;
        damage = 0;
        HighlightTarget();
        
    }
    
}
