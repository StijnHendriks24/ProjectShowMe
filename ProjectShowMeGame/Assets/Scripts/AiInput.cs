using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiInput : MonoBehaviour
{
    public float turnThreshold = 3;
    private float steering;
    private float throttle = 1;
    private CarController carController;
    protected Transform Target;
    private NavMeshAgent agent;

    public virtual void Start()
    {
        carController = GetComponent<CarController>();
        carController.autoReset = true;

        agent = GetComponentInChildren<NavMeshAgent>();

        agent.SetDestination(Target.position);
    }

    public virtual void Update()
    {
        agent.transform.localPosition = Vector3.zero;

        agent.SetDestination(Target.position);

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
