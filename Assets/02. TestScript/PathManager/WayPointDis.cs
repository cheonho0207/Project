using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WayPoint : MonoBehaviour
{
    // WayPoint ���� �Ÿ� ��꿡 �ʿ��� �޼���
    public float DistanceTo(WayPoint other)
    {
        return Vector3.Distance(this.transform.position, other.transform.position);
    }
}