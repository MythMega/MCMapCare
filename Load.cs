namespace MCMapCare
{
    public partial class Load : Form
    {
        public Load()
        {
            InitializeComponent();
        }

        private void Load_Load(object sender, EventArgs e)
        {
            // Applique le thème sombre
            ThemeManager.Apply(this);
            ThemeManager.ApplyPrimaryButton(btnOpenSelectedWorld);
            LoadWorldList();
        }

        private void LoadWorldList()
        {
            WorldLists.Items.Clear();
            string worldListFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "saves");
            string[] folders = Directory.GetDirectories(worldListFolder);
            foreach (string folder in folders)
            {
                if (File.Exists(Path.Combine(folder, "level.dat")))
                {
                    string folderName = Path.GetFileName(folder);
                    WorldLists.Items.Add(folderName);
                }
            }
        }

        private void btnOpenSelectedWorld_Click(object sender, EventArgs e)
        {
            if (WorldLists.SelectedItems.Count != 1)
            {
                MessageBox.Show("Select exactly one world to load");
                return;
            }
            string selectedWorld = WorldLists.Text;
            string folderToload = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "saves", selectedWorld);
            new WorldManagement(folderToload).Show();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadWorldList();
        }
    }
}