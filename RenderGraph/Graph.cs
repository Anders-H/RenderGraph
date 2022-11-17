using System.Collections.Generic;
using System.Linq;

namespace RenderGraph
{
    public class Graph : List<Node>
    {
        public int Score { get; private set; }

        public Graph CopyNodes()
        {
            var result = new Graph();
            
            result.AddRange(this.Select(n => n.CopyNode()));

            return result;
        }

        public void CopyRelations(Graph source)
        {
            foreach (var n in this)
                n.CopyRelations(source, this);
        }

        public Node GetNodeById(string id) =>
            this.First(n => n.Id == id);

        public void AddingCompleted() =>
            CalculateScore();

        private void CalculateScore()
        {
            var score = 0;

            foreach (var node1 in this)
            {
                foreach (var node2 in this)
                {
                    if (node2 == node1)
                        continue;

                    if (node1.Location.IntersectsWith(node2.Location))
                        score += 10;
                }
            }

            var relations = GetAllRelations();

            foreach (var r1 in relations)
            {
                foreach (var r2 in relations)
                {
                    if (r1.Is(r2))
                        continue;

                    var firstRelationStart = r1.ParentNode;
                    var firstRelationEnd = r1.TargetNode;
                    var lastRelationStart = r2.ParentNode;
                    var lastRelationEnd = r2.TargetNode;

                    if (IsSame(firstRelationStart, firstRelationEnd, lastRelationStart, lastRelationEnd))
                        continue;

                    if (firstRelationStart.LinesIntersect(firstRelationEnd, lastRelationStart, lastRelationEnd))
                        score += 5;
                }
            }

            score += relations.Sum(r => (
                from n in this
                where n != r.ParentNode && n != r.TargetNode
                where n.IsIntersectedByLine(r.ParentNode, r.TargetNode)
                select 4
            ).Sum());

            Score = score;
        }

        private static bool IsSame(Node a, Node b, Node c, Node d) =>
            a == b || a == c || a == d || b == c || b == d;

        private List<Relation> GetAllRelations() =>
            this.SelectMany(x => x.Relations).ToList();

        public void Mutate()
        {
            var changes = MainWindow.Random.Next(5) + 1;

            for (var i = 0; i < changes; i++)
            {
                var index = MainWindow.Random.Next(Count);
                var n = this[index];
                n.ToRandomLocation();
            }

            CalculateScore();
        }

        public override string ToString() =>
            $@"Count: {Count}, Score: {Score}";
    }
}