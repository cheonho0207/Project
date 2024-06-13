using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System;
using UnityEngine.UIElements;
using UnityEditor.Rendering;

[CustomEditor(typeof(StageLevelManager))]
public class StageLevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        StageLevelManager manager = target as StageLevelManager;

        GUILayout.Space(10);

        manager.editTilePrefab =
            EditorGUILayout.ObjectField("타일 프리펩",
                                        manager.editTilePrefab,
                                        typeof(GameObject)) as GameObject;

        manager.objectPrefab =
            EditorGUILayout.ObjectField("Object prefab",
                                        manager.objectPrefab,
                                        typeof(GameObject)) as GameObject;

        manager.waypointPrefab =
            EditorGUILayout.ObjectField("WayPoint prefab",
                                        manager.waypointPrefab,
                                        typeof(GameObject)) as GameObject;

        manager.startpointPrefab =
            EditorGUILayout.ObjectField("StartPoint prefab",
                                        manager.startpointPrefab,
                                        typeof(GameObject)) as GameObject;

        manager.endpointPrefab =
            EditorGUILayout.ObjectField("EndPoint prefab",
                                        manager.endpointPrefab,
                                        typeof(GameObject)) as GameObject;
        /*
        manager.editorDatabase =
            EditorGUILayout.ObjectField("DataBase",
                                        manager.editorDatabase,
                                        typeof(ObjectsDatabaseSO)) as ObjectsDatabaseSO;
        */
        GUILayout.Space(20);

        manager.stageSize = EditorGUILayout.IntSlider("Size", manager.stageSize, 5, 16);

        EditorGUILayout.BeginHorizontal();

        manager.stageId = EditorGUILayout.IntField("ID", manager.stageId);
        if (GUILayout.Button("Create Level"))
        {
            CreateLevel(manager);
            //Debug.Log("Create Level 버튼 눌림!");
        }

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Clean Up"))
        {
            CleanUp(manager);
        }

        GUILayout.Space(20);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Save Stage"))
        {
            Save(manager);
        }
        if (GUILayout.Button("Load Stage"))
        {
            Load(manager);
        }

        EditorGUILayout.EndHorizontal();

    }

    void CreateLevel(StageLevelManager manager)
    {
        manager.CreateNewStage();
    }

    void CleanUp(StageLevelManager manager)
    {
        manager.CleanUpStage();
    }

    void Save(StageLevelManager manager)
    {
        string title = "Save";
        string message = "저장 하시겠습니까?";
        if (EditorUtility.DisplayDialog(title, message, "OK", "Cancel"))
        {
            string fileName = "stage-" + manager.stageId;
            string path =
                EditorUtility.SaveFilePanel(title,
                                            Application.dataPath + "/Resources/Stage",
                                            fileName, "xml");
            if (path.Length < 0) return;

            StageLevelManager.StageInfo stage = StageSerialize(manager);
            if (stage != null)
            {
                XmlSerializer ser = new XmlSerializer(typeof(StageLevelManager.StageInfo));
                StreamWriter writer = new StreamWriter(path);
                ser.Serialize(writer, stage);
                writer.Close();

                AssetDatabase.Refresh();
            }
        }
    }

    StageLevelManager.StageInfo StageSerialize(StageLevelManager manager)
    {
        var stage = new StageLevelManager.StageInfo();

        var stageCells = new List<StageLevelManager.StageCell>();
        int tileCount = manager.stageTiles.Count;
        for (int i = 0; i < tileCount; i++)
        {
            var ci = manager.stageTiles[i].GetCell();
            stageCells.Add(ci);
            Debug.Log("cell : x=" + ci.x + " / y=" + ci.y);
        }

        stage.id = manager.stageId;
        stage.size = manager.stageSize;
        stage.cells = stageCells.ToArray();
        return stage;
    }

    void Load(StageLevelManager manager)
    {
        string title = "Load";
        string message = "파일을 불러오시겠습니까?";
        if (EditorUtility.DisplayDialog(title, message, "네", "아니오"))
        {
            string path =
                EditorUtility.OpenFilePanel(title,
                                            Application.dataPath + "/Resources/Stage",
                                            "xml");

            if (path.Length < 0) return;

            WWW www = new WWW("file://" + path);

            XmlSerializer ser = new XmlSerializer(typeof(StageLevelManager.StageInfo));
            var info = ser.Deserialize(new StringReader(www.text)) as StageLevelManager.StageInfo;

            manager.LoadStage(info);
        }
    }

    void OnSceneGUI()
    {
        int id = GUIUtility.GetControlID(FocusType.Passive);
        HandleUtility.AddDefaultControl(id);

        StageLevelManager manager = target as StageLevelManager;

        Handles.BeginGUI();

        ShowSceneGUI(manager);
        PickTileEdit(manager);

        Handles.EndGUI();
    }

    void ShowSceneGUI(StageLevelManager manager)
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 20;
        GUI.Label(new Rect(20f, 20f, 120f, 30f), "EDIT TILE : " + manager.curEditType, style);

        if (GUI.Button(new Rect(20f, 70f, 100f, 30f), "None"))
        {
            manager.curEditType = StageLevelManager.StageCellType.None;
        }

        if (GUI.Button(new Rect(140f, 70f, 100f, 30f), "Object"))
        {
            manager.curEditType = StageLevelManager.StageCellType.Object;
        }

        if (GUI.Button(new Rect(260f, 70f, 100f, 30f), "WayPoint"))
        {
            manager.curEditType = StageLevelManager.StageCellType.WayPoint;
        }

        if (GUI.Button(new Rect(380f, 70f, 100f, 30f), "StartPoint"))
        {
            manager.curEditType = StageLevelManager.StageCellType.StartPoint;
        }

        if (GUI.Button(new Rect(20f, 110f, 100f, 30f), "EndPoint"))
        {
            manager.curEditType = StageLevelManager.StageCellType.EndPoint;
        }
    }
    
    static int wayPointCount = 0;

    void PickTileEdit(StageLevelManager manager)
    {
        string baseName = "WayPoint";

        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Vector3 mousePos = Event.current.mousePosition;
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject obj = hit.collider.gameObject;
                StageEditTile tile = obj.GetComponent<StageEditTile>();
                if (tile != null)
                {
                    var tileType = manager.curEditType;
                    /*
                    switch(tileType)
                    {
                        case StageLevelManager.StageCellType.None:
                            tile.EditType(tileType, manager.GetTileTypePrefab(tileType));
                            break;

                        case StageLevelManager.StageCellType.Object:
                            tile.EditType(tileType, manager.GetTileTypePrefab(tileType));
                            //AddGridData(manager.GetTileTypePrefab(, tileType));
                            break;

                        case StageLevelManager.StageCellType.WayPoint:
                            tile.EditType(tileType, manager.GetTileTypePrefab(tileType));
                            tile.transform.GetChild(0).name = baseName + wayPointCount;
                            //AddGridData(manager.GetTileTypePrefab(0, tileType));
                            wayPointCount++;
                            break;

                        case StageLevelManager.StageCellType.StartPoint:
                            tile.EditType(tileType, manager.GetTileTypePrefab(tileType));
                            //AddGridData(manager.GetTileTypePrefab(1, tileType));
                            break;

                        case StageLevelManager.StageCellType.EndPoint:
                            tile.EditType(tileType, manager.GetTileTypePrefab(tileType));
                            //AddGridData(manager.GetTileTypePrefab(2, tileType));
                            break;
                    }
                    */

                    if (tileType == StageLevelManager.StageCellType.WayPoint)
                    {
                        tile.EditType(tileType, manager.GetTileTypePrefab(tileType));
                        tile.transform.GetChild(0).name = baseName + wayPointCount;
                        //AddGridData(manager.GetTileTypePrefab(tileType));
                        wayPointCount++;
                    }
                    else
                    {
                        tile.EditType(tileType, manager.GetTileTypePrefab(tileType));
                        //AddGridData(manager.GetTileTypePrefab(tileType));
                    }

                    //Vector3 tilePosition = hit.point;
                }
            }
        }
    }

    #region Check Placement
    public int selectedObjectIndex;

    public List<GameObject> placedGameObjects = new();

    void AddGridData(int ID, GameObject prefab)
    {
        selectedObjectIndex = StageLevelManager.editorDatabase.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID foudn {ID}");
            return;
        }

        placedGameObjects.Add(prefab);
        Vector3 mousePosition = Event.current.mousePosition;
        Vector3Int gridPosition = StageLevelManager.grid.WorldToCell(mousePosition);
        GlobalVariables.selectedData.AddObjectAt(gridPosition,
            StageLevelManager.editorDatabase.objectsData[selectedObjectIndex].Size,
            StageLevelManager.editorDatabase.objectsData[selectedObjectIndex].ID,
            placedGameObjects.Count-1);
    }
    #endregion
}
