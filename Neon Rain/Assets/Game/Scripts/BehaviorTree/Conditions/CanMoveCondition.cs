public class CanMoveCondition : BTNode
{
    private BaseAI MyAI;
    
    public CanMoveCondition(BaseAI baseAI)
    {
        MyAI = baseAI;
    }

    public override BTNodeStates Evaluate()
    {
        return MyAI.CanMove() ? BTNodeStates.SUCCESS : BTNodeStates.FAILURE;
    }
}