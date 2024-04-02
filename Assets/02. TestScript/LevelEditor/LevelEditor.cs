using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    public Level selected;


    private void OnEnable()
    {

        if (AssetDatabase.Contains(target))
        {
            selected = null;
        }
        else
        {
            selected = (Level)target;
        }
    }

    [Header("Test1")]
    [SerializeField]
    private GameObject testGrid;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("TestButton"))
        {
            selected._settingStr = "asd";
        }
        if (GUILayout.Button("Save"))
        {
            selected._settingStr = "";
        }
        else if (GUILayout.Button("Load"))
        {
        }
    }
}
