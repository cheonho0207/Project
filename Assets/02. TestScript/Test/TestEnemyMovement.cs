using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyMovement : MonoBehaviour
{
    public float speed = 5f;  // 적의 이동 속도
    private Transform[] waypoints;  // 웨이포인트 배열
    private List<Transform> remainingWaypoints = new List<Transform>(); // 남은 웨이포인트 리스트
    private Transform currentWaypoint;  // 현재 목표 웨이포인트
    public GameObject endPoint;  // 마지막 EndPoint 오브젝트

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
            // 남은 웨이포인트 리스트 초기화
            remainingWaypoints.AddRange(waypoints);
            // 처음에 가장 가까운 웨이포인트 찾기
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
            // EndPoint로 이동
            currentWaypoint = endPoint.transform;
        }

        if (currentWaypoint == null)
        {
            return;
        }

        // 현재 이동 중인 웨이포인트를 향해 이동
        Vector3 targetPosition = currentWaypoint.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // 만약 적 오브젝트가 웨이포인트에 도착했다면, 다음으로 가장 가까운 웨이포인트 찾기
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            if (currentWaypoint == endPoint.transform)
            {
                EndPath();
            }
            else
            {
                //Debug.Log("Reached waypoint: " + currentWaypoint.name);
                // 현재 웨이포인트를 제거하고, 다음으로 가장 가까운 웨이포인트 찾기
                remainingWaypoints.Remove(currentWaypoint);
                currentWaypoint = null;  // 추가: 다음 웨이포인트를 설정하기 전에 currentWaypoint를 null로 설정

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
        Debug.Log("남은 적 : " + WaveSpawner.EnemiesAlive);
        Destroy(this.gameObject);
        hpManager.HpDown();
    }
}