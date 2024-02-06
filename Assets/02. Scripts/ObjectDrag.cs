using UnityEditor.Presets;
using UnityEngine;


public class ObjectDrag : MonoBehaviour
{
    public GameObject settower;
    int count = 0;

    private Turret turret;
    private Turret2 turret2;
    private Turret3  turret3;

    private void Start()
    {
        turret = GetComponent<Turret>();
        turret2 = GetComponent<Turret2>();
        turret3 = GetComponent<Turret3>();
    }

    private void OnMouseUp()
    {
        // 클릭 가능한 상태로 유지하기 위해 ObjectDrag 스크립트를 제거
        Destroy(GetComponent<ObjectDrag>());

        // turret 컴포넌트가 존재할 때
        if (turret != null)
        {
            turret.DeactivateSparkEffect();
        }

        // turret2 컴포넌트가 존재할 때
        if (turret2 != null)
        {
            turret2.DeactivateSparkEffect();
        }

        if (turret3 != null)
        {
            turret3.DeactivateSparkEffect();
        }

        settower = GameObject.Find("Grid");
        settower.GetComponent<BuildingSystem>().SetTower(gameObject, count);
        // Turret이 마우스에 따라 옮겨질 때 Spark Effect을 활성화
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