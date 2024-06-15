using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;

    [Header("Attributes")]
    public float range = 1.2f;
    public float fireRate = 3f;
    private float fireCountdown = 0f;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnspeed = 60f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    private Animator playerAnimator;

    [Header("Bullet Attributes")]
    public float bulletSpeed = 3f;
    public float upwardForce = 4.0f;

    public GameObject sparkEffectPrefab;
    private GameObject sparkEffectInstance;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.03f);
        float updateRate = Mathf.Clamp(0.1f - GameObject.FindGameObjectsWithTag(enemyTag).Length * 0.01f, 0.02f, 0.1f);
        playerAnimator = GetComponent<Animator>();
        fireRate = 1f;
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            Vector3 positionDifference = transform.position - enemy.transform.position;
            positionDifference.y = 0;
            float distanceToEnemy = positionDifference.magnitude;

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            Debug.Log("적 감지됨: " + target.name); // 적 감지 시 콘솔 메시지 출력
            RotateAndShoot();
            if (!IsInvoking("FireArrowsContinuously"))
            {
                InvokeRepeating("FireArrowsContinuously", 0f, 1f / fireRate);
            }
        }
        else
        {
            target = null;
            CancelInvoke("FireArrowsContinuously");
        }
    }

    void Update()
    {
        fireCountdown -= Time.deltaTime;
    }

    void FireArrowsContinuously()
    {
        if (target != null)
        {
            RotateAndShoot();
        }
    }

    void RotateAndShoot()
    {
        if (target == null)
            return;

        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnspeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (fireCountdown <= 0f)
        {
            Shoot();
            playerAnimator.SetTrigger("Shooting");
            fireCountdown = 1f / fireRate;
        }
    }

    void Shoot()
    {
        if (firePoint == null)
        {
            Debug.LogError("포인트가 포탑에 할당되지 않았습니다.");
            return;
        }

        if (target == null)
        {
            Debug.LogError("타겟이 없습니다.");
            return;
        }

        // 타겟 방향으로 총알 발사
        Vector3 shootingDirection = (target.position - firePoint.position).normalized;

        // 총알 프리팹을 인스턴스화합니다.
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = shootingDirection * bulletSpeed;
        }

        // 일정 시간이 지난 후에 총알을 파괴합니다.
        Destroy(bullet, 0.4f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 gizmoPosition = transform.position - new Vector3(0, 0.6f, 0);
        Gizmos.DrawWireSphere(gizmoPosition, range);
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
}
