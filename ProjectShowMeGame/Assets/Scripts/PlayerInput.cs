using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public GameObject cameraObject;

    private float throttleInput = 0f;
    private float steerInput = 0f;
    private CarController carController;

    [Header("Audio")]
    public bool debugAudio = false;
    public float collisionForceThreshold = 200;
    public AudioClip collisionClip;
    public AudioSource effectsAudioSource;

    private float nextTimeToEffect;

    void Start()
    {
        carController = GetComponent<CarController>();

        carController.SetupConnection(CollisionOccured);
    }

    void Update()
    {
        throttleInput = Input.GetAxisRaw("Vertical");
        steerInput = Input.GetAxisRaw("Horizontal");

        carController.GiveInput(throttleInput, steerInput);
    }

    void CollisionOccured(float impactForce)
    {
        if (Time.time < nextTimeToEffect)
            return;

        // TODO: Combine with AI

        if (debugAudio)
        {
            Debug.Log(gameObject.name + " ImpactForce: " + impactForce);
        }

        if (impactForce < collisionForceThreshold)
            return;

        effectsAudioSource.clip = collisionClip;
        effectsAudioSource.Play();

        nextTimeToEffect = Time.time + 0.2f;

        StartCoroutine(Shake(0.15f, 0.1f));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 orignalPosition = cameraObject.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cameraObject.transform.localPosition = new Vector3(x, y, orignalPosition.z);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        cameraObject.transform.localPosition = orignalPosition;
    }
}
