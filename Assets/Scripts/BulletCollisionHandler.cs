using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // �ٸ� �ݶ��̴��� �浹�ϸ� �ı�
        Destroy(gameObject);
    }
}
