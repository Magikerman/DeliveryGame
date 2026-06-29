using System;
using System.Collections.Generic;

public class AStar
{
    public static List<PathfindingNode> Run(PathfindingNode initialNode, Func<PathfindingNode, bool> isSatisfied, Func<PathfindingNode, List<PathfindingNode>> getConnections, /*Func<PathfindingNode, PathfindingNode, float> getCosts,*/ Func<PathfindingNode, float> heuristic)
    {
        PriorityQueue<PathfindingNode> pending = new PriorityQueue<PathfindingNode>();
        HashSet<PathfindingNode> visited = new HashSet<PathfindingNode>();
        Dictionary<PathfindingNode, PathfindingNode> parents = new Dictionary<PathfindingNode, PathfindingNode>();
        Dictionary<PathfindingNode, float> costs = new Dictionary<PathfindingNode, float>();

        if (initialNode == null)
            return new List<PathfindingNode>();

        costs[initialNode] = 0;

        pending.Enqueue(initialNode, 0);
        visited.Add(initialNode);

        while (!pending.IsEmpty)
        {
            PathfindingNode node = pending.Dequeue();
            if (isSatisfied(node))
            {
                List<PathfindingNode> path = new List<PathfindingNode>();
                path.Add(node);
                PathfindingNode current = node;

                while (parents.ContainsKey(current))
                {
                    path.Add(parents[current]);
                    current = parents[current];
                }

                path.Reverse();
                return path;
            }
            else
            {
                List<PathfindingNode> children = getConnections(node);

                for (int i = 0; i < children.Count; ++i)
                {
                    if (visited.Contains(children[i]))
                    {
                        continue;
                    }
                    float currentCosts = costs[node] + 1f /*+ getCosts(node, children[i])*/;
                    if (costs.ContainsKey(children[i]) && currentCosts > costs[children[i]])
                    {
                        continue;
                    }
                    costs[children[i]] = currentCosts;
                    pending.Enqueue(children[i], currentCosts + heuristic(children[i]));
                    visited.Add(children[i]);
                    parents[children[i]] = node;
                }
            }
        }
        return new List<PathfindingNode>();
    }
}
