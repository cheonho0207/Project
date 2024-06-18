using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scor : MonoBehaviour
{
    private WaveSpawner waveSpawner;

    [SerializeField]
 
    private TextMeshProUGUI ScoreText2;
    // Start is called before the first frame update
    void Start()
    {
        waveSpawner = FindObjectOfType<WaveSpawner>();
    }

    // Update is called once per frame
    void Update()
    {

        ScoreText2.text = WaveSpawner.gameScore.ToString();
    }
}
