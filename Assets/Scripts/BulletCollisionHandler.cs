using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // 다른 콜라이더와 충돌하면 파괴
        Destroy(gameObject);
    }
}
