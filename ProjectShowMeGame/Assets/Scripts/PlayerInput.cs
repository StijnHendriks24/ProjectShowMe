using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
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
    }
}
