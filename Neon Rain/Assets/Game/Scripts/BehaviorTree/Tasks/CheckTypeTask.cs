public class CheckTypeTask : BTNode
{
    private IBehaviorAI myAI;

    public CheckTypeTask(IBehaviorAI _ai)
    {
        myAI = _ai;
    }
    public override BTNodeStates Evaluate()
    {
        throw new System.NotImplementedException();
    }
}