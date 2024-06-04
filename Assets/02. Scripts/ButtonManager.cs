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
    public int coinScore = 60; // 시작 코인 스코어
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
        coinScoreText.text = coinScore.ToString() + " :전";
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
            UpdateCoinScoreText();
            Tower1.onClick.AddListener(OnTower1Click);
        }
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
            UpdateCoinScoreText();
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
            UpdateCoinScoreText();
            Tower3.onClick.AddListener(OnTower3Click);
        }
    }

    private void OnTower1Click()
    {
        textUI2.gameObject.SetActive(true);
        ActivateAndDeactivateTextUI2();
        selectedPrefeb = prefeb5;
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
        StartCoroutine(ActivateAndDeactivateRoutine());
    }

    private IEnumerator ActivateAndDeactivateRoutine()
    {
        textUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        textUI.gameObject.SetActive(false);
    }

    private void ActivateAndDeactivateTextUI2()
    {
        StartCoroutine(ActivateAndDeactivateRoutine2());
    }

    private void ActivateAndDeactivateTextUI3()
    {
        StartCoroutine(ActivateAndDeactivateRoutine3());
    }

    private void ActivateAndDeactivateTextUI4()
    {
        StartCoroutine(ActivateAndDeactivateRoutine4());
    }

    private IEnumerator ActivateAndDeactivateRoutine2()
    {
        textUI2.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        textUI2.gameObject.SetActive(false);
    }

    private IEnumerator ActivateAndDeactivateRoutine3()
    {
        textUI3.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        textUI3.gameObject.SetActive(false);
    }

    private IEnumerator ActivateAndDeactivateRoutine4()
    {
        textUI4.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        textUI4.gameObject.SetActive(false);
    }

    public void SelectScene()
    {

        SceneManager.LoadScene("SelectScene");
    }

    public void MainScene()
    {
        // MainScene을 로드하기 전에 스코어를 초기화합니다.

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
        spawntower.GetComponent<BuildingSystem>().InitializeWithObject(selectedPrefeb);
    }

    private void UpdateCoinScoreText()
    {
        coinScoreText.text = coinScore.ToString() + " :전";
        if (coinScore <= 0)
        {
            textUI2.gameObject.SetActive(true);
            ActivateAndDeactivateTextUI4();
            selectedPrefeb = prefeb5;
        }
        Tower1.gameObject.SetActive(coinScore < 20);
        if (coinScore < 20)
        {
            Tower1.onClick.RemoveAllListeners();
            Tower1.onClick.AddListener(OnTower1Click);
            ActivateAndDeactivateTextUI4();
            selectedPrefeb = prefeb5;
        }
        Tower2.gameObject.SetActive(coinScore < 5);
        if (coinScore < 5)
        {
            Tower2.onClick.RemoveAllListeners();
            Tower2.onClick.AddListener(OnTower2Click);
            ActivateAndDeactivateTextUI4();
            selectedPrefeb = prefeb5;
        }
        Tower3.gameObject.SetActive(coinScore < 15);
        if (coinScore < 15)
        {
            Tower3.onClick.RemoveAllListeners();
            Tower3.onClick.AddListener(OnTower3Click);
            ActivateAndDeactivateTextUI4();
            selectedPrefeb = prefeb5;
        }
    }

}
