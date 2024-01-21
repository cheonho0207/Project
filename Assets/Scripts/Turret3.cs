using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Turret3 : MonoBehaviour
{
    private Transform target;

    [Header("Attributes")]
    public float range = 3f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnspeed = 5f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    private Animator anim;

    [Header("Bullet Attributes")]
    public float bulletSpeed = 10f;
    public float upwardForce = 5f;

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
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
        anim = GetComponent<Animator>();
        SpPoint = GameObject.Find("SpPoint").transform;
        power3 = 550;
        motionScript = GameObject.FindObjectOfType<Motion2_2>();
        if (motionScript == null)
        {
            Debug.LogError("������ Motion2_2 ��ũ��Ʈ�� ã�� �� �����ϴ�.");
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
            rotation.y += 5f;
            partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            // Ÿ�ٰ��� �Ÿ��� ����մϴ�.
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // ����� �ڽ� ���� �ȿ� �ְų� �Ÿ��� ���� �Ÿ� �̳��� ��쿡�� �߻��մϴ�.
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
            Debug.LogError("����Ʈ�� ��ž�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        // Ÿ���� ������ �߻����� �ʽ��ϴ�.
        if (target == null)
            return;

        // SparkEffect�� �ߵ��մϴ�.
        motionScript.TriggerSparkEffects();

        int numberOfBulletsPerRow = 3;
        int numberOfRows = 4;
        float spacing = 0.5f;

        for (int row = 0; row < numberOfRows; row++)
        {
            for (int col = 0; col < numberOfBulletsPerRow; col++)
            {
                // ������ �Ѿ� �ν��Ͻ��� �����մϴ�.
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

                if (bulletRigidbody != null)
                {
                    // �Ѿ��� ��ġ�� �����Ͽ� 3x4 ���·� ��ġ�մϴ�.
                    Vector3 offset = new Vector3((col - numberOfBulletsPerRow / 2) * spacing, row * spacing, 0f);
                    bullet.transform.position += offset;

                    // Ÿ���� ��ġ�� �Ѿ��� ������ �����մϴ�.
                    Vector3 targetDirection = target.position - bullet.transform.position;

                    // Ÿ�� �������� �Ѿ��� ���� ȸ����ŵ�ϴ�.
                    bullet.transform.rotation = Quaternion.LookRotation(targetDirection.normalized);

                    // �Ѿ� �߻�
                    Vector3 forwardForce = targetDirection.normalized * bulletSpeed;
                    bulletRigidbody.AddForce(forwardForce, ForceMode.Impulse);

                    // ������ �� �߰� (���� ������ ����)
                    Vector3 upwardForceVector = Vector3.up * upwardForce;
                    bulletRigidbody.AddForce(upwardForceVector, ForceMode.Impulse);
                }

                bullet.AddComponent<BulletCollisionHandler>();
                // ������ �Ѿ˿� ���� 1�� �Ŀ� �ı��մϴ�.
                Destroy(bullet, 0.5f);
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
    private void DeactivateSparkEffect2()
    {
        if (sparkEffect2.activeSelf)
        {
            sparkEffect2.SetActive(false);
        }
    }

}
