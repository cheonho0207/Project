using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class StartPointAttact : MonoBehaviour
{
    public string targetTag = "StartPoint";

    void Start()
    {
        GameObject[] startObject = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in startObject)
        {
            // B ������Ʈ�� A ������Ʈ�� �ڽ����� �����մϴ�.
            this.transform.SetParent(obj.transform);
            this.transform.position = obj.transform.position;
        }
    }
}
