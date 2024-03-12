using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Wave
{
    public GameObject[] enemies; // 여러 종류의 적을 담을 배열
    public int[] counts; // 각 적의 수를 담을 배열
    public float rate;
}