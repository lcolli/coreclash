using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTarget : StateMachineBehaviour {

    GameObject NPC;
    AIPlayer AI;
    int[] path;
    int next;
    Block nextBlock;
    AIDrill drill;
    bool ready2Attack;
    bool targetBelow;
    private int unstuck;
   

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            if (player.name == "AI Player")
                NPC = player;
        }
        AI = NPC.GetComponent<AIPlayer>();
        next = 0;
        path = AI.getPath();
        getNextBlock();
        drill = AI.AIdrill;
       
    }

    public void getNextBlock()
    {
        unstuck = 0;
        if (next == path.Length)
        {
            AI.TargetEliminated();
        }
        else
        {
            //AI.PrintString(AI.GetPos().ToString()+" - "+path[next].ToString()+" = " +(AI.GetPos() - path[next]).ToString());
            switch (AI.GetPos() - path[next])
            {
                case 3:
                    AI.drill.PointUp();
                    break;
                case -6:
                case -3:
                    targetBelow = true;
                    AI.drill.PointDown();
                    break;
                case 1:
                    AI.drill.PointLeft();
                    break;
                case -1:
                    AI.drill.PointRight();
                    break;
            }
            if (AI.getTargets()[path[next]] == null)
                nextBlock = null;
            else
                nextBlock = AI.getTargets()[path[next]].GetComponent<Block>();
            next++;
        }
    }

    public void MoveToNextBlock()
    {
        ready2Attack = false;
        if(AI.GetPos()-path[next-1]==1)
        {
            AI.moveLeft();
            AI.currentPos -= 1;
        }
        else if(AI.GetPos() - path[next - 1] == -1)
        {
            AI.moveRight();
            AI.currentPos += 1;
        }

        getNextBlock();
    }

    public bool CheckForRelease(float dmg)
    {
        return (dmg >= nextBlock.health);
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        unstuck++;
        if (unstuck >= 750)
            AI.ForgetTarget();
        if (AI.State == state.idle)
        {
           
            if (nextBlock == null)
            {
                MoveToNextBlock();
            }
            if (drill.holding)
            {
               
                float dmg = AI.drill.damage;
               
                if (CheckForRelease(dmg) && !drill.overheated)
                {
                    

                    if (drill.diamond)//if the diamond drill is activated it will attack the target and the one behind it
                    {

                        Vector3 start = new Vector3(AI.transform.position.x, AI.transform.position.y, AI.transform.position.z);
                        Vector3 direction = nextBlock.transform.position;
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

                                GO.GetComponent<Block>().attack(dmg, AI.drill, AI.drill.pointing);
                            }
                        }
                        drill.diamond = false;
                    }
                    else if (targetBelow && nextBlock.attack(dmg, AI.drill, AI.drill.pointing))
                    {
                        AI.GetComponent<Player>().destroyedBelow();
                        targetBelow = false;
                        AI.currentPos += 3;

                        //getNextBlock();

                    }

                    //AI.PrintString(dmg.ToString());
                    drill.holding = false;
                    ready2Attack = false;
                    drill.resetDrillState();
                }
            }
            else if (ready2Attack)
            {
                
                drill.holding = true;
            }
            else
            {
                
                ready2Attack = true;
            }

            
        }
       

	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
