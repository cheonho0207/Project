using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class HPManager : MonoBehaviour
{
    public Image HpBar;
    public static readonly float initHP = 100.0f;
    public static float CurrHP = initHP;

    void Awake()
    {
        HpBar = GameObject.FindGameObjectWithTag("HP")?.GetComponent<Image>();
        CurrHP = initHP;
    }

    public void HpDown()
    {
        if (CurrHP > 0.0f)
        {
            CurrHP -= 10;
            DisplayHealth();
            if (CurrHP <= 0.0f)
            {
                CurrHP = 0;
                DisplayHealth();
                SceneManager.LoadScene("LoseScene");
            }
        }
    }

    void DisplayHealth() //HP 상태를 보여줌
    {
        if (HpBar != null)
        {
            HpBar.fillAmount = CurrHP / initHP; //UI의 HP 계산
        }
        else
        {
            Debug.Log("Unity Editor에서 HpBar가 할당되지 않았습니다.");
        }
    }
}
