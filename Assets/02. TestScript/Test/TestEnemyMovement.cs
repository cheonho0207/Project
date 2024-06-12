using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyMovement : MonoBehaviour
{
    public float speed = 5f;  // ���� �̵� �ӵ�
    private Transform[] waypoints;  // ��������Ʈ �迭
    private List<Transform> remainingWaypoints = new List<Transform>(); // ���� ��������Ʈ ����Ʈ
    private Transform currentWaypoint;  // ���� ��ǥ ��������Ʈ

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
    }

    void Update()
    {
        if (remainingWaypoints.Count == 0)
        {
            Debug.Log("All waypoints visited!");
            return;
        }

        // ���� �̵� ���� ��������Ʈ�� ���� �̵�
        Vector3 targetPosition = currentWaypoint.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // ���� �� ������Ʈ�� ��������Ʈ�� �����ߴٸ�, �������� ���� ����� ��������Ʈ ã��
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            //Debug.Log("Reached waypoint: " + currentWaypoint.name);
            // ���� ��������Ʈ�� �����ϰ�, �������� ���� ����� ��������Ʈ ã��
            remainingWaypoints.Remove(currentWaypoint);
            currentWaypoint = FindClosestWaypoint();
            //Debug.Log("Next closest waypoint: " + (currentWaypoint != null ? currentWaypoint.name : "None"));
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
}