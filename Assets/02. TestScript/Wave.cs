using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Wave
{
    public GameObject[] enemies; // ���� ������ ���� ���� �迭
    public int[] counts; // �� ���� ���� ���� �迭
    public float rate;
}