using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyMovement : MonoBehaviour
{
    public float speed = 5f;  // ���� �̵� �ӵ�
    private Transform[] waypoints;  // ��������Ʈ �迭
    private List<Transform> remainingWaypoints = new List<Transform>(); // ���� ��������Ʈ ����Ʈ
    private Transform currentWaypoint;  // ���� ��ǥ ��������Ʈ
    public GameObject endPoint;  // ������ EndPoint ������Ʈ

    private HPManager hpManager;

    void Start()
    {
        waypoints = WayPoints.points;

        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning("No waypoints found!");
        }
        else
        {
            // ���� ��������Ʈ ����Ʈ �ʱ�ȭ
            remainingWaypoints.AddRange(waypoints);
            // ó���� ���� ����� ��������Ʈ ã��
            currentWaypoint = FindClosestWaypoint();
        }

        endPoint = GameObject.FindGameObjectWithTag("EndPoint");
        if (endPoint == null)
        {
            Debug.LogError("EndPoint not found! Make sure an object with the tag 'EndPoint' exists in the scene.");
        }
    }

    void Update()
    {
        //Debug.Log("Remaining waypoints: " + remainingWaypoints.Count + ", Current waypoint: " + (currentWaypoint != null ? currentWaypoint.name : "None"));

        if (remainingWaypoints.Count == 0 && currentWaypoint == null)
        {
            Debug.Log("All waypoints visited!");
            // EndPoint�� �̵�
            currentWaypoint = endPoint.transform;
        }

        if (currentWaypoint == null)
        {
            return;
        }

        // ���� �̵� ���� ��������Ʈ�� ���� �̵�
        Vector3 targetPosition = currentWaypoint.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // ���� �� ������Ʈ�� ��������Ʈ�� �����ߴٸ�, �������� ���� ����� ��������Ʈ ã��
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            if (currentWaypoint == endPoint.transform)
            {
                EndPath();
            }
            else
            {
                //Debug.Log("Reached waypoint: " + currentWaypoint.name);
                // ���� ��������Ʈ�� �����ϰ�, �������� ���� ����� ��������Ʈ ã��
                remainingWaypoints.Remove(currentWaypoint);
                currentWaypoint = null;  // �߰�: ���� ��������Ʈ�� �����ϱ� ���� currentWaypoint�� null�� ����

                if (remainingWaypoints.Count > 0)
                {
                    currentWaypoint = FindClosestWaypoint();
                }
                else
                {
                    currentWaypoint = endPoint.transform;
                }
                //Debug.Log("Next closest waypoint: " + (currentWaypoint != null ? currentWaypoint.name : "None"));
            }
        }
    }

    Transform FindClosestWaypoint()
    {
        Transform closestWaypoint = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform waypoint in remainingWaypoints)
        {
            float distance = Vector3.Distance(transform.position, waypoint.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestWaypoint = waypoint;
            }
        }

        return closestWaypoint;
    }

    void EndPath()
    {
        //PlayerStats.Lives--;
        WaveSpawner.EnemiesAlive--;
        Debug.Log("���� �� : " + WaveSpawner.EnemiesAlive);
        Destroy(this.gameObject);
        hpManager.HpDown();
    }
}