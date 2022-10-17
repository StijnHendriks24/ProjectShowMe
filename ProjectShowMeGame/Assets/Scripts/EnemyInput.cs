using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyInput : AiInput
{
    public Transform target;

    public override void Start()
    {
        Target = target;

        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }
}
