using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private bool alreadyProcessed = false;

    public int power = 5;

    public bool IsAlreadyProcessed()
    {
        return alreadyProcessed;
    }

    public void MarkAsProcessed()
    {
        alreadyProcessed = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 아무 것도 추가할 필요가 없다면 비워둘 수 있습니다.
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
           
            // 여기서는 Enemy 태그를 가진 객체와 충돌했을 때 화살을 파괴합니다.
            Destroy(gameObject); // 현재 화살 객체를 파괴합니다.
        }

        if (other.gameObject.CompareTag("Turret4"))
        {
            return; // 이 경우 추가 처리를 하지 않고 반환합니다.
        }

    }
}