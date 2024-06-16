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

    public Color gizmoColor = Color.blue;
    public float range = 3f;
    public string enemyTag = "Enemy";
    public float speed = 10f; // 포탄의 속도

    private bool hasSpawnedEffect = false;

    void Start()
    {
        arrowRigidbody = GetComponent<Rigidbody>();
        SetInitialVelocity();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void SetInitialVelocity()
    {
        Vector3 direction = transform.forward; // 기본 방향은 forward
        direction.y = 0; // Y축 이동을 원하지 않는다면 이 줄을 추가하여 Y축 이동을 제거합니다.
        direction.Normalize(); // 방향을 정규화하여 속도 설정 시 일정한 속도로 이동하게 함

        arrowRigidbody.velocity = direction * speed;
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
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
            Destroy(gameObject, 10f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !hasSpawnedEffect)
        {
            ProcessCollision(collision.gameObject);
        }
    }

    private void ProcessCollision(GameObject enemy)
    {
        TestEnemy enemyScript = enemy.GetComponent<TestEnemy>();
        if (enemyScript != null)
        {
            int damage = 5;
            enemyScript.TakeDamage(damage);
        }

        if (arrowRigidbody != null)
        {
            arrowRigidbody.isKinematic = true;
        }

        Effect[] effects = GetComponentsInChildren<Effect>();
        foreach (Effect effect in effects)
        {
            if (effect != this)
            {
                Destroy(effect.gameObject);
            }
        }

        if (sparkEffect != null)
        {
            Vector3 sparkEffectPosition = new Vector3(transform.position.x, 1.384f, transform.position.z);
            GameObject sparkEffectInstance = Instantiate(sparkEffect, sparkEffectPosition, Quaternion.identity);
            Destroy(sparkEffectInstance, 3f);
        }
        else
        {
            Debug.LogError("sparkEffect is not set. Please assign a valid GameObject to sparkEffect in the Inspector.");
        }

        if (sparkEffect2 != null)
        {
            Vector3 sparkEffect2Position = new Vector3(transform.position.x, 1.384f, transform.position.z);
            GameObject sparkEffectInstance2 = Instantiate(sparkEffect2, sparkEffect2Position, Quaternion.identity);
            Destroy(sparkEffectInstance2, 3f);
        }
        else
        {
            Debug.LogError("sparkEffect2 is not set. Please assign a valid GameObject to sparkEffect2 in the Inspector.");
        }

        isArrowInactive = true;
        hasSpawnedEffect = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, range);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= range && !hasSpawnedEffect)
            {
                ProcessCollision(enemy);
            }
        }
    }
}
