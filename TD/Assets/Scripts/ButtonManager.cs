using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject spawntower;

    public GameObject prefeb1;
    public GameObject prefeb2;
    public GameObject prefeb3;

    private GameObject selectedPrefeb;

    public void SetSelectedPrefeb1()
    {
        selectedPrefeb = prefeb1;
    }

    public void SetSelectedPrefeb2()
    {
        selectedPrefeb = prefeb2;
    }

    public void SetSelectedPrefeb3()
    {
        selectedPrefeb = prefeb3;
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
        //spawntower.GetComponent<BuildingSystem>().SpawnTower();
        spawntower.GetComponent<BuildingSystem>().InitializeWithObject(selectedPrefeb);
    }
}
