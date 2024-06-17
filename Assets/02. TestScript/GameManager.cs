using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public void WinLevel()
    {
        // 승리 씬으로 이동합니다.
        SceneManager.LoadScene("WinScene");
    }
}
