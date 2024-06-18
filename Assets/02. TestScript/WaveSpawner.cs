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
    public float timeBetweenWaves = 5f; // 웨이브 간의 시간
    [SerializeField]
    private float countdown = 30f;
    //space
    public GameObject sparkEffectPrefab;
    public TextMeshProUGUI waveCompleteText1;  // 새 UI 요소
    public TextMeshProUGUI waveCompleteText2;  // 새 UI 요소
    public Image waveCompleteImage; // 새 UI 요소
    //space
    private int waveIndex = 0;
    private bool isGameCompleted = false; // 게임 완료 여부를 체크하기 위한 플래그
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
        enemyText.text = "남은 적 : " + EnemiesAlive.ToString();
        scoreText.text = "점수 : " + gameScore.ToString();

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

        waveCountdownText.text = "시간 : " + string.Format("{0:00}", countdown);
    }
    void waveTextUp()
    {
        int waveCount = waveIndex + 1;
        waveText.text = "웨이브 : " + waveCount.ToString();

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

        if (!isGameCompleted) // 게임 완료 여부 체크
        {
            countdown = 13;
            yield return StartCoroutine(DisplayWaveCompleteUI());
        }
    }

    IEnumerator DisplayWaveCompleteUI()
    {
        if (isGameCompleted) // 게임 완료 여부 체크
        {
            yield break; // 게임이 완료되었으면 UI 표시를 하지 않고 종료
        }
        if (HPManager.CurrHP <= 0.0f)
        {
            yield break;
        }
        // UI 요소 보이기
        waveCompleteText1.gameObject.SetActive(true);
        waveCompleteText2.gameObject.SetActive(true);
        waveCompleteImage.gameObject.SetActive(true);

        // 초기 알파 값 설정
        Color textColor1 = waveCompleteText1.color;
        Color textColor2 = waveCompleteText2.color;
        Color imageColor = waveCompleteImage.color;

        textColor1.a = 1;
        textColor2.a = 1;
        imageColor.a = 1;

        waveCompleteText1.color = textColor1;
        waveCompleteText2.color = textColor2;
        waveCompleteImage.color = imageColor;

        // 3초 대기
        yield return new WaitForSeconds(4);

        // 2초 동안 서서히 사라짐
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

        // 알파 값을 0으로 설정
        textColor1.a = 0;
        textColor2.a = 0;
        imageColor.a = 0;

        waveCompleteText1.color = textColor1;
        waveCompleteText2.color = textColor2;
        waveCompleteImage.color = imageColor;

        // UI 요소 숨기기
        waveCompleteText1.gameObject.SetActive(false);
        waveCompleteText2.gameObject.SetActive(false);
        waveCompleteImage.gameObject.SetActive(false);
    }

    public void StageScene()
    {
        isGameCompleted = true; // 게임 완료 플래그 설정
        SceneManager.LoadScene("WinScene");
    }
}