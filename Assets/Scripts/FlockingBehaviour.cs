using System;
using System.Collections.Generic;
using UnityEngine;

public class FlockingBehaviour : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private FlockObject agentPrefab;
    [SerializeField] private int minAgents;
    [SerializeField] private int maxAgents;
    [SerializeField] private Vector3 spawnExtents = new Vector3(10f, 5f, 5f);

    [Header("Movement")]
    [SerializeField] private float minSpeed = 4f;
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float maxForce = 10f;

    [Header("Neighbors")]
    [SerializeField] private float neighborRadius = 4f;
    [SerializeField] private float separationRadius = 2f;

    [Header("Weights")]
    [SerializeField] private float separationWeight = 2f;
    [SerializeField] private float alignmentWeight = 1f;
    [SerializeField] private float cohesionWeight = 1f;
    [SerializeField] private float targetWeight = 0.6f;
    [SerializeField] private float boundsWeight = 1.5f;

    [Header("Optional Target")]
    [SerializeField] private Transform globalTarget;

    private readonly List<FlockObject> agents = new List<FlockObject>();

    public List<FlockObject> Agents => agents;
    public float MinSpeed => minSpeed;
    public float MaxSpeed => maxSpeed;
    public float MaxForce => maxForce;
    public float NeighborRadius => neighborRadius;
    public float SeparationRadius => separationRadius;
    public float SeparationWeight => separationWeight;
    public float AlignmentWeight => alignmentWeight;
    public float CohesionWeight => cohesionWeight;
    public float TargetWeight => targetWeight;
    public float BoundsWeight => boundsWeight;
    public Transform GlobalTarget => globalTarget;
    public Vector3 BoundsCenter => transform.position;
    public Vector3 BoundsExtents => spawnExtents;

    public Action flockMemberDied;

    private void Start()
    {
        globalTarget = GameManager.instance.player.transform;
        SpawnAgents();
    }

    private void SpawnAgents()
    {
        for (int i = 0; i < UnityEngine.Random.Range(minAgents, maxAgents + 1); i++)
        {
            Vector3 randomOffset = new Vector3(
                UnityEngine.Random.Range(-spawnExtents.x, spawnExtents.x),
                UnityEngine.Random.Range(-spawnExtents.y, spawnExtents.y)
            );

            Vector3 spawnPosition = transform.position + randomOffset;

            FlockObject newAgent = Instantiate(agentPrefab, spawnPosition, Quaternion.identity, transform);
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }
    }

    public void FlockMemberDied(FlockObject victim)
    {
        agents.Remove(victim);

        if (flockMemberDied != null)
            flockMemberDied.Invoke();

        if (agents.Count <= 0)
            Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, spawnExtents * 2f);
    }
}
