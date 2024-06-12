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
            // B 오브젝트를 A 오브젝트의 자식으로 설정합니다.
            this.transform.SetParent(obj.transform);
            this.transform.position = obj.transform.position;
        }
    }
}
