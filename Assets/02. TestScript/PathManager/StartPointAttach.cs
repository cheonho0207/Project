using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPointAttach : MonoBehaviour
{
    public string targetTag = "StartPoint";

    void Start()
    {
        GameObject[] startObject = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in startObject)
        {
            this.transform.SetParent(obj.transform);
            this.transform.position = obj.transform.position;
        }
    }
}
