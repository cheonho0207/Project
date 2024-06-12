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
            // B ������Ʈ�� A ������Ʈ�� �ڽ����� �����մϴ�.
            this.transform.SetParent(obj.transform);
            this.transform.position = obj.transform.position;
        }
    }
}
