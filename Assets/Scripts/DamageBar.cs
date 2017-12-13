using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageBar : MonoBehaviour
{

    public GameObject bar;
    public GameObject backgroundbar;
    public int player;
    public Material barMat;
    public Material backMat;
    public KeyCode key;

    private Player playerScript;
    private float barSpeed = 0.35f;
    private bool active;
    private float dmg;
    private Vector3 tempScale;
    private Drill drill;
    float maxLength=4f;

    public void Start()
    {

        

        if (player == 1)
            playerScript = GetComponent<Player1>();
        else if (player == 2)
            playerScript = GetComponent<Player2>();

        drill = playerScript.drill;

        bar = Instantiate(bar,this.gameObject.transform) as GameObject;
        backgroundbar = Instantiate(backgroundbar, this.gameObject.transform) as GameObject;
        bar.GetComponent<MeshRenderer>().material = barMat;
        backgroundbar.GetComponent<MeshRenderer>().material = backMat;
        backgroundbar.transform.localPosition = new Vector3(-1.13f, -7, -6);
        backgroundbar.transform.localScale = new Vector3(0, .4f, .4f);
        bar.transform.localPosition = new Vector3(-1.13f, -8, -6);
        bar.transform.localScale = new Vector3(0, .2f, .2f);
        backMat.color = Color.white;
    }

    void Update()
    {

        
        tempScale = new Vector3(0, .5f, .5f);   
        
        if (drill.damage > 0)
        {
            float x;
            if (drill.damage < 3)
            {
                 x = (drill.damage / 3) * maxLength;
            }
            else
            {
                x= maxLength;
            }
            
            tempScale.x += x;///3*maxLength;
            bar.transform.localScale = tempScale;
            
        }
        /*else if (drill.damage >= 0 && playerScript.State != state.overheat)
        {
            tempScale = bar.transform.localScale;
            tempScale.x -= drill.damage * barSpeed;
            bar.transform.localScale = tempScale;
        }*/

        if (drill.damage==0)
        {
            tempScale.x = .1f;
            bar.transform.localScale = tempScale;
        }
        backgroundbar.transform.localScale = tempScale + new Vector3(.2f, .2f, .2f);

        if (drill.damage >=1 && drill.damage < 2)
            barMat.color = Color.green;
        else if (drill.damage >= 2 && drill.damage < 3)
            barMat.color = Color.yellow;
        else if (drill.damage >= 3 && drill.damage < 4)
            barMat.color = Color.red;
        else if (drill.overheated)
            barMat.color = Color.black;
        else
            barMat.color = Color.blue;

    }
}