using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [Header("Scene Objects")]
    public Transform player;
    public Transform[] objectsToMark;

    [Header("Minimap")]
    public bool rotateMinimap = false;
    public Transform minimap;
    public Camera minimapCamera;
    public GameObject markerPrefab;
    public RectTransform markerParentRectTransform;

    private RectTransform[] markedObjects;

    void Awake()
    {
        markedObjects = new RectTransform[objectsToMark.Length];

        for (int i = 0; i < objectsToMark.Length; i++)
        {
            RectTransform rectTransform = Instantiate(markerPrefab, markerParentRectTransform).GetComponent<RectTransform>();
            markedObjects[i] = rectTransform;
        }
    }

    void Update()
    {
        for (int i = 0; i < markedObjects.Length; i++)
        {
            Vector3 offset = Vector3.ClampMagnitude(objectsToMark[i].transform.position - player.transform.position, minimapCamera.orthographicSize);
            offset = offset / minimapCamera.orthographicSize * (markerParentRectTransform.rect.width / 2.2f);
            markedObjects[i].anchoredPosition = new Vector2(offset.x, offset.z);
        }

        transform.position = player.position + Vector3.up * 5f;

        if(rotateMinimap)
            RotateOverlay();
    }

    private void RotateOverlay() 
    {
        minimap.localRotation = Quaternion.Euler(0, 0, player.eulerAngles.y);
    }
}
