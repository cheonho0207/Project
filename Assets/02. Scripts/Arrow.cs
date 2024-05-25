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
        // �ƹ� �͵� �߰��� �ʿ䰡 ���ٸ� ����� �� �ֽ��ϴ�.
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
           
            // ���⼭�� Enemy �±׸� ���� ��ü�� �浹���� �� ȭ���� �ı��մϴ�.
            Destroy(gameObject); // ���� ȭ�� ��ü�� �ı��մϴ�.
        }

        if (other.gameObject.CompareTag("Turret4"))
        {
            return; // �� ��� �߰� ó���� ���� �ʰ� ��ȯ�մϴ�.
        }

    }
}