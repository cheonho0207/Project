using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion : MonoBehaviour
{
    public GameObject sparkEffect;

    private Animator playerAnimator;
    private GameObject sparkEffectInstance;
    private Material sparkEffectMaterial;

    private float fadeSpeed = 2f; // 더 빠른 투명도 변화


    void Start()
    {
        // Player 오브젝트 찾기
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // Player 오브젝트가 존재하는지 확인
        if (playerObject != null)
        {
            // Player 오브젝트의 Animator 컴포넌트 찾기
            playerAnimator = playerObject.GetComponent<Animator>();

            // Animator 컴포넌트가 없는 경우 에러 메시지 출력
            if (playerAnimator == null)
            {
                Debug.LogError("Player 오브젝트에 Animator 컴포넌트가 없습니다!");
            }
        }
        else
        {
            Debug.LogError("Player 오브젝트를 찾을 수 없습니다!");
        }
    }

    void Update()
    {
        // playerAnimator가 null인 경우 처리
        if (playerAnimator == null)
        {
            return;
        }

        // 만약 "Shooting" 애니메이션 클립이 재생 중이라면 sparkEffect 생성
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