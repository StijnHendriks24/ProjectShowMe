using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float Acceleration = 15f;
    public float TopSpeed = 30f;
    public float GripX = 15f;
    public float GripZ = 5f;
    public float Rotate = 120f;
    public float RotVel = 0.8f;

    public float AngDragGround = 5.0f;
    public float AngDragAir = 0.05f;
    public float distToGround = 1f;

    private float MinRotSpd = 1f;

    private Rigidbody rigidBody;

    private float rotate;
    private float accel;
    private float gripX;
    private float gripZ;
    private float rotVel;

    private float isRight = 1.0f;
    private float isForward = 1.0f;

    private float throttle = 0f;
    private float steering = 0f;

    private bool isRotating = false;
    private bool isGrounded = true;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private Vector3 vel = new Vector3(0f, 0f, 0f);
    private Vector3 velLocal = new Vector3(0f, 0f, 0f);

    [HideInInspector] public bool autoReset = false;
    private bool inReset = false;
    private bool isStuck = false;

    Vector3 drot = new Vector3(0f, 0f, 0f);

    [Header("Audio")]
    public bool debugAudio = false;
    public float collisionForceThreshold = 200;
    public AudioClip collisionClip;
    public AudioClip engineRunningClip;
    public AudioSource effectsAudioSource;
    public AudioSource engineAudioSource;

    private float nextTimeToEffect;
    private float engineRunningVolume;
    private Coroutine engineVolumeCoroutine;

    void Start()
    {
        engineAudioSource.clip = engineRunningClip;
        engineRunningVolume = engineAudioSource.volume;
        engineAudioSource.Play();

        rigidBody = GetComponent<Rigidbody>();

        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (transform.position.y < -10)
        {
            transform.position = startPosition;
            transform.rotation = startRotation;

            rigidBody.velocity = new Vector3();
        }
    }

    void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, -transform.up, distToGround + 0.1f);
        if (!isGrounded)
        {
            accel = 0f;
            gripX = 0f;
            gripZ = 0f;
            rigidBody.angularDrag = AngDragAir;
        }
    }

    void FixedUpdate()
    {
        accel = Acceleration;
        gripX = GripX;
        gripZ = GripZ;
        rotate = Rotate;
        rigidBody.angularDrag = AngDragGround;

        CheckGrounded();

        CalculateRotation();

        Controller();

        AlterVelocity();

        rigidBody.velocity = transform.TransformDirection(vel);
    }

    public void GiveInput(float throttle, float steering)
    {
        if (throttle > 0 || throttle < 0)
        {
            if(engineVolumeCoroutine != null)
                StopCoroutine(engineVolumeCoroutine);
            engineVolumeCoroutine = StartCoroutine(ChangeEngineVolume(engineRunningVolume, 1));
        }
        else
        {
            if (engineVolumeCoroutine != null)
                StopCoroutine(engineVolumeCoroutine);
            engineVolumeCoroutine = StartCoroutine(ChangeEngineVolume(engineRunningVolume / 2, 1));
        }

        this.throttle = throttle;
        this.steering = steering;
    }

    void RotateGradConst(float isCW)
    {
        drot.y = isCW * rotate * Time.deltaTime;
        transform.rotation *= Quaternion.AngleAxis(drot.y, transform.up);
        isRotating = true;
    }

    void AlterVelocity()
    {
        vel = transform.InverseTransformDirection(rigidBody.velocity);

        if (isRotating)
        {
            vel = vel * (1 - rotVel) + velLocal * rotVel;
        }

        isRight = vel.x > 0f ? 1f : -1f;
        vel.x -= isRight * gripX * Time.deltaTime;
        if (vel.x * isRight < 0f) vel.x = 0f;

        isForward = vel.z > 0f ? 1f : -1f;
        vel.z -= isForward * gripZ * Time.deltaTime;
        if (vel.z * isForward < 0f) vel.z = 0f;

        vel = new Vector3(vel.x, vel.y, Mathf.Clamp(vel.z, -TopSpeed, TopSpeed));
    }

    void CalculateRotation()
    {
        if (velLocal.magnitude < MinRotSpd)
        {
            rotate = 0f;
        }
        else
        {
            rotate = Mathf.Abs(velLocal.z) / 10 * rotate;
        }

        if (rotate > Rotate)
            rotate = Rotate;
    }

    void Controller()
    {
        if (throttle > 0.5f || throttle < -0.5f)
        {
            rigidBody.velocity += accel * throttle * Time.deltaTime * transform.forward;
            gripZ = 0f;
        }

        if (autoReset)
        {
            if (velLocal.magnitude <= 0.15f)
            {
                inReset = isStuck;
                isStuck = true;
            }
            else
            {
                isStuck = false;
            }
        }

        if (inReset)
        {
            float y = transform.eulerAngles.y;
            transform.eulerAngles = new Vector3(0, y, 0);
            rigidBody.velocity = new Vector3(0, -1f, 0);
            transform.position += Vector3.up * 2;
            inReset = false;
        }

        isRotating = false;

        velLocal = transform.InverseTransformDirection(rigidBody.velocity);

        if (steering > 0.5f || steering < -0.5f)
        {
            float dir = (velLocal.z < 0) ? -1 : 1;
            RotateGradConst(steering * dir);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Time.time < nextTimeToEffect)
            return;

        float ImpactForce = (collision.impulse / Time.fixedDeltaTime).magnitude;
        
        if (debugAudio)
        {
            Debug.Log(gameObject.name + " ImpactForce: " + ImpactForce);
        }

        if (ImpactForce > collisionForceThreshold)
        {
            effectsAudioSource.clip = collisionClip;
            effectsAudioSource.Play();
        }

        nextTimeToEffect = Time.time + 0.2f;
    }

    private IEnumerator ChangeEngineVolume(float end, float duration)
    {
        float startVolume = engineAudioSource.volume;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float newVolume = Mathf.Lerp(startVolume, end, t / duration);
            engineAudioSource.volume = newVolume;
            yield return null;
        }
    }
}
