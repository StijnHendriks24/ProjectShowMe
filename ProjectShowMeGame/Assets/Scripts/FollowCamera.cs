using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour 
{
    public Transform target;

    [Tooltip("Local offset vs target.")]
    public Vector3 relative = new Vector3(0, 0, 0);

    [Tooltip("Global offset vs target.")]
    public Vector3 offset = new Vector3(0, 5, -5);

    [Tooltip("Static rotation target (degree).")]
    public Vector3 Rotation = new Vector3(30, 0, 0);

    [Tooltip("Approximately the time it will take to reach the target.")]
    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    void FixedUpdate() 
    {
        Vector3 targetPosition = target.TransformPoint(relative) + offset;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        transform.LookAt(target);
    }
}