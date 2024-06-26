using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credit : MonoBehaviour
{
    static private int startCredit = 30;
    public int haveCredit = 0;

    private HPManager hpManager;

    public Text creditText;

    private void Start()
    {
        hpManager = FindObjectOfType<HPManager>();

        haveCredit = startCredit;
        InvokeRepeating("ChargeCredit", 3.0f, 1.0f);
        UpdateCoinText();
    }

    public void UpdateCoinText()
    {
        creditText.text = haveCredit.ToString() + "전";
    }

    void ChargeCredit()
    {
        if (hpManager != null && HPManager.CurrHP > 0)
        {
            haveCredit++;
            UpdateCoinText();
            Debug.Log("현재 보유 재화 : " + haveCredit);
        }
    }

    public void SumCredit()
    {
        haveCredit = haveCredit + 5;
    }
}