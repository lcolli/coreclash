using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : Player {

    private Animator anim;


    new void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }




}
