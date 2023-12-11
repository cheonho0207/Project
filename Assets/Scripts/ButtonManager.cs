using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private TowerManager towerManager;

    public GameObject spawnTower;
    private GameObject selectedPrefab;

    public void SetSelectedPrefab(int id)
    {
        selectedPrefab = towerManager.Towers[id].Prefab;
    }

    #region TowerSelect
    public void SetSelectedPrefab1()
    {
        SetSelectedPrefab(0);
    }

    public void SetSelectedPrefab2()
    {
        SetSelectedPrefab(1);
    }

    public void SetSelectedPrefab3()
    {
        SetSelectedPrefab(2);
    }
    #endregion

    #region SceneSelect
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
    #endregion

    public void SpawnTower()
    {
        spawnTower = GameObject.Find("Grid");
        //spawntower.GetComponent<BuildingSystem>().SpawnTower();
        spawnTower.GetComponent<BuildingSystem>().InitializeWithObject(selectedPrefab);
    }
}
