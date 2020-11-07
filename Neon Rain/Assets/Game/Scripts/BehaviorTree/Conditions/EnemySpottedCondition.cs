
public class EnemySpottedCondition : BTNode
{
    private BaseAI MyAI;

    public EnemySpottedCondition(BaseAI baseAI)
    {
        MyAI = baseAI;
    }
    
    public override BTNodeStates Evaluate()
    {
        return MyAI.EnemySpotted() ? BTNodeStates.SUCCESS : BTNodeStates.FAILURE;
    }
}
