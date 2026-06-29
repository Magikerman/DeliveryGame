using UnityEngine;

public class ZombieTree : EnemyTree
{
    private void Awake()
    {
        ActionNode pursue = new ActionNode(action => action.ChangeToPursue());
        ActionNode wander = new ActionNode(action => action.ChangeToWander());
        ActionNode attack = new ActionNode(action => action.ChangeToAttack());

        QuestionNode attackRange = new QuestionNode(context => LineOfSight.InRange(context.self, context.player, context.keepClose)
         , attack
         , pursue
         );


        rootNode = new QuestionNode(context =>
        LineOfSight.HasLoS(context.self, context.player, context.range, context.angle, context.mask)
        , attackRange
        , wander
        );
    }
}
