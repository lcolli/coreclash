using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeterminePath : StateMachineBehaviour {

    GameObject NPC;
    AIPlayer AI;

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
        if (AI.victoryBlock)
        {
            VictoryRush();
        }
        else
        {
            switch (AI.GetTarget())
            {
                case 0:
                    switch (AI.pos)
                    {
                        case position.left:
                            NextToTarget();
                            break;
                        case position.right:
                            FarUpper();
                            break;
                        default:
                            UpperLeft();
                            break;
                    }
                    break;
                case 1:
                    switch (AI.pos)
                    {
                        case position.left:
                            UpperRight();
                            break;
                        case position.right:
                            UpperLeft();
                            break;
                        default:
                            NextToTarget();
                            break;
                    }
                    break;
                case 2:
                    switch (AI.pos)
                    {
                        case position.left:
                            FarUpper();
                            break;
                        case position.right:
                            NextToTarget();
                            break;
                        default:
                            UpperRight();
                            break;
                    }
                    break;
                case 3:
                    switch (AI.pos)
                    {
                        case position.right:
                            FarSide();
                            break;
                        default:
                            NextToTarget();
                            break;
                    }
                    break;
                case 4:
                    NextToTarget();
                    break;
                case 5:
                    switch (AI.pos)
                    {
                        case position.left:
                            FarSide();
                            break;
                        default:
                            NextToTarget();
                            break;
                    }
                    break;
                case 6:
                    switch (AI.pos)
                    {
                        case position.left:
                            NextToTarget();
                            break;
                        case position.right:
                            FarMid();
                            break;
                        default:
                            MidLeft();
                            break;
                    }
                    break;
                case 7:
                    switch (AI.pos)
                    {
                        case position.left:
                            MidRight();
                            break;
                        case position.right:
                            MidLeft();
                            break;
                        default:
                            NextToTarget();
                            break;
                    }
                    break;
                case 8:
                    switch (AI.pos)
                    {
                        case position.left:
                            FarMid();
                            break;
                        case position.right:
                            NextToTarget();
                            break;
                        default:
                            MidRight();
                            break;
                    }
                    break;
                case 9:
                    switch (AI.pos)
                    {
                        case position.left:
                            FarBelow();
                            break;
                        case position.right:
                            FarLow();
                            break;
                        default:
                            LowLeft();
                            break;
                    }
                    break;
                case 10:
                    switch (AI.pos)
                    {
                        case position.left:
                            LowRight();
                            break;
                        case position.right:
                            LowLeft();
                            break;
                        default:
                            FarBelow();
                            break;
                    }
                    break;
                case 11:
                    switch (AI.pos)
                    {
                        case position.left:
                            FarLow();
                            break;
                        case position.right:
                            FarBelow();
                            break;
                        default:
                            LowRight();
                            break;
                    }
                    break;

            }
        }
    }


    public void VictoryRush()
    {
        int offset = 0;
        switch(AI.pos)
        {
            
            case position.left:
                offset -=1;
                break;
            case position.right:
                offset += 1;
                break;
            case position.middle:
                break;
        }

        AI.SetPath(new int[] {7+offset,10+offset});

    }

    public void UpperLeft()
    {
         AI.SetPath(new int[] {AI.GetPos()-1 ,AI.GetTarget() });
    }
    

    public void UpperRight()
    {
        AI.SetPath(new int[] {AI.GetPos()+1 ,AI.GetTarget() });
    }

    public void FarUpper()
    {
        int offset = 0;
        if (AI.pos == position.left)
            offset += 2;
        AI.SetPath(new int[] {4,3+offset,AI.GetTarget() });
    }   

    public void FarLow()
    {
        
        int[][] paths = new int[6][];
        for(int i=0; i<6;i++)
        {
            paths[i] = new int[3];
        }        
        if(AI.pos==position.left)
        {
           
            paths[0] = new int[] {6,9,10, AI.GetTarget() };
            //SetPath(paths[0], paths);
            paths[1] = new int[] { 6,7,10, AI.GetTarget() };
            //SetPath(paths[1], paths);
            paths[2] = new int[] {4,7,10, AI.GetTarget() };
            //SetPath(paths[2], paths);
            paths[3] = new int[] {6,7,8, AI.GetTarget() };
            //SetPath(paths[3], paths);
            paths[4] = new int[] {4,7,8, AI.GetTarget() };
            //SetPath(paths[4], paths);
            paths[5] = new int[] {4,5,8, AI.GetTarget() };
            //SetPath(paths[5], paths);
        }
        else
        {
            paths[0] = new int[] { 8, 11, 10, AI.GetTarget() };
            //SetPath(paths[0], paths);
            paths[1] = new int[] { 8, 7, 10, AI.GetTarget() };
            //SetPath(paths[1], paths);
            paths[2] = new int[] { 4, 7, 10, AI.GetTarget() };
           //SetPath(paths[2], paths);
            paths[3] = new int[] { 8, 7, 6, AI.GetTarget() };
            //SetPath(paths[3], paths);
            paths[4] = new int[] { 4, 7, 6, AI.GetTarget() };
            //SetPath(paths[4], paths);
            paths[5] = new int[] { 4, 3, 6, AI.GetTarget() };
            //SetPath(paths[5], paths);
        }

        AI.SetPath(paths[ShortestPath(paths)]);

    }

    public void FarMid()
    {
        //three paths
        
       int[][] paths = new int[4][];        
        
        if(AI.pos==position.left)
        {
            
            paths[0] = new int[] { 6, 7, AI.GetTarget() };
            //SetPath(paths[0], paths);
            paths[1] = new int[] { 4, 7, AI.GetTarget() };
            //SetPath(paths[1], paths);
            paths[2] = new int[] {4,5, AI.GetTarget() };
            //SetPath(paths[2], paths);
            paths[3] = new int[] { 6, 9, 10, 11, AI.GetTarget() };
            //SetPath(paths[3], paths);
        }
        else
        {
            paths[0] = new int[] { 8, 7, AI.GetTarget() };
            //SetPath(paths[0], paths);
            paths[1] = new int[] { 4, 7, AI.GetTarget() };
            //SetPath(paths[1], paths);
            paths[2] = new int[] { 4,3, AI.GetTarget() };
            //SetPath(paths[2], paths);
            paths[3] = new int[] { 8, 11, 10, 9, AI.GetTarget() };
            //SetPath(paths[3], paths);
        }

        AI.SetPath(paths[ShortestPath(paths)]);       
    }

    public void FarSide()
    {        
            AI.SetPath(new int[] {4, AI.GetTarget()});       
    }

    public void FarBelow()
    {
       int[][] paths = new int[3][];
        paths[1] = new int[] { 4,7,10, AI.GetTarget() };

        int offset = 0;
        switch(AI.pos)
        {
            case position.left:
                offset -= 1;                 
                paths[2] = new int[] {4,5,8,11,10,AI.GetTarget()};
                
                //SetPath(paths[2], paths);
                break;
            case position.middle:
                paths[2] = new int[] { 3, 6, 9, AI.GetTarget() };
                paths[1] = new int[] { 5 , 8 , 11 , AI.GetTarget() };
                //SetPath(paths[2], paths);
                break;
            case position.right:
                offset += 1;
                paths[2] = new int[] {4,3,6,9,10, AI.GetTarget() };               
                //SetPath(paths[2], paths);
                break;
        }       
       
        //SetPath(paths[1], paths);
        paths[0] = new int[] {7+offset, AI.GetTarget()};

        AI.SetPath(paths[ShortestPath(paths)]);


    }

    public void LowRight()
    {
        int[][] paths = new int[4][];        
        int offset = 0;
        if (AI.pos == position.left)
        {
            offset -= 1;
        }

        
        paths[0] = new int[] { 7 + offset, 10 + offset, AI.GetTarget() };
        //SetPath(paths[0], paths);
        paths[1] = new int[] { 7 + offset, 8+ offset, AI.GetTarget() };
        //SetPath(paths[0], paths);
        paths[2] = new int[] { 5 + offset, 8 + offset, AI.GetTarget() };
        //SetPath(paths[0], paths);

        if (offset == -1)
        {
            paths[3] = new int[] { 4, 5, 8, 11, AI.GetTarget() };
        }
        else
        {
            paths[3] = new int[] { 3, 6, 9, 10, AI.GetTarget() };
        }
        //SetPath(paths[3], paths);

        AI.SetPath(paths[ShortestPath(paths)]);
    }

    public void LowLeft()
    {
        int[][] paths = new int[4][];        
        int offset = 0;
        if (AI.pos == position.right)
        {
            offset += 1;
        }
       
        paths[0] = new int[] { 7 + offset, 10 + offset, AI.GetTarget() };
        //SetPath(paths[0], paths);
        paths[1] = new int[] { 7 + offset, 6 + offset, AI.GetTarget() };
        //SetPath(paths[0], paths);
        paths[2] = new int[] { 3 + offset, 6+ offset, AI.GetTarget() };
        //SetPath(paths[0], paths);

        if (offset == -1)
        {
            paths[3] = new int[] {4,3,6,9, AI.GetTarget() };       
        }
        else
        {
            paths[3] = new int[] {5,8,11,10, AI.GetTarget() };
        }
        //SetPath(paths[3], paths);

        AI.SetPath(paths[ShortestPath(paths)]);

    }



    public void MidRight()
    {

        int[][] paths;
        int offset = 0;
        //GameObject[] RightPath;       
        if (AI.pos == position.left)
        {
            offset -= 1;
            paths = new int[2][];
        }
        else
        {
            paths = new int[3][];
        }
        paths[1] = new int[] { 5 + offset, AI.GetTarget() };
        //SetPath(LeftPath, GOs);
        paths[0] = new int[] { 7 + offset, AI.GetTarget() };
        if (offset == 0)
            paths[2] = new int[] { 3, 6, 9, 10,11, AI.GetTarget() };
        //SetPath(DownPath, GOs);
        AI.SetPath(paths[ShortestPath(paths)]);
    }


    public void MidLeft()
    {

        int[][] paths;
        int offset = 0;
        //GameObject[] RightPath;       
        if (AI.pos == position.right)
        {
            offset += 1;
            paths = new int[2][];
        }
        else
        {
            paths = new int[3][];
        }
        paths[1]= new int[] { 3 + offset, AI.GetTarget() };
        //SetPath(LeftPath, GOs);
        paths[0] = new int[] { 7 + offset, AI.GetTarget() };
        if (offset == 0)
            paths[2] = new int[] {5,8,11,10,9, AI.GetTarget()};
        //SetPath(DownPath, GOs);
        AI.SetPath(paths[ShortestPath(paths)]);
    }   

   public void NextToTarget()
    {
        AI.SetPath(new int[] { AI.GetTarget()});
    }

    public int PathHealth(int[] Path)
    {
        int health = 0;

        for(int i=0;i<Path.Length;i++)
        {
            if (AI.GetBlock(Path[i])!= null)
            {
                health += (int)AI.GetBlock(Path[i]).health;
                if (AI.GetBlock(Path[i]).tag == "Water")
                    health += 6;
                if (AI.GetBlock(Path[i]).tag == "Magma")
                    health += 2;
            }
        }
        return health;
    }

    public int ShortestPath(int[][] pathChoices)
    {
        int lowestPathNumber = 0;
        int lowestHealth = PathHealth(pathChoices[0]);

        for (int i = 1; i < pathChoices.Length; i++)
        {
            int temp = PathHealth(pathChoices[i]);
            if (temp < lowestHealth)
            {
                lowestHealth = temp;
                lowestPathNumber = i;
            }
        }
        return lowestPathNumber;
    }

   /* public void SetPath(GameObject[] path,int[] objects)
    {
        for (int i=0;i<path.Length;i++)
        {
            path[i] = AI.targets[objects[i]];
        }
    }*/

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

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
