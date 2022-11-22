using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using RenderGraph;

namespace Tests
{
    [TestClass]
    public class IntersectionTests
    {
        [TestMethod]
        public void CanCheckIntersect()
        {
            var line1Point1 = new Point(10, 10);
            var line1Point2 = new Point(100, 100);
            var line2Point1 = new Point(100, 10);
            var line2Point2 = new Point(10, 100);
            Assert.IsTrue(Node.DoLinesIntersect(line1Point1, line1Point2, line2Point1, line2Point2));
        }

        [TestMethod]
        public void CanCheckNoIntersection()
        {
            var line1Point1 = new Point(10, 10);
            var line1Point2 = new Point(100, 10);
            var line2Point1 = new Point(10, 100);
            var line2Point2 = new Point(100, 100);
            Assert.IsFalse(Node.DoLinesIntersect(line1Point1, line1Point2, line2Point1, line2Point2));
        }

        [TestMethod]
        public void LinesIntersect()
        {
            var n1 = new Node("A", "A", null);
            n1.Location = new Rectangle(10, 10, 50, 50);

            var n2 = new Node("B", "B", null);
            n2.Location = new Rectangle(1000, 1000, 50, 50);

            var n3 = new Node("C", "C", null);
            n3.Location = new Rectangle(1000, 10, 50, 50);

            var n4 = new Node("D", "D", null);
            n4.Location = new Rectangle(10, 1000, 50, 50);

            Assert.IsTrue(n1.LinesIntersect(n2, n3, n4));

            n1.Location = new Rectangle(10, 10, 50, 50);
            n2.Location = new Rectangle(10, 1000, 50, 50);
            n3.Location = new Rectangle(1000, 10, 50, 50);
            n4.Location = new Rectangle(1000, 1000, 50, 50);

            Assert.IsFalse(n1.LinesIntersect(n2, n3, n4));
        }

        [TestMethod]
        public void IsIntersectedByLine()
        {
            var n = new Node("A", "A", null);
            n.Location = new Rectangle(100, 100, 50, 50);

            var left = new Node("B", "B", null);
            left.Location = new Rectangle(10, 10, 50, 50);

            var right = new Node("C", "C", null);
            right.Location = new Rectangle(200, 200, 50, 50);

            Assert.IsTrue(n.IsIntersectedByLine(left, right));

            right.Location = new Rectangle(10, 200, 50, 50);

            Assert.IsFalse(n.IsIntersectedByLine(left, right));
        }
    }
}