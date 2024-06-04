using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI txtScore; // txtScore를 특정하게 설정
    private int totScore = 0;

    private static Score instance;

    public Button notresetButton;
    public Button resetButton;

    public static Score Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Score>();
                if (instance == null)
                {
                    Debug.LogError("Score 인스턴스를 찾을 수 없습니다.");
                }
                else
                {
                    instance.Initialize();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        Initialize();
        SetSelectedPrefeb();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드될 때 특정 태그나 이름을 가진 TextMeshProUGUI를 찾아 txtScore에 할당
        var newTxtScore = GameObject.FindWithTag("Score")?.GetComponent<TextMeshProUGUI>();
        if (newTxtScore != null)
        {
            txtScore = newTxtScore;
            DipScore(0);  // 현재 점수를 반영하여 UI 업데이트
        }

        var resetBtn = GameObject.FindWithTag("Reset")?.GetComponent<Button>();
        if (resetBtn != null)
        {
            resetButton = resetBtn;
            resetButton.onClick.AddListener(ResetScore); // 리셋 버튼에 리스너 추가
        }
    }

    public void SetSelectedPrefeb()
    {
        resetButton.onClick.AddListener(ResetScore);
    }

    public void Initialize()
    {
        totScore = PlayerPrefs.GetInt("TOT_SCORE", 0);
        DipScore(0);
    }

    public void DipScore(int score)
    {
        totScore += score;
        if (txtScore != null)
        {
            txtScore.text = "점수 : <color=#ff0000>" + totScore.ToString() + "</color>";
        }
        PlayerPrefs.SetInt("TOT_SCORE", totScore);
        PlayerPrefs.Save(); // 데이터를 즉시 디스크에 저장
    }

    public void ResetScore()
    {

        Debug.Log("ResetScore 호출됨"); // 로그 추가
        totScore = 0;
        PlayerPrefs.SetInt("TOT_SCORE", totScore);
        PlayerPrefs.Save(); // 데이터를 즉시 디스크에 저장
        if (txtScore != null)
        {
            txtScore.text = "점수 : <color=#ff0000>" + totScore.ToString() + "</color>";
        }
    }
}
