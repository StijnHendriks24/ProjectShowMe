using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public GameObject cameraObject;

    private float throttleInput = 0f;
    private float steerInput = 0f;
    private CarController carController;

    void Start()
    {
        carController = GetComponent<CarController>();
    }

    void Update()
    {
        throttleInput = Input.GetAxisRaw("Vertical");
        steerInput = Input.GetAxisRaw("Horizontal");

        carController.GiveInput(throttleInput, steerInput);

        // TODO: Properly implement screenshake
        if (Input.GetKeyDown(KeyCode.Space))
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
