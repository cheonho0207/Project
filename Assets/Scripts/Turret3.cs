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
    public Transform partToRotate; // �� �κ� ����
    public float turnspeed = 1f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    private Animator anim;

    [Header("Bullet Attributes")]
    public float bulletSpeed = 5f;
    public float upwardForce = 5f;
    private bool arrowSpawned = false;
    public int power3;
    private Transform SpPoint;

    public float DestroyTime = 3.0f;
    public float turretUpwardForce = 10.0f;

    private Material sparkEffectMaterial;
    // GameObject sparkEffectPrefab;
    private GameObject sparkEffectInstance;
    private Motion2_2 motionScript;

    public GameObject sparkEffect2;
    private GameObject sparkEffectInstance2;

    private float fadeSpeed = 2f;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.03f);
        anim = GetComponent<Animator>();
        SpPoint = GameObject.Find("FirePoint").transform;
        power3 = 550;
        motionScript = GameObject.FindObjectOfType<Motion2_2>();
        if (motionScript == null)
        {
            Debug.LogError("������ Motion2_2 ��ũ��Ʈ�� ã�� �� �����ϴ�.");
        }
        sparkEffect2.SetActive(false);

        fireRate = 1f;
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
            if (!arrowSpawned)
            {
                arrowSpawned = true;
                anim.SetTrigger("Attack");
                StartCoroutine(FireArrowsContinuously(3.0f, 30)); // 30 arrows
            }
        }
        else
        {
            if (target != null)
            {
                StartCoroutine(DelayDeactivation()); // Call Coroutine for delay
            }
        }
    }

    IEnumerator DelayDeactivation()
    {
        yield return new WaitForSeconds(3f); // Delay deactivation for 3 seconds

        if (target != null && Vector3.Distance(transform.position, target.position) > range)
        {
            target = null;
            StopAllCoroutines(); // Stop firing arrows if the target moves out of range
            arrowSpawned = false;
        }
    }

    void Update()
    {

        if (target == null || Vector3.Distance(transform.position, target.position) > range)
        {
            if (arrowSpawned && !IsInvoking("DelayDeactivation"))
            {
                StartCoroutine(DelayDeactivation()); // Ensure coroutine isn't already running
            }
            return;
        }

        if (!sparkEffect2.activeSelf)
        {
            sparkEffect2.SetActive(true);
            Invoke("DeactivateSparkEffect2", 3f);
        }

    }
    IEnumerator FireArrowsContinuously(float duration, int arrowCount)
    {
        float endTime = Time.time + duration;
        int arrowsFired = 0;

        while (Time.time <= endTime && arrowsFired < arrowCount)
        {
            Shoot();
            arrowsFired++;
            yield return new WaitForSeconds(duration / arrowCount);  // ��ü �ð��� ȭ�� ������ ������ �߻� ���� ����
        }
        arrowSpawned = false;  // �߻� �Ϸ� �� �÷��� �ʱ�ȭ
    }

    void Shoot()
    {
        // �߻� ��ġ�� ���� ������ X�� Z ������ �߰�
        float horizontalOffset = Random.Range(-0.3f, 0.3f); // �¿� ������ �������� ���Դϴ�.
        float verticalOffset = Random.Range(-0.3f, 0.3f); // ���� ������ �������� ���Դϴ�.

        // �߻� ��ġ ����
        Vector3 firePosition = firePoint.position + new Vector3(horizontalOffset, 0, verticalOffset); // X�� Z �������� �����մϴ�.

        // ȭ�� ���� �� ���� ����
        GameObject bullet = Instantiate(bulletPrefab, firePosition, Quaternion.identity);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        if (bulletRigidbody != null)
        {
            bulletRigidbody.useGravity = false;
            Vector3 shootingDirection = partToRotate.forward; // ��ǰ�� ����Ű�� ������ �߻� �������� ���

            bulletRigidbody.AddForce(shootingDirection * bulletSpeed, ForceMode.Impulse);
        }

        // ������ �ð� �� ȭ�� �ı�
        Destroy(bullet, DestroyTime);
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
        /*
        if (sparkEffectPrefab != null)
        {
            sparkEffectInstance = Instantiate(sparkEffectPrefab, transform.position, Quaternion.identity);
            sparkEffectInstance.SetActive(true);
        }
        */
    }

    private void DeactivateSparkEffect2()
    {
        if (sparkEffect2.activeSelf)
        {
            sparkEffect2.SetActive(false);
        }
    }

    IEnumerator ActivateAndFadeSparkEffect2()
    {
        // sparkEffect2�� Renderer ������Ʈ�� �����ɴϴ�.
        Renderer renderer = sparkEffect2.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Ȱ��ȭ ���·� ����
            sparkEffect2.SetActive(true);

            Color initialColor = renderer.material.color;
            float time = 0;

            // �ʱ� ���İ��� 0���� ����
            initialColor.a = 0;
            renderer.material.color = initialColor;

            // ���̵���: ���İ��� 0���� 1�� ����
            while (initialColor.a < 1)
            {
                time += Time.deltaTime / fadeSpeed; // fadeSpeed�� ������ ���������� ����
                initialColor.a = Mathf.Lerp(0, 1, time); // ���İ��� 0���� 1�� ����
                renderer.material.color = initialColor; // ���� ������Ʈ
                yield return null; // ���� �����ӱ��� ��ٸ�
            }
        }

        // ���̵����� ���� �� ���� �ð�(��: 3��) ����
        yield return new WaitForSeconds(3f);

        // �ð��� ���� �� ��Ȱ��ȭ
        sparkEffect2.SetActive(false);
    }

}