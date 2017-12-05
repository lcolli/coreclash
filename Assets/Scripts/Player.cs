using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//used to control what the player is doing at the moment
public enum state
{
    idle,
    falling,
    overheat,
    moving
}
//shows if the player is in which column
public enum position
{
    left,
    right,
    middle
}


public class Player : MonoBehaviour {

    [Header("Set In Unity")]
    //public float movementSpeed = 4f;                    //how fast the drill moves
    //public int FramesBeforeMove = 5;                    //the delay in time from when the player moves to when they can move again
    public Vector3 left = new Vector3(5.3f, 0, 0);      //the left position
    public Vector3 right = new Vector3(9.3f, 0, 0);     //the right position
    public Vector3 middle = new Vector3(7.3f, 0, 0);    //the middle position
    public KeyCode moveleft = KeyCode.LeftArrow;
    public KeyCode moveright = KeyCode.RightArrow;
    public KeyCode lookup = KeyCode.UpArrow;
    public KeyCode drilluse = KeyCode.Space;
    public KeyCode usePU = KeyCode.G;                   //powerup usage
    public KeyCode pause = KeyCode.P;                   //the pause button


    [Header("Set Dynamically")]

    public Drill drill;                                 //link to the drill script attached to player1
    //public int framesTilMove;                           //timer to delay movement while holding down move left/right
    public state State;                                 //the state of the player, moving idle falling overheat
    public position pos;                                // the position on the track, left right or middle
    public Powerup powerup;                             //the power up slot for this player
    public bool playing;
    public Camera playerCam;
    bool paused = false;

    public float movementTime = .2f;
    //private float moveDistance = 2.0f;

    //private bool isMoving;

    private Vector3 startPos;
    private Vector3 endPos;

    private float timeStartedMoving;

    private void Start()
    {
        State = state.falling;                  //is currently starting in the air
        pos = position.middle;                  //and in the middle
        //framesTilMove = 0;
        powerup = this.GetComponent<Powerup>();

        Time.timeScale = 1;


        //this is going to find the drill that is a child of this player
        GameObject[] rigs = GameObject.FindGameObjectsWithTag("Rig");
        foreach (GameObject trig in rigs)
        {
            if (trig.transform.IsChildOf(this.gameObject.transform))
            {

                drill = trig.GetComponent<Drill>();
                drill.drilluse = drilluse;
            }
        }

    }

    public void Update()
    {
        if (Input.GetKeyDown(pause))
        {
            PauseGame();
        }

        
        //overrides the movement counter if players press an input again
        //if (Input.GetKeyDown(moveleft) || Input.GetKeyDown(moveright))
        //framesTilMove = FramesBeforeMove;
        /*if (Input.GetKey(moveleft) && (transform.position.x > left.x + 1) && !isMoving)
        {
            StartMoveLeft();
        }

        if (Input.GetKey(moveright) && (transform.position.x < right.x - 1) && !isMoving)
        {
            StartMoveRight();
        }*/



       

        //if falling or idle you can change the direction of the drill but wil only move if idle
        //defaults to the drill pointing down if nothing is pressed
        if (State != state.overheat && State!=state.moving)
        {
            if (Input.GetKey(lookup))
                drill.PointUp();
           else if (Input.GetKey(moveleft))
                moveLeft();
            else if (Input.GetKey(moveright))
                moveRight();
            else
               drill.PointDown();

        }
    }

    public void FixedUpdate(){

        //countdown until player can move again if holding down left or right
        //will give a delay so players can stop at middle
        //if (framesTilMove <= FramesBeforeMove)
        //framesTilMove++;


        //if the state is moving, will move to endpos over the set time in the beginning
        if (!paused)
        {
            if (State == state.moving)
            {
                //finding the current percentage of the total movement time has been completed
                float timeSinceStarted = Time.time - timeStartedMoving;
                float percentageComplete = timeSinceStarted / movementTime;

                //lerp
                transform.position = Vector3.Lerp(startPos, endPos, percentageComplete);

                //lerping ends
                //if(transform.position.x >= endPos.x){
                //    isMoving = false;
                //}

                if (timeSinceStarted >= movementTime)
                {
                    EndMove(); //this sets the x value to a specific x value;
                }
            }
        }
    }



    //sets the start position the end position and starts the moving state
    void StartMove(Vector3 dest)
    {
        State = state.moving;
       //drill.PointLeft();
        timeStartedMoving = Time.time;

        startPos = transform.position;
        //endPos = transform.position + Vector3.left * moveDistance;
        endPos = new Vector3(0, transform.position.y, transform.position.z)+dest;
    }

    /*void StartMoveRight()
    {
        State = state.moving;
        //drill.PointRight();
        timeStartedMoving = Time.time;

        startPos = transform.position;
        endPos = transform.position + Vector3.right * moveDistance;
    }*/


        //looks left and if it can move will start the moving
    void moveLeft()
    {
        //points left first then decides if it can move
        drill.PointLeft();
        if (pos != position.left && drill.left == null && State == state.idle)
        //if its not in the left most position and there's no block in the way
        {
            //changes the state
            if (pos == position.middle && drill.left == null)
            {
                pos = position.left;
                StartMove(left);
            }
            else
            {
                pos = position.middle;
                StartMove(middle); //moves
            }
        }
    }


    //works the same as moveleft but in the other direction
    void moveRight()
    {
        drill.PointRight();
        if (pos != position.right && drill.Right == null && State == state.idle)
        {
            if (pos == position.middle)
            {
                pos = position.right;
                StartMove(right);
            }
            else
            { 
                pos = position.middle;            
                StartMove(middle);
            }
        }
    }


    //placeholder for the child functions
    public void UsePowerup()
    {
        
    }


    //finishes the move, sets state back to idle and puts the player at a specific x value
    private void EndMove()
    {
        Vector3 destination = new Vector3(0, transform.position.y, transform.position.z);
        //gets the y and z values so that it doesn't change them

        //it adds the left right or middle vector depending on the state which is (x,0,0) to destinations y and z values
        switch (pos)
        {
            case position.left:
                destination += left;
                break;
            case position.right:
                destination += right;
                break;
            case position.middle:
                destination += middle;
                break;
       }
        transform.position = destination;   //puts the player at the appropriate position
        drill.getTargets();                 //gets the drill to find the possible targets around it
        State = state.idle;
        //framesTilMove = 0;                  //resets the counter

    }

    //if something below is destroyed the rig will fall. updates the state
    public void destroyedBelow()
    {
        playerCam.GetComponent<FollowCam>().moveCam();
        if(State!=state.overheat)
            State = state.falling;
    }

    //for when the block stops falling turns state back to idle
    public void OnCollisionEnter(Collision collision)
    {

        if (State == state.falling)
        {
            drill.getTargets();
            State = state.idle;

        }
    }

    //uses the power up that is attached to this player if it has one
    

    //changes the power up from none to whatever value it is
    public void getPowerup()
    {
        drill.shielded = false;
        powerup.getPowerup();
        if (powerup.Name == "Shield")
            drill.shielded = true;
    }


    //if something falls on the target it will go to the space available to the left or right of it
    public void Shift()
    {
        
        switch(pos)
        {
            case position.left:
               
                pos = position.middle;
                EndMove();
                break;
            case position.right:
                
                pos = position.middle;
                EndMove();
                break;
            case position.middle:
               
                if (drill.left == null)
                    pos = position.left;
                else
                    pos = position.right;
                EndMove();
                break;
        }
    }


    //the dynamite powerup, attacks everything in a 9x9 square around the player
    public void DynamiteBlast()
    {
        
        Vector3 half = new Vector3(1.5f, 1.5f, 0.1f);
        Vector3 centeroffset = new Vector3(0f, .5f, 0f);

        Collider[] cols = Physics.OverlapBox(transform.position + centeroffset, half);

        List<GameObject> gos = new List<GameObject>();
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

                GO.GetComponent<Block>().attack(3f, drill,"up");
            }
        }
        State = state.falling;
    }
    
    
   
    //pauses the game
    private void PauseGame()
    {
        if(Time.timeScale==1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    } 

}
