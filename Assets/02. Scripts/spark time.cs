using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sparktime : MonoBehaviour
{
    public GameObject sparkEffect; // Spark effect ������Ʈ�� �巡�� �� ������� ����
    public float fadeDuration = 2f; // ���̵� �� �ð�

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

            canvasGroup.alpha = 0f; // ������ �� ���� �����ϰ� ����
            sparkEffect.SetActive(false); // ������Ʈ�� ��Ȱ��ȭ

            StartCoroutine(ActivateSparkEffectAfterDelay(10f)); // 10�� �Ŀ� ���̵� �� ����
        }
        else
        {
            Debug.LogError("sparkEffect�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    IEnumerator ActivateSparkEffectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        sparkEffect.SetActive(true); // ������Ʈ Ȱ��ȭ
        StartCoroutine(FadeInEffect()); // ���̵� �� ����
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
