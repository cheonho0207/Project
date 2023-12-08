using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;

    [Header("Attributes")]
    public float range = 1.7f;
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
    public float upwardForce = 5.0f;

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
        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if(nearestEnemy != null && shortestDistance <= range)
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

        Vector3 dir = target.position - transform.position;
        Quaternion LookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = LookRotation.eulerAngles;
        // Shooting 애니메이션을 위해 y축 각도에 50도를 더합니다.
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
            bullet.transform.localRotation = Quaternion.Euler(80, 40, 0);
            // 터렛의 현재 방향을 고려하여 총알의 로컬 회전을 설정합니다.
            bullet.transform.rotation = firePoint.rotation;

            // 전방 힘을 계산하고 적용합니다.
            Vector3 forwardForce = bullet.transform.forward * bulletSpeed; // z축을 사용하여 위로 향하게 수정
            bulletRigidbody.AddForce(forwardForce, ForceMode.Impulse);

            // 상향 힘을 계산하고 적용합니다. (조절이 필요할 수 있습니다.)
            Vector3 upwardForceVector = Vector3.up * (upwardForce / 2); // 강도를 조절
            bulletRigidbody.AddForce(upwardForceVector, ForceMode.Impulse);
        }

        // 일정 시간이 지난 후에 총알을 파괴합니다.
        Destroy(bullet, 0.4f);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

   
}
