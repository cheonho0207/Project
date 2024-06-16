using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public GameObject sparkEffect;
    public GameObject sparkEffect2;
    public GameObject sparkEffect3;
    public GameObject sparkEffect4;

    private GameObject sparkEffectInstance;
    private GameObject sparkEffectInstance2;
    private GameObject sparkEffectInstance3;
    private GameObject sparkEffectInstance4;

    private Rigidbody arrowRigidbody;
    private bool isArrowInactive = false;

    void Start()
    {
        arrowRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isArrowInactive)
        {
            // Arrow가 비활성화된 상태에서 10초 뒤에 파괴
            Destroy(gameObject, 10f);
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("Enemy"))
        {
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

            // 스파크 이펙트를 생성
            sparkEffectInstance = Instantiate(sparkEffect, transform.position, Quaternion.identity);
            Destroy(sparkEffectInstance, 3f);

            sparkEffectInstance2 = Instantiate(sparkEffect2, transform.position, Quaternion.identity);
            Destroy(sparkEffectInstance2, 3f);

            sparkEffectInstance = Instantiate(sparkEffect3, transform.position, Quaternion.identity);
            Destroy(sparkEffectInstance3, 3f);

            sparkEffectInstance = Instantiate(sparkEffect4, transform.position, Quaternion.identity);
            Destroy(sparkEffectInstance, 3f);


            isArrowInactive = true; // Arrow를 비활성화 상태로 표시
        }
    }
}