using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManagerment : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;

    [SerializeField]
    private LayerMask placementLayermask;

    public event Action OnClicked, OnExit;

    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnClicked?.Invoke();
        if(Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();
    }

    
    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();
    

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);

        //Debug.DrawRay(ray.origin, ray.direction * 20, Color.red, 1f);      //show ray
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit/*, placementLayermask*/))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }
}
