using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour 
{
    public Transform Target;

    [Tooltip("Use scene's current setup for offset.")]
    public bool UseInitial = false;

    [Tooltip("Local offset vs target.")]
    public Vector3 Relative = new Vector3(0, 0, 0);

    [Tooltip("Global offset vs target.")]
    public Vector3 Offset = new Vector3(0, 5, -5);

    [Tooltip("Static rotation target (degree).")]
    public Vector3 Rotation = new Vector3(30, 0, 0);

    [Tooltip("Approximately the time it will take to reach the target.")]
    public float smoothTime = 0.3F;

    private Vector3 relative;
    private Vector3 offset;
    private Vector3 rotation;
    private Vector3 velocity = Vector3.zero;


    void Start() 
    {
        if (UseInitial) 
        {
            relative = Relative;
            offset = transform.position - Target.transform.position;
            rotation = transform.rotation.eulerAngles;
        } 
        else 
        {
            relative = Relative;
            offset = Offset;
            rotation = Rotation;
        }
    }

    void FixedUpdate() 
    {
        Vector3 targetPosition = Target.TransformPoint(relative) + offset;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        transform.rotation = Quaternion.Euler(rotation);
    }
}