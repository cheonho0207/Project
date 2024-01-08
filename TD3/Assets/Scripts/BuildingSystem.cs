using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;

    public GridLayout gridLayout;
    private Grid grid;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;

    public GameObject prefab1;
    public GameObject prefab2;
    //public GameObject prefab3;

    private PlaceableObject objectToPlace;
    public string enemyTag = "Enemy";
    #region Unity methods

    private void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    private void Update()
    {
        if (!objectToPlace)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (CanBePlaced(objectToPlace))
            {
                objectToPlace.Place();
                Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
                TakeArea(start, objectToPlace.Size);
            }
            else
            {
                Debug.Log("you can`t set here");
                Destroy(objectToPlace.gameObject);
            }
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
        if(Physics.Raycast(ray, out RaycastHit raycastHit))
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

    public void SettingWithObject(GameObject prefab, int a)
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        // 마우스 위치를 그리드로 변환
        Vector3 position = SnapCoordinateToGrid(mousePosition);

        // 지정된 위치에 이미 오브젝트가 있는지 확인
        if (IsPositionOccupied(position))
        {
            Debug.Log("이 위치에 타워를 놓을 수 없습니다. 위치가 이미 사용 중입니다.");
            return; // 위치가 사용 중이면 배치를 건너뜁니다.
        }

        // 지정된 위치에 있는 기존 오브젝트를 제거합니다 (있는 경우)
        RemoveObjectAtPosition(position);

        // 마우스 위치에 타워를 배치합니다.
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);

        // 오브젝트를 회전시킵니다.
        obj.transform.Rotate(Vector3.up, 90f * a);

        // 배치된 오브젝트에서 PlaceableObject 컴포넌트를 가져옵니다.
        PlaceableObject placeableObject = obj.GetComponent<PlaceableObject>();

        if (placeableObject != null)
        {
            // 오브젝트에 PlaceableObject 스크립트가 있으면 BuildingSystem에 설정합니다.
            objectToPlace = placeableObject;
        }
        else
        {
            Debug.LogError("배치된 오브젝트에서 PlaceableObject 스크립트를 찾을 수 없습니다!");
        }
    }

    private void RemoveObjectAtPosition(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapBox(position, Vector3.one * 0.5f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != null && collider.gameObject.CompareTag("Tower"))
            {
                Destroy(collider.gameObject);
            }
        }
    }

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
    #endregion
}