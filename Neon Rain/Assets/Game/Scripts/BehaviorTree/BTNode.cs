[System.Serializable]
    public abstract class BTNode
    {
        protected BTNodeStates nodeState;

        public BTNodeStates NodeState
        {
            get { return nodeState; }
        }

        public BTNode()
        {
            
        }

        public abstract BTNodeStates Evaluate();
    }
