using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject spawntower;

    public GameObject prefeb1;
    public GameObject prefeb2;
    public GameObject prefeb3;
    public GameObject prefeb4;
    public GameObject prefeb5;
    public GameObject prefeb6;

    private GameObject selectedPrefeb;
    public TextMeshProUGUI textUI;
    public TextMeshProUGUI textUI2;
    public TextMeshProUGUI textUI3;
    public TextMeshProUGUI textUI4;
    public int coinScore = 60; // ���� ���� ���ھ�
    public Text coinScoreText;
    public Button Tower1;
    public Button Tower2;
    public Button Tower3;
    void Start()
    {
        textUI.gameObject.SetActive(false);
        textUI2.gameObject.SetActive(false);
        textUI3.gameObject.SetActive(false);
        textUI4.gameObject.SetActive(false);
        coinScoreText.text =  coinScore.ToString()+ " :��" ;
        Tower1.gameObject.SetActive(false);
        Tower2.gameObject.SetActive(false);
        Tower3.gameObject.SetActive(false);
    }
    public void SetSelectedPrefeb1()
    {
        if (coinScore >= 20)
        {
            coinScore -= 20;
            UpdateCoinScoreText();
            selectedPrefeb = prefeb1;
        }
        else 
        {
            coinScore -= 10;
            UpdateCoinScoreText();
            Tower1.gameObject.SetActive((coinScore < 20) ? true : false);
            Tower1.onClick.AddListener(OnTower1Click);
        }
        
        
    }

    private void OnTower1Click()
    {
        textUI2.gameObject.SetActive(true);
        ActivateAndDeactivateTextUI2();
        selectedPrefeb = prefeb4;
    }

    public void SetSelectedPrefeb2()
    {
        if (coinScore >= 5)
        {
            coinScore -= 5;
            UpdateCoinScoreText();
            selectedPrefeb = prefeb2;
        }
        else
        {
            Tower2.gameObject.SetActive((coinScore < 5) ? true : false);
            Tower2.onClick.AddListener(OnTower2Click);

        }
    }

    public void SetSelectedPrefeb3()
    {
        if (coinScore >= 15)
        {
            coinScore -= 15;
            UpdateCoinScoreText();
            selectedPrefeb = prefeb3;
        }
        else
        {
            coinScore -= 5;
            Tower3.gameObject.SetActive((coinScore < 15) ? true : false);
            Tower3.onClick.AddListener(OnTower3Click);

        }
    }

    private void OnTower2Click()
    {
        textUI3.gameObject.SetActive(true);
        ActivateAndDeactivateTextUI3();
        selectedPrefeb = prefeb5;
    }

    private void OnTower3Click()
    {
        textUI3.gameObject.SetActive(true);
        ActivateAndDeactivateTextUI4();
        selectedPrefeb = prefeb5;
    }

    public void SetSelectedPrefeb4()
    {
        selectedPrefeb = prefeb6;
        ActivateAndDeactivateTextUI();
    }

    private void ActivateAndDeactivateTextUI()
    {
        // Ȱ��ȭ�� �� 3�� �ڿ� �ٽ� ��Ȱ��ȭ
        StartCoroutine(ActivateAndDeactivateRoutine());
    }

    private IEnumerator ActivateAndDeactivateRoutine()
    {
        // Text ���� UI�� Ȱ��ȭ
        textUI.gameObject.SetActive(true);

        // 3�� ���
        yield return new WaitForSeconds(3f);

        // Text ���� UI�� ��Ȱ��ȭ
        textUI.gameObject.SetActive(false);
    }
    private void ActivateAndDeactivateTextUI2()
    {
        // Ȱ��ȭ�� �� 3�� �ڿ� �ٽ� ��Ȱ��ȭ
        StartCoroutine(ActivateAndDeactivateRoutine2());
    }

    private void ActivateAndDeactivateTextUI3()
    {
        // Ȱ��ȭ�� �� 3�� �ڿ� �ٽ� ��Ȱ��ȭ
        StartCoroutine(ActivateAndDeactivateRoutine3());
    }

    private void ActivateAndDeactivateTextUI4()
    {
        // Ȱ��ȭ�� �� 3�� �ڿ� �ٽ� ��Ȱ��ȭ
        StartCoroutine(ActivateAndDeactivateRoutine4());
    }

    private void ActivateAndDeactivateTextUI5()
    {
        // Ȱ��ȭ�� �� 3�� �ڿ� �ٽ� ��Ȱ��ȭ
        StartCoroutine(ActivateAndDeactivateRoutine5());
    }

    private IEnumerator ActivateAndDeactivateRoutine2()
    {
        // Text ���� UI�� Ȱ��ȭ
        textUI2.gameObject.SetActive(true);

        // 3�� ���
        yield return new WaitForSeconds(3f);

        // Text ���� UI�� ��Ȱ��ȭ
        textUI2.gameObject.SetActive(false);
    }

    private IEnumerator ActivateAndDeactivateRoutine3()
    {
        // Text ���� UI�� Ȱ��ȭ
        textUI3.gameObject.SetActive(true);

        // 3�� ���
        yield return new WaitForSeconds(3f);

        // Text ���� UI�� ��Ȱ��ȭ
        textUI3.gameObject.SetActive(false);
    }

    private IEnumerator ActivateAndDeactivateRoutine4()
    {
        // Text ���� UI�� Ȱ��ȭ
        textUI4.gameObject.SetActive(true);

        // 3�� ���
        yield return new WaitForSeconds(3f);

        // Text ���� UI�� ��Ȱ��ȭ
        textUI4.gameObject.SetActive(false);
    }

    private IEnumerator ActivateAndDeactivateRoutine5()
    {
        // Text ���� UI�� Ȱ��ȭ
        textUI4.gameObject.SetActive(true);

        // 3�� ���
        yield return new WaitForSeconds(3f);

        // Text ���� UI�� ��Ȱ��ȭ
        textUI4.gameObject.SetActive(false);
    }

    public void SelectScene()
    {
        SceneManager.LoadScene("SelectScene");
    }

    public void MainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Stage1Scene()
    {
        SceneManager.LoadScene("TileMap2");
    }

    public void Stage2Scene()
    {
        SceneManager.LoadScene("TileMap2");
    }

    public void Exit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void SpawnTower()
    {
        spawntower = GameObject.Find("Grid");
        //spawntower.GetComponent<BuildingSystem>().SpawnTower();
        spawntower.GetComponent<BuildingSystem>().InitializeWithObject(selectedPrefeb);
    }

    private void UpdateCoinScoreText()
    {
        coinScoreText.text = coinScore.ToString() + " :��";
        if (coinScore <= 0)
        {
            Tower1.gameObject.SetActive((coinScore < 20) ? true : false);
            Tower2.gameObject.SetActive((coinScore < 5) ? true : false);
            Tower3.gameObject.SetActive((coinScore < 15) ? true : false);
            textUI2.gameObject.SetActive(true);
            ActivateAndDeactivateTextUI4();
            
        }
        else if(coinScore <= 20)
        {
            Tower1.gameObject.SetActive((coinScore < 20) ? true : false);
            Tower1.onClick.AddListener(OnTower1Click);
        }
        else if(coinScore <=5)
        {
            Tower2.gameObject.SetActive((coinScore < 5) ? true : false);
            Tower2.onClick.AddListener(OnTower2Click);
        }
        else if (coinScore <= 15)
        {
            Tower3.gameObject.SetActive((coinScore < 15) ? true : false);
            Tower3.onClick.AddListener(OnTower3Click);
        }

    }
}
