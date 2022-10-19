using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpeech : MonoBehaviour
{
    public Image iconImage;
    public Sprite[] speechSprites;
    public Vector2 minMaxChangeTime = new Vector2(2, 5);

    private float nextChangeIn;

    void Start()
    {
        ChangeSpeech();
    }

    void Update()
    {
        Vector3 rotation = Quaternion.LookRotation(Camera.main.transform.position).eulerAngles;
        rotation.y = 0f;
        rotation.z = 0f;

        transform.rotation = Quaternion.Euler(rotation);
        //transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);

        nextChangeIn -= Time.deltaTime;
        if(nextChangeIn <= 0)
        {
            ChangeSpeech();
        }
    }

    void ChangeSpeech()
    {
        nextChangeIn = Random.Range(minMaxChangeTime.x, minMaxChangeTime.y);
        iconImage.sprite = speechSprites[Random.Range(0, speechSprites.Length)];
    }
}
