using System; //
using System.Collections; //
using System.Collections.Generic; //
using Unity.VisualScripting; //
using UnityEngine; //

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float previewYOffset = 0.06f;

    [SerializeField]
    private GameObject cellIndicator;
    private GameObject previewObject;

    [SerializeField]
    private Material previewMaterialsPrefab;
    private Material previewMaterialInstance;

    private Renderer cellIndicatorRenderer;

    bool SetTower = false;
    public int count;

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialsPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        RemoveScriptsRecursive(previewObject.transform);
        PreparePreaview(previewObject);
        PrepareCursor(size);
        cellIndicator.SetActive(true);
        SetTower = true;
    }

    void RemoveScriptsRecursive(Transform parentTransform)
    {
        // 현재 Transform의 모든 MonoBehaviour 가져오기
        MonoBehaviour[] allScripts = parentTransform.GetComponents<MonoBehaviour>();

        // 모든 스크립트 컴포넌트 제거
        foreach (var script in allScripts)
        {
            // 스크립트를 비활성화한 후에 제거하기
            script.enabled = false;
            Destroy(script);
        }

        // 모든 자식 Transform에 대해 재귀적으로 호출
        foreach (Transform childTransform in parentTransform)
        {
            RemoveScriptsRecursive(childTransform);
        }
    }

    private void Update()
    {
        if (SetTower == true)
        {
            int count = 0;
            if (Input.GetMouseButtonDown(1))
            {
                previewObject.transform.GetChild(0).transform.Rotate(Vector3.up, 90f);
                //previewObject.transform.Rotate(Vector3.up, 90f);
                count++;
                if (count == 4)
                {
                    count -= 4;
                }
            }
            /*
            if (count == 0)
            {
                cellIndicator.transform.localScale = new Vector3(
                    cellIndicator.transform.localScale.x,
                    cellIndicator.transform.localScale.y,
                    cellIndicator.transform.localScale.z);
            }
            else if (count == 1)
            {
                cellIndicator.transform.localScale = new Vector3(
                    cellIndicator.transform.localScale.z,
                    cellIndicator.transform.localScale.y,
                    -cellIndicator.transform.localScale.x + 1f);
            }
            else if (count == 2)
            {
                cellIndicator.transform.localScale = new Vector3(
                    -cellIndicator.transform.localScale.x + 1f,
                    cellIndicator.transform.localScale.y,
                    -cellIndicator.transform.localScale.z + 1f);
            }
            else if (count == 3)
            {
                cellIndicator.transform.localScale = new Vector3(
                    -cellIndicator.transform.localScale.z + 1f,
                    cellIndicator.transform.localScale.y,
                    cellIndicator.transform.localScale.x);
            }
            */
        }
        else
        {
            return;
        }
            
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {

            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRenderer.material.mainTextureScale = size;
            
            /*
            if (count == 0)
            {
                cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
                cellIndicatorRenderer.material.mainTextureScale = size;
            }
            else if (count == 1)
            {
                cellIndicator.transform.localScale = new Vector3(size.y, 1, -size.x);
                cellIndicatorRenderer.material.mainTextureScale = size;
            }
            else if (count == 2)
            {
                cellIndicator.transform.localScale = new Vector3(-size.x, 1, -size.y);
                cellIndicatorRenderer.material.mainTextureScale = size;
            }
            else if (count == 3)
            {
                cellIndicator.transform.localScale = new Vector3(-size.y, 1, size.x);
                cellIndicatorRenderer.material.mainTextureScale = size;
            }
            */
        }
    }

    private void PreparePreaview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false );
        Destroy( previewObject );
        SetTower = false;
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        MovePreview(position);
        MoveCursor(position);
        ApplyFeedback(validity);
    }

    private void ApplyFeedback(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        cellIndicatorRenderer.material.color = c;
        c.a = 0.5f;
        previewMaterialInstance.color = c;
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(
            position.x, 
            position.y + previewYOffset, 
            position.z );
    }
}
