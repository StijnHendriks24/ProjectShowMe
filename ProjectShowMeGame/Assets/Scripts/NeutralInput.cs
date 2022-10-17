using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NeutralInput : AiInput
{
    public Transform[] targets;
    public float targetCompletionDistance = 10;

    private int targetIndex = 0;

    public override void Start()
    {
        Target = targets[targetIndex];
        
        base.Start();
    }

    public override void Update()
    {
        if (Vector3.Distance(transform.position, Target.position) < targetCompletionDistance)
        {
            targetIndex++;
            if (targetIndex >= targets.Length)
                targetIndex = 0;

            Target = targets[targetIndex];
        }

        base.Update();
    }
}
