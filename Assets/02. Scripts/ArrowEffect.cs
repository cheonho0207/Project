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
            // Arrow�� ��Ȱ��ȭ�� ���¿��� 10�� �ڿ� �ı�
            Destroy(gameObject, 10f);
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("Enemy"))
        {
            // Arrow�� Rigidbody�� Kinematic���� ����
            if (arrowRigidbody != null)
            {
                arrowRigidbody.isKinematic = true;
            }

            // Arrow�� ����� ��� ����Ʈ���� ����
            Effect[] effects = GetComponentsInChildren<Effect>();
            foreach (Effect effect in effects)
            {
                if (effect != this) // ���� ����Ʈ ��ũ��Ʈ ��ü�� �������� �ʵ��� ó��
                {
                    Destroy(effect.gameObject);
                }
            }

            // ����ũ ����Ʈ�� ����
            sparkEffectInstance = Instantiate(sparkEffect, transform.position, Quaternion.identity);
            Destroy(sparkEffectInstance, 3f);

            sparkEffectInstance2 = Instantiate(sparkEffect2, transform.position, Quaternion.identity);
            Destroy(sparkEffectInstance2, 3f);

            sparkEffectInstance = Instantiate(sparkEffect3, transform.position, Quaternion.identity);
            Destroy(sparkEffectInstance3, 3f);

            sparkEffectInstance = Instantiate(sparkEffect4, transform.position, Quaternion.identity);
            Destroy(sparkEffectInstance, 3f);


            isArrowInactive = true; // Arrow�� ��Ȱ��ȭ ���·� ǥ��
        }
    }
}