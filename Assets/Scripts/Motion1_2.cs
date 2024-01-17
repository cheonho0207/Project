using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion1_2 : MonoBehaviour
{
    public GameObject sparkEffect;

    private Animator playerAnimator;
    private GameObject sparkEffectInstance;
    private Material sparkEffectMaterial;

    private float fadeSpeed = 2f;

    void Start()
    {
        // Player ������Ʈ�� Animator ������Ʈ ã��
        GameObject player = GameObject.FindGameObjectWithTag("Player2");
        if (player != null)
        {
            playerAnimator = player.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("Player ������Ʈ�� ã�� �� �����ϴ�!");
        }
    }

    void Update()
    {
        // "Attack" Ʈ���Ű� �ߵ��ϸ� sparkEffect ����
        if (playerAnimator != null && playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (sparkEffectInstance == null)
            {
                sparkEffectInstance = Instantiate(sparkEffect, transform.position, Quaternion.identity);
                sparkEffectMaterial = sparkEffectInstance.GetComponent<Renderer>().material;
            }
        }
        else
        {
            // Ʈ���Ű� �ߵ����� ������ sparkEffect ���̵� �ƿ�
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