using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NeutralInput : AiInput
{
    public bool random = false;
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
            if (!random)
            {
                targetIndex++;
                if (targetIndex >= targets.Length)
                    targetIndex = 0;
            }
            else
            {
                targetIndex = Random.Range(0, targets.Length);
            }

            Target = targets[targetIndex];
        }

        base.Update();
    }
}
