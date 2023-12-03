using UnityEngine;

//public enum AttackType { Range, Expolosion, Melee };

public class ObjectDrag : MonoBehaviour
{
    private Vector3 offset;

    private void OnMouseDown()
    {
        // offset = transform.position - BuildingSystem.GetMouseWorldPosition();
    }

    // private void OnMouseDrag()
    private void Update()
    {
        // Vector3 pos = BuildingSystem.GetMouseWorldPosition() + offset;
        Vector3 pos = BuildingSystem.GetMouseWorldPosition();
        transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos);
    }

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
}
/*
public class Monster : MonoBehaviour
{
    public void SetDamage(AttackType attackType, float damage)
    {
        // Set Damage
    }
}
*/