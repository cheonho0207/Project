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
        // Ŭ�� ������ ���·� �����ϱ� ���� ObjectDrag ��ũ��Ʈ�� ����
        Destroy(GetComponent<ObjectDrag>());

        // turret ������Ʈ�� ������ ��
        if (turret != null)
        {
            turret.DeactivateSparkEffect();
        }

        // turret2 ������Ʈ�� ������ ��
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
        // Turret�� ���콺�� ���� �Ű��� �� Spark Effect�� Ȱ��ȭ
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