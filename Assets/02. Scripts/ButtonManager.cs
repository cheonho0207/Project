using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    void Start()
    {
        /*
        textUI.gameObject.SetActive(false);
        textUI2.gameObject.SetActive(false);
        textUI3.gameObject.SetActive(false);
        textUI4.gameObject.SetActive(false);
        coinScoreText.text =  coinScore.ToString()+ " :Àü" ;
        Tower1.gameObject.SetActive(false);
        Tower2.gameObject.SetActive(false);
        Tower3.gameObject.SetActive(false);
        */
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
        WaveSpawner.gameScore = 0;
        SceneManager.LoadScene("TileMap4");
    }

    public void Stage2Scene()
    {
        WaveSpawner.gameScore = 0;
        SceneManager.LoadScene("TileMap5");
    }

    public void Exit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
    /*
    public void SpawnTower()
    {
        spawntower = GameObject.Find("Grid");
        //spawntower.GetComponent<BuildingSystem>().SpawnTower();

        spawntower.GetComponent<BuildingSystem>().InitializeWithObject(selectedPrefeb);
       
    }

    private void UpdateCoinScoreText()
    {
        coinScoreText.text = coinScore.ToString() + " :Àü";
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
    */
}
