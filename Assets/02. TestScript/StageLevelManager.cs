using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using UnityEditor;

public class StageLevelManager : MonoBehaviour
{
    
    private GameObject obj;

   [SerializeField]
    private Preview preview;

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
        Wall,
        ItemBox,
        Goal,
        Player,
    }

    public List<StageEditTile> stageTiles = new List<StageEditTile>();

    public GameObject editTilePrefab;
    public GameObject wallPrefab;
    public GameObject itemBoxPrefab;
    public GameObject goalPrefab;
    public GameObject playerPrefab;

    public int stageId = 0;
    public int stageSize = 10;

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
                //Debug.Log("CreateNewStage : x=" + x + " / y=" + y);
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
            case StageCellType.Wall:
                prefab = wallPrefab;
                obj = wallPrefab;
                break;

            case StageCellType.ItemBox:
                prefab = itemBoxPrefab;
                obj = itemBoxPrefab;
                break;

            case StageCellType.Goal:
                prefab = goalPrefab;
                obj = goalPrefab;
                break;

            case StageCellType.Player:
                prefab = playerPrefab;
                obj = playerPrefab;
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

    public void PreviewObject()
    {
        GameObject newObject = Instantiate(obj);
        Vector3 mousePos = Event.current.mousePosition;
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObj = hit.collider.gameObject;
            newObject.transform.position = hitObj.transform.position;
        }
    }
}
