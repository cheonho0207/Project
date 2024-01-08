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
        // ���콺 ��ġ�� �׸���� ��ȯ
        Vector3 position = SnapCoordinateToGrid(mousePosition);

        // ������ ��ġ�� �̹� ������Ʈ�� �ִ��� Ȯ��
        if (IsPositionOccupied(position))
        {
            Debug.Log("�� ��ġ�� Ÿ���� ���� �� �����ϴ�. ��ġ�� �̹� ��� ���Դϴ�.");
            return; // ��ġ�� ��� ���̸� ��ġ�� �ǳʶݴϴ�.
        }

        // ������ ��ġ�� �ִ� ���� ������Ʈ�� �����մϴ� (�ִ� ���)
        RemoveObjectAtPosition(position);

        // ���콺 ��ġ�� Ÿ���� ��ġ�մϴ�.
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);

        // ������Ʈ�� ȸ����ŵ�ϴ�.
        obj.transform.Rotate(Vector3.up, 90f * a);

        // ��ġ�� ������Ʈ���� PlaceableObject ������Ʈ�� �����ɴϴ�.
        PlaceableObject placeableObject = obj.GetComponent<PlaceableObject>();

        if (placeableObject != null)
        {
            // ������Ʈ�� PlaceableObject ��ũ��Ʈ�� ������ BuildingSystem�� �����մϴ�.
            objectToPlace = placeableObject;
        }
        else
        {
            Debug.LogError("��ġ�� ������Ʈ���� PlaceableObject ��ũ��Ʈ�� ã�� �� �����ϴ�!");
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
                return true; // �ش� ��ġ�� �ٸ� ������Ʈ�� �̹� ������ ��Ÿ���Ƿ� true ��ȯ
            }
        }

        return false; // �ش� ��ġ�� �ٸ� ������Ʈ�� ������ ��Ÿ���Ƿ� false ��ȯ
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        MainTilemap.BoxFill(start, null, start.x, start.y, start.x + size.x, start.y + size.y);
    }
    #endregion
}