namespace RenderGraph
{
    public class Relation
    {
        public int Id { get; }
        public string Name { get; set; }
        public Node ParentNode { get; set; }
        public Node TargetNode { get; set; }

        public Relation(int id, string name, Node parentNode, Node targetNode)
        {
            Id = id;
            Name = name;
            ParentNode = parentNode;
            TargetNode = targetNode;
        }

        public bool Is(Relation other) =>
            Id == other.Id;
    }
}