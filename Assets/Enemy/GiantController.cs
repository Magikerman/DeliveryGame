using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GiantController : EnemyController
{
    [SerializeField] private float rotSpeed;
    [SerializeField] private float speed;
    [SerializeField, Range(0, 1)] private float circleRadius;
    [SerializeField] private float maxPredictionTime;

    [SerializeField] private float maxSpeed;

    private float timer;

    [SerializeField] private float timeToAttack;

    [SerializeField] private float radius;
    [SerializeField] private LayerMask nodeLayer;
    [SerializeField] private LayerMask obstacleLayer;
    private List<PathfindingNode> path = new List<PathfindingNode>();

    void Awake()
    {
        context = GetComponent<Context>();
        tree = GetComponent<EnemyTree>();
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = maxSpeed;
    }

    void Update()
    {
        tree.Evaluate(this, context);

        switch (enemyState)
        {
            case state.idle:
                rotation = SteeringBehaviour.GetRotation(transform, context.player, context.playerRb, maxPredictionTime);
                inRange = true;
                break;
            case state.pursue:
                rotation = SteeringBehaviour.GetRotation(transform, context.player, context.playerRb, maxPredictionTime);
                inRange = true;
                break;
            case state.range:
                rotation = -SteeringBehaviour.GetRotation(transform, context.player, context.playerRb, maxPredictionTime);
                inRange = true;
                break;
            case state.circle:
                rotation = Vector3.Lerp(transform.right, transform.forward, circleRadius);
                inRange = false;
                break;
            case state.attack:
                rotation = SteeringBehaviour.GetRotation(transform, context.player, context.playerRb, maxPredictionTime);
                timer += Time.deltaTime;

                if (timer >= timeToAttack)
                {
                    timer = 0;
                    GameManager.instance.gasTank.Refill(-0.1f);
                }

                inRange = true;
                break;
            case state.pathfinding:

                path = AStar.Run(GetClosestNode(), NodeIsGoal, GetNeighbours, GetHeuristic);
                if (path.Count > 0)
                {
                    Vector3 dir = path[1].transform.position - transform.position;

                    Ray ray = new Ray(transform.position, dir);
                    Debug.DrawRay(transform.position, dir, Color.red);

                    if (LineOfSight.InSight(transform, path[1].transform, obstacleLayer))
                    {
                        rotation = SteeringBehaviour.GetRotation(transform, path[1].transform);
                    }
                    else
                    {
                        rotation = SteeringBehaviour.GetRotation(transform, path[0].transform);
                    }

                }

                inRange = false;
                break;
        }
    }

    private void FixedUpdate()
    {
        rotation.y = transform.forward.y;
        Vector3 dir = transform.forward;
        dir.y = 0;

        if (enemyState != state.idle && enemyState != state.attack)
        {
            transform.forward = Vector3.Lerp(transform.forward, rotation, rotSpeed * Time.fixedDeltaTime);
            rb.AddForce(dir * speed * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    public PathfindingNode GetClosestNode()
    {
        PathfindingNode closestNode = null;

        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, radius, Vector3.forward, radius, nodeLayer, QueryTriggerInteraction.UseGlobal);

        float closestNodeDistance = Mathf.Infinity;

        foreach (RaycastHit hit in hits)
        {
            if (hit.distance < closestNodeDistance)
            {
                PathfindingNode checkingNode = hit.collider.gameObject.GetComponent<PathfindingNode>();
                if (checkingNode != null)
                {
                    closestNode = checkingNode;
                    closestNodeDistance = hit.distance;
                }
            }
        }

        return closestNode;
    }

    private bool NodeIsGoal(PathfindingNode recieved)
    {
        return recieved == GameManager.instance.playerClosestNode;
    }

    private List<PathfindingNode> GetNeighbours(PathfindingNode node)
    {
        return node.neightbourds;
    }

    private float GetHeuristic(PathfindingNode node)
    {
        return Vector3.Distance(GameManager.instance.playerClosestNode.transform.position, node.transform.position);
    }
}
