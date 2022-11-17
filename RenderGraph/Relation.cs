namespace RenderGraph
{
    public class Relation
    {
        public int Id { get; }
        public Node ParentNode { get; set; }
        public Node TargetNode { get; set; }

        public Relation(int id, Node parentNode, Node targetNode)
        {
            Id = id;
            ParentNode = parentNode;
            TargetNode = targetNode;
        }

        public bool Is(Relation other) =>
            Id == other.Id;
    }
}