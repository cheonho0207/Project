using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scor : MonoBehaviour
{
    private WaveSpawner waveSpawner;

    [SerializeField]
    private Text scoreText;
    
    // Start is called before the first frame update
    void Start()
    {
        waveSpawner = FindObjectOfType<WaveSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = WaveSpawner.gameScore.ToString();
    }
}
