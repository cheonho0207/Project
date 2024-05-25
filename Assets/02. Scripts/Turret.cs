using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;

    [Header("Attributes")]
<<<<<<< HEAD
    public float range = 5;
    public float fireRate = 1f;
=======
    public float range = 1.3f;
    public float fireRate = 3f;
>>>>>>> seon2
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
    public float bulletSpeed = 3f; // ����: �Ѿ� �ӵ�
    public float upwardForce = 4.0f;

    public GameObject sparkEffectPrefab; // �� �� �߰�
    private GameObject sparkEffectInstance; // �� �� �߰�
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
        float updateRate = Mathf.Clamp(0.1f - GameObject.FindGameObjectsWithTag(enemyTag).Length * 0.01f, 0.02f, 0.1f);
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
        positionDifference.y = 0; // y���� �����ϰ� ���
        float distanceToEnemy = positionDifference.magnitude; // Vector3.magnitude ���
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
        UpdateTarget();
        if (target == null)
            return;
        Enemy enemyScript = target.GetComponent<Enemy>();
        
            Vector3 dir = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnspeed).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        if (enemyScript != null && !enemyScript.GetComponent<Tween_Path>().HasReachedEnd() && enemyScript.Hp > 0)
        {
            if (fireCountdown <= 0f)
            {
                Shoot();
                playerAnimator.SetTrigger("Shooting");
                fireCountdown = 1f / fireRate;
            }
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        if (firePoint == null)
        {
            Debug.LogError("����Ʈ�� ��ž�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        // �Ѿ� �������� �ν��Ͻ�ȭ�մϴ�.
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        if (bulletRigidbody != null)
        {
            Vector3 shootingDirection = (target.position - firePoint.position).normalized;
            bulletRigidbody.velocity = shootingDirection * bulletSpeed;
        }

        // ���� �ð��� ���� �Ŀ� �Ѿ��� �ı��մϴ�.
        Destroy(bullet, 0.4f);
    }

    class BulletCollisionHandler : MonoBehaviour
    {
        void Start()
        {
            // �� ������Ʈ�� ������ GameObject�� Collider ����
            Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                collider.isTrigger = false; // �Ϲ����� �浹�� ����
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                // ���� �浹���� ���� ���� ����
                // ��: ������ �������� �ְ� �Ѿ� �ı�
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    // ������ ������ �ֱ� ���� �߰�
                }
                Destroy(gameObject); // �Ѿ� �ı�
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;       
        Vector3 gizmoPosition = transform.position - new Vector3(0, 0.6f, 0);
        Gizmos.DrawWireSphere(gizmoPosition, range);
    }

   
        public void DeactivateSparkEffect()
        {
            // Effect1 ��Ȱ��ȭ �Ǵ� �ٸ� ���� �߰�
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
