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
            Destroy(gameObject); // 현재 화살 객체를 파괴합니다.
        }
    }
}