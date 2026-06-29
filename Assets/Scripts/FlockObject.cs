using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FlockObject : MonoBehaviour
{
    private FlockingBehaviour manager;
    private Rigidbody rb;

    public void Initialize(FlockingBehaviour flockManager)
    {
        manager = flockManager;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (rb.linearVelocity == Vector3.zero)
        {
            Vector3 randomDir = Random.onUnitSphere;
            rb.linearVelocity = randomDir * Random.Range(manager.MinSpeed, manager.MaxSpeed);
        }
    }

    public Vector3 GetSteeringRot(int targetOverride, int boundsOverride, int alignmentOverride)
    {
        Vector3 targetDistance =  transform.position - manager.GlobalTarget.position;

        Vector3 separation = CalculateSeparation();
        Vector3 alignment = CalculateAlignment() * alignmentOverride;
        Vector3 cohesion = CalculateCohesion();
        Vector3 targetForce = CalculateTargetForce() * targetOverride;
        Vector3 boundsForce = CalculateBoundsForce() * boundsOverride;

        Vector3 steering =
            separation * manager.SeparationWeight +
            alignment * manager.AlignmentWeight +
            cohesion * manager.CohesionWeight +
            targetForce * manager.TargetWeight +
            boundsForce * manager.BoundsWeight;

        Vector3 acceleration = Vector3.ClampMagnitude(steering, 1f);

        return acceleration.normalized;
    }

    private Vector3 CalculateSeparation()
    {
        Vector3 force = Vector3.zero;
        int count = 0;

        for (int i = 0; i < manager.Agents.Count; i++)
        {
            FlockObject other = manager.Agents[i];

            if (other == this)
                continue;

            Vector3 offset = transform.position - other.transform.position;
            float distance = offset.magnitude;

            if (distance > 0f && distance < manager.SeparationRadius)
            {
                force += offset.normalized / distance;
                count++;
            }
        }

        if (count == 0)
            return Vector3.zero;

        force /= count;
        return force.normalized;
    }

    private Vector3 CalculateAlignment()
    {
        Vector3 averageVelocity = Vector3.zero;
        int count = 0;

        for (int i = 0; i < manager.Agents.Count; i++)
        {
            FlockObject other = manager.Agents[i];

            if (other == this)
                continue;

            float distance = Vector3.Distance(transform.position, other.transform.position);

            if (distance < manager.NeighborRadius)
            {
                averageVelocity += other.rb.linearVelocity;
                count++;
            }
        }

        if (count == 0)
            return Vector3.zero;

        averageVelocity /= count;

        if (averageVelocity == Vector3.zero)
            return Vector3.zero;

        return averageVelocity.normalized;
    }

    private Vector3 CalculateCohesion()
    {
        Vector3 center = Vector3.zero;
        int count = 0;

        for (int i = 0; i < manager.Agents.Count; i++)
        {
            FlockObject other = manager.Agents[i];

            if (other == this)
                continue;

            float distance = Vector3.Distance(transform.position, other.transform.position);

            if (distance < manager.NeighborRadius)
            {
                center += other.transform.position;
                count++;
            }
        }

        if (count == 0)
            return Vector3.zero;

        center /= count;

        Vector3 dirToCenter = center - transform.position;

        if (dirToCenter == Vector3.zero)
            return Vector3.zero;

        return dirToCenter.normalized;
    }

    private Vector3 CalculateTargetForce()
    {
        if (manager.GlobalTarget == null)
            return Vector3.zero;

        Vector3 dir = manager.GlobalTarget.position - transform.position;

        if (dir == Vector3.zero)
            return Vector3.zero;

        return dir.normalized;
    }

    private Vector3 CalculateBoundsForce()
    {
        Vector3 center = manager.BoundsCenter;
        Vector3 extents = manager.BoundsExtents;
        Vector3 localOffset = transform.position - center;

        bool outsideX = Mathf.Abs(localOffset.x) > extents.x;
        bool outsideY = Mathf.Abs(localOffset.y) > extents.y;
        bool outsideZ = Mathf.Abs(localOffset.z) > extents.z;

        if (!outsideX && !outsideY && !outsideZ)
            return Vector3.zero;

        Vector3 dirToCenter = center - transform.position;

        if (dirToCenter == Vector3.zero)
            return Vector3.zero;

        return dirToCenter.normalized;
    }

    private void OnDestroy()
    {
        manager.FlockMemberDied(this);
    }
}
