using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tween_Path : MonoBehaviour
{
    public GameObject m_PlayerObj;
    public Transform[] positionPoint;
    [Range(0, 1)]
    public float value;
    private bool hasCollided = false;
    private bool isPaused = false;
    public float range = 1.0f;
    private bool hasReachedEnd = false;

    private void Start()
    {
        if (m_PlayerObj == null)
        {
            m_PlayerObj = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void Update()
    {
        if (!isPaused && hasCollided)
        {
            if (value < 1)
            {
                value += Time.deltaTime / 10;
                iTween.PutOnPath(m_PlayerObj, positionPoint, value);
            }
            else
            {
                hasReachedEnd = true; // ��� ���� �����ϸ� �÷��׸� �����մϴ�.
            }
        }
    }

    public bool HasReachedEnd()
    {
        return hasReachedEnd;
    }

    private void OnDrawGizmos()
    {
        iTween.DrawPath(positionPoint, Color.green);
    }

    public bool HasCollided()
    {
        return hasCollided;
    }

    public void SetPause(bool pause)
    {
        isPaused = pause;

        if (!isPaused && hasCollided)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, range);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    hasCollided = false;
                    value = 0f;
                    break;
                }
            }
        }
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Touch"))
        {
            if (!isPaused)
            {
                hasCollided = true;
            }
        }
    }

    private void ResumePath()
    {
        isPaused = false;
        hasCollided = true;
        value = 0f;
        iTween.Resume(m_PlayerObj);
    }

    // �߰��� �޼���: ���� ��ο��� ��� �� ȣ��Ǿ� ��θ� �簳�մϴ�.
    public void EnemyExitedPath()
    {
        if (isPaused)
        {
            ResumePath();
        }
    }
}
