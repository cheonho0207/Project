using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Presets;
using UnityEngine;


public class ObjectDrag : MonoBehaviour
{
    public GameObject settower;
    int count = 0;



    private void OnMouseUp()
    {
        Destroy(gameObject);
        SetTower();
    }
    private void SetTower()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // 충돌한 gameobject의 태그가 "tile"인지 확인합니다.
            if (hit.collider.CompareTag("Tile"))
            {
                Debug.Log("tile");
                settower = GameObject.Find("Grid");
                settower.GetComponent<BuildingSystem>().SetTower(gameObject, count);
            }
            else
            {
                Debug.Log("not tile");
            }
        }
    }

    private void Update()
    {
        Vector3 pos = BuildingSystem.GetMouseWorldPosition();
        transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos);
        if (Input.GetMouseButtonDown(1))
        {
            gameObject.transform.Rotate(Vector3.up, 90f);
            count++;
            if (count == 4)
            {
                count -= 4;
            }
        }
    }
}

//public enum AttackType { Range, Expolosion, Melee };
/*
public class Monster : MonoBehaviour
{
    public void SetDamage(AttackType attackType, float damage)
    {
        // Set Damage
    }
}
*/
  /*  private AttackType attackType;
    private RaycastHit[] raycastHits = new RaycastHit[2];

    private void OnCollisionEnter(Collision collision)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        //Physics.Raycast(ray, out );
        //if (raycastHits[0].transform.TryGetComponent(out Monster monster))
        if (collision.transform.TryGetComponent(out Monster monster))
        {
            monster.SetDamage(attackType, 2.0f);
        }
    }
  */