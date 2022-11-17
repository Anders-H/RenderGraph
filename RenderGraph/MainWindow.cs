﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

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
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Refresh();

            var filename = "";

            using (var x = new OpenFileDialog())
            {
                x.Title = @"Select XML-file to load";

                if (x.ShowDialog(this) != DialogResult.OK)
                {
                    Close();
                    return;
                }

                filename = x.FileName;
            }

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
                Nodes.Add(node);
            }

            foreach (XmlElement n in doc.SelectNodes("Nodes/Node"))
            {
                var node = Nodes.GetNodeById(n.SelectSingleNode("ID").InnerText);

                foreach (XmlElement r in n.SelectNodes("RelatesTo/Relation"))
                {
                    var target = Nodes.GetNodeById(r.SelectSingleNode("ID").InnerText);
                    var relation = new Relation(r.SelectSingleNode("Text").InnerText, node, target);
                    node.Relations.Add(relation);
                }
            }

            Nodes.AddingCompleted();
            timer1.Enabled = true;
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

            var n = generations.OrderBy(x => x.Score).First();

            Nodes = n;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.DarkGreen);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

#if DEBUG
            e.Graphics.DrawRectangle(Pens.DarkOliveGreen, 0, 0, ImageWidth, ImageHeight);
#endif

            foreach (var node in Nodes)
                node.PaintRelations(e.Graphics);

            foreach (var node in Nodes)
                node.PaintNode(e.Graphics, Font);
        }
    }
}