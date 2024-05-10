using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;

    [Header("Attributes")]
    public float range = 1.3f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;


    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnspeed = 10f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    Animator playerAnimator;
    // Start is called before the first frame update

    [Header("Bullet Attributes")]
    public float bulletSpeed = 3f; // 수정: 총알 속도
    public float upwardForce = 4.0f;

    public GameObject sparkEffectPrefab; // 이 줄 추가
    private GameObject sparkEffectInstance; // 이 줄 추가
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);

        playerAnimator = GetComponent<Animator>();
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
             Vector3 positionDifference = transform.position - enemy.transform.position;
        positionDifference.y = 0; // y축을 무시하고 계산
        float distanceToEnemy = positionDifference.magnitude; // Vector3.magnitude 사용
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

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        // 대상이 Enemy이고 적이 살아있으며 경로 끝에 도달하고 HP가 0이라면 공격하지 않습니다.
        Enemy enemyScript = target.GetComponent<Enemy>();
        if (enemyScript != null && !enemyScript.GetComponent<Tween_Path>().HasReachedEnd() && enemyScript.Hp > 0)
        {
            Vector3 dir = target.position - transform.position;
            Quaternion LookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = LookRotation.eulerAngles;
            rotation.y += 90f;
            partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            if (fireCountdown <= 0f)
            {
                Shoot();
                playerAnimator.SetTrigger("Shooting");
                fireCountdown = 1f / fireRate;
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

        // 총알 프리팹을 인스턴스화합니다.
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        if (bulletRigidbody != null)
        {
            // 총알의 로컬 회전 x 값을 80도로 설정합니다.
            bullet.transform.localRotation = Quaternion.Euler(80, 40, 0);
            Vector3 shootingDirection = partToRotate.forward;

            // 터렛의 현재 방향을 고려하여 총알의 전방 힘을 설정합니다.
            Vector3 forwardForce = firePoint.forward * bulletSpeed;
            bulletRigidbody.AddForce(forwardForce, ForceMode.Impulse);

            // 상향 힘을 계산하고 적용합니다. (조절이 필요할 수 있습니다.)
            Vector3 upwardForceVector = Vector3.up * (upwardForce / 2); // 강도를 조절
            bulletRigidbody.AddForce(upwardForceVector, ForceMode.Impulse);
        }

        // 일정 시간이 지난 후에 총알을 파괴합니다.
        Destroy(bullet, 0.4f);
    }

    class BulletCollisionHandler : MonoBehaviour
    {
        public void Initialize()
        {
            GetComponent<Collider>().isTrigger = true; // Start with trigger disabled
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                GetComponent<Collider>().isTrigger = false; // Enable trigger when hitting an enemy
            }
        }
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

}
