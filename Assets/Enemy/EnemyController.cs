using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private enum state { idle, pursue, range, circle}

    [SerializeField] private state enemyState;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float speed;
    [SerializeField, Range(0, 1)] private float circleRadius;
    [SerializeField] private float maxPredictionTime;

    [SerializeField] private Color color;

    private bool inRange;

    private Context context;
    private EnemyTree tree;
    private Rigidbody rb;
    [SerializeField] private Renderer carColor;

    [Header("Cannon")]
    [SerializeField] private GameObject cannon;
    [SerializeField] private float cannonVertAngle;
    [SerializeField] private float bulletForce;
    [SerializeField] private Transform bulletPos;
    [SerializeField] private float maxShootTime;
    private float timeToShoot;

    private Vector3 rotation = new Vector3(1,0,0);

    void Awake()
    {
        
        context = GetComponent<Context>();
        tree = GetComponent<EnemyTree>();
        rb = GetComponent<Rigidbody>();

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        carColor.GetPropertyBlock(mpb, 0);
        mpb.SetColor("_Color", color);
        carColor.SetPropertyBlock(mpb, 0);

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
        }
    }

    private void FixedUpdate()
    {
        rotation.y = transform.forward.y;
        Vector3 dir = transform.forward;
        dir.y = 0;

        if (enemyState != state.idle)
        {
            transform.forward = Vector3.Lerp(transform.forward, rotation, rotSpeed * Time.fixedDeltaTime);
            rb.AddForce(dir * speed * Time.fixedDeltaTime, ForceMode.Impulse);
        }

        rotation.y = cannonVertAngle;
        switch (enemyState)
        {
            case state.pursue:
                cannon.transform.forward = rotation;
                break;
            case state.range:
                cannon.transform.forward = new Vector3(-rotation.x, rotation.y, -rotation.z);
                break;
            case state.circle:
                cannon.transform.forward = new Vector3(transform.forward.x, cannonVertAngle, transform.forward.z);
                break;
            case state.idle:
                cannon.transform.forward = rotation;
                break;
        }

        timeToShoot -= Time.fixedDeltaTime;
        if (inRange && timeToShoot <= 0)
        {
            GameObject bulletObject = EnemyBulletPool.instance.GetBullet();
            bulletObject.SetActive(true);
            Rigidbody bullet = bulletObject.GetComponent<Rigidbody>();
            bulletObject.transform.position = bulletPos.position;

            ThrowData data = SteeringBehaviour.GetPredictedPositionThrowData(context.player.position, bulletPos.position, context.playerRb);
            bullet.linearVelocity = data.throwVelocity;
            timeToShoot = maxShootTime;
        }
    }

    public void ChangeToIdle()
    {
        enemyState = state.idle;
    }

    public void ChangeToPursue()
    {
        enemyState = state.pursue;
    }

    public void ChangeToRange()
    {
        enemyState = state.range;
    }

    public void ChangeToCircle()
    {
        enemyState = state.circle;
    }
}
