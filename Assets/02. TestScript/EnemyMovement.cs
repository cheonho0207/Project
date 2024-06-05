using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TestEnemy))]
public class EnemyMovement : MonoBehaviour
{
    private GameObject endPoint;
    private Transform target;
    private int wavepointIndex = 0;

    private TestEnemy enemy;

    void Start()
    {
        enemy = GetComponent<TestEnemy>();
        
        target = Waypoints.points[0];

        endPoint = GameObject.FindGameObjectWithTag("EndPoint");
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
        }

        enemy.speed = enemy.startSpeed;
    }

    void GetNextWaypoint()
    {
        if (wavepointIndex >= Waypoints.points.Length - 1)
        {
            //target = endPoint.transform;
            //EndPath();
            EndPoint();
            return;
        }

        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];
    }

    void EndPoint()
    {
        target = endPoint.transform;
        if(gameObject.transform == endPoint.transform)
        {
            EndPath();
            return;
        }
    }

    void EndPath()
    {
        //PlayerStats.Lives--;
        WaveSpawner.EnemiesAlive--;
        Destroy(gameObject);
    }

}