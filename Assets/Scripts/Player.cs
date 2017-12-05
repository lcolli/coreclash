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
    public float movementSpeed = 2f;                    //how fast the drill moves
    public int FramesBeforeMove = 5;                    //the delay in time from when the player moves to when they can move again
    public Vector3 left = new Vector3(5.3f, 0, 0);      //the left position
    public Vector3 right = new Vector3(9.3f, 0, 0);     //the right position
    public Vector3 middle = new Vector3(7.3f, 0, 0);    //the middle position
    public KeyCode moveleft = KeyCode.LeftArrow;
    public KeyCode moveright = KeyCode.RightArrow;
    public KeyCode lookup = KeyCode.UpArrow;
    public KeyCode drilluse = KeyCode.Space;
    public KeyCode usePU = KeyCode.G;


    [Header("Set Dynamically")]

    public Drill drill;                                 //link to the drill script attached to player1
    public int framesTilMove;                           //timer to delay movement while holding down move left/right
    public state State;                                 //the state of the player, moving idle falling overheat
    public position pos;                                // the position on the track, left right or middle
    public Powerup powerup;                             //the power up slot for this player
    public bool playing;
    public Camera playerCam;

    private float movementTime = 1.0f;
    private float moveDistance = 2.0f;

    private bool isMoving;

    private Vector3 startPos;
    private Vector3 endPos;

    private float timeStartedMoving;

    private void Start()
    {
        State = state.falling;
        pos = position.middle;
        framesTilMove = 0;
        powerup = this.GetComponent<Powerup>();


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


        //overrides the movement counter if players press an input again
        //if (Input.GetKeyDown(moveleft) || Input.GetKeyDown(moveright))
        //framesTilMove = FramesBeforeMove;
        if (Input.GetKey(moveleft) && (transform.position.x > left.x + 1) && !isMoving)
        {
            StartMoveLeft();
        }

        if (Input.GetKey(moveright) && (transform.position.x < right.x - 1) && !isMoving)
        {
            StartMoveRight();
        }

       

        //if countdown isn't done it won't do anything but check until
        //if (State == state.moving)
        //{
        //    if (framesTilMove >= FramesBeforeMove)
        //        State = state.idle;
        //}
        //else if (State != state.overheat)
        //{
        //    if (Input.GetKey(lookup))
        //        drill.PointUp();
        //    else if (Input.GetKey(moveleft))
        //        moveLeft();
        //    else if (Input.GetKey(moveright))
        //        moveRight();
        //    else
        //        drill.PointDown();

        //}
    }

    public void FixedUpdate(){

        //countdown until player can move again if holding down left or right
        //will give a delay so players can stop at middle
        //if (framesTilMove <= FramesBeforeMove)
            //framesTilMove++;
        
        if (isMoving)
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
            if (percentageComplete > 1.0f)
            {
                isMoving = false;
            }
        }
    }

    void StartMoveLeft(){
        isMoving = true;
        drill.PointLeft();
        timeStartedMoving = Time.time;

        startPos = transform.position;
        endPos = transform.position + Vector3.left * moveDistance;
    }

    void StartMoveRight()
    {
        isMoving = true;
        drill.PointRight();
        timeStartedMoving = Time.time;

        startPos = transform.position;
        endPos = transform.position + Vector3.right * moveDistance;
    }

    //void moveLeft()
    //{
    //    //points left first then decides if it can move
    //    drill.PointLeft();
    //    if (pos != position.left && drill.left == null && State == state.idle)
    //    //if its not in the left most position and there's no block in the way
    //    {
    //        //changes the state
    //        if (pos == position.middle && drill.left == null)
    //            pos = position.left;
    //        else
    //            pos = position.middle;
    //        Move(); //moves
    //    }
    //}


    ////works the same as moveleft but in the other direction
    //void moveRight()
    //{
    //    drill.PointRight();
    //    if (pos != position.right && drill.Right == null && State == state.idle)
    //    {
    //        if (pos == position.middle)
    //            pos = position.right;
    //        else
    //            pos = position.middle;
    //        Move();
    //    }
    //}

    public void UsePowerup()
    {
        
    }



    //private void Move()
    //{
    //    Vector3 destination = new Vector3(0, transform.position.y, transform.position.z);
    //    //gets the y and z values so that it doesn't change them

    //    //it adds the left right or middle vector depending on the state which is (x,0,0) to destinations y and z values
    //    switch (pos)
    //    {
    //        case position.left:
    //            destination += left;
    //            break;
    //        case position.right:
    //            destination += right;
    //            break;
    //        case position.middle:
    //            destination += middle;
    //            break;
    //    }
    //    transform.position = destination;   //puts the player at the appropriate position
    //    drill.getTargets();                 //gets the drill to find the possible targets around it
    //    State = state.moving;               // changes the state to moving to add a delay
    //    framesTilMove = 0;                  //resets the counter
       
    //}

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

    public void Shift()
    {
        
        switch(pos)
        {
            case position.left:
               
                pos = position.middle;
                //Move();
                break;
            case position.right:
                
                pos = position.middle;
                //Move();
                break;
            case position.middle:
               
                if (drill.left == null)
                    pos = position.left;
                else
                    pos = position.right;
                //Move();
                break;
        }
    }

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
    
   public bool isBelow(Vector3 blockPos)
    {
        return true;
    }

}
