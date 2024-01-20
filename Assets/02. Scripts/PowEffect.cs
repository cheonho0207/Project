using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PowEffect : MonoBehaviour
{
    private Transform target;
    public GameObject sparkEffect;
    public GameObject sparkEffect2;

    private Rigidbody arrowRigidbody;
    private bool isArrowInactive = false;
    private bool alreadyProcessed = false;

    // 기즈모 박스의 색상을 지정할 변수
    public Color gizmoColor = Color.blue;

    // 기즈모 박스의 반지름을 지정할 변수
    public float range = 3f;
    public string enemyTag = "Enemy";

    // 이미 생성된 이펙트를 추적하기 위한 변수 추가
    private bool hasSpawnedEffect = false;

    void Start()
    {
        arrowRigidbody = GetComponent<Rigidbody>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
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
        if (isArrowInactive)
        {
            // Arrow가 비활성화된 상태에서 10초 뒤에 파괴
            Destroy(gameObject, 10f);
        }

        if (target != null && !hasSpawnedEffect) // 이미 이펙트가 생성되지 않은 경우에만 실행
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget <= range)
            {
                Enemy enemyScript = target.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    // Enemy에게 데미지를 입히기
                    int damage = 5;
                    enemyScript.TakeDamage(damage);
                }

                // 아래는 이미 구현한 코드
                // Arrow의 Rigidbody를 Kinematic으로 설정
                if (arrowRigidbody != null)
                {
                    arrowRigidbody.isKinematic = true;
                }

                // Arrow에 연결된 모든 이펙트들을 삭제
                Effect[] effects = GetComponentsInChildren<Effect>();
                foreach (Effect effect in effects)
                {
                    if (effect != this) // 현재 이펙트 스크립트 자체를 삭제하지 않도록 처리
                    {
                        Destroy(effect.gameObject);
                    }
                }

                // 스파크 이펙트 생성 및 검사
                if (sparkEffect != null)
                {
                    Vector3 sparkEffectPosition = new Vector3(transform.position.x, 1.384f, transform.position.z);

                    GameObject sparkEffectInstance = Instantiate(sparkEffect, transform.position, Quaternion.identity);
                    Destroy(sparkEffectInstance, 3f);
                }
                else
                {
                    Debug.LogError("sparkEffect is not set. Please assign a valid GameObject to sparkEffect in the Inspector.");
                }

               if (sparkEffect2 != null)
{
    // 현재 위치에서 y 값을 1.384로 설정하여 인스턴스 생성
    Vector3 sparkEffect2Position = new Vector3(transform.position.x, 1.384f, transform.position.z);
    GameObject sparkEffectInstance2 = Instantiate(sparkEffect2, sparkEffect2Position, Quaternion.identity);
    Destroy(sparkEffectInstance2, 3f);
}
else
{
    Debug.LogError("sparkEffect2 is not set. Please assign a valid GameObject to sparkEffect2 in the Inspector.");
}

                isArrowInactive = true; // Arrow를 비활성화 상태로 표시
                hasSpawnedEffect = true; // 이펙트가 생성되었음을 표시
            }
        }
    }

    // 나머지 코드는 동일하게 유지됩니다.

    // 기즈모를 그리는 코드
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
