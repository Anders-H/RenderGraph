namespace RenderGraph
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.EditContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tryToOrganizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RunningContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.EditContextMenu.SuspendLayout();
            this.RunningContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // EditContextMenu
            // 
            this.EditContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tryToOrganizeToolStripMenuItem,
            this.propertiesToolStripMenuItem,
            this.saveImageToolStripMenuItem});
            this.EditContextMenu.Name = "EditContextMenu";
            this.EditContextMenu.Size = new System.Drawing.Size(160, 70);
            this.EditContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.EditContextMenu_Opening);
            // 
            // tryToOrganizeToolStripMenuItem
            // 
            this.tryToOrganizeToolStripMenuItem.Name = "tryToOrganizeToolStripMenuItem";
            this.tryToOrganizeToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.tryToOrganizeToolStripMenuItem.Text = "Try to organize";
            this.tryToOrganizeToolStripMenuItem.Click += new System.EventHandler(this.tryToOrganizeToolStripMenuItem_Click);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.propertiesToolStripMenuItem.Text = "Properties... (F4)";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // saveImageToolStripMenuItem
            // 
            this.saveImageToolStripMenuItem.Name = "saveImageToolStripMenuItem";
            this.saveImageToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.saveImageToolStripMenuItem.Text = "Save image...";
            this.saveImageToolStripMenuItem.Click += new System.EventHandler(this.saveImageToolStripMenuItem_Click);
            // 
            // RunningContextMenu
            // 
            this.RunningContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stopToolStripMenuItem});
            this.RunningContextMenu.Name = "RunningContextMenu";
            this.RunningContextMenu.Size = new System.Drawing.Size(99, 26);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "lock.png");
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1597, 1061);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainWindow";
            this.Text = "Render Graph";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainWindow_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainWindow_MouseDown);
            this.EditContextMenu.ResumeLayout(false);
            this.RunningContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ContextMenuStrip EditContextMenu;
        private System.Windows.Forms.ContextMenuStrip RunningContextMenu;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tryToOrganizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
    }
}

