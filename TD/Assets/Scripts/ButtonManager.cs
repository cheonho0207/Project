using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
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
        Application.Quit();
    }
}
