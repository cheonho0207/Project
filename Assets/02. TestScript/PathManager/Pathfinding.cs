using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public WayPoint[] wayPoints;

    private List<WayPoint> FindPath()
    {
        WayPoint startWP = startPoint.GetComponent<WayPoint>();
        WayPoint endWP = endPoint.GetComponent<WayPoint>();

        List<WayPoint> openList = new List<WayPoint>();
        HashSet<WayPoint> closedList = new HashSet<WayPoint>();

        openList.Add(startWP);

        Dictionary<WayPoint, WayPoint> cameFrom = new Dictionary<WayPoint, WayPoint>();
        Dictionary<WayPoint, float> gScore = new Dictionary<WayPoint, float>();
        Dictionary<WayPoint, float> fScore = new Dictionary<WayPoint, float>();

        foreach (var wp in wayPoints)
        {
            gScore[wp] = Mathf.Infinity;
            fScore[wp] = Mathf.Infinity;
        }

        gScore[startWP] = 0;
        fScore[startWP] = startWP.DistanceTo(endWP);

        while (openList.Count > 0)
        {
            WayPoint current = openList[0];

            foreach (var wp in openList)
            {
                if (fScore[wp] < fScore[current])
                {
                    current = wp;
                }
            }

            if (current == endWP)
            {
                return ReconstructPath(cameFrom, current);
            }

            openList.Remove(current);
            closedList.Add(current);

            foreach (var neighbor in wayPoints)
            {
                if (closedList.Contains(neighbor) || current == neighbor)
                {
                    continue;
                }

                float tentativeGScore = gScore[current] + current.DistanceTo(neighbor);

                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
                else if (tentativeGScore >= gScore[neighbor])
                {
                    continue;
                }

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + neighbor.DistanceTo(endWP);
            }
        }

        return null;
    }

    private List<WayPoint> ReconstructPath(Dictionary<WayPoint, WayPoint> cameFrom, WayPoint current)
    {
        List<WayPoint> totalPath = new List<WayPoint> { current };

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }

        return totalPath;
    }

    void Start()
    {
        List<WayPoint> path = FindPath();
        if (path != null)
        {
            foreach (var wp in path)
            {
                Debug.Log("WayPoint: " + wp.name);
            }
        }
        else
        {
            Debug.Log("Path not found");
        }
    }
}