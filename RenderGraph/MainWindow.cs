using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace RenderGraph
{
    public partial class MainWindow : Form
    {
        public static readonly Random Random;
        public static int ImageWidth = 800;
        public static int ImageHeight = 600;
        public static int NodeWidth = 100;
        public static int NodeHeight = 50;

        private Graph Nodes { get; set; }

        static MainWindow()
        {
            Random = new Random();
        }

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            Nodes = new Graph();

            //TODO: Remove - should be loaded from file
            var root = new Node("Hej jag är en root!");
            Nodes.Add(root);

            var child1 = new Node("Jag är child 1");
            root.Relations.Add(new Relation("Tjo", root, child1));
            Nodes.Add(child1);

            var child2 = new Node("Jag är child 2");
            root.Relations.Add(new Relation("Tjohej", root, child2));
            Nodes.Add(child2);

            var child3 = new Node("Jag är child 3");
            root.Relations.Add(new Relation("Tjo", root, child3));
            Nodes.Add(child3);

            var child4 = new Node("Jag är child 4");
            root.Relations.Add(new Relation("Tjo", root, child4));
            Nodes.Add(child4);

            var cchild1 = new Node("CC1");
            child2.Relations.Add(new Relation("1", child2, cchild1));
            Nodes.Add(cchild1);

            var cchild2 = new Node("CC2");
            child2.Relations.Add(new Relation("2", child2, cchild2));
            Nodes.Add(cchild2);

            var cchild3 = new Node("CC3");
            child2.Relations.Add(new Relation("3", child2, cchild3));
            Nodes.Add(cchild3);

            var ccchild1 = new Node("CCC1");
            child1.Relations.Add(new Relation("x3a", child1, ccchild1));
            Nodes.Add(ccchild1);

            var ccchild2 = new Node("CCC2");
            child1.Relations.Add(new Relation("x3b", child1, ccchild2));
            Nodes.Add(ccchild2);

            var ccchild3 = new Node("CCC3");
            child1.Relations.Add(new Relation("x3c", child1, ccchild3));
            Nodes.Add(ccchild3);

            var ccchild4 = new Node("CCC4");
            child1.Relations.Add(new Relation("x3d", child1, ccchild4));
            Nodes.Add(ccchild4);

            var ccchild5 = new Node("CCC5");
            child1.Relations.Add(new Relation("x3e", child1, ccchild5));
            Nodes.Add(ccchild5);

            Nodes.AddingCompleted();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Refresh();
            // TODO Load
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ScrambleModel();
            Invalidate();
        }

        private void ScrambleModel()
        {
            var currentScore = Nodes.Score;

            Text = $@"Render Graph ({currentScore})";

            if (currentScore <= 0)
            {
                timer1.Enabled = false;
                // TODO Save
                return;
            }

            var generations = new List<Graph>();

            for (var i = 0; i < 50; i++)
                generations.Add(Nodes.CopyNodes());

            foreach (var generation in generations)
                generation.CopyRelations(Nodes);

            foreach (var generation in generations)
                generation.Mutate();

            Console.WriteLine();
            foreach (var generation in generations)
            {
                Console.WriteLine(generation);
            }

            var n = generations.OrderBy(x => x.Score).First();

            Nodes = n;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Aqua);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

#if DEBUG
            e.Graphics.DrawRectangle(Pens.Red, 0, 0, ImageWidth, ImageHeight);
#endif

            foreach (var node in Nodes)
                node.PaintRelations(e.Graphics);

            foreach (var node in Nodes)
                node.PaintNode(e.Graphics, Font);
        }
    }
}