public class InAttackRangeCondition : BTNode
{
    private BaseAI Myai;
    
    public InAttackRangeCondition(BaseAI baseAI)
    {
        Myai = baseAI;
    }
    
    public override BTNodeStates Evaluate()
    {
        return Myai.InAttackRange ? BTNodeStates.SUCCESS : BTNodeStates.FAILURE;
    }
}