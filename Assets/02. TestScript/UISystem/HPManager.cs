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

    void DisplayHealth() //HP ���¸� ������
    {
        if (HpBar != null)
        {
            HpBar.fillAmount = CurrHP / initHP; //UI�� HP ���
        }
        else
        {
            Debug.Log("Unity Editor���� HpBar�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}
