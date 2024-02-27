using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Turret2 : MonoBehaviour
{

    private Transform target;

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
    public int power3;
    private Transform SpPoint;

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
        power3 = 550;
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
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        if (target == null)
            return;

        Enemy enemyScript = target.GetComponent<Enemy>();
        if (enemyScript != null && !enemyScript.GetComponent<Tween_Path>().HasReachedEnd() && enemyScript.Hp > 0)
        {
            Vector3 dir = target.position - transform.position;
            Quaternion LookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = LookRotation.eulerAngles;
            rotation.y += 90f;
            partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            // 타겟과의 거리를 계산합니다.
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // 기즈모 박스 영역 안에 있거나 거리가 사정 거리 이내인 경우에만 발사합니다.
            if (sparkEffect2.activeSelf && distanceToTarget <= range)
            {
                if (fireCountdown <= 0f)
                { 
                    Shoot();
                    anim.SetTrigger("Attack");
                    fireCountdown = 1f / fireRate;
                }
            }
            else if (!sparkEffect2.activeSelf)
            {
                sparkEffect2.SetActive(true);
                Invoke("DeactivateSparkEffect2", 3f);
            }


            fireCountdown -= Time.deltaTime;
        }
    }

    void Shoot()
    {
        if (firePoint == null)
        {
            Debug.LogError("포인트가 포탑에 할당되지 않았습니다.");
            return;
        }

        // 타겟이 없으면 발사하지 않습니다.
        if (target == null)
            return;

        // SparkEffect를 발동합니다.
        motionScript.TriggerSparkEffects();

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        if (bulletRigidbody != null)
        {
            // 타겟의 위치로 총알의 방향을 조정합니다.
            Vector3 targetDirection = target.position - firePoint.position;
            bulletRigidbody.rotation = Quaternion.LookRotation(targetDirection.normalized);

            // 총알 발사
            Vector3 forwardForce = targetDirection.normalized * bulletSpeed;
            bulletRigidbody.AddForce(forwardForce, ForceMode.Impulse);

            // 업워드 힘 추가
            Vector3 upwardForceVector = Vector3.up * (upwardForce / 2);
            bulletRigidbody.AddForce(upwardForceVector, ForceMode.Impulse);
        }

        Destroy(bullet, 2f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }


    public void DeactivateSparkEffect()
    {
        // Effect1 비활성화 또는 다른 로직 추가
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
