using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class HPManager : MonoBehaviour
{
    public static Image HpBar;
    public static readonly float initHP = 100.0f;
    public static float CurrHP = initHP;

    void Start()
    {
        HpBar = GameObject.FindGameObjectWithTag("HP")?.GetComponent<Image>();
    }

    public void HpDown()
    {
        if (CurrHP > 0.0f)
        {
            CurrHP -= 10;
            DisplayHealth();
            if (CurrHP < 0.0f)
            {
                SceneManager.LoadScene("LoseScene");

                CurrHP = 100;
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
            Debug.LogError("Unity Editor���� HpBar2�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}
