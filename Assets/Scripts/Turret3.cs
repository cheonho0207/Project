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
    public Transform partToRotate; // 이 부분 삭제
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
            yield return new WaitForSeconds(duration / arrowCount);  // 전체 시간을 화살 개수로 나눠서 발사 간격 결정
        }
        arrowSpawned = false;  // 발사 완료 후 플래그 초기화
    }

    void Shoot()
    {
        // 발사 위치를 위한 무작위 X와 Z 오프셋 추가
        float horizontalOffset = Random.Range(-0.3f, 0.3f); // 좌우 무작위 오프셋을 줄입니다.
        float verticalOffset = Random.Range(-0.3f, 0.3f); // 상하 무작위 오프셋을 줄입니다.

        // 발사 위치 수정
        Vector3 firePosition = firePoint.position + new Vector3(horizontalOffset, 0, verticalOffset); // X와 Z 오프셋을 적용합니다.

        // 화살 생성 및 방향 설정
        GameObject bullet = Instantiate(bulletPrefab, firePosition, Quaternion.identity);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        if (bulletRigidbody != null)
        {
            bulletRigidbody.useGravity = false;
            Vector3 shootingDirection = partToRotate.forward; // 부품이 가리키는 방향을 발사 방향으로 사용

            bulletRigidbody.AddForce(shootingDirection * bulletSpeed, ForceMode.Impulse);
        }

        // 설정한 시간 후 화살 파괴
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
        // Effect1 비활성화 또는 다른 로직 추가
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