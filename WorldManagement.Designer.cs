namespace MCMapCare
{
    partial class WorldManagement
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorldManagement));
            labelWorldTitle = new Label();
            tabControlMain = new TabControl();
            tabPageInfo = new TabPage();
            panelInfoScroll = new Panel();
            tableLayoutInfo = new TableLayoutPanel();
            tabPageMerge = new TabPage();
            labelStatus = new Label();
            progressBarMerge = new ProgressBar();
            btnStartMerge = new Button();
            groupBoxDimensions = new GroupBox();
            checkBoxOverworld = new CheckBox();
            checkBoxNether = new CheckBox();
            checkBoxEnd = new CheckBox();
            groupBoxWorldList = new GroupBox();
            dataGridViewWorlds = new DataGridView();
            colWorldName = new DataGridViewTextBoxColumn();
            colWorldDirection = new DataGridViewComboBoxColumn();
            colWorldArchitecture = new DataGridViewTextBoxColumn();
            colRegionOverworld = new DataGridViewTextBoxColumn();
            colRegionNether = new DataGridViewTextBoxColumn();
            colRegionEnd = new DataGridViewTextBoxColumn();
            colRegionOther = new DataGridViewTextBoxColumn();
            panelWorldListButtons = new Panel();
            btnAddFromSaves = new Button();
            btnAddCustom = new Button();
            btnRemoveWorld = new Button();
            labelKeyWorldName = new Label();
            labelValWorldName = new Label();
            labelKeySeed = new Label();
            labelValSeed = new Label();
            labelKeyGameMode = new Label();
            labelValGameMode = new Label();
            labelKeyDifficulty = new Label();
            labelValDifficulty = new Label();
            labelKeyVersion = new Label();
            labelValVersion = new Label();
            labelKeyHardcore = new Label();
            labelValHardcore = new Label();
            labelKeySpawn = new Label();
            labelValSpawn = new Label();
            labelKeyCheats = new Label();
            labelValCheats = new Label();
            labelKeyGameTime = new Label();
            labelValGameTime = new Label();
            labelKeyDataVersion = new Label();
            labelValDataVersion = new Label();
            tabControlMain.SuspendLayout();
            tabPageInfo.SuspendLayout();
            panelInfoScroll.SuspendLayout();
            tabPageMerge.SuspendLayout();
            groupBoxDimensions.SuspendLayout();
            groupBoxWorldList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewWorlds).BeginInit();
            panelWorldListButtons.SuspendLayout();
            SuspendLayout();
            // 
            // labelWorldTitle
            // 
            labelWorldTitle.Dock = DockStyle.Top;
            labelWorldTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            labelWorldTitle.Location = new Point(0, 0);
            labelWorldTitle.Name = "labelWorldTitle";
            labelWorldTitle.Size = new Size(992, 52);
            labelWorldTitle.TabIndex = 1;
            labelWorldTitle.Text = "Chargement...";
            labelWorldTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tabControlMain
            // 
            tabControlMain.Controls.Add(tabPageInfo);
            tabControlMain.Controls.Add(tabPageMerge);
            tabControlMain.Dock = DockStyle.Fill;
            tabControlMain.Location = new Point(0, 52);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.SelectedIndex = 0;
            tabControlMain.Size = new Size(992, 608);
            tabControlMain.TabIndex = 0;
            // 
            // tabPageInfo
            // 
            tabPageInfo.Controls.Add(panelInfoScroll);
            tabPageInfo.Location = new Point(4, 24);
            tabPageInfo.Name = "tabPageInfo";
            tabPageInfo.Padding = new Padding(6);
            tabPageInfo.Size = new Size(984, 580);
            tabPageInfo.TabIndex = 0;
            tabPageInfo.Text = "Infos";
            // 
            // panelInfoScroll
            // 
            panelInfoScroll.AutoScroll = true;
            panelInfoScroll.Controls.Add(tableLayoutInfo);
            panelInfoScroll.Dock = DockStyle.Fill;
            panelInfoScroll.Location = new Point(6, 6);
            panelInfoScroll.Name = "panelInfoScroll";
            panelInfoScroll.Size = new Size(972, 568);
            panelInfoScroll.TabIndex = 0;
            // 
            // tableLayoutInfo
            // 
            tableLayoutInfo.AutoSize = true;
            tableLayoutInfo.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutInfo.ColumnCount = 2;
            tableLayoutInfo.ColumnStyles.Add(new ColumnStyle());
            tableLayoutInfo.ColumnStyles.Add(new ColumnStyle());
            tableLayoutInfo.Dock = DockStyle.Top;
            tableLayoutInfo.Location = new Point(0, 0);
            tableLayoutInfo.Name = "tableLayoutInfo";
            tableLayoutInfo.Padding = new Padding(8);
            tableLayoutInfo.RowCount = 10;
            tableLayoutInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tableLayoutInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tableLayoutInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tableLayoutInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tableLayoutInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tableLayoutInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tableLayoutInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tableLayoutInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tableLayoutInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tableLayoutInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tableLayoutInfo.Size = new Size(972, 396);
            tableLayoutInfo.TabIndex = 0;
            // 
            // tabPageMerge
            // 
            tabPageMerge.Controls.Add(labelStatus);
            tabPageMerge.Controls.Add(progressBarMerge);
            tabPageMerge.Controls.Add(btnStartMerge);
            tabPageMerge.Controls.Add(groupBoxDimensions);
            tabPageMerge.Controls.Add(groupBoxWorldList);
            tabPageMerge.Location = new Point(4, 24);
            tabPageMerge.Name = "tabPageMerge";
            tabPageMerge.Padding = new Padding(8);
            tabPageMerge.Size = new Size(984, 580);
            tabPageMerge.TabIndex = 1;
            tabPageMerge.Text = "Fusionner";
            // 
            // labelStatus
            // 
            labelStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            labelStatus.Font = new Font("Segoe UI", 9F);
            labelStatus.ForeColor = Color.DimGray;
            labelStatus.Location = new Point(8, 438);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(960, 22);
            labelStatus.TabIndex = 0;
            labelStatus.Text = "Pret.";
            // 
            // progressBarMerge
            // 
            progressBarMerge.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBarMerge.Location = new Point(8, 407);
            progressBarMerge.Name = "progressBarMerge";
            progressBarMerge.Size = new Size(960, 24);
            progressBarMerge.TabIndex = 1;
            // 
            // btnStartMerge
            // 
            btnStartMerge.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnStartMerge.Location = new Point(8, 365);
            btnStartMerge.Name = "btnStartMerge";
            btnStartMerge.Size = new Size(180, 34);
            btnStartMerge.TabIndex = 2;
            btnStartMerge.Text = "Lancer la fusion";
            btnStartMerge.Click += btnStartMerge_Click;
            // 
            // groupBoxDimensions
            // 
            groupBoxDimensions.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxDimensions.Controls.Add(checkBoxOverworld);
            groupBoxDimensions.Controls.Add(checkBoxNether);
            groupBoxDimensions.Controls.Add(checkBoxEnd);
            groupBoxDimensions.Location = new Point(8, 301);
            groupBoxDimensions.Name = "groupBoxDimensions";
            groupBoxDimensions.Size = new Size(960, 56);
            groupBoxDimensions.TabIndex = 3;
            groupBoxDimensions.TabStop = false;
            groupBoxDimensions.Text = "Dimensions a fusionner";
            // 
            // checkBoxOverworld
            // 
            checkBoxOverworld.AutoSize = true;
            checkBoxOverworld.Checked = true;
            checkBoxOverworld.CheckState = CheckState.Checked;
            checkBoxOverworld.Location = new Point(16, 22);
            checkBoxOverworld.Name = "checkBoxOverworld";
            checkBoxOverworld.Size = new Size(81, 19);
            checkBoxOverworld.TabIndex = 0;
            checkBoxOverworld.Text = "Overworld";
            // 
            // checkBoxNether
            // 
            checkBoxNether.AutoSize = true;
            checkBoxNether.Location = new Point(130, 22);
            checkBoxNether.Name = "checkBoxNether";
            checkBoxNether.Size = new Size(62, 19);
            checkBoxNether.TabIndex = 1;
            checkBoxNether.Text = "Nether";
            // 
            // checkBoxEnd
            // 
            checkBoxEnd.AutoSize = true;
            checkBoxEnd.Location = new Point(220, 22);
            checkBoxEnd.Name = "checkBoxEnd";
            checkBoxEnd.Size = new Size(46, 19);
            checkBoxEnd.TabIndex = 2;
            checkBoxEnd.Text = "End";
            // 
            // groupBoxWorldList
            // 
            groupBoxWorldList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxWorldList.Controls.Add(dataGridViewWorlds);
            groupBoxWorldList.Controls.Add(panelWorldListButtons);
            groupBoxWorldList.Location = new Point(8, 8);
            groupBoxWorldList.Name = "groupBoxWorldList";
            groupBoxWorldList.Size = new Size(960, 285);
            groupBoxWorldList.TabIndex = 4;
            groupBoxWorldList.TabStop = false;
            groupBoxWorldList.Text = "Mondes a fusionner";
            // 
            // dataGridViewWorlds
            // 
            dataGridViewWorlds.AllowUserToAddRows = false;
            dataGridViewWorlds.AllowUserToDeleteRows = false;
            dataGridViewWorlds.AllowUserToOrderColumns = true;
            dataGridViewWorlds.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewWorlds.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewWorlds.Columns.AddRange(new DataGridViewColumn[] { colWorldName, colWorldDirection, colWorldArchitecture, colRegionOverworld, colRegionNether, colRegionEnd, colRegionOther });
            dataGridViewWorlds.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridViewWorlds.Location = new Point(8, 22);
            dataGridViewWorlds.MultiSelect = false;
            dataGridViewWorlds.Name = "dataGridViewWorlds";
            dataGridViewWorlds.RowHeadersVisible = false;
            dataGridViewWorlds.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewWorlds.Size = new Size(944, 218);
            dataGridViewWorlds.TabIndex = 0;
            // 
            // colWorldName
            // 
            colWorldName.HeaderText = "Monde";
            colWorldName.Name = "colWorldName";
            colWorldName.ReadOnly = true;
            colWorldName.Width = 70;
            // 
            // colWorldDirection
            // 
            dataGridViewCellStyle1.NullValue = "E";
            colWorldDirection.DefaultCellStyle = dataGridViewCellStyle1;
            colWorldDirection.HeaderText = "Direction d'import";
            colWorldDirection.Items.AddRange(new object[] { "N", "NE", "E", "SE", "S", "SO", "O", "NO" });
            colWorldDirection.Name = "colWorldDirection";
            colWorldDirection.Width = 110;
            // 
            // colWorldArchitecture
            // 
            colWorldArchitecture.HeaderText = "Architecture";
            colWorldArchitecture.Name = "colWorldArchitecture";
            colWorldArchitecture.ReadOnly = true;
            colWorldArchitecture.Width = 97;
            // 
            // colRegionOverworld
            // 
            colRegionOverworld.HeaderText = "Overworld";
            colRegionOverworld.Name = "colRegionOverworld";
            colRegionOverworld.ReadOnly = true;
            colRegionOverworld.Width = 87;
            // 
            // colRegionNether
            // 
            colRegionNether.HeaderText = "Nether";
            colRegionNether.Name = "colRegionNether";
            colRegionNether.ReadOnly = true;
            colRegionNether.Width = 68;
            // 
            // colRegionEnd
            // 
            colRegionEnd.HeaderText = "End";
            colRegionEnd.Name = "colRegionEnd";
            colRegionEnd.ReadOnly = true;
            colRegionEnd.Width = 52;
            // 
            // colRegionOther
            // 
            colRegionOther.HeaderText = "Autres";
            colRegionOther.Name = "colRegionOther";
            colRegionOther.ReadOnly = true;
            colRegionOther.Width = 66;
            // 
            // panelWorldListButtons
            // 
            panelWorldListButtons.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelWorldListButtons.Controls.Add(btnAddFromSaves);
            panelWorldListButtons.Controls.Add(btnAddCustom);
            panelWorldListButtons.Controls.Add(btnRemoveWorld);
            panelWorldListButtons.Location = new Point(8, 248);
            panelWorldListButtons.Name = "panelWorldListButtons";
            panelWorldListButtons.Size = new Size(944, 32);
            panelWorldListButtons.TabIndex = 1;
            // 
            // btnAddFromSaves
            // 
            btnAddFromSaves.Location = new Point(0, 2);
            btnAddFromSaves.Name = "btnAddFromSaves";
            btnAddFromSaves.Size = new Size(200, 28);
            btnAddFromSaves.TabIndex = 0;
            btnAddFromSaves.Text = "Ajouter depuis .minecraft";
            btnAddFromSaves.Click += btnAddFromSaves_Click;
            // 
            // btnAddCustom
            // 
            btnAddCustom.Location = new Point(208, 2);
            btnAddCustom.Name = "btnAddCustom";
            btnAddCustom.Size = new Size(160, 28);
            btnAddCustom.TabIndex = 1;
            btnAddCustom.Text = "Importer un dossier...";
            btnAddCustom.Click += btnAddCustom_Click;
            // 
            // btnRemoveWorld
            // 
            btnRemoveWorld.Location = new Point(376, 2);
            btnRemoveWorld.Name = "btnRemoveWorld";
            btnRemoveWorld.Size = new Size(90, 28);
            btnRemoveWorld.TabIndex = 2;
            btnRemoveWorld.Text = "Retirer";
            btnRemoveWorld.Click += btnRemoveWorld_Click;
            // 
            // labelKeyWorldName
            // 
            labelKeyWorldName.Location = new Point(0, 0);
            labelKeyWorldName.Name = "labelKeyWorldName";
            labelKeyWorldName.Size = new Size(100, 23);
            labelKeyWorldName.TabIndex = 0;
            // 
            // labelValWorldName
            // 
            labelValWorldName.Location = new Point(0, 0);
            labelValWorldName.Name = "labelValWorldName";
            labelValWorldName.Size = new Size(100, 23);
            labelValWorldName.TabIndex = 0;
            // 
            // labelKeySeed
            // 
            labelKeySeed.Location = new Point(0, 0);
            labelKeySeed.Name = "labelKeySeed";
            labelKeySeed.Size = new Size(100, 23);
            labelKeySeed.TabIndex = 0;
            // 
            // labelValSeed
            // 
            labelValSeed.Location = new Point(0, 0);
            labelValSeed.Name = "labelValSeed";
            labelValSeed.Size = new Size(100, 23);
            labelValSeed.TabIndex = 0;
            // 
            // labelKeyGameMode
            // 
            labelKeyGameMode.Location = new Point(0, 0);
            labelKeyGameMode.Name = "labelKeyGameMode";
            labelKeyGameMode.Size = new Size(100, 23);
            labelKeyGameMode.TabIndex = 0;
            // 
            // labelValGameMode
            // 
            labelValGameMode.Location = new Point(0, 0);
            labelValGameMode.Name = "labelValGameMode";
            labelValGameMode.Size = new Size(100, 23);
            labelValGameMode.TabIndex = 0;
            // 
            // labelKeyDifficulty
            // 
            labelKeyDifficulty.Location = new Point(0, 0);
            labelKeyDifficulty.Name = "labelKeyDifficulty";
            labelKeyDifficulty.Size = new Size(100, 23);
            labelKeyDifficulty.TabIndex = 0;
            // 
            // labelValDifficulty
            // 
            labelValDifficulty.Location = new Point(0, 0);
            labelValDifficulty.Name = "labelValDifficulty";
            labelValDifficulty.Size = new Size(100, 23);
            labelValDifficulty.TabIndex = 0;
            // 
            // labelKeyVersion
            // 
            labelKeyVersion.Location = new Point(0, 0);
            labelKeyVersion.Name = "labelKeyVersion";
            labelKeyVersion.Size = new Size(100, 23);
            labelKeyVersion.TabIndex = 0;
            // 
            // labelValVersion
            // 
            labelValVersion.Location = new Point(0, 0);
            labelValVersion.Name = "labelValVersion";
            labelValVersion.Size = new Size(100, 23);
            labelValVersion.TabIndex = 0;
            // 
            // labelKeyHardcore
            // 
            labelKeyHardcore.Location = new Point(0, 0);
            labelKeyHardcore.Name = "labelKeyHardcore";
            labelKeyHardcore.Size = new Size(100, 23);
            labelKeyHardcore.TabIndex = 0;
            // 
            // labelValHardcore
            // 
            labelValHardcore.Location = new Point(0, 0);
            labelValHardcore.Name = "labelValHardcore";
            labelValHardcore.Size = new Size(100, 23);
            labelValHardcore.TabIndex = 0;
            // 
            // labelKeySpawn
            // 
            labelKeySpawn.Location = new Point(0, 0);
            labelKeySpawn.Name = "labelKeySpawn";
            labelKeySpawn.Size = new Size(100, 23);
            labelKeySpawn.TabIndex = 0;
            // 
            // labelValSpawn
            // 
            labelValSpawn.Location = new Point(0, 0);
            labelValSpawn.Name = "labelValSpawn";
            labelValSpawn.Size = new Size(100, 23);
            labelValSpawn.TabIndex = 0;
            // 
            // labelKeyCheats
            // 
            labelKeyCheats.Location = new Point(0, 0);
            labelKeyCheats.Name = "labelKeyCheats";
            labelKeyCheats.Size = new Size(100, 23);
            labelKeyCheats.TabIndex = 0;
            // 
            // labelValCheats
            // 
            labelValCheats.Location = new Point(0, 0);
            labelValCheats.Name = "labelValCheats";
            labelValCheats.Size = new Size(100, 23);
            labelValCheats.TabIndex = 0;
            // 
            // labelKeyGameTime
            // 
            labelKeyGameTime.Location = new Point(0, 0);
            labelKeyGameTime.Name = "labelKeyGameTime";
            labelKeyGameTime.Size = new Size(100, 23);
            labelKeyGameTime.TabIndex = 0;
            // 
            // labelValGameTime
            // 
            labelValGameTime.Location = new Point(0, 0);
            labelValGameTime.Name = "labelValGameTime";
            labelValGameTime.Size = new Size(100, 23);
            labelValGameTime.TabIndex = 0;
            // 
            // labelKeyDataVersion
            // 
            labelKeyDataVersion.Location = new Point(0, 0);
            labelKeyDataVersion.Name = "labelKeyDataVersion";
            labelKeyDataVersion.Size = new Size(100, 23);
            labelKeyDataVersion.TabIndex = 0;
            // 
            // labelValDataVersion
            // 
            labelValDataVersion.Location = new Point(0, 0);
            labelValDataVersion.Name = "labelValDataVersion";
            labelValDataVersion.Size = new Size(100, 23);
            labelValDataVersion.TabIndex = 0;
            // 
            // WorldManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(992, 660);
            Controls.Add(tabControlMain);
            Controls.Add(labelWorldTitle);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(800, 600);
            Name = "WorldManagement";
            Text = "MCMapCare - Gestion du monde";
            Load += WorldManagement_Load;
            tabControlMain.ResumeLayout(false);
            tabPageInfo.ResumeLayout(false);
            panelInfoScroll.ResumeLayout(false);
            panelInfoScroll.PerformLayout();
            tabPageMerge.ResumeLayout(false);
            groupBoxDimensions.ResumeLayout(false);
            groupBoxDimensions.PerformLayout();
            groupBoxWorldList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewWorlds).EndInit();
            panelWorldListButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Label labelWorldTitle;
        private TabControl tabControlMain;
        private TabPage tabPageInfo;
        private TabPage tabPageMerge;
        private Panel panelInfoScroll;
        private TableLayoutPanel tableLayoutInfo;
        private Label labelKeyWorldName,    labelValWorldName;
        private Label labelKeySeed,         labelValSeed;
        private Label labelKeyGameMode,     labelValGameMode;
        private Label labelKeyDifficulty,   labelValDifficulty;
        private Label labelKeyVersion,      labelValVersion;
        private Label labelKeyHardcore,     labelValHardcore;
        private Label labelKeySpawn,        labelValSpawn;
        private Label labelKeyCheats,       labelValCheats;
        private Label labelKeyGameTime,     labelValGameTime;
        private Label labelKeyDataVersion,  labelValDataVersion;
        private GroupBox groupBoxWorldList;
        private DataGridView dataGridViewWorlds;
        private DataGridViewTextBoxColumn colWorldName;
        private DataGridViewComboBoxColumn colWorldDirection;
        private DataGridViewTextBoxColumn colWorldArchitecture;
        private DataGridViewTextBoxColumn colRegionOverworld;
        private DataGridViewTextBoxColumn colRegionNether;
        private DataGridViewTextBoxColumn colRegionEnd;
        private DataGridViewTextBoxColumn colRegionOther;
        private Panel panelWorldListButtons;
        private Button btnAddFromSaves;
        private Button btnAddCustom;
        private Button btnRemoveWorld;
        private GroupBox groupBoxDimensions;
        private CheckBox checkBoxOverworld;
        private CheckBox checkBoxNether;
        private CheckBox checkBoxEnd;
        private Button btnStartMerge;
        private ProgressBar progressBarMerge;
        private Label labelStatus;
    }
}