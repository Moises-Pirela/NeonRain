using DG.Tweening;

public class FaceMoveDirectionTask : BTNode
{
    private BaseAI Myai;
    
    public FaceMoveDirectionTask(BaseAI baseAI)
    {
        Myai = baseAI;
    }
    
    public override BTNodeStates Evaluate()
    {

        Myai.transform.DOKill();

        Myai.transform.DOLookAt(Myai.MovePosition, 0.1f);
        
        return BTNodeStates.SUCCESS;
    }
}