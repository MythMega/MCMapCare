namespace MCMapCare
{
    partial class WorldSelectorDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorldSelectorDialog));
            labelInfo = new Label();
            checkedListBoxWorlds = new CheckedListBox();
            btnOk = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // labelInfo
            // 
            labelInfo.Location = new Point(12, 12);
            labelInfo.Name = "labelInfo";
            labelInfo.Size = new Size(360, 20);
            labelInfo.TabIndex = 0;
            labelInfo.Text = "Sélectionnez les mondes à ajouter à la fusion :";
            // 
            // checkedListBoxWorlds
            // 
            checkedListBoxWorlds.CheckOnClick = true;
            checkedListBoxWorlds.Location = new Point(12, 38);
            checkedListBoxWorlds.Name = "checkedListBoxWorlds";
            checkedListBoxWorlds.Size = new Size(360, 274);
            checkedListBoxWorlds.TabIndex = 1;
            // 
            // btnOk
            // 
            btnOk.Location = new Point(12, 328);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(170, 30);
            btnOk.TabIndex = 2;
            btnOk.Text = "Ajouter la sélection";
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(292, 328);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(80, 30);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Annuler";
            btnCancel.Click += btnCancel_Click;
            // 
            // WorldSelectorDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(390, 374);
            Controls.Add(labelInfo);
            Controls.Add(checkedListBoxWorlds);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "WorldSelectorDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Ajouter des mondes depuis .minecraft";
            Load += WorldSelectorDialog_Load;
            ResumeLayout(false);
        }

        private Label labelInfo;
        private CheckedListBox checkedListBoxWorlds;
        private Button btnOk;
        private Button btnCancel;
    }
}
