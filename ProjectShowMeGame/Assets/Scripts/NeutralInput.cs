using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NeutralInput : MonoBehaviour
{
    public Transform[] targets;
    public float turnThreshold = 10f;
    public float targetCompletionDistance = 10;
    private float steering;
    private float throttle = 1;
    private CarController carController;
    private Transform target;
    private int targetIndex;
    private NavMeshAgent agent;

    void Start()
    {
        carController = GetComponent<CarController>();
        carController.autoReset = true;

        agent = GetComponentInChildren<NavMeshAgent>();

        target = targets[targetIndex];
        agent.SetDestination(target.position);
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, target.position) < targetCompletionDistance)
        {
            targetIndex++;
            if (targetIndex >= targets.Length)
                targetIndex = 0;
            target = targets[targetIndex];

            agent.SetDestination(target.position);
        }

        Vector3 normalizedMovement = agent.desiredVelocity.normalized;

        float angle = AngleOffset(Angle2Points(transform.position, transform.position + normalizedMovement * 3), 0);

        Vector3 rot = transform.eulerAngles;
        float delta = Mathf.DeltaAngle(rot.y, angle);

        if (delta > turnThreshold)
            steering = 1f;
        else if (delta < -turnThreshold)
            steering = -1f;
        else
            steering = 0f;

        carController.GiveInput(throttle, steering);
    }

    float Angle2Points(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(b.x - a.x, b.z - a.z) * Mathf.Rad2Deg;
    }

    float AngleOffset(float raw, float offset)
    {
        raw = (raw + offset) % 360;
        if (raw > 180.0f) raw -= 360.0f;
        if (raw < -180.0f) raw += 360.0f;
        return raw;
    }
}
