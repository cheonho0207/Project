using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Turret2 : MonoBehaviour
{
    private List<Transform> target = new List<Transform>();
    private Transform SpPoint;

    [Header("Attributes")]
    public float range = 3f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnspeed = 10f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    private Animator anim;

    [Header("Bullet Attributes")]
    public float bulletSpeed = 10f;
    public float upwardForce = 6.0f;

    private bool arrowSpawned = false;

    public float DestroyTime = 3.0f;
    public float turretUpwardForce = 10.0f;

    private Material sparkEffectMaterial;
    public GameObject sparkEffectPrefab;
    private GameObject sparkEffectInstance;
    private Motion2_2 motionScript;

    public GameObject sparkEffect2;
    private GameObject sparkEffectInstance2;

    private float fadeSpeed = 2f;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        anim = GetComponent<Animator>();
        SpPoint = GameObject.Find("SpPoint").transform;
        motionScript = GameObject.FindObjectOfType<Motion2_2>();
        if (motionScript == null)
        {
            Debug.LogError("씬에서 Motion2_2 스크립트를 찾을 수 없습니다.");
        }
        sparkEffect2.SetActive(false);
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
            target.Add(nearestEnemy.transform);
        }
    }

    void Update()
    {
        if (target.Count == 0)
            return;

        for (int i = target.Count - 1; i >= 0; i--)
        {
            Transform currentTarget = target[i];
            if (currentTarget == null)
            {
                target.RemoveAt(i);
                continue;
            }

            TestEnemy enemyScript = currentTarget.GetComponent<TestEnemy>();
            if (enemyScript != null && enemyScript.GetHealth() > 0)
            {
                Vector3 dir = currentTarget.position - transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                Vector3 rotation = lookRotation.eulerAngles;
                rotation.y += 90f;
                partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

                if (sparkEffect2.activeSelf && dir.magnitude <= range)
                {
                    if (fireCountdown <= 0f)
                    {
                        Shoot(currentTarget.position);
                        anim.SetTrigger("Attack");
                        fireCountdown = 1f / fireRate;
                    }
                }
                else if (!sparkEffect2.activeSelf)
                {
                    sparkEffect2.SetActive(true);
                    Invoke("DeactivateSparkEffect2", 3f);
                }
            }
        }
        fireCountdown -= Time.deltaTime;
    }

    void Shoot(Vector3 targetPosition)
    {
        if (firePoint == null)
        {
            Debug.LogError("포인트가 포탑에 할당되지 않았습니다.");
            return;
        }

        if (target.Count == 0)
            return;

        motionScript.TriggerSparkEffects();

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        if (bulletRigidbody != null)
        {
            Vector3 targetDirection = targetPosition - firePoint.position;
            bulletRigidbody.rotation = Quaternion.LookRotation(targetDirection.normalized);

            Vector3 forwardForce = targetDirection.normalized * bulletSpeed;
            bulletRigidbody.AddForce(forwardForce, ForceMode.Impulse);

            Vector3 upwardForceVector = Vector3.up * (upwardForce / 2);
            bulletRigidbody.AddForce(upwardForceVector, ForceMode.Impulse);
        }

        Destroy(bullet, 2f);

        target.RemoveAt(0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void DeactivateSparkEffect()
    {
        GameObject effect1 = transform.Find("Effect1").gameObject;
        if (effect1 != null)
        {
            effect1.SetActive(false);
        }
    }

    public void ActivateSparkEffect()
    {
        if (sparkEffectPrefab != null)
        {
            sparkEffectInstance = Instantiate(sparkEffectPrefab, transform.position, Quaternion.identity);
            sparkEffectInstance.SetActive(true);
        }
    }

    private void DeactivateSparkEffect2()
    {
        if (sparkEffect2.activeSelf)
        {
            sparkEffect2.SetActive(false);
        }
    }

}
