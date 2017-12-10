using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTarget : StateMachineBehaviour {

    GameObject[] possibleTargets=new GameObject[12];
    private float upperY, sameY, middleY, bottomY;
    private float leftX=-9, midX=-7, rightX=-5;
    GameObject NPC;
    bool nearBottom;
    int[] priorities=new int[12];
    private Animator anim;
    
    

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        
        nearBottom = false;
        for(int i=0; i<12;i++)
        {
            possibleTargets[i] = null;
        }
        List<GameObject> UnsortedTargets = new List<GameObject>();
        NPC=GameObject.FindGameObjectWithTag("AI");
        float x = -7, z = 0;
        float y = Mathf.Floor(NPC.transform.position.y);
        Vector3 center = new Vector3(x, y, z);
        Vector3 half =new Vector3 (2, 4, .5f);
        Collider[] surroundings = Physics.OverlapBox(center, half,new Quaternion(0,0,0,0));
        foreach(Collider col in surroundings)
        {
            if (col.gameObject.tag == "Goal")
                nearBottom = true;
            if(col.gameObject.layer == LayerMask.NameToLayer("Playing field"))
            {
                UnsortedTargets.Add(col.gameObject);
            }
        }
        if (nearBottom)
        {

        }
        else
        {
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

            for(int i=0;i<12;i++)
            {
                priorities[i] = FindPriority(possibleTargets[i]);
            }

            int targetNumber = ParsePriorities();

            //state transitions
            switch(targetNumber)
            {
                case 0:
                    
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;
                case 10:
                    break;
                case 11:
                    break;
            }
        }
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
            priorities[i*3 + 0] -= i;
            priorities[i*3 + 1] -= 1;
            priorities[i*3 + 2] -= 1;
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
        int priority=0;
        if (GO == null)
            priority = -100;
        else
            switch (GO.tag)
            {
                case "Dirt":
                    priority=4;
                    break;

                case "Diamond":
                    priority = 6;                
                    break;

                case "Gold":                
                    priority=6;
                    break;

                case "Magma":
                    priority = -4;
                    break;

                case "Metal":
                    priority = 2;
                    break;

                case "Sand":
                    priority = 4;
                    break;

                case "Stone":
                    priority = 3;
                    break;

                case "Treasure":
                    //treasure stats
                   priority= 5;
                    break;

                case "Water":
                    priority = -3;
                    break;

                default:
                    break;

        }
        return priority;
    }

    bool IsSpot(Vector3 target,float x,float y)
    {
        if (target.x>x-.4f && target.x<x+.5f && target.y > y - .4f && target.y < y)
            return true;
        else
            return false;
    }

}
