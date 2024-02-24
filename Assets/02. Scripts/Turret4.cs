using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Turret4 : MonoBehaviour
{
     private Transform target;

    [Header("Attributes")]
    public float range = 1.7f;
    public float fireRate = 4f;
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
    public float bulletSpeed = 2f; // 수정: 총알 속도
    public float upwardForce = 4f;

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
            Debug.LogError("FirePoint가 터렛에 할당되지 않았습니다.");
            return;
        }

        float[] shootAngles = new float[] { -30f, -20f, 0, 20f, 30f };

        foreach (float angle in shootAngles)
        {
            Quaternion additionalRotation = Quaternion.Euler(0, angle, 0);
            Quaternion bulletRotation = firePoint.rotation * additionalRotation;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            Collider bulletCollider = bullet.GetComponent<Collider>();

            if (bulletCollider != null)
            {
                bulletCollider.enabled = false; // 화살의 Collider를 일시적으로 비활성화합니다.
                StartCoroutine(ActivateColliderAfterDelay(bulletCollider, 0.2f)); // 0.2초 후에 Collider를 활성화합니다.
            }

            if (bulletRigidbody != null)
            {
                float increasedBulletSpeed = 5f;
                Vector3 forwardForce = bullet.transform.forward * increasedBulletSpeed;
                bulletRigidbody.AddForce(forwardForce, ForceMode.Impulse);
                Vector3 upwardForceVector = Vector3.up * (upwardForce / 2); // 강도를 조절
                bulletRigidbody.AddForce(upwardForceVector, ForceMode.Impulse);
            }

            Destroy(bullet, 0.8f);
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
    IEnumerator ActivateColliderAfterDelay(Collider collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        collider.enabled = true;
    }

}
