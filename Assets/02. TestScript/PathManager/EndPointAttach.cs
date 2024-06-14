using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointAttach : MonoBehaviour
{
    public string targetTag = "EndPoint";

    void Start()
    {
        GameObject[] endObject = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in endObject)
        {
            this.transform.SetParent(obj.transform);
            this.transform.position = obj.transform.position;
        }
    }
}
