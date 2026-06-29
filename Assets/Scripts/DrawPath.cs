using System;
using System.Collections.Generic;
using UnityEngine;

public class DrawPath : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private PathfindingNode goal;
    [SerializeField] private float timeBetweenChecks;
    [SerializeField] private float addedTimeToDestroy;

    private float counter;

    private List<PathfindingNode> path = new List<PathfindingNode>();

    [SerializeField] private GameObject linePrefab;

    private void FixedUpdate()
    {
        counter += Time.fixedDeltaTime;

        if (counter >= timeBetweenChecks)
        {
            counter = 0;

            GetPath();

            if (path.Count > 0)
            {
                DrawThePath(player.transform.position, path[0].transform.position);
                for (int i = 0; i < path.Count - 1; i++)
                {
                    DrawThePath(path[i].transform.position, path[i + 1].transform.position);
                }
            }
        }

        
    }

    private void GetPath()
    {
        player.GetClosestNode();
        path = AStar.Run(GameManager.instance.playerClosestNode, NodeIsGoal, GetNeighbours, GetHeuristic);
    }

    private void DrawThePath(Vector3 from, Vector3 to)
    {
        if (linePrefab == null)
            return;
        Vector3 dir = to - from;
        float distance = dir.magnitude;
        if (distance <= 0.001f)
            return;
        GameObject lightning = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        LineRenderer lr = lightning.GetComponent<LineRenderer>();
        if (lr == null)
        {
            Destroy(lightning);
            return;
        }
        lr.useWorldSpace = true;
        Vector3 forward = dir.normalized;
        Vector3 perp = Vector3.Cross(forward, Vector3.forward).normalized;

        lr.positionCount = 2;
        lr.SetPosition(0, from);
        lr.SetPosition(1, to);
        Destroy(lightning, timeBetweenChecks + addedTimeToDestroy);
    }

    private bool NodeIsGoal(PathfindingNode recieved)
    {
        return recieved == goal;
    }

    private List<PathfindingNode> GetNeighbours(PathfindingNode node)
    {
        return node.neightbourds;
    }

    private float GetHeuristic(PathfindingNode node)
    {
        return Vector3.Distance(goal.transform.position, node.transform.position);
    }
}
