using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndGoal : MonoBehaviour
{
    public float requiredTime = 5;
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public TextMeshProUGUI timerText;

    private bool playerEntered = false;
    private float currentTime = 0;
    private List<GameObject> currentEnemies = new List<GameObject>();
    private int numberOfEnemies = 0;

    private void Start()
    {
        timerText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerEntered)
        {
            currentTime -= Time.deltaTime;
            timerText.text = currentTime.ToString("F");
            if(currentTime <= 0)
            {
                // TODO: Win?
                Debug.Log(currentEnemies.Count);
                Debug.Break();
            }
        }
        else
            currentTime = requiredTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if((playerLayer & (1 << other.gameObject.layer)) != 0)
        {
            playerEntered = true;
            timerText.gameObject.SetActive(true);
        }
        else if ((enemyLayer & (1 << other.gameObject.layer)) != 0)
        {
            if (!currentEnemies.Contains(other.gameObject))
                currentEnemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((playerLayer & (1 << other.gameObject.layer)) != 0)
        {
            playerEntered = false;
            timerText.gameObject.SetActive(false);
        }
        else if ((enemyLayer & (1 << other.gameObject.layer)) != 0)
        {
            if (currentEnemies.Contains(other.gameObject))
                currentEnemies.Remove(other.gameObject);
        }
    }
}
