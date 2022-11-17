using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RenderGraph
{
    public class Node
    {
        public Rectangle Location { get; set; }
        public string Name { get; set; }
        public List<Relation> Relations { get; }

        public Node(string name)
        {
            Location = GetRandomLocation();
            Relations = new List<Relation>();
            Name = name;
        }

        public Node CopyNode() =>
            new Node(Name)
            {
                Location = Location
            };

        public void CopyRelations(Graph source, Graph target)
        {
            var sourceNode = source.GetNodeByName(Name);

            foreach (var relation in sourceNode.Relations)
            {
                var relatesTo = target.GetNodeByName(relation.TargetNode.Name);

                var r = new Relation(relation.Name, this, relatesTo);

                Relations.Add(r);
            }
        }

        private static Rectangle GetRandomLocation() =>
            new Rectangle(MainWindow.Random.Next(MainWindow.ImageWidth - MainWindow.NodeWidth), MainWindow.Random.Next(MainWindow.ImageHeight - MainWindow.NodeHeight), MainWindow.NodeWidth, MainWindow.NodeHeight);

        public void ToRandomLocation() =>
            Location = GetRandomLocation();

        private Point GetCenter() =>
            new Point(Location.X + Location.Width / 2, Location.Y + Location.Height / 2);

        public bool LinesIntersect(Node other, Node target1, Node target2)
        {
            var pA1 = GetCenter();
            var pA2 = other.GetCenter();

            var pB1 = target1.GetCenter();
            var pB2 = target2.GetCenter();

            return DoLinesIntersect(pA1, pA2, pB1, pB2);
        }

        public bool IsIntersectedByLine(Node start, Node end)
        {
            var startPoint = start.GetCenter();
            var endPoint = end.GetCenter();

            var lA1 = new Point(Location.X, Location.Y);
            var lA2 = new Point(Location.X + Location.Width, Location.Y);

            var lB1 = new Point(Location.X + Location.Width, Location.Y);
            var lB2 = new Point(Location.X + Location.Width, Location.Y + Location.Height);

            var lC1 = new Point(Location.X + Location.Width, Location.Y + Location.Height);
            var lC2 = new Point(Location.X, Location.Y + Location.Height);

            var lD1 = new Point(Location.X, Location.Y);
            var lD2 = new Point(Location.X, Location.Y + Location.Height);

            return DoLinesIntersect(startPoint, endPoint, lA1, lA2)
                || DoLinesIntersect(startPoint, endPoint, lB1, lB2)
                || DoLinesIntersect(startPoint, endPoint, lC1, lC2)
                || DoLinesIntersect(startPoint, endPoint, lD1, lD2);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public static bool DoLinesIntersect(Point pA1, Point pA2, Point pB1, Point pB2)
        {
            var o1 = GetOrientation(pA1, pA2, pB1);
            var o2 = GetOrientation(pA1, pA2, pB2);
            var o3 = GetOrientation(pB1, pB2, pA1);
            var o4 = GetOrientation(pB1, pB2, pA2);

            if (o1 != o2 && o3 != o4)
                return true;

            if (o1 == 0 && IsOnSegment(pA1, pB1, pA2))
                return true;

            if (o2 == 0 && IsOnSegment(pA1, pB2, pA2))
                return true;

            if (o3 == 0 && IsOnSegment(pB1, pA1, pB2))
                return true;

            if (o4 == 0 && IsOnSegment(pB1, pA2, pB2))
                return true;

            return false;
        }

        private static byte GetOrientation(Point p, Point q, Point r)
        {
            var v = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);

            return v == 0 ? (byte)0 : v > 0 ? (byte)1 : (byte)2;
        }

        private static bool IsOnSegment(Point p, Point q, Point r) =>
            q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) && q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y);

        public void PaintRelations(Graphics g)
        {
            foreach (var targetPosition in from relation in Relations where relation.TargetNode != null select relation.TargetNode.GetCenter())
                g.DrawLine(Pens.Black, GetCenter(), targetPosition);
        }

        public void PaintNode(Graphics g, Font font)
        {
            g.FillRectangle(Brushes.Bisque, Location);
            g.DrawString(Name, font, Brushes.Black, Location.X + 2, Location.Y + 2);
            g.DrawRectangle(Pens.Black, Location);
        }
    }
}