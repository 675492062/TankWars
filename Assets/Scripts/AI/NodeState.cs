namespace GameClient.AI
{
    public enum NodeState
    {
        // The node has not been visited yet.
        Untested,
        
        /// The node is a possible path.
        Open,

        /// The node has visited once and will not be visited again.
        Closed
    }
}
