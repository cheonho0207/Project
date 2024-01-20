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
        // Player 오브젝트의 Animator 컴포넌트 찾기
        GameObject player = GameObject.FindGameObjectWithTag("Player2");
        if (player != null)
        {
            playerAnimator = player.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("Player 오브젝트를 찾을 수 없습니다!");
        }
    }

    void Update()
    {
        // "Attack" 트리거가 발동하면 sparkEffect 생성
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
            // 트리거가 발동하지 않으면 sparkEffect 페이드 아웃
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