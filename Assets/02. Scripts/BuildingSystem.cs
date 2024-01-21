using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
//using Unity.Burst.CompilerServices;
//using Unity.VisualScripting;
//using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;

    public GridLayout gridLayout;
    private Grid grid;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;

    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;

    private PlaceableObject objectToPlace;

    public static bool setable = false;

    #region Unity methods

    private void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 20, Color.blue, 60f); //show ray

            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            Debug.Log(hits[0].collider.name);
            Debug.Log(hits[0].collider.tag);
            Debug.Log(hits[1].collider.name);
            Debug.Log(hits[1].collider.tag);
        }
    }
    
    public void SetTower(GameObject prefeb,int a)
    {
        SettingWithObject(prefeb,a);
    }
    

    #endregion

    #region Utils

    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, z:0);
            array[counter]=tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    #endregion

    #region Building Placement

    public void InitializeWithObject(GameObject prefeb)
    {
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);

        GameObject obj = Instantiate(prefeb, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();
    }


    public void SettingWithObject(GameObject prefeb,int a)
    {
        Vector3 mousePosition = GetMouseWorldPosition();    //transform mousepotion with grid
        Vector3 position = SnapCoordinateToGrid(mousePosition);     //setting prefeb with mouseposition

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 20, Color.red, 60f);      //show ray

        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        if (hits[1].collider.CompareTag("Tile"))
        {
            Debug.Log("Spawn Tower");
            GameObject obj = Instantiate(prefeb, position, Quaternion.identity);
            obj.transform.Rotate(Vector3.up, 90f * a);
            objectToPlace = obj.GetComponent<PlaceableObject>();
        }
        else
        {
            Debug.Log("You Can`t Spawn Tower here");
        }
        /*
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.name);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.name);

                // if tag = tile, set tower
                if (hit.collider.CompareTag("Tile"))
                {
                    Debug.Log("tile");
                    GameObject obj = Instantiate(prefeb, position, Quaternion.identity);
                    obj.transform.Rotate(Vector3.up, 90f * a);
                    objectToPlace = obj.GetComponent<PlaceableObject>();
                }
                else
                {
                    Debug.Log("not tile");
                }
            }
        }
        */
        
    }
    

    /*
    private bool CanBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = placeableObject.Size;

        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);

        foreach(var b in baseArray)
        {
            if (b == whiteTile)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsPositionOccupied(Vector3 position)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(position);
        area.size = objectToPlace.Size;

        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);

        foreach (var b in baseArray)
        {
            if (b == whiteTile)
            {
                return true; // 해당 위치에 다른 오브젝트가 이미 있음을 나타내므로 true 반환
            }
        }

        return false; // 해당 위치에 다른 오브젝트가 없음을 나타내므로 false 반환
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        MainTilemap.BoxFill(start, null, start.x, start.y, start.x + size.x, start.y + size.y);
    }
    */
    #endregion
}