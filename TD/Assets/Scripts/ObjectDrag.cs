using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEditor.Presets;
using UnityEngine;


public class ObjectDrag : MonoBehaviour
{
    public GameObject settower;
    int count = 0;

    /*
    private void OnMouseUp()
    {
        Destroy(gameObject);
        SetTower();
    }
    */

    private void SetTower()
    {
        settower = GameObject.Find("Grid");
        settower.GetComponent<BuildingSystem>().SetTower(gameObject, count);

        /*
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 20, Color.red, 60f); //show ray

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.name);

            // if tag = tile, set tower
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
        */
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
        else if (Input.GetMouseButtonDown(0)) 
        {
            Destroy(gameObject) ;
            SetTower();
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