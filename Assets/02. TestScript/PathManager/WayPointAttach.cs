using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointAttach : MonoBehaviour
{
    // 찾을 프리팹 태그
    public string targetTag = "WayPoint";
    // 이름을 부여할 때 사용할 기본 이름
    public string baseName = "WayPoint";

    void Awake()
    {
        // 태그를 가진 모든 B 오브젝트들을 찾습니다.
        GameObject[] objectsToReparent = GameObject.FindGameObjectsWithTag(targetTag);

        // 이름을 부여할 때 사용할 카운터
        int counter = 0;

        // 각 B 오브젝트를 A 오브젝트의 자식으로 설정하고 이름을 변경합니다.
        foreach (GameObject obj in objectsToReparent)
        {
            // 이름을 WayPoint (1), WayPoint (2) ... 이런 식으로 설정합니다.
            obj.name = baseName + " (" + counter + ")";
            counter++;

            // B 오브젝트를 A 오브젝트의 자식으로 설정합니다.
            obj.transform.SetParent(this.transform);
        }
    }
}
