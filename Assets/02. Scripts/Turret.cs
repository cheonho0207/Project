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
    public float bulletSpeed = 3f; // ����: �Ѿ� �ӵ�
    public float upwardForce = 4.0f;

    public GameObject sparkEffectPrefab; // �� �� �߰�
    private GameObject sparkEffectInstance; // �� �� �߰�
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
        if (target == null)
            return;

        // ����� Enemy�̰� ���� ��������� ��� ���� �����ϰ� HP�� 0�̶�� �������� �ʽ��ϴ�.
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
            Debug.LogError("����Ʈ�� ��ž�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        // �Ѿ� �������� �ν��Ͻ�ȭ�մϴ�.
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        if (bulletRigidbody != null)
        {
            // �Ѿ��� ���� ȸ�� x ���� 80���� �����մϴ�.
            bullet.transform.localRotation = Quaternion.Euler(80, 40, 0);
            Vector3 shootingDirection = partToRotate.forward;

            // �ͷ��� ���� ������ ����Ͽ� �Ѿ��� ���� ���� �����մϴ�.
            Vector3 forwardForce = firePoint.forward * bulletSpeed;
            bulletRigidbody.AddForce(forwardForce, ForceMode.Impulse);

            // ���� ���� ����ϰ� �����մϴ�. (������ �ʿ��� �� �ֽ��ϴ�.)
            Vector3 upwardForceVector = Vector3.up * (upwardForce / 2); // ������ ����
            bulletRigidbody.AddForce(upwardForceVector, ForceMode.Impulse);
        }

        // ���� �ð��� ���� �Ŀ� �Ѿ��� �ı��մϴ�.
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
