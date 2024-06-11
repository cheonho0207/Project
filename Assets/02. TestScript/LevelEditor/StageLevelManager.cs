using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using UnityEditor;

public class StageLevelManager : MonoBehaviour
{
    void OnEnable()
    {
        // manager.grid로 그리드 가져오기
        grid = FindObjectOfType<Grid>();
        if (grid == null)
        {
            Debug.LogWarning("Grid not found in the scene. Make sure to have a Grid GameObject in the scene.");
        }
    }


    [XmlRoot("stageInfo")]
    public class StageInfo
    {
        [XmlElement("id")]
        public int id;

        [XmlElement("size")]
        public int size;

        [XmlElement("cells")]
        public StageCell[] cells;

        // [XmlIgnore] public string a;
    }

    public struct StageCell
    {
        [XmlElement("x")]
        public int x;

        [XmlElement("y")]
        public int y;

        [XmlElement("type")]
        public StageCellType type;

        public StageCell(int x, int y, StageCellType type)
        {
            this.x = x;
            this.y = y;
            this.type = type;
        }
    }

    public enum StageCellType
    {
        None,
        Object,
        WayPoint,
        StartPoint,
        EndPoint,
    }

    public List<StageEditTile> stageTiles = new List<StageEditTile>();

    public GameObject editTilePrefab;
    public GameObject objectPrefab;
    public GameObject waypointPrefab;
    public GameObject startpointPrefab;
    public GameObject endpointPrefab;

    public ObjectsDatabaseSO editorDatabase;


    #region Check Placement
    [SerializeField]
    public Grid grid;

    #endregion

    public int stageId = 0;
    public int stageSize = 16;

    public StageCellType curEditType = StageCellType.None;


    public void CreateNewStage()
    {
        if (stageTiles.Count > 0)
        {
            CleanUpStage();
        }

        for (int x = 0; x < stageSize; x++)
        {
            for (int y = 0; y < stageSize; y++)
            {
                CreateTile(x, y, StageCellType.None);
                Debug.Log("CreateNewStage : x=" + x + " / y=" + y);
            }
        }
    }

    public void LoadStage(StageInfo info)
    {
        if (stageTiles.Count > 0)
        {
            CleanUpStage();
        }

        stageId = info.id;
        stageSize = info.size;
        for (int x = 0; x < stageSize; x++)
        {
            for (int y = 0; y < stageSize; y++)
            {
                foreach (var c in info.cells)
                {
                    if (c.x == x && c.y == y) CreateTile(x, y, c.type);
                }
            }
        }
    }

    void CreateTile(int x, int y, StageCellType type)
    {
        StageCell cell = new StageCell(x, y, type);

        GameObject editTile = Instantiate(editTilePrefab, transform);
        editTile.name = "tile(" + x + "," + y + ")";
        SetTilePosition(editTile, x, y);

        StageEditTile tile = editTile.GetComponent<StageEditTile>();
        tile.SetCell(cell);
        stageTiles.Add(tile);

        GameObject child = GetTileTypePrefab(type);
        if (child != null)
        {
            tile.EditType(type, child);
        }
    }

    public GameObject GetTileTypePrefab(StageCellType type)
    {
        GameObject prefab = null;
        switch (type)
        {
            case StageCellType.Object:
                prefab = objectPrefab;
                break;

            case StageCellType.WayPoint:
                prefab = waypointPrefab;
                break;

            case StageCellType.StartPoint:
                prefab = startpointPrefab;
                break;

            case StageCellType.EndPoint:
                prefab = endpointPrefab;
                break;
        }
        return prefab;
    }

    public void CleanUpStage()
    {
        int tileCount = stageTiles.Count;
        for (int i = 0; i < tileCount; i++)
        {
            GameObject tileObj = stageTiles[0].gameObject;
            stageTiles.RemoveAt(0);

            DestroyImmediate(tileObj);
        }
    }

    void SetTilePosition(GameObject tile, int x, int z)
    {
        Vector3 newPos = new Vector3(x, 0f, -z);
        Vector3 correction = new Vector3((stageSize * 0.5f), 0f, -(stageSize * 0.5f));
        tile.transform.localPosition = newPos - correction - new Vector3(-0.5f, 0f, 0.5f);
    }
}
