using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageBar : MonoBehaviour
{

    public GameObject bar;
    public int player;
    public Material barMat;
    public KeyCode key;

    private Player playerScript;
    private float barSpeed = 0.35f;
    private bool active;
    private float dmg;
    private Vector3 tempScale;

    public void Start()
    {
        if (player == 1)
            playerScript = GetComponent<Player1>();
        else if (player == 2)
            playerScript = GetComponent<Player2>();

        bar = Instantiate(bar) as GameObject;
        bar.GetComponent<MeshRenderer>().material = barMat;
        bar.transform.localScale = new Vector3(0, 0.2f, 1);
    }

    void Update()
    {

        bar.transform.position = new Vector3(transform.position.x - 0.4f, transform.position.y + 2, -2);

        if (Input.GetKey(key) && tempScale.x < 1.5f && playerScript.State == state.idle)
        {
            tempScale = bar.transform.localScale;
            tempScale.x += Time.deltaTime * barSpeed;
            bar.transform.localScale = tempScale;
        }
        else if (tempScale.x >= 0 && playerScript.State != state.overheat)
        {
            tempScale = bar.transform.localScale;
            tempScale.x -= Time.deltaTime * barSpeed;
            bar.transform.localScale = tempScale;
        }

        if (tempScale.x < 0)
        {
            tempScale.x = 0;
            bar.transform.localScale = tempScale;
        }

        if (tempScale.x >= 0.175f && tempScale.x < 0.35f)
            barMat.color = Color.green;
        else if (tempScale.x >= 0.35f && tempScale.x < 0.525f)
            barMat.color = Color.yellow;
        else if (tempScale.x >= 0.525f && tempScale.x < 0.7f)
            barMat.color = Color.red;
        else if (tempScale.x >= 0.7f)
            barMat.color = Color.black;
        else
            barMat.color = Color.blue;

    }
}