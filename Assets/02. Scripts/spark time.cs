using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sparktime : MonoBehaviour
{
    public GameObject sparkEffect; // Spark effect 오브젝트를 드래그 앤 드롭으로 연결
    public float fadeDuration = 2f; // 페이드 인 시간

    private CanvasGroup canvasGroup;

    void Start()
    {
        if (sparkEffect != null)
        {
            canvasGroup = sparkEffect.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = sparkEffect.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = 0f; // 시작할 때 완전 투명하게 설정
            sparkEffect.SetActive(false); // 오브젝트를 비활성화

            StartCoroutine(ActivateSparkEffectAfterDelay(10f)); // 10초 후에 페이드 인 시작
        }
        else
        {
            Debug.LogError("sparkEffect가 할당되지 않았습니다.");
        }
    }

    IEnumerator ActivateSparkEffectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        sparkEffect.SetActive(true); // 오브젝트 활성화
        StartCoroutine(FadeInEffect()); // 페이드 인 시작
    }

    IEnumerator FadeInEffect()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
    }
}
