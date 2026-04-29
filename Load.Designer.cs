namespace MCMapCare
{
    partial class Load
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Load));
            menuStrip1 = new MenuStrip();
            openToolStripMenuItem = new ToolStripMenuItem();
            browseToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            recentsWorldsToolStripMenuItem = new ToolStripMenuItem();
            WorldLists = new ListBox();
            btnOpenSelectedWorld = new Button();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(448, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { browseToolStripMenuItem, toolStripSeparator1, recentsWorldsToolStripMenuItem });
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(48, 20);
            openToolStripMenuItem.Text = "Open";
            // 
            // browseToolStripMenuItem
            // 
            browseToolStripMenuItem.Name = "browseToolStripMenuItem";
            browseToolStripMenuItem.Size = new Size(166, 22);
            browseToolStripMenuItem.Text = "Browse";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(163, 6);
            // 
            // recentsWorldsToolStripMenuItem
            // 
            recentsWorldsToolStripMenuItem.Name = "recentsWorldsToolStripMenuItem";
            recentsWorldsToolStripMenuItem.Size = new Size(166, 22);
            recentsWorldsToolStripMenuItem.Text = "Recents Worlds >";
            // 
            // WorldLists
            // 
            WorldLists.FormattingEnabled = true;
            WorldLists.Location = new Point(12, 37);
            WorldLists.Name = "WorldLists";
            WorldLists.Size = new Size(424, 184);
            WorldLists.TabIndex = 1;
            // 
            // btnOpenSelectedWorld
            // 
            btnOpenSelectedWorld.Location = new Point(12, 227);
            btnOpenSelectedWorld.Name = "btnOpenSelectedWorld";
            btnOpenSelectedWorld.Size = new Size(424, 92);
            btnOpenSelectedWorld.TabIndex = 2;
            btnOpenSelectedWorld.Text = "btnOpenSelectedWorld";
            btnOpenSelectedWorld.UseVisualStyleBackColor = true;
            btnOpenSelectedWorld.Click += btnOpenSelectedWorld_Click;
            // 
            // Load
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(448, 331);
            Controls.Add(btnOpenSelectedWorld);
            Controls.Add(WorldLists);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "Load";
            Text = "FormWorldLoad";
            Load += Load_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem browseToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem recentsWorldsToolStripMenuItem;
        private ListBox WorldLists;
        private Button btnOpenSelectedWorld;
    }
}
