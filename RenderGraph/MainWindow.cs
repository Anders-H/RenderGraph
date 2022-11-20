using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace RenderGraph
{
    public partial class MainWindow : Form
    {
        private int _mouseX;
        private int _mouseY;
        private readonly Pen _pen = new Pen(Color.Black, 2f);
        public static int ChangeAmt;
        public static int SiblingsPerGeneration;
        public static int GenerationsOfUnimprovementBeforeScramble;
        public static int Generations;
        public static int GenerationsOfUnimprovement;
        public static int LastScore;
        public static readonly Random Random;
        public static int ImageWidth = 800;
        public static int ImageHeight = 600;
        public static int NodeWidth = 100;
        public static int NodeHeight = 50;

        private Graph Graph { get; set; }

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
            Graph = new Graph();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Refresh();

            var filename = "";

            using (var x = new OpenFileDialog())
            {
                x.Title = @"Select XML-file to load";
                x.Filter = @"XML (*.xml)|*.xml";
                if (x.ShowDialog(this) != DialogResult.OK)
                {
                    Close();
                    return;
                }

                filename = x.FileName;
            }

            Cursor = Cursors.WaitCursor;

            try
            {
                var xml = File.ReadAllText(filename, Encoding.UTF8);

                var dom = new XmlDocument();
                dom.LoadXml(xml);
                var doc = dom.DocumentElement;

                ImageWidth = int.Parse(doc.SelectSingleNode("Properties/PlayfieldWidth").InnerText);
                ImageHeight = int.Parse(doc.SelectSingleNode("Properties/PlayfieldHeight").InnerText);
                NodeWidth = int.Parse(doc.SelectSingleNode("Properties/NodeWidth").InnerText);
                NodeHeight = int.Parse(doc.SelectSingleNode("Properties/NodeHeight").InnerText);

                foreach (XmlElement n in doc.SelectNodes("Nodes/Node"))
                {
                    var node = new Node(n.SelectSingleNode("ID").InnerText, n.SelectSingleNode("Text").InnerText);
                    node.NodeType = n.SelectSingleNode("Type").InnerText;
                    Graph.Add(node);
                }

                var relationId = 0;

                foreach (XmlElement n in doc.SelectNodes("Nodes/Node"))
                {
                    var node = Graph.GetNodeById(n.SelectSingleNode("ID").InnerText);

                    foreach (XmlElement r in n.SelectNodes("RelatesTo/Relation"))
                    {
                        var target = Graph.GetNodeById(r.SelectSingleNode("ID").InnerText);
                        var relation = new Relation(relationId++, node, target);
                        node.Relations.Add(relation);
                    }
                }

                Cursor = Cursors.Default;
            }
            catch (SystemException ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(this, ex.Message, @"Load failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Graph.AddingCompleted();
            ContextMenuStrip = EditContextMenu;
            Refresh();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ScrambleModel();
            Invalidate();
        }

        private void ScrambleModel()
        {
            Generations++;

            var currentScore = Graph.Score;

            if (currentScore >= LastScore)
                GenerationsOfUnimprovement++;
            else
                GenerationsOfUnimprovement = 0;

            LastScore = currentScore;

            if (Generations%3 == 0 && SiblingsPerGeneration < 1500)
                SiblingsPerGeneration++;

            if (Generations%45 == 0 && ChangeAmt < 20)
                ChangeAmt++;

            if (Generations%35 == 0 && GenerationsOfUnimprovementBeforeScramble > 40)
                GenerationsOfUnimprovementBeforeScramble--;

            Text = $@"Render Graph [Gen: {Generations}, Score: {currentScore}, Generations without improvement: {GenerationsOfUnimprovement} ({GenerationsOfUnimprovementBeforeScramble}), Siblings per generation: {SiblingsPerGeneration}, Change amount: {ChangeAmt}]";

            if (currentScore <= 0)
            {
                timer1.Enabled = false;
                Refresh();
                return;
            }

            var generations = new List<Graph>();

            for (var i = 0; i < SiblingsPerGeneration; i++)
                generations.Add(Graph.CopyNodes());

            foreach (var generation in generations)
                generation.CopyRelations(Graph);

            var scrambeled = false;

            if (GenerationsOfUnimprovement >= GenerationsOfUnimprovementBeforeScramble)
            {
                foreach (var generation in generations)
                    generation.Scramble();

                scrambeled = true;
            }
            else
            {
                foreach (var generation in generations)
                    generation.Mutate();
            }

            var n = generations.OrderBy(x => x.Score).First();

            if (scrambeled || n.Score < Graph.Score)
                Graph = n;
        }

        private void SaveSvg(string filename)
        {
            using (var sw = new StreamWriter(filename, false, Encoding.UTF8))
            {
                sw.WriteLine($@"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 {ImageWidth} {ImageHeight}"">");

                foreach (var node in Graph)
                {
                    foreach (var targetPosition in from relation in node.Relations where relation.TargetNode != null select relation.TargetNode.GetCenter())
                    {
                        var p = node.GetCenter();
                        sw.WriteLine($@"<line x1=""{p.X}"" y1=""{p.Y}"" x2=""{targetPosition.X}"" y2=""{targetPosition.Y}""  style=""stroke:#000000;stroke-width:2""/>");
                    }
                }

                foreach (var node in Graph)
                {
                    var background = "#000000";

                    if (node.NodeType == "A")
                        background = "#000077";
                    else if (node.NodeType == "B")
                        background = "#660066";

                    sw.WriteLine($@"<rect x=""{node.Location.X}"" y=""{node.Location.Y}"" width=""{node.Location.Width}"" height=""{node.Location.Height}"" stroke=""#000000"" stroke-width=""2px"" fill=""{background}""/>
<svg x=""{node.Location.X}"" y=""{node.Location.Y}"" width=""{node.Location.Width}"" height=""{node.Location.Height}"">
<text x=""50%"" y=""50%"" dominant-baseline=""middle"" text-anchor=""middle"" fill=""#ffffff"">{node.Text}</text>
</svg>");
                }

                sw.WriteLine(@"</svg>");
                sw.Flush();
                sw.Close();
            }
        }

        private void SavePng(string filename)
        {
            using (var b = new Bitmap(ImageWidth, ImageHeight))
            {
                var g = Graphics.FromImage(b);
                g.Clear(Color.DarkGreen);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                foreach (var node in Graph)
                    node.PaintRelations(g, _pen);

                foreach (var node in Graph)
                    node.PaintNode(g, Font, _pen, false);

                b.Save(filename, ImageFormat.Png);
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.DarkGreen);
            e.Graphics.SmoothingMode = SmoothingMode.None;
            e.Graphics.DrawRectangle(Pens.DarkOliveGreen, 0, 0, ImageWidth, ImageHeight);

            foreach (var node in Graph)
                node.PaintRelations(e.Graphics, _pen);

            foreach (var node in Graph)
                node.PaintNode(e.Graphics, Font, _pen, true);

            foreach (var node in Graph)
            {
                if (node.Selected)
                {
                    node.PaintSelection(e.Graphics);
                    break;
                }
            }

            if (timer1.Enabled)
                e.Graphics.DrawString("Running...", Font, Brushes.Red, 10, 10);
        }

        private void tryToOrganizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeAmt = 2;
            SiblingsPerGeneration = 5;
            GenerationsOfUnimprovementBeforeScramble = 100;
            Generations = 0;
            GenerationsOfUnimprovement = 0;
            LastScore = int.MaxValue;
            ContextMenuStrip = RunningContextMenu;
            timer1.Enabled = true;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            Text = @"Render Graph";
            ContextMenuStrip = EditContextMenu;
            Refresh();
        }

        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var x = new SaveFileDialog())
            {
                x.Title = @"Save image";
                x.Filter = @"PNG (*.png)|*.png|SVG (*.svg)|*.svg";

                if (x.ShowDialog(this) != DialogResult.OK)
                    return;

                if (x.FileName.EndsWith(".svg", StringComparison.CurrentCultureIgnoreCase))
                {
                    SaveSvg(x.FileName);
                }
                else
                {
                    SavePng(x.FileName);
                }
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var s = Graph.GetSelectedNode();

            if (s == null)
                return;

            using (var x = new NodeProperties())
            {
                x.ShowDialog(this);
            }
        }

        private void EditContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void MainWindow_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseX = e.X;
            _mouseY = e.Y;
            Graph.Deselect();

            var n = Graph.GetAt(_mouseX, _mouseY);

            if (n == null)
            {
                propertiesToolStripMenuItem.Enabled = false;
            }
            else
            {
                propertiesToolStripMenuItem.Enabled = true;
                n.Selected = true;
            }
            
            Refresh();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
                propertiesToolStripMenuItem_Click(sender, EventArgs.Empty);
        }
    }
}