using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PowEffect : MonoBehaviour
{
    private Transform target;
    public GameObject sparkEffect;
    public GameObject sparkEffect2;

    private Rigidbody arrowRigidbody;
    private bool isArrowInactive = false;
    private bool alreadyProcessed = false;

    // ����� �ڽ��� ������ ������ ����
    public Color gizmoColor = Color.blue;

    // ����� �ڽ��� �������� ������ ����
    public float range = 3f;
    public string enemyTag = "Enemy";

    // �̹� ������ ����Ʈ�� �����ϱ� ���� ���� �߰�
    private bool hasSpawnedEffect = false;

    void Start()
    {
        arrowRigidbody = GetComponent<Rigidbody>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        if (isArrowInactive)
        {
            // Arrow�� ��Ȱ��ȭ�� ���¿��� 10�� �ڿ� �ı�
            Destroy(gameObject, 10f);
        }

        if (target != null && !hasSpawnedEffect) // �̹� ����Ʈ�� �������� ���� ��쿡�� ����
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget <= range)
            {
                Enemy enemyScript = target.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    // Enemy���� �������� ������
                    int damage = 5;
                    enemyScript.TakeDamage(damage);
                }

                // �Ʒ��� �̹� ������ �ڵ�
                // Arrow�� Rigidbody�� Kinematic���� ����
                if (arrowRigidbody != null)
                {
                    arrowRigidbody.isKinematic = true;
                }

                // Arrow�� ����� ��� ����Ʈ���� ����
                Effect[] effects = GetComponentsInChildren<Effect>();
                foreach (Effect effect in effects)
                {
                    if (effect != this) // ���� ����Ʈ ��ũ��Ʈ ��ü�� �������� �ʵ��� ó��
                    {
                        Destroy(effect.gameObject);
                    }
                }

                // ����ũ ����Ʈ ���� �� �˻�
                if (sparkEffect != null)
                {
                    Vector3 sparkEffectPosition = new Vector3(transform.position.x, 1.384f, transform.position.z);

                    GameObject sparkEffectInstance = Instantiate(sparkEffect, transform.position, Quaternion.identity);
                    Destroy(sparkEffectInstance, 3f);
                }
                else
                {
                    Debug.LogError("sparkEffect is not set. Please assign a valid GameObject to sparkEffect in the Inspector.");
                }

               if (sparkEffect2 != null)
{
    // ���� ��ġ���� y ���� 1.384�� �����Ͽ� �ν��Ͻ� ����
    Vector3 sparkEffect2Position = new Vector3(transform.position.x, 1.384f, transform.position.z);
    GameObject sparkEffectInstance2 = Instantiate(sparkEffect2, sparkEffect2Position, Quaternion.identity);
    Destroy(sparkEffectInstance2, 3f);
}
else
{
    Debug.LogError("sparkEffect2 is not set. Please assign a valid GameObject to sparkEffect2 in the Inspector.");
}

                isArrowInactive = true; // Arrow�� ��Ȱ��ȭ ���·� ǥ��
                hasSpawnedEffect = true; // ����Ʈ�� �����Ǿ����� ǥ��
            }
        }
    }

    // ������ �ڵ�� �����ϰ� �����˴ϴ�.

    // ����� �׸��� �ڵ�
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
