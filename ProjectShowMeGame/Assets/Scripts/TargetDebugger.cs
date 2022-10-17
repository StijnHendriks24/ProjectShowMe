using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDebugger : MonoBehaviour
{
    public GameObject linerendererPrefab;
    private List<Transform> targets = new List<Transform>();

    private GameObject lineHolder;

    void Start()
    {
        lineHolder = new GameObject("Line Holder");

        int children = transform.childCount;
        for (int i = 0; i < children; i++)
            targets.Add(transform.GetChild(i));

        for (int i = 0; i < targets.Count - 1; i++)
        {
            LineRenderer lr = Instantiate(linerendererPrefab, lineHolder.transform).GetComponent<LineRenderer>();
            lr.SetPosition(0, targets[i].position);
            lr.SetPosition(1, targets[i + 1].position);
        }

        LineRenderer lr2 = Instantiate(linerendererPrefab, lineHolder.transform).GetComponent<LineRenderer>();
        lr2.SetPosition(0, targets[targets.Count - 1].position);
        lr2.SetPosition(1, targets[0].position);

        Destroy(linerendererPrefab);
    }
}
