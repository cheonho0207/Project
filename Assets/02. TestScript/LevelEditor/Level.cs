using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Test1")]
    [SerializeField]
    private GameObject _baseGrid;
    [SerializeField]
    private bool _settingTest1;

    [Header("Test2")]
    [SerializeField][Range(0, 100)]
    private float _settingNumBar;
    [SerializeField]
    private int _settingNum;

    [Header("Test3")]
    [SerializeField]
    public string _settingStr;
}
