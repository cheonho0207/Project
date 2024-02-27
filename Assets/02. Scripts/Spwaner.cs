using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Spawner : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform spawnPoint;
    public float timeBetweenWaves = 3f;
    private float countdown = 10f; // ù ��° ���̺꿡 ���� �ʱ� ī��Ʈ�ٿ�
    private int waveNumber = 1;
    private bool isFirstWave = true; // ù ��° ���̺� ���θ� Ȯ���ϱ� ���� �÷���

    // Ÿ�̸� ���� ����
    public Text timeText;
    private float timeRemaining;

    // �߰��� ����
    private int totalEnemiesToSpawn = 10;
    private int spawnedEnemyCount = 0;

    void Start()
    {
        // 10�� �Ŀ� SpawnWave �޼��带 ȣ���մϴ�.
        InvokeRepeating("SpawnWave", 10f, 1f);

        // Ÿ�̸� �ʱ�ȭ
        timeRemaining = 30f;
        UpdateTimeText();
    }

    void Update()
    {
        if (countdown <= 0f)
        {
            if (isFirstWave)
            {
                // ������ ���̺꿡 ���� �ʱ� ī��Ʈ�ٿ� ����
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
            timeRemaining = Mathf.Max(0f, timeRemaining - Time.deltaTime); // Ÿ�̸� ����
            UpdateTimeText(); // UI ������Ʈ
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
            // ��� ���� ��ȯ������ ����
            CancelInvoke("SpawnWave");
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    // UI�� �ð��� ������Ʈ�մϴ�.
    private void UpdateTimeText()
    {
        if (timeText != null)
        {
            timeText.text = "�ð�: " + Mathf.Round(timeRemaining);
        }
        else
        {
            Debug.LogError("timeText�� �Ҵ���� �ʾҽ��ϴ�. Inspector���� timeText�� �Ҵ����ּ���.");
        }
    }
}