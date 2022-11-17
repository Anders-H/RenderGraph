namespace RenderGraph
{
    public class Relation
    {
        public string Name { get; set; }
        public Node ParentNode { get; set; }
        public Node TargetNode { get; set; }

        public Relation(string name, Node parentNode, Node targetNode)
        {
            Name = name;
            ParentNode = parentNode;
            TargetNode = targetNode;
        }

        public bool Is(Relation other) =>
            Name == other.Name;
    }
}