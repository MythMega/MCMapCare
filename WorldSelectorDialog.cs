namespace MCMapCare
{
    /// <summary>
    /// Dialogue de sélection de mondes depuis le dossier .minecraft/saves.
    /// Affiche une liste cochable de mondes disponibles (hors monde actuellement ouvert).
    /// </summary>
    public partial class WorldSelectorDialog : Form
    {
        private readonly string _savesPath;
        private readonly string _excludePath;

        // Stocke la liste ordonnée de (Nom, Chemin) correspondant aux éléments de la liste
        private readonly List<(string Name, string Path)> _availableWorlds = new();

        /// <summary>
        /// Les mondes cochés et validés par l'utilisateur, sous forme de paires (Nom, Chemin).
        /// </summary>
        public List<(string Name, string Path)> SelectedWorlds { get; } = new();

        public WorldSelectorDialog(string savesPath, string excludePath)
        {
            InitializeComponent();
            _savesPath = savesPath;
            _excludePath = Path.GetFullPath(excludePath);
        }

        private void WorldSelectorDialog_Load(object sender, EventArgs e)
        {
            // Applique le thème sombre
            ThemeManager.Apply(this);
            ThemeManager.ApplyPrimaryButton(btnOk);
            // Parcourt les sous-dossiers du dossier saves et liste les mondes valides
            foreach (string folder in Directory.GetDirectories(_savesPath))
            {
                // Vérifie que le dossier est bien un monde Minecraft
                if (!File.Exists(Path.Combine(folder, "level.dat"))) continue;

                // Exclut le monde actuellement ouvert pour éviter une auto-fusion
                if (Path.GetFullPath(folder).Equals(_excludePath, StringComparison.OrdinalIgnoreCase)) continue;

                string worldName = Path.GetFileName(folder);
                _availableWorlds.Add((worldName, folder));
                checkedListBoxWorlds.Items.Add(worldName);
            }

            if (checkedListBoxWorlds.Items.Count == 0)
            {
                labelInfo.Text = "Aucun autre monde trouvé dans .minecraft/saves.";
                btnOk.Enabled = false;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SelectedWorlds.Clear();

            foreach (int i in checkedListBoxWorlds.CheckedIndices)
                SelectedWorlds.Add(_availableWorlds[i]);

            if (SelectedWorlds.Count == 0)
            {
                MessageBox.Show("Cochez au moins un monde à ajouter.", "Aucune sélection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
