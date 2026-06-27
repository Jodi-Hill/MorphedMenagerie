using System;
using UnityEngine;

public class Path : MonoBehaviour
{
    public enum PathType
    {
        Loop,
        ReverseWhenComplete,
        StopAtEnd
    }

    public Transform[] waypoints;
    public PathType pathType = PathType.Loop;

    private int direction = 1;
    public int index;

    public Vector3 GetCurrentWayPoint()
    {
        return waypoints[index].position;
    }

    public Vector3 GetNextWaypoint()
    {
        if (waypoints.Length == 0) return transform.position;

        index = GetNextWaypointIndex();
        Vector3 nextWaypoint = waypoints[index].position;

        return nextWaypoint;
    }

    private int GetNextWaypointIndex()
    {
        if (pathType == PathType.StopAtEnd && index == waypoints.Length - 1)
        {
            return index;
        }
        
        index += direction;

        if (pathType == PathType.Loop)
        {
            index %= waypoints.Length;
        }
        else if (pathType == PathType.ReverseWhenComplete)
        {
        if (index >= waypoints.Length || index < 0)
            {
                index *= -1;
                index += direction * 2;
            }
        }

        return index;
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Gizmos.color = Color.white;

        for (int i = 0; i < waypoints.Length -1; i++)
        {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }

        if (pathType == PathType.Loop)
        {
            Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
        }

        Gizmos.color = Color.red;
        foreach (Transform waypoint in waypoints)
        {
            Gizmos.DrawSphere(waypoint.position, 0.2f);
        }
    }

    public static object Combine(string dataPath, string v)
    {
        throw new NotImplementedException();
    }

    public bool IsLastWayPoint()
    {
        return index == waypoints.Length - 1;
    }
}
