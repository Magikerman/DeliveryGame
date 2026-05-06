using UnityEngine;

public class EnemyTree : MonoBehaviour
{
    private Node rootNode;

    private void Awake()
    {
        ActionNode idle = new ActionNode(action => action.ChangeToIdle());
        ActionNode pursue = new ActionNode(action => action.ChangeToPursue());
        ActionNode range = new ActionNode(action => action.ChangeToRange());
        ActionNode circle = new ActionNode(action => action.ChangeToCircle());

        QuestionNode keepClose = new QuestionNode(context => LineOfSight.InRange(context.self, context.player, context.keepClose)
        , idle
        , pursue
        );

        QuestionNode keepRange = new QuestionNode(context => LineOfSight.InRange(context.self, context.player, context.keepDistance)
        , range
        , keepClose
        );

        rootNode = new QuestionNode(context => 
        LineOfSight.HasLoS(context.self, context.player, context.range, context.angle, context.mask)
        , keepRange
        , circle
        );
    }

    public void Evaluate(EnemyController enemy, Context context)
    {
        rootNode.Evaluate(enemy, context);
    }
}
