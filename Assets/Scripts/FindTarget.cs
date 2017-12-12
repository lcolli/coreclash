using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTarget : StateMachineBehaviour {

    GameObject[] possibleTargets=new GameObject[12];
    private float upperY, sameY, middleY, bottomY;
    private float leftX=-9, midX=-7, rightX=-5;
    GameObject NPC,goal;
    
    int[] priorities=new int[12];
    private Animator anim;
    private AIPlayer AI;
    
    

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        
        
        for(int i=0; i<12;i++)
        {
            possibleTargets[i] = null;
        }
        List<GameObject> UnsortedTargets = new List<GameObject>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            if (player.name == "AI Player")
                NPC = player;
        }
        AI = NPC.GetComponent<AIPlayer>();
        float x = -7, z = 0;
        float y = Mathf.Floor(NPC.transform.position.y);
        Vector3 center = new Vector3(x, y, z);
        Vector3 half =new Vector3 (2, 3, .5f);
        Collider[] surroundings = Physics.OverlapBox(center, half,new Quaternion(0,0,0,0));
        foreach(Collider col in surroundings)
        {
            if (col.gameObject.tag == "Goal")
                goal = col.gameObject;
            if(col.gameObject.layer == LayerMask.NameToLayer("Playing field"))
            {
               
                UnsortedTargets.Add(col.gameObject);
            }
        }
        AI.ResetPos();
            center += new Vector3(.1f, .5f, 0);
            sameY = y + 1;
            upperY = sameY + 2;
            middleY = sameY - 2;
            bottomY = middleY - 2;
            
            foreach (GameObject GO in UnsortedTargets)
            {
           
                Vector3 GOPos = GO.transform.position;
                if (IsSpot(GOPos, upperY, leftX))
                    possibleTargets[0] = GO;
                else if (IsSpot(GOPos, upperY, midX))
                    possibleTargets[1] = GO;
                else if (IsSpot(GOPos, upperY, rightX))
                    possibleTargets[2] = GO;
                else if (IsSpot(GOPos, sameY, leftX))
                    possibleTargets[3] = GO;
                else if (IsSpot(GOPos, sameY, midX))
                    possibleTargets[4] = GO;
                else if (IsSpot(GOPos, sameY, rightX))
                    possibleTargets[5] = GO;
                else if (IsSpot(GOPos, middleY, leftX))
                    possibleTargets[6] = GO;
                else if (IsSpot(GOPos, middleY, midX))
                    possibleTargets[7] = GO;
                else if (IsSpot(GOPos, middleY, rightX))
                    possibleTargets[8] = GO;
                if (IsSpot(GOPos, bottomY, leftX))
                    possibleTargets[9] = GO;
                else if (IsSpot(GOPos, bottomY, midX))
                    possibleTargets[10] = GO;
                else if (IsSpot(GOPos, bottomY, rightX))
                    possibleTargets[11] = GO;
            }
            AI.SetTargets(possibleTargets);
            for(int i=0;i<12;i++)
            {
                priorities[i] = FindPriority(possibleTargets[i]);
            }

        if (goal != null)
            AI.SetTarget(goal);
        else
            AI.SetTarget(ParsePriorities());

            //state transitions
            
        
    }

    /*
    int figureOutValue(GameObject left,GameObject middle,GameObject right)
    {
        int which=0;
        int priorityL=FindPriority(left);
        int priorityR=FindPriority(right);
        int PriorityM=FindPriority(middle);
        switch(NPC.GetComponent<Player>().pos)
        {
            case (position.left):
                break;
            case position.right:
                
                break;
            case position.middle:
                
                break;                
        }
        return which;
    }*/

    int ParsePriorities()
    {
        int highest = 0;
        for (int i = 0; i<4; i++)
        {
            
                priorities[i*3+0] += i;
                priorities[i*3+1] += i;
                priorities[i*3+2] += i;

                       
            switch (NPC.GetComponent<Player>().pos)
            { case (position.left):
                    priorities[i*3 + 1] -= 1;
                    priorities[i*3 + 2] -= 2;
                    break;
                case position.right:
                    priorities[i*3 + 1] -= 1;
                    priorities[i*3 + 0] -= 2;
                    break;
                case position.middle:
                    priorities[i * 3 + 2] -= 1;
                    priorities[i * 3 + 0] -= 1;
                    break;
            }

            if (priorities[i * 3 + 0] > priorities[highest])
                highest = i * 3 + 0;
            if (priorities[i * 3 + 1] > priorities[highest])
                highest = i * 3 + 1;
            if (priorities[i * 3 + 2] > priorities[highest])
                highest = i * 3 + 2;
        }

        return highest;
    }


    int FindPriority(GameObject GO)
    {
        int priority=-100;            
        if(GO!=null)
            switch (GO.tag)
            {
                case "Dirt":
                    priority=6;
                    break;

                case "Diamond":
                    priority = 11;                
                    break;

                case "Gold":                
                    priority=10;
                    break;

                case "Magma":
                    priority = -7;
                    break;

                case "Metal":
                    priority = 2;
                    break;

                case "Sand":
                    priority = 6;
                    break;

                case "Stone":
                    priority = 4;
                    break;

                case "Treasure":
                    //treasure stats
                   priority= 8;
                    break;

                case "Water":
                    priority = -5;
                    break;

                default:
                    break;

        }
        return priority;
    }

    bool IsSpot(Vector3 target,float y,float x)
    {
        
        if (target.x==x && target.y > y - .4f && target.y < y+.4f)
            return true;
        else
            return false;
    }

}
