public class AttackTask : BTNode
{
    private BaseAI Myai;
    
    public AttackTask(BaseAI baseAI)
    {
        Myai = baseAI;
    }
    
    public override BTNodeStates Evaluate()
    {
        Myai.Attack();

        return BTNodeStates.SUCCESS;
    }
}