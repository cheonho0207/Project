using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointAttach : MonoBehaviour
{
    // ã�� ������ �±�
    public string targetTag = "WayPoint";
    // �̸��� �ο��� �� ����� �⺻ �̸�
    public string baseName = "WayPoint";

    void Awake()
    {
        // �±׸� ���� ��� B ������Ʈ���� ã���ϴ�.
        GameObject[] objectsToReparent = GameObject.FindGameObjectsWithTag(targetTag);

        // �̸��� �ο��� �� ����� ī����
        int counter = 0;

        // �� B ������Ʈ�� A ������Ʈ�� �ڽ����� �����ϰ� �̸��� �����մϴ�.
        foreach (GameObject obj in objectsToReparent)
        {
            // �̸��� WayPoint (1), WayPoint (2) ... �̷� ������ �����մϴ�.
            obj.name = baseName + " (" + counter + ")";
            counter++;

            // B ������Ʈ�� A ������Ʈ�� �ڽ����� �����մϴ�.
            obj.transform.SetParent(this.transform);
        }
    }
}
