using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AIPlayer : Player {

    private Animator anim;
    public GameObject[] targets = new GameObject[12];
    public int[] path;
    private int targetBlock;
    public int currentPos;
    public AIDrill AIdrill;
    public bool victoryBlock;

    private void Awake()
    {
        this.gameObject.name = "AI Player";
        //S = this;
    }

    new void Start()
    {
        //this.name = "AI Player";
        victoryBlock = false;
        base.Start();
        GameObject[] rigs = GameObject.FindGameObjectsWithTag("Rig");
        foreach (GameObject trig in rigs)
        {
            if (trig.transform.IsChildOf(this.gameObject.transform))
            {
                print(trig.name);
                AIdrill = trig.GetComponent<AIDrill>();
                AIdrill.drilluse = drilluse;
            }
        }
        anim = GetComponent<Animator>();
        anim.SetBool("isPlaying",false);        
    }

    new public void Update()
    {
        switch(powerup.name)
        {
            case "Diamond":
                UsePowerup();
                break;
            case "Dynamite":
                UsePowerup();
                break;
            case "EMP":
                UsePowerup();
                break;
            case "Grapple":
                if (game.player1.GetComponent<Player1>().powerup.name != "none")
                    UsePowerup();
                break;
            case "Overclock":
                UsePowerup();
                break;
            default:
                break;
        }
    }

    new public void DynamiteBlast()
    {
        base.DynamiteBlast();
        ForgetTarget();
    }

    new public void UsePowerup()
    {
        powerup.use(this);
    }

    public void ForgetTarget()
    {
        anim.SetTrigger("Forget Target");
    }

    new public void Shift()
    {
        base.Shift();
        anim.SetTrigger("Forget Target");
    }

    public void PrintString(string String)
    {
        print(String);
    }

    public void resetTarget()
    {
        path = null;
        ResetPos();
        targetBlock = -1;
        targets = null;
    }

    public void Victory()
    {
        //trigger victory
        anim.SetTrigger("Victory");
    }

    public void TargetEliminated()
    {
        //trigger to target reached
        anim.SetTrigger("Target Eliminated");
    }

    public void SetPath(int[] path)
    {        
        this.path = path;
        //trigger move to start moving
        anim.SetTrigger("Path Found");
    }

    public Block GetBlock(int i)
    {
        if (targets[i] == null)
            return null;        
        return targets[i].GetComponent<Block>();
    }

    public void ResetPos()
    {
        switch(pos)
        {
            case position.right:
                currentPos = 5;
                break;
            case position.left:
                currentPos = 3;
                break;
            default:
                currentPos = 4;
                break;
        }
        
    }

    new public void destroyedBelow()
    {
        //currentPos -= 3;
        anim.SetBool("idle", false);
        base.destroyedBelow();        
       
    }

    public int GetPos()
    {
        return currentPos;
    }

    public void SetTarget(GameObject goal)
    {
        victoryBlock = true;
        anim.SetTrigger("Target Found");
        //set victory block = to true
    }

    public void SetTarget(int target)
    {        
        targetBlock = target;
        
        //trigger move to determine path
        anim.SetTrigger("Target Found");
    }

    public int GetTarget()
    {
        return targetBlock;
    }

    public void SetTargets(GameObject[] targets)
    {
        this.targets = targets;
    }

    public GameObject[] getTargets()
    {
        return targets;
    }

    public int[] getPath()
    {
        return path;
    }

    //looks left and if it can move will start the moving
    new public void moveLeft()
    {       
               //changes the state
            if (pos == position.middle )
            {
                pos = position.left;
                StartMove(left);
            }
            else
            {
                pos = position.middle;
                StartMove(middle); //moves
            }
        //currentPos -= 1;
        
    }


    //works the same as moveleft but in the other direction
    new public void moveRight()
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
        //currentPos += 1;
    }

    new public void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        anim.SetBool("idle", true);

    }

}
