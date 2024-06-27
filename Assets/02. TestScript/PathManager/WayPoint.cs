using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public float DistanceTo(WayPoint other)
    {
        return Vector3.Distance(this.transform.position, other.transform.position);
    }
}