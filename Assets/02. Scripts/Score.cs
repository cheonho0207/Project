using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI txtScore; // txtScore�� Ư���ϰ� ����
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
                    Debug.LogError("Score �ν��Ͻ��� ã�� �� �����ϴ�.");
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
        // ���� �ε�� �� Ư�� �±׳� �̸��� ���� TextMeshProUGUI�� ã�� txtScore�� �Ҵ�
        var newTxtScore = GameObject.FindWithTag("Score")?.GetComponent<TextMeshProUGUI>();
        if (newTxtScore != null)
        {
            txtScore = newTxtScore;
            DipScore(0);  // ���� ������ �ݿ��Ͽ� UI ������Ʈ
        }

        var resetBtn = GameObject.FindWithTag("Reset")?.GetComponent<Button>();
        if (resetBtn != null)
        {
            resetButton = resetBtn;
            resetButton.onClick.AddListener(ResetScore); // ���� ��ư�� ������ �߰�
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
            txtScore.text = "���� : <color=#ff0000>" + totScore.ToString() + "</color>";
        }
        PlayerPrefs.SetInt("TOT_SCORE", totScore);
        PlayerPrefs.Save(); // �����͸� ��� ��ũ�� ����
    }

    public void ResetScore()
    {

        Debug.Log("ResetScore ȣ���"); // �α� �߰�
        totScore = 0;
        PlayerPrefs.SetInt("TOT_SCORE", totScore);
        PlayerPrefs.Save(); // �����͸� ��� ��ũ�� ����
        if (txtScore != null)
        {
            txtScore.text = "���� : <color=#ff0000>" + totScore.ToString() + "</color>";
        }
    }
}
