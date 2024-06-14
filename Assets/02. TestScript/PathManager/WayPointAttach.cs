using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointAttach : MonoBehaviour
{
    public string targetTag = "WayPoint";
    public string baseName = "WayPoint";

    void Awake()
    {
        GameObject[] objectsToReparent = GameObject.FindGameObjectsWithTag(targetTag);

        int counter = 0;

        foreach (GameObject obj in objectsToReparent)
        {
            obj.name = baseName + " (" + counter + ")";
            counter++;

            obj.transform.SetParent(this.transform);
        }
    }
}
