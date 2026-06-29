using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Node
{
    public abstract void Evaluate(EnemyController enemy, Context context);
}

public class  QuestionNode : Node
{
    private Func<Context, bool> question;

    private Node trueNode;
    private Node falseNode;

    public QuestionNode(Func<Context, bool> context, Node trueNode, Node falseNode)
    {
        this.trueNode = trueNode;
        this.falseNode = falseNode;

        this.question = context;
    }

    public override void Evaluate(EnemyController enemy, Context context)
    {
        if (question(context))
        {
            trueNode.Evaluate(enemy, context);
        }
        else
        {
            falseNode.Evaluate(enemy, context);
        }
    }
}

public class ActionNode : Node
{
    public Action<EnemyController> action;

    public ActionNode(Action<EnemyController> action)
    {
        this.action = action;
    }

    public override void Evaluate(EnemyController enemy, Context context)
    {
        action(enemy);
    }
}

public class WeightedActionNode : Node
{
    public (float weight, Action<EnemyController> action)[] actions;

    public WeightedActionNode((float weight, Action<EnemyController> action)[] actions)
    {
        this.actions = actions;
    }

    public override void Evaluate(EnemyController enemy, Context context)
    {
        float totalWeight = 0f;

        foreach (var option in actions)
        {
            totalWeight += option.weight;
        }

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        foreach (var options in actions)
        {
            currentWeight += options.weight;

            if (randomValue <= currentWeight)
            {
                options.action(enemy);
                return;
            }
        }
    }
}
