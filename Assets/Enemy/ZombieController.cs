using UnityEngine;

public class ZombieController : EnemyController
{
    [SerializeField] private float rotSpeed;
    [SerializeField] private float speed;
    [SerializeField, Range(0, 1)] private float circleRadius;
    [SerializeField] private float maxPredictionTime;

    [SerializeField] private float maxSpeed;

    private float timer;

    [SerializeField] private float timeToAttack;
    private FlockObject flock;

    [SerializeField, Range(0, 1)] private float wanderMult;
    [SerializeField] private float wanderAngle;

    void Awake()
    {
        context = GetComponent<Context>();
        tree = GetComponent<EnemyTree>();
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = maxSpeed;
        flock = GetComponent<FlockObject>();
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
                //rotation = SteeringBehaviour.GetRotation(transform, context.player, context.playerRb, maxPredictionTime);
                rotation = flock.GetSteeringRot(1, 0, 1);
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
            case state.wander:
                //rotation = SteeringBehaviour.Wander(rotation, 5f);
                rotation = Vector3.Lerp(flock.GetSteeringRot(0, 1, 0), SteeringBehaviour.Wander(rotation, wanderAngle), wanderMult);
                rotation = rotation.normalized;
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
}
