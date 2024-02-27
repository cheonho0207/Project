using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Spawner : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform spawnPoint;
    public float timeBetweenWaves = 3f;
    private float countdown = 10f; // 첫 번째 웨이브에 대한 초기 카운트다운
    private int waveNumber = 1;
    private bool isFirstWave = true; // 첫 번째 웨이브 여부를 확인하기 위한 플래그

    // 타이머 관련 변수
    public Text timeText;
    private float timeRemaining;

    // 추가된 변수
    private int totalEnemiesToSpawn = 10;
    private int spawnedEnemyCount = 0;

    void Start()
    {
        // 10초 후에 SpawnWave 메서드를 호출합니다.
        InvokeRepeating("SpawnWave", 10f, 1f);

        // 타이머 초기화
        timeRemaining = 30f;
        UpdateTimeText();
    }

    void Update()
    {
        if (countdown <= 0f)
        {
            if (isFirstWave)
            {
                // 이후의 웨이브에 대한 초기 카운트다운 설정
                countdown = 5f;
                isFirstWave = false;
            }
            else
            {
                countdown = timeBetweenWaves;
            }
        }
        else
        {
            countdown -= Time.deltaTime;
            timeRemaining = Mathf.Max(0f, timeRemaining - Time.deltaTime); // 타이머 감소
            UpdateTimeText(); // UI 업데이트
        }
    }

    void SpawnWave()
    {
        if (spawnedEnemyCount < totalEnemiesToSpawn)
        {
            SpawnEnemy();
            spawnedEnemyCount++;
        }

        if (spawnedEnemyCount >= totalEnemiesToSpawn)
        {
            // 모든 적을 소환했으면 중지
            CancelInvoke("SpawnWave");
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    // UI에 시간을 업데이트합니다.
    private void UpdateTimeText()
    {
        if (timeText != null)
        {
            timeText.text = "시간: " + Mathf.Round(timeRemaining);
        }
        else
        {
            Debug.LogError("timeText가 할당되지 않았습니다. Inspector에서 timeText를 할당해주세요.");
        }
    }
}