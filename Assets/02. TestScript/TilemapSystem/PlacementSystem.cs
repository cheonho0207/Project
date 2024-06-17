using System; //
using System.Collections; //
using System.Collections.Generic; //
using UnityEditor.VersionControl;
using UnityEngine; //

public class PlacementSystem : MonoBehaviour
{
    #region setting
    [SerializeField]
    private GameObject mouseIndicator; //cellIndicator
    [SerializeField]
    private InputManagerment inputManager;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private GameObject stopText;

    [SerializeField]
    private ObjectsDatabaseSO database;
    private int selectedObjectIndex;

    [SerializeField]
    private GameObject gridVisualization;

    /*
    [SerializeField]
    private AudioSource source;
    */

    private GridData floorData, furnitureData;

    private Renderer previewRenderer;

    private List<GameObject> placedGameObjects = new();

    [SerializeField]
    private PreviewSystem preview;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private Credit credit;

    int count;

    #endregion

    private void Start()
    {
        StopPlacement();
        floorData = new();
        furnitureData = new();
        //previewRenderer = cellIndicator.GetComponentInChildren<Renderer>();
        credit = FindObjectOfType<Credit>();
    }

    #region 타워설치
    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID foudn {ID}");
            return;
        }
        gridVisualization.SetActive(true);
        preview.StartShowingPlacementPreview(
            database.objectsData[selectedObjectIndex].Prefab,
            database.objectsData[selectedObjectIndex].Size);
        //cellIndicator.SetActive(true);
        mouseIndicator.SetActive(false);
        stopText.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        //source.Play(); //sound play

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
            return;

        int cost = database.objectsData[selectedObjectIndex].Cost;
        if (credit.haveCredit < cost)
        {
            Debug.Log("재화가 부족합니다");
            return;
        }
        credit.haveCredit -= cost;
        credit.UpdateCoinText();


        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        newObject.transform.GetChild(0).transform.Rotate(Vector3.up, 90 * count);
        Debug.Log(count);

        placedGameObjects.Add(newObject);
        GlobalVariables.selectedData = database.objectsData[selectedObjectIndex].ID == 0 ?
            floorData :
            furnitureData;
        GlobalVariables.selectedData.AddObjectAt(gridPosition,
            database.objectsData[selectedObjectIndex].Size,
            database.objectsData[selectedObjectIndex].ID,
            placedGameObjects.Count - 1);
        preview.UpdatePosition(grid.CellToWorld(gridPosition), false);
        StopPlacement();
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ?
            floorData :
            furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        preview.StopShowingPreview();  //cellIndicator.SetActive(false);
        mouseIndicator.SetActive(false);
        stopText.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        count = 0;
    }
    #endregion
    private void Update()
    {

        if (selectedObjectIndex < 0)
            return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (lastDetectedPosition != gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

            //previewRenderer.material.color = placementValidity ? Color.white : Color.red;

            mouseIndicator.transform.position = mousePosition; //mouse point
                                                               //cellIndicator.transform.position = grid.CellToWorld(gridPosition); //show pointed sell
            preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
            lastDetectedPosition = gridPosition;
        }

        if (Input.GetMouseButtonDown(1))
        {
            count++;
            if (count == 4)
            {
                count -= 4;
            }
        }
    }
}