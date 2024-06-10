using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WayPoint : MonoBehaviour
{
    // WayPoint 간의 거리 계산에 필요한 메서드
    public float DistanceTo(WayPoint other)
    {
        return Vector3.Distance(this.transform.position, other.transform.position);
    }
}