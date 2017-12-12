using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDrill : Drill
{
   public bool release,holding;

    new private void Start()
    {
        base.Start();
        release = false;
        holding = false;
    }


    new private void Update()
    {
        /*if (release && !overheated)
        {
            release = false;
            
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
            holding = false;
        }*/
    }

    private void FixedUpdate()
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
            if (holding && !overheated)
            {


                damage += drillDmgRamp;
                playerGO.GetComponent<Player>().Revving(damage / overheatdmg);

                /*if (damage < 0.5f)
                    resetDrillState ();
                else*/

                if (damage > overheatdmg)
                    StartCoroutine(overheat(overheatTime));

                //Material newMaterial = new Material(Shader.Find("Specular"));
                //newMaterial.color = overHeatColor;

               
            }
        }
    }
}