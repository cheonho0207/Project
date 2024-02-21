using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Turret3 : MonoBehaviour
{
    private Transform target;

    [Header("Attributes")]
    public float range = 3f;
    public float fireRate = 2f;
    private float fireCountdown = 0f;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate; // 이 부분 삭제
    public GameObject bulletPrefab;
    public Transform firePoint;
    private Animator anim;

    [Header("Bullet Attributes")]
    public float bulletSpeed = 3f;
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
            Debug.LogError("씬에서 Motion2_2 스크립트를 찾을 수 없습니다.");
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
            // 타겟과의 거리를 계산합니다.
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // 거리가 사정 거리 이내인 경우에만 발사합니다.
            if (distanceToTarget <= range)
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

            fireCountdown -= Time.deltaTime *2;
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
            return;

        motionScript.TriggerSparkEffects();
        StartCoroutine(ActivateAndFadeSparkEffect2());
        float spacing = 0.4f; // 총알 사이의 간격
        int rows = 2; // 행의 수
        int columns = 3; // 열의 수

        // 3x3 직사각형 패턴으로 총알 발사
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // 발사 지점 계산
                Vector3 spawnPosition = firePoint.position + (transform.right * (x - 1) * spacing) + (transform.up * (y - 1) * spacing);

                GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
                Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

                if (bulletRigidbody != null)
                {
                    Vector3 shootDirection = -transform.forward;
                    bulletRigidbody.AddForce(shootDirection * bulletSpeed, ForceMode.Impulse);

                    Vector3 upwardForceVector = transform.up * upwardForce;
                    bulletRigidbody.AddForce(upwardForceVector, ForceMode.Impulse);
                }

                bullet.AddComponent<BulletCollisionHandler>();
                Destroy(bullet, DestroyTime);
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

    private void DeactivateSparkEffect2()
    {
        if (sparkEffect2.activeSelf)
        {
            sparkEffect2.SetActive(false);
        }
    }

    IEnumerator ActivateAndFadeSparkEffect2()
    {
        // sparkEffect2의 Renderer 컴포넌트를 가져옵니다.
        Renderer renderer = sparkEffect2.GetComponent<Renderer>();
        if (renderer != null)
        {
            // 활성화 상태로 설정
            sparkEffect2.SetActive(true);

            Color initialColor = renderer.material.color;
            float time = 0;

            // 초기 알파값을 0으로 설정
            initialColor.a = 0;
            renderer.material.color = initialColor;

            // 페이드인: 알파값을 0에서 1로 변경
            while (initialColor.a < 1)
            {
                time += Time.deltaTime / fadeSpeed; // fadeSpeed로 나누어 점진적으로 증가
                initialColor.a = Mathf.Lerp(0, 1, time); // 알파값을 0에서 1로 보간
                renderer.material.color = initialColor; // 색상 업데이트
                yield return null; // 다음 프레임까지 기다림
            }
        }

        // 페이드인이 끝난 후 일정 시간(예: 3초) 유지
        yield return new WaitForSeconds(3f);

        // 시간이 지난 후 비활성화
        sparkEffect2.SetActive(false);
    }

}
