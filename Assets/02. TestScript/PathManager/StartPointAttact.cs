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
            // B 오브젝트를 A 오브젝트의 자식으로 설정합니다.
            this.transform.SetParent(obj.transform);
            this.transform.position = obj.transform.position;
        }
    }
}
