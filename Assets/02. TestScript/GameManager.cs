using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public void WinLevel()
    {
        // �¸� ������ �̵��մϴ�.
        SceneManager.LoadScene("WinScene");
    }
}
