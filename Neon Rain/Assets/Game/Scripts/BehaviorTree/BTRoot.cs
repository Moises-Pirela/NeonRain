    using System.Collections.Generic;

    public class BTRoot: BTNode
    {
        protected List<BTNode> myNodes = new List<BTNode>();

        public BTRoot(List<BTNode> nodes)
        {
            myNodes = nodes;
        }
        
        public override BTNodeStates Evaluate()
        {
            foreach (var child in myNodes)
            {
                switch (child.Evaluate())
                {
                    case BTNodeStates.FAILURE:
                        continue;
                    case BTNodeStates.SUCCESS:
                        nodeState = BTNodeStates.SUCCESS;
                        return nodeState;
                    default:
                        continue;
                }
            }

            nodeState = BTNodeStates.FAILURE;
            return nodeState;
        }
    }