using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween_Path : MonoBehaviour
{
    // �÷��̾� ������Ʈ�� ������ ����
    public GameObject m_PlayerObj;

    // ��θ� �����ϴ� �������� �迭
    public Transform[] positionPoint;

    // 0���� 1 ������ ���� ������ ����
    [Range(0, 1)]
    public float value;

    // �浹 ���θ� �����ϱ� ���� ����
    private bool hasCollided = false;

  

    void Update()
    {
        if (hasCollided) // �浹���� ���� ��� �̵�
        {
            // value ���� 1�� ������Ű�µ� �ð��� ���� õõ�� ����
            if (value < 1)
            {
                value += Time.deltaTime / 10;
            }

            // ��� �󿡼� value ��ġ�� �÷��̾� ������Ʈ�� ��ġ
            iTween.PutOnPath(m_PlayerObj, positionPoint, value);
        }
    }

    // ������ �󿡼� �ð������� ��θ� ��Ÿ���� �Լ�
    private void OnDrawGizmos()
    {
        iTween.DrawPath(positionPoint, Color.green);
        //iTween ���̺귯���� DrawPath �Լ��� ����Ͽ� positionPoint �迭�� ������ �ʷϻ� ��θ� �׸�
    }

    // �浹�� �����ϴ� �Լ�
    private void OnCollisionEnter(Collision collision)
    {
        // ���� �浹�� ������Ʈ�� "Touch" �±׸� ������ �ִٸ� �Ʒ� �ڵ带 ����
        if (collision.gameObject.CompareTag("Touch"))
        {
            // �浹�� �����Ǹ� �浹 �÷��׸� �����Ͽ� ��� �̵��� ����
            hasCollided = true;
        }
    }
}

