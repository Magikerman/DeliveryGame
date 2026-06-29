using UnityEngine;

public class EnemyTree : MonoBehaviour
{
    protected Node rootNode;

    public void Evaluate(EnemyController enemy, Context context)
    {
        rootNode.Evaluate(enemy, context);
    }
}
