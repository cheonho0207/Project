using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour
{
    public static int EnemiesAlive = 0;

    public Wave[] waves;
    public Transform spawnPoint;
    public float timeBetweenWaves = 5f; // ���̺� ���� �ð�
    [SerializeField]
    private float countdown = 30f;
    //space
    public GameObject sparkEffectPrefab;
    public TextMeshProUGUI waveCompleteText1;  // �� UI ���
    public TextMeshProUGUI waveCompleteText2;  // �� UI ���
    public Image waveCompleteImage; // �� UI ���
    //space
    private int waveIndex = 0;
    private bool isGameCompleted = false; // ���� �Ϸ� ���θ� üũ�ϱ� ���� �÷���
    //space
    public Text waveCountdownText;
    [SerializeField]
    private TextMeshProUGUI waveText;
    [SerializeField]
    private Text enemyText;
    [SerializeField]
    private Text scoreText;
    //space
    static public int score = 0;
    static public int gameScore;
    private HPManager hpManager;
    void Start()
    {
        hpManager = FindObjectOfType<HPManager>();
        waveCompleteText1.gameObject.SetActive(false);
        waveCompleteText2.gameObject.SetActive(false);
        waveCompleteImage.gameObject.SetActive(false);
    }

    void Update()
    {
        gameScore = score * 100;
        enemyText.text = "���� �� : " + EnemiesAlive.ToString();
        scoreText.text = "���� : " + gameScore.ToString();

        if (EnemiesAlive > 0)
        {
            return;
        }

        if (waveIndex == waves.Length)
        {
            StageScene();
            this.enabled = false;
            return;
        }

        if (countdown <= 0f)
        {
            waveTextUp();
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;

        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        waveCountdownText.text = "�ð� : " + string.Format("{0:00}", countdown);
    }
    void waveTextUp()
    {
        int waveCount = waveIndex + 1;
        waveText.text = "���̺� : " + waveCount.ToString();

    }
    IEnumerator SpawnWave()
    {
        // PlayerStats.Rounds++;

        Wave wave = waves[waveIndex];

        EnemiesAlive = wave.count;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        waveIndex++;
        StartCoroutine(CheckEnemiesAlive());
    }

    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        Instantiate(sparkEffectPrefab, spawnPoint.position, Quaternion.Euler(0, 90, 90));
        Destroy(sparkEffectPrefab, 2f);
    }

    IEnumerator CheckEnemiesAlive()
    {
        while (EnemiesAlive > 0)
        {
            yield return null;
        }

        if (!isGameCompleted) // ���� �Ϸ� ���� üũ
        {
            countdown = 13;
            yield return StartCoroutine(DisplayWaveCompleteUI());
        }
    }

    IEnumerator DisplayWaveCompleteUI()
    {
        if (isGameCompleted) // ���� �Ϸ� ���� üũ
        {
            yield break; // ������ �Ϸ�Ǿ����� UI ǥ�ø� ���� �ʰ� ����
        }
        if (HPManager.CurrHP <= 0.0f)
        {
            yield break;
        }
        // UI ��� ���̱�
        waveCompleteText1.gameObject.SetActive(true);
        waveCompleteText2.gameObject.SetActive(true);
        waveCompleteImage.gameObject.SetActive(true);

        // �ʱ� ���� �� ����
        Color textColor1 = waveCompleteText1.color;
        Color textColor2 = waveCompleteText2.color;
        Color imageColor = waveCompleteImage.color;

        textColor1.a = 1;
        textColor2.a = 1;
        imageColor.a = 1;

        waveCompleteText1.color = textColor1;
        waveCompleteText2.color = textColor2;
        waveCompleteImage.color = imageColor;

        // 3�� ���
        yield return new WaitForSeconds(4);

        // 2�� ���� ������ �����
        float fadeDuration = 2f;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            textColor1.a = Mathf.Lerp(1, 0, normalizedTime);
            textColor2.a = Mathf.Lerp(1, 0, normalizedTime);
            imageColor.a = Mathf.Lerp(1, 0, normalizedTime);

            waveCompleteText1.color = textColor1;
            waveCompleteText2.color = textColor2;
            waveCompleteImage.color = imageColor;

            yield return null;
        }

        // ���� ���� 0���� ����
        textColor1.a = 0;
        textColor2.a = 0;
        imageColor.a = 0;

        waveCompleteText1.color = textColor1;
        waveCompleteText2.color = textColor2;
        waveCompleteImage.color = imageColor;

        // UI ��� �����
        waveCompleteText1.gameObject.SetActive(false);
        waveCompleteText2.gameObject.SetActive(false);
        waveCompleteImage.gameObject.SetActive(false);
    }

    public void StageScene()
    {
        isGameCompleted = true; // ���� �Ϸ� �÷��� ����
        SceneManager.LoadScene("WinScene");
    }
}