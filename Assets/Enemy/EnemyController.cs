using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected enum state { idle, pursue, range, circle, attack, wander, pathfinding}

    [SerializeField] protected state enemyState;

    protected bool inRange;

    protected Context context;
    protected EnemyTree tree;
    protected Rigidbody rb;

    protected Vector3 rotation = new Vector3(1,0,0);

    void Awake()
    {
        context = GetComponent<Context>();
        tree = GetComponent<EnemyTree>();
        rb = GetComponent<Rigidbody>();
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

    public void ChangeToWander()
    {
        enemyState = state.wander;
    }

    public void ChangeToAttack()
    {
        enemyState = state.attack;
    }

    public void ChangeToPathFinding()
    {
        enemyState = state.pathfinding;
    }
}
