using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion : MonoBehaviour
{
    public GameObject sparkEffect;

    private Animator playerAnimator;
    private GameObject sparkEffectInstance;
    private Material sparkEffectMaterial;

    private float fadeSpeed = 2f; // �� ���� ���� ��ȭ


    void Start()
    {
        // Player ������Ʈ ã��
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // Player ������Ʈ�� �����ϴ��� Ȯ��
        if (playerObject != null)
        {
            // Player ������Ʈ�� Animator ������Ʈ ã��
            playerAnimator = playerObject.GetComponent<Animator>();

            // Animator ������Ʈ�� ���� ��� ���� �޽��� ���
            if (playerAnimator == null)
            {
                Debug.LogError("Player ������Ʈ�� Animator ������Ʈ�� �����ϴ�!");
            }
        }
        else
        {
            Debug.LogError("Player ������Ʈ�� ã�� �� �����ϴ�!");
        }
    }

    void Update()
    {
        // playerAnimator�� null�� ��� ó��
        if (playerAnimator == null)
        {
            return;
        }

        // ���� "Shooting" �ִϸ��̼� Ŭ���� ��� ���̶�� sparkEffect ����
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Shooting"))
        {
            if (sparkEffectInstance == null)
            {
                sparkEffectInstance = Instantiate(sparkEffect, transform.position, Quaternion.identity);
                sparkEffectMaterial = sparkEffectInstance.GetComponent<Renderer>().material;
            }
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if (sparkEffectMaterial != null)
            {
                Color currentColor = sparkEffectMaterial.GetColor("_TintColor");
                float newAlpha = Mathf.Lerp(currentColor.a, 0f, Time.deltaTime * fadeSpeed);
                currentColor.a = newAlpha;
                sparkEffectMaterial.SetColor("_TintColor", currentColor);

                if (newAlpha <= 0.01f)
                {
                    Destroy(sparkEffectInstance);
                }
            }
        }
    }
}