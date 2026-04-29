using System.Text;

namespace MCMapCare
{
    // ===========================
    // Types pour le rapport de fusion
    // ===========================

    /// <summary>Résultat de la fusion d'une dimension pour un monde donné.</summary>
    internal record DimensionMergeResult(string Label, int RegionsCopied, bool Skipped);

    /// <summary>Résultat complet de la fusion d'un monde importé.</summary>
    internal record WorldMergeResult(string WorldName, string Direction, bool IsNewArch, List<DimensionMergeResult> Dimensions);

    /// <summary>Rapport global retourné après la fusion complète.</summary>
    internal record MergeReport(string FusedWorldName, TimeSpan Duration, List<WorldMergeResult> WorldResults);

    /// <summary>
    /// Formulaire principal de gestion d'un monde Minecraft.
    /// Composé de deux onglets : Infos (lecture du level.dat) et Fusionner (merge de mondes).
    /// </summary>
    public partial class WorldManagement : Form
    {
        // Identifiants internes des trois dimensions supportées
        private const string DimOverworld = "overworld";

        private const string DimNether = "nether";
        private const string DimEnd = "end";

        private readonly string _worldPath;

        // Stocke les chemins des mondes à fusionner, indexés de façon synchrone avec les lignes du DataGridView
        private readonly List<string> _mergeWorldPaths = new();

        public WorldManagement(string worldPath)
        {
            InitializeComponent();
            _worldPath = worldPath;
        }

        private async void WorldManagement_Load(object sender, EventArgs e)
        {
            // Applique le thème sombre avant l'affichage
            ThemeManager.Apply(this);
            ThemeManager.ApplyPrimaryButton(btnStartMerge);

            // Configure l'onglet Info (le Designer VS ne gère pas l'ajout de contrôles
            // dans un TableLayoutPanel via des méthodes personnalisées)
            SetupInfoTab();

            // Configure l'onglet Datapack (branchement des boutons + chargement des listes)
            SetupDatapackTab();

            labelWorldTitle.Text = Path.GetFileName(_worldPath);
            await LoadWorldInfoAsync();
        }

        /// <summary>
        /// Configure l'onglet Info : corrige les styles de colonnes et ajoute
        /// les paires clé/valeur dans le TableLayoutPanel.
        /// </summary>
        private void SetupInfoTab()
        {
            // Corrige les ColumnStyles que le Designer remet à leur valeur par défaut
            tableLayoutInfo.ColumnStyles.Clear();
            tableLayoutInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210F));
            tableLayoutInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutInfo.Controls.Clear();

            // Ajoute chaque ligne clé/valeur avec son style
            AddInfoRow(0, labelKeyWorldName, "Nom du monde :", labelValWorldName);
            AddInfoRow(1, labelKeySeed, "Graine (Seed) :", labelValSeed);
            AddInfoRow(2, labelKeyGameMode, "Mode de jeu :", labelValGameMode);
            AddInfoRow(3, labelKeyDifficulty, "Difficulté :", labelValDifficulty);
            AddInfoRow(4, labelKeyVersion, "Version Minecraft :", labelValVersion);
            AddInfoRow(5, labelKeyHardcore, "Hardcore :", labelValHardcore);
            AddInfoRow(6, labelKeySpawn, "Point d'apparition :", labelValSpawn);
            AddInfoRow(7, labelKeyCheats, "Triche activée :", labelValCheats);
            AddInfoRow(8, labelKeyGameTime, "Temps de jeu :", labelValGameTime);
            AddInfoRow(9, labelKeyDataVersion, "Version des données :", labelValDataVersion);
        }

        /// <summary>
        /// Ajoute une ligne clé/valeur dans la grille d'informations avec le style du thème.
        /// </summary>
        private void AddInfoRow(int row, Label keyLabel, string keyText, Label valueLabel)
        {
            keyLabel.Text = keyText;
            keyLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            keyLabel.Dock = DockStyle.Fill;
            keyLabel.TextAlign = ContentAlignment.MiddleLeft;
            keyLabel.ForeColor = ThemeManager.TextSecondary;
            keyLabel.BackColor = Color.Transparent;

            valueLabel.Text = "—";
            valueLabel.Font = new Font("Segoe UI", 10F);
            valueLabel.Dock = DockStyle.Fill;
            valueLabel.TextAlign = ContentAlignment.MiddleLeft;
            valueLabel.ForeColor = ThemeManager.TextPrimary;
            valueLabel.BackColor = Color.Transparent;

            tableLayoutInfo.Controls.Add(keyLabel, 0, row);
            tableLayoutInfo.Controls.Add(valueLabel, 1, row);
        }

        // ===========================
        // ONGLET INFO
        // ===========================

        /// <summary>
        /// Lit le fichier level.dat de manière asynchrone et remplit les labels de l'onglet Info.
        /// </summary>
        private async Task LoadWorldInfoAsync()
        {
            string levelDatPath = Path.Combine(_worldPath, "level.dat");

            if (!File.Exists(levelDatPath))
            {
                labelValWorldName.Text = "Fichier level.dat introuvable.";
                return;
            }

            try
            {
                // Lecture NBT en arrière-plan pour ne pas geler l'interface
                var nbt = await Task.Run(() => NbtReader.ReadLevelDat(levelDatPath));

                // Helper : retourne la valeur du tag ou un tiret si absent
                string Get(string key) => nbt.TryGetValue(key, out var v) ? v : "—";

                string levelName = Get("Data.LevelName");
                labelValWorldName.Text = levelName;
                labelWorldTitle.Text = levelName;

                // Graine : nouveau format (1.16+) ou ancien format
                string seed = Get("Data.WorldGenSettings.seed");
                if (seed == "—") seed = Get("Data.RandomSeed");
                labelValSeed.Text = seed;

                labelValGameMode.Text = Get("Data.GameType") switch
                {
                    "0" => "Survie",
                    "1" => "Créatif",
                    "2" => "Aventure",
                    "3" => "Spectateur",
                    var raw => $"Inconnu ({raw})"
                };

                labelValDifficulty.Text = Get("Data.Difficulty") switch
                {
                    "0" => "Paisible",
                    "1" => "Facile",
                    "2" => "Normale",
                    "3" => "Difficile",
                    var raw => $"Inconnue ({raw})"
                };

                labelValVersion.Text = Get("Data.Version.Name");
                labelValHardcore.Text = Get("Data.hardcore") == "1" ? "Oui" : "Non";
                labelValSpawn.Text = $"X={Get("Data.SpawnX")}  Y={Get("Data.SpawnY")}  Z={Get("Data.SpawnZ")}";
                labelValCheats.Text = Get("Data.allowCommands") == "1" ? "Oui" : "Non";

                // Temps de jeu : conversion ticks vers heures/minutes
                string timeRaw = Get("Data.Time");
                if (long.TryParse(timeRaw, out long ticks))
                {
                    long seconds = ticks / 20;
                    long hours = seconds / 3600;
                    long minutes = (seconds % 3600) / 60;
                    labelValGameTime.Text = $"{hours}h {minutes}min  ({ticks:N0} ticks)";
                }
                else
                {
                    labelValGameTime.Text = timeRaw;
                }

                labelValDataVersion.Text = Get("Data.DataVersion");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erreur lors de la lecture du fichier level.dat :\n\n{ex.Message}",
                    "Erreur de lecture", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===========================
        // DÉTECTION DE L'ARCHITECTURE DES MONDES
        // ===========================

        /// <summary>
        /// Détecte si un monde utilise la nouvelle architecture MC 1.21.4+ (26+).
        /// Nouvelle architecture  : WorldFolder/dimensions/minecraft/overworld/region/
        /// Architecture classique : WorldFolder/region/
        /// </summary>
        private static bool IsNewArchitecture(string worldPath)
            => Directory.Exists(Path.Combine(worldPath, "dimensions", "minecraft", "overworld", "region"));

        /// <summary>
        /// Retourne le chemin absolu du dossier de régions d'une dimension dans un monde donné.
        /// Détecte automatiquement l'architecture (classique ou MC 26+).
        /// Le dossier peut ne pas encore exister (dimension non visitée).
        /// </summary>
        private static string GetRegionFolder(string worldPath, string dimension)
        {
            bool isNew = IsNewArchitecture(worldPath);
            return dimension switch
            {
                DimOverworld => isNew
                    ? Path.Combine(worldPath, "dimensions", "minecraft", "overworld", "region")
                    : Path.Combine(worldPath, "region"),
                DimNether => isNew
                    ? Path.Combine(worldPath, "dimensions", "minecraft", "the_nether", "region")
                    : Path.Combine(worldPath, "DIM-1", "region"),
                DimEnd => isNew
                    ? Path.Combine(worldPath, "dimensions", "minecraft", "the_end", "region")
                    : Path.Combine(worldPath, "DIM1", "region"),
                _ => throw new ArgumentException($"Dimension inconnue : {dimension}")
            };
        }

        /// <summary>
        /// Compte les fichiers .mca dans le dossier de régions d'une dimension.
        /// Retourne 0 si le dossier n'existe pas.
        /// </summary>
        private static int CountRegions(string worldPath, string dimension)
        {
            string folder = GetRegionFolder(worldPath, dimension);
            if (!Directory.Exists(folder)) return 0;
            return Directory.GetFiles(folder, "r.*.*.mca").Length;
        }

        /// <summary>
        /// Compte les fichiers .mca de toutes les dimensions non standard d'un monde.
        /// Parcourt récursivement les sous-dossiers de "dimensions/" et exclut les trois
        /// dimensions de base (overworld, the_nether, the_end).
        /// Retourne 0 pour les mondes en architecture classique (pas de dossier dimensions/).
        /// </summary>
        private static int CountOtherDimensions(string worldPath)
        {
            string dimensionsRoot = Path.Combine(worldPath, "dimensions");
            if (!Directory.Exists(dimensionsRoot)) return 0;

            // Chemins des trois dimensions de base à exclure
            var knownRegionPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                Path.Combine(dimensionsRoot, "minecraft", "overworld",  "region"),
                Path.Combine(dimensionsRoot, "minecraft", "the_nether", "region"),
                Path.Combine(dimensionsRoot, "minecraft", "the_end",    "region"),
            };

            int total = 0;

            // Cherche tous les sous-dossiers nommés "region" sous dimensions/ (profondeur variable)
            foreach (string regionDir in Directory.GetDirectories(dimensionsRoot, "region", SearchOption.AllDirectories))
            {
                if (!knownRegionPaths.Contains(regionDir))
                    total += Directory.GetFiles(regionDir, "r.*.*.mca").Length;
            }

            return total;
        }

        /// <summary>Retourne le libellé affiché d'une dimension à partir de son identifiant interne.</summary>
        private static string DimToLabel(string dimension) => dimension switch
        {
            DimOverworld => "Overworld",
            DimNether => "Nether",
            DimEnd => "End",
            _ => dimension
        };

        // ===========================
        // ONGLET FUSIONNER — Gestion de la liste
        // ===========================

        /// <summary>Ouvre le dialogue de sélection de mondes depuis .minecraft/saves.</summary>
        private void btnAddFromSaves_Click(object sender, EventArgs e)
        {
            string savesPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                ".minecraft", "saves");

            if (!Directory.Exists(savesPath))
            {
                MessageBox.Show(
                    "Le dossier .minecraft/saves est introuvable sur cet ordinateur.",
                    "Dossier manquant", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var dialog = new WorldSelectorDialog(savesPath, _worldPath);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var entry in dialog.SelectedWorlds)
                    AddWorldToMergeList(entry.Name, entry.Path);
            }
        }

        /// <summary>Ouvre un FolderBrowserDialog pour importer un monde depuis un chemin personnalisé.</summary>
        private void btnAddCustom_Click(object sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog
            {
                Description = "Sélectionnez le dossier d'un monde Minecraft à importer",
                UseDescriptionForTitle = true
            };

            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            string selectedPath = dialog.SelectedPath;

            if (!File.Exists(Path.Combine(selectedPath, "level.dat")))
            {
                MessageBox.Show(
                    "Le dossier sélectionné ne semble pas être un monde Minecraft valide.\n" +
                    $"Le fichier level.dat est absent de :\n{selectedPath}",
                    "Dossier invalide", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AddWorldToMergeList(Path.GetFileName(selectedPath), selectedPath);
        }

        /// <summary>Retire le monde sélectionné de la liste de fusion.</summary>
        private void btnRemoveWorld_Click(object sender, EventArgs e)
        {
            if (dataGridViewWorlds.CurrentRow == null) return;

            int rowIndex = dataGridViewWorlds.CurrentRow.Index;
            dataGridViewWorlds.Rows.RemoveAt(rowIndex);
            _mergeWorldPaths.RemoveAt(rowIndex);
        }

        /// <summary>
        /// Ajoute un monde à la liste de fusion.
        /// Détecte son architecture et compte ses régions par dimension pour affichage immédiat.
        /// </summary>
        private void AddWorldToMergeList(string worldName, string worldPath)
        {
            // Vérification des doublons par chemin normalisé
            string normalizedPath = Path.GetFullPath(worldPath);
            if (_mergeWorldPaths.Any(p =>
                Path.GetFullPath(p).Equals(normalizedPath, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show(
                    $"Le monde « {worldName} » est déjà dans la liste de fusion.",
                    "Doublon", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _mergeWorldPaths.Add(worldPath);

            // Détection de l'architecture du monde importé
            bool isNew = IsNewArchitecture(worldPath);
            string arch = isNew ? "MC 26+" : "Classique";

            // Comptage des régions par dimension
            int owCount = CountRegions(worldPath, DimOverworld);
            int neCount = CountRegions(worldPath, DimNether);
            int enCount = CountRegions(worldPath, DimEnd);
            // Dimensions non standard : uniquement visibles sur nouvelle architecture
            int otherCount = isNew ? CountOtherDimensions(worldPath) : 0;
            string otherDisplay = isNew ? otherCount.ToString() : "—";

            // Direction "E" (Est) par défaut
            dataGridViewWorlds.Rows.Add(worldName, "E", arch, owCount, neCount, enCount, otherDisplay);
        }

        // ===========================
        // ONGLET FUSIONNER — Processus de fusion
        // ===========================

        /// <summary>
        /// Valide les entrées, désactive l'interface et lance le processus de fusion asynchrone.
        /// </summary>
        private async void btnStartMerge_Click(object sender, EventArgs e)
        {
            if (dataGridViewWorlds.Rows.Count == 0)
            {
                MessageBox.Show("Ajoutez au moins un monde à fusionner avant de lancer l'opération.",
                    "Aucun monde", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!checkBoxOverworld.Checked && !checkBoxNether.Checked && !checkBoxEnd.Checked)
            {
                MessageBox.Show("Sélectionnez au moins une dimension à fusionner.",
                    "Aucune dimension", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Collecte les entrées (chemin + direction) depuis le DataGridView
            var mergeEntries = new List<(string Path, string Direction)>();
            for (int i = 0; i < dataGridViewWorlds.Rows.Count; i++)
            {
                string path = _mergeWorldPaths[i];
                string direction = dataGridViewWorlds.Rows[i].Cells[colWorldDirection.Name].Value?.ToString() ?? "E";
                mergeEntries.Add((path, direction));
            }

            // Collecte les dimensions sélectionnées par leur identifiant interne
            var selectedDimensions = new List<string>();
            if (checkBoxOverworld.Checked) selectedDimensions.Add(DimOverworld);
            if (checkBoxNether.Checked) selectedDimensions.Add(DimNether);
            if (checkBoxEnd.Checked) selectedDimensions.Add(DimEnd);

            SetUiEnabled(false);
            progressBarMerge.Value = 0;
            labelStatus.ForeColor = ThemeManager.TextSecondary;

            try
            {
                MergeReport report = await RunMergeAsync(mergeEntries, selectedDimensions);

                labelStatus.Text = "Fusion terminée avec succès !";
                labelStatus.ForeColor = ThemeManager.Success;
                MessageBox.Show(BuildMergeSummary(report),
                    "Fusion terminée", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                labelStatus.Text = $"Erreur : {ex.Message}";
                labelStatus.ForeColor = ThemeManager.Error;
                MessageBox.Show(
                    $"Une erreur est survenue pendant la fusion :\n\n{ex.Message}",
                    "Erreur de fusion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetUiEnabled(true);
            }
        }

        /// <summary>Active ou désactive les contrôles pour bloquer toute action pendant la fusion.</summary>
        private void SetUiEnabled(bool enabled) => tabControlMain.Enabled = enabled;

        /// <summary>
        /// Logique principale de fusion :
        /// 1. Duplique le monde de base avec le suffixe "_fused"
        /// 2. Pour chaque monde importé et chaque dimension sélectionnée :
        ///    - Détecte l'architecture du monde source (classique ou MC 26+)
        ///    - Résout les dossiers de régions source et cible selon leurs architectures respectives
        ///    - Calcule l'offset de coordonnées pour éviter tout chevauchement
        ///    - Copie les fichiers .mca en les renommant avec l'offset
        /// 3. Retourne un rapport détaillé de l'opération.
        /// </summary>
        private async Task<MergeReport> RunMergeAsync(
            List<(string Path, string Direction)> mergeEntries,
            List<string> selectedDimensions)
        {
            var startTime = DateTime.Now;

            string parentDir = Path.GetDirectoryName(_worldPath)!;
            string baseName = Path.GetFileName(_worldPath);
            string fusedPath = Path.Combine(parentDir, baseName + "_fused");

            // Étape 1 : supprime une éventuelle fusion précédente
            if (Directory.Exists(fusedPath))
            {
                SetStatus("Suppression de l'ancienne version fusionnée...");
                await Task.Run(() => Directory.Delete(fusedPath, recursive: true));
            }

            // Étape 2 : copie intégrale du monde de base — le monde fusionné héritera de son architecture
            SetStatus($"Copie du monde de base vers « {baseName}_fused »...");
            await Task.Run(() => CopyDirectory(_worldPath, fusedPath));
            SetProgress(10);

            // Étape 3 : traitement de chaque monde à importer
            var worldResults = new List<WorldMergeResult>();
            int totalOperations = mergeEntries.Count * selectedDimensions.Count;
            int completedOperations = 0;

            foreach (var (sourcePath, direction) in mergeEntries)
            {
                string sourceWorldName = Path.GetFileName(sourcePath);
                bool sourceIsNew = IsNewArchitecture(sourcePath);
                var dimResults = new List<DimensionMergeResult>();

                foreach (string dim in selectedDimensions)
                {
                    string dimLabel = DimToLabel(dim);

                    // Résolution des dossiers selon l'architecture de chaque monde indépendamment
                    // Le dossier cible est basé sur l'architecture du monde de BASE (copié dans fusedPath)
                    string sourceRegionDir = GetRegionFolder(sourcePath, dim);
                    string targetRegionDir = GetRegionFolder(fusedPath, dim);

                    // Dimension absente dans ce monde source : on la saute sans erreur
                    if (!Directory.Exists(sourceRegionDir))
                    {
                        SetStatus($"[{sourceWorldName}] Dimension {dimLabel} absente dans ce monde — ignorée.");
                        dimResults.Add(new DimensionMergeResult(dimLabel, RegionsCopied: 0, Skipped: true));
                        completedOperations++;
                        SetProgress(10 + (completedOperations * 88 / totalOperations));
                        continue;
                    }

                    // Crée le dossier cible si absent (ex : Nether non visité dans le monde de base)
                    Directory.CreateDirectory(targetRegionDir);

                    // Calcul de l'offset de coordonnées de régions pour placement sans chevauchement
                    SetStatus($"[{sourceWorldName}] Analyse des régions {dimLabel} pour calcul de l'offset...");
                    var (offsetX, offsetZ) = await Task.Run(() =>
                        CalculateRegionOffset(targetRegionDir, sourceRegionDir, direction));

                    string archLabel = sourceIsNew ? "MC 26+" : "Classique";
                    SetStatus($"[{sourceWorldName}] Fusion {dimLabel} — direction {direction}, " +
                              $"arch. {archLabel}, offset X={offsetX:+0;-0;0} Z={offsetZ:+0;-0;0}...");

                    // Copie et renommage des fichiers .mca avec l'offset appliqué
                    int copied = 0;
                    await Task.Run(() =>
                    {
                        copied = CopyRegionsWithOffset(
                            sourceRegionDir, targetRegionDir, offsetX, offsetZ,
                            fileName => SetStatus(
                                $"[{sourceWorldName}] {dimLabel} — copie de {fileName}..."));
                    });

                    dimResults.Add(new DimensionMergeResult(dimLabel, RegionsCopied: copied, Skipped: false));
                    completedOperations++;
                    SetProgress(10 + (completedOperations * 88 / totalOperations));
                }

                worldResults.Add(new WorldMergeResult(sourceWorldName, direction, sourceIsNew, dimResults));
            }

            SetProgress(100);
            return new MergeReport(Path.GetFileName(fusedPath), DateTime.Now - startTime, worldResults);
        }

        /// <summary>
        /// Construit le message de résumé détaillé affiché à la fin de la fusion.
        /// Inclut les mondes traités, l'architecture de chacun, les régions copiées par dimension et le temps total.
        /// </summary>
        private static string BuildMergeSummary(MergeReport report)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Fusion terminée en {FormatDuration(report.Duration)}");
            sb.AppendLine();
            sb.AppendLine("Mondes fusionnés :");

            int totalRegions = 0;

            foreach (var world in report.WorldResults)
            {
                string arch = world.IsNewArch ? "MC 26+" : "Classique";
                sb.AppendLine($"  • {world.WorldName}  (direction : {world.Direction} | {arch})");

                foreach (var dim in world.Dimensions)
                {
                    if (dim.Skipped)
                        sb.AppendLine($"      {dim.Label,-12} — absent dans ce monde, ignoré");
                    else
                    {
                        sb.AppendLine($"      {dim.Label,-12} : {dim.RegionsCopied} région(s) copiée(s)");
                        totalRegions += dim.RegionsCopied;
                    }
                }
            }

            sb.AppendLine();
            sb.AppendLine($"Total : {totalRegions} région(s) copiée(s)");
            sb.AppendLine($"Monde résultant : {report.FusedWorldName}");

            return sb.ToString();
        }

        /// <summary>Formate une durée de façon lisible (ex: "2m 34s").</summary>
        private static string FormatDuration(TimeSpan ts)
        {
            if (ts.TotalSeconds < 60) return $"{ts.Seconds}s";
            if (ts.TotalMinutes < 60) return $"{(int)ts.TotalMinutes}m {ts.Seconds}s";
            return $"{(int)ts.TotalHours}h {ts.Minutes}m {ts.Seconds}s";
        }

        // ===========================
        // CALCUL DE L'OFFSET DE RÉGIONS
        // ===========================

        /// <summary>
        /// Calcule le décalage (offsetX, offsetZ) en coordonnées de régions à appliquer
        /// aux fichiers du monde source pour les placer adjacents au monde cible,
        /// dans la direction cardinale choisie, sans aucun chevauchement.
        /// Convention Minecraft : Z négatif = Nord, Z positif = Sud, X positif = Est.
        /// </summary>
        private static (int OffsetX, int OffsetZ) CalculateRegionOffset(
            string targetRegionDir, string sourceRegionDir, string direction)
        {
            var targetCoords = ParseRegionCoordinates(targetRegionDir);
            var sourceCoords = ParseRegionCoordinates(sourceRegionDir);

            if (targetCoords.Count == 0 || sourceCoords.Count == 0)
                return (0, 0);

            int targetMinX = targetCoords.Min(c => c.X);
            int targetMaxX = targetCoords.Max(c => c.X);
            int targetMinZ = targetCoords.Min(c => c.Z);
            int targetMaxZ = targetCoords.Max(c => c.Z);

            int sourceMinX = sourceCoords.Min(c => c.X);
            int sourceMaxX = sourceCoords.Max(c => c.X);
            int sourceMinZ = sourceCoords.Min(c => c.Z);
            int sourceMaxZ = sourceCoords.Max(c => c.Z);

            int targetCenterX = (targetMinX + targetMaxX) / 2;
            int targetCenterZ = (targetMinZ + targetMaxZ) / 2;
            int sourceCenterX = (sourceMinX + sourceMaxX) / 2;
            int sourceCenterZ = (sourceMinZ + sourceMaxZ) / 2;

            // Offset X : colle à l'Est, à l'Ouest, ou centre sur l'axe X
            int offsetX = direction switch
            {
                "E" or "NE" or "SE" => targetMaxX + 1 - sourceMinX,
                "O" or "NO" or "SO" => targetMinX - 1 - sourceMaxX,
                _ => targetCenterX - sourceCenterX
            };

            // Offset Z : colle au Sud, au Nord, ou centre sur l'axe Z
            int offsetZ = direction switch
            {
                "S" or "SE" or "SO" => targetMaxZ + 1 - sourceMinZ,
                "N" or "NE" or "NO" => targetMinZ - 1 - sourceMaxZ,
                _ => targetCenterZ - sourceCenterZ
            };

            return (offsetX, offsetZ);
        }

        /// <summary>
        /// Analyse tous les fichiers .mca d'un dossier et retourne leurs coordonnées (X, Z).
        /// Format des fichiers de régions Minecraft : r.X.Z.mca
        /// </summary>
        private static List<(int X, int Z)> ParseRegionCoordinates(string regionDir)
        {
            var coords = new List<(int X, int Z)>();
            if (!Directory.Exists(regionDir)) return coords;

            foreach (string file in Directory.GetFiles(regionDir, "r.*.*.mca"))
            {
                string name = Path.GetFileNameWithoutExtension(file);
                string[] parts = name.Split('.');

                if (parts.Length >= 3
                    && int.TryParse(parts[1], out int x)
                    && int.TryParse(parts[2], out int z))
                {
                    coords.Add((x, z));
                }
            }

            return coords;
        }

        /// <summary>
        /// Copie tous les fichiers .mca du dossier source vers le dossier cible,
        /// en appliquant l'offset aux coordonnées dans le nom de fichier.
        /// Retourne le nombre de fichiers copiés.
        /// Lève une exception si un fichier de destination existe déjà (protection des données).
        /// </summary>
        private static int CopyRegionsWithOffset(
            string sourceDir, string targetDir,
            int offsetX, int offsetZ,
            Action<string> progressCallback)
        {
            int copied = 0;

            foreach (string sourceFile in Directory.GetFiles(sourceDir, "r.*.*.mca"))
            {
                string name = Path.GetFileNameWithoutExtension(sourceFile);
                string[] parts = name.Split('.');

                if (parts.Length < 3
                    || !int.TryParse(parts[1], out int x)
                    || !int.TryParse(parts[2], out int z))
                    continue; // Ignore les noms malformés

                int newX = x + offsetX;
                int newZ = z + offsetZ;

                string newFileName = $"r.{newX}.{newZ}.mca";
                string destFile = Path.Combine(targetDir, newFileName);

                progressCallback(newFileName);

                if (File.Exists(destFile))
                    throw new IOException(
                        $"Conflit détecté : le fichier « {newFileName} » existe déjà dans le dossier cible.\n" +
                        "Cela indique une erreur dans le calcul de l'offset. " +
                        "Fusion annulée pour protéger les données.");

                File.Copy(sourceFile, destFile);
                copied++;
            }

            return copied;
        }

        /// <summary>Copie récursivement un dossier et tout son contenu.</summary>
        private static void CopyDirectory(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);

            foreach (string file in Directory.GetFiles(sourceDir))
                File.Copy(file, Path.Combine(destDir, Path.GetFileName(file)));

            foreach (string subDir in Directory.GetDirectories(sourceDir))
                CopyDirectory(subDir, Path.Combine(destDir, Path.GetFileName(subDir)));
        }

        // ===========================
        // UTILITAIRES THREAD-SAFE
        // ===========================

        /// <summary>Met à jour le label de statut depuis n'importe quel thread.</summary>
        private void SetStatus(string message)
        {
            if (InvokeRequired)
                Invoke(() => labelStatus.Text = message);
            else
                labelStatus.Text = message;
        }

        /// <summary>Met à jour la barre de progression depuis n'importe quel thread.</summary>
        private void SetProgress(int percent)
        {
            int clamped = Math.Clamp(percent, 0, 100);
            if (InvokeRequired)
                Invoke(() => progressBarMerge.Value = clamped);
            else
                progressBarMerge.Value = clamped;
        }

        private void buttonGenerateDatapack_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txbDatapackCreationId.Text))
            {
                MessageBox.Show("ERROR : ID is null");
                return;
            }
            if (txbDatapackCreationId.Text.Contains(' '))
            {
                txbDatapackCreationId.Text = txbDatapackCreationId.Text.Replace(" ", "");
                MessageBox.Show("WARNING : spaces has been removed from the name of the datapack");
            }
            if (txbDatapackCreationId.Text.Length < 3)
            {
                MessageBox.Show("ERROR : need a name/id with more than 3 characters");
                return;
            }
            string folderDatapacksGenerate = Path.Combine(_worldPath, "datapacks");

            if (Directory.Exists(Path.Combine(folderDatapacksGenerate, txbDatapackCreationId.Text)))
            {
                MessageBox.Show("ERROR : it seems that a datapack with name/id already exist");
                return;
            }

            string folderNewDatapack = Path.Combine(folderDatapacksGenerate, txbDatapackCreationId.Text);
            string packmcmetaFile = Path.Combine(folderNewDatapack, "pack.mcmeta");
            string folderNewDatapackData = Path.Combine(folderNewDatapack, "data");
            string folderNewDatapackDataCustomDomain = Path.Combine(folderNewDatapackData, txbDatapackCreationId.Text);
            string folderNewDatapackDataMinecraftDomain = Path.Combine(folderNewDatapackData, "minecraft");

            string folderNewDatapackAdvancement = Path.Combine(folderNewDatapackDataCustomDomain, "advancement");
            string folderNewDatapackDialog = Path.Combine(folderNewDatapackDataCustomDomain, "dialog");
            string folderNewDatapackDimension = Path.Combine(folderNewDatapackDataCustomDomain, "dimension");
            string folderNewDatapackDimension_type = Path.Combine(folderNewDatapackDataCustomDomain, "dimension_type");
            string folderNewDatapackEnchantment = Path.Combine(folderNewDatapackDataCustomDomain, "enchantment");
            string folderNewDatapackFunction = Path.Combine(folderNewDatapackDataCustomDomain, "function");
            string folderNewDatapackLoot_table = Path.Combine(folderNewDatapackDataCustomDomain, "loot_table");
            string folderNewDatapackRecipe = Path.Combine(folderNewDatapackDataCustomDomain, "recipe");
            string folderNewDatapackStructure = Path.Combine(folderNewDatapackDataCustomDomain, "structure");
            string folderNewDatapackTags = Path.Combine(folderNewDatapackDataCustomDomain, "tags");
            string folderNewDatapackWorldgen = Path.Combine(folderNewDatapackDataCustomDomain, "worldgen");
            string[] levelOneFolders =
                [
                folderNewDatapackAdvancement,
                folderNewDatapackDialog,
                folderNewDatapackDimension,
                folderNewDatapackDimension_type,
                folderNewDatapackEnchantment,
                folderNewDatapackFunction,
                folderNewDatapackLoot_table,
                folderNewDatapackRecipe,
                folderNewDatapackStructure,
                folderNewDatapackTags,
                folderNewDatapackWorldgen,
            ];
            string packmcmetaContent = $@"{{
  ""pack"": {{
    ""description"": ""{txbDatapackCreationDesc.Text}"",
    ""min_format"": [
      101,
      1
    ],
    ""max_format"": 999
  }}
}}";
            // global folders pack
            if (!Directory.Exists(folderDatapacksGenerate))
                Directory.CreateDirectory(folderDatapacksGenerate);
            if (!Directory.Exists(folderNewDatapack))
                Directory.CreateDirectory(folderNewDatapack);
            if (!File.Exists(packmcmetaFile))
                File.WriteAllText(packmcmetaFile, packmcmetaContent);
            if (!Directory.Exists(folderNewDatapackData))
                Directory.CreateDirectory(folderNewDatapackData);
            if (!Directory.Exists(folderNewDatapackDataCustomDomain))
                Directory.CreateDirectory(folderNewDatapackDataCustomDomain);
            if (!Directory.Exists(folderNewDatapackDataMinecraftDomain))
                Directory.CreateDirectory(folderNewDatapackDataMinecraftDomain);

            // custom domain folders pack
            foreach (string folder in levelOneFolders)
            {
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
            }

            // minecraft folders pack
            string tagsFolder = Path.Combine(folderNewDatapackDataMinecraftDomain, "tags");
            string tagsFunctionFolder = Path.Combine(tagsFolder, "function");

            if (!Directory.Exists(tagsFolder))
                Directory.CreateDirectory(tagsFolder);
            if (!Directory.Exists(tagsFunctionFolder))
                Directory.CreateDirectory(tagsFunctionFolder);

            if (checkBoxDatapackLoopFunction.Checked)
            {
                string loopFunction = Path.Combine(folderNewDatapackFunction, "loop.mcfunction");
                string loopFunctionTag = Path.Combine(tagsFunctionFolder, "tick.json");
                if (!File.Exists(loopFunctionTag))
                {
                    string loopFunctionText = $@"{{
 ""values"": [
 ""{txbDatapackCreationId.Text}:loop""
 ]
}}";
                    File.WriteAllText(loopFunctionTag, loopFunctionText);
                }
                if (!File.Exists(loopFunction))
                {
                    File.WriteAllText(loopFunction, "");
                }
            }

            if (checkBoxDatapackLoadFunction.Checked)
            {
                string loopFunction = Path.Combine(folderNewDatapackFunction, "load.mcfunction");
                string loopFunctionTag = Path.Combine(tagsFunctionFolder, "load.json");
                if (!File.Exists(loopFunctionTag))
                {
                    string loopFunctionText = $@"{{
 ""values"": [
 ""{txbDatapackCreationId.Text}:load""
 ]
}}";
                    File.WriteAllText(loopFunctionTag, loopFunctionText);
                }
                if (!File.Exists(loopFunction))
                {
                    File.WriteAllText(loopFunction, "");
                }
            }

            // Rafraîchit la liste Management après création
            RefreshDatapackLists();
        }

        // ===========================
        // ONGLET DATAPACK — Management & Pack DB
        // ===========================

        /// <summary>Chemin de la base de données locale de packs (AppData/Roaming/JMD/mcmodcare/packdb).</summary>
        private static readonly string PackDbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "JMD", "mcmodcare", "packdb");

        /// <summary>Chemin du dossier datapacks du monde actuellement ouvert.</summary>
        private string WorldDatapacksPath => Path.Combine(_worldPath, "datapacks");

        /// <summary>
        /// Configure les ListViews et branche les événements des boutons de l'onglet Datapack.
        /// Appelé depuis WorldManagement_Load (le Designer VS ne gère pas ces branchements).
        /// </summary>
        private void SetupDatapackTab()
        {
            // ListView des datapacks du monde
            listViewWorldPacks.View          = View.List;
            listViewWorldPacks.FullRowSelect = true;
            listViewWorldPacks.MultiSelect   = true;

            // ListView de la base de packs
            listViewPackDB.View          = View.List;
            listViewPackDB.FullRowSelect = true;
            listViewPackDB.MultiSelect   = true;

            // Groupe Management — datapacks du monde courant
            btnDatapackManagementDelete.Click    += async (_, _) => await DeleteSelectedAsync(listViewWorldPacks, WorldDatapacksPath);
            btnDatapackManagementRename.Click    += async (_, _) => await RenameSelectedAsync(listViewWorldPacks, WorldDatapacksPath);
            btnDatapackManagementOpenFolden.Click += (_, _)      => OpenFolder(WorldDatapacksPath, create: true);
            btnDatapackManagementSendInDB.Click  += async (_, _) => await SendPacksAsync(listViewWorldPacks, WorldDatapacksPath, PackDbPath);

            // Groupe Pack DB — base de données locale
            btnDatapackDBDelete.Click      += async (_, _) => await DeleteSelectedAsync(listViewPackDB, PackDbPath);
            btnDatapackDBRename.Click      += async (_, _) => await RenameSelectedAsync(listViewPackDB, PackDbPath);
            btnDatapackDBOpenFolder.Click  += (_, _)       => OpenFolder(PackDbPath, create: true);
            btnDatapackDBSendInWorld.Click += async (_, _) => await SendPacksAsync(listViewPackDB, PackDbPath, WorldDatapacksPath);

            RefreshDatapackLists();
        }

        /// <summary>
        /// Rafraîchit les deux ListViews en relisant les dossiers depuis le disque.
        /// À appeler après toute modification (suppression, renommage, envoi).
        /// </summary>
        private void RefreshDatapackLists()
        {
            RefreshListView(listViewWorldPacks, WorldDatapacksPath);
            RefreshListView(listViewPackDB,     PackDbPath);
        }

        /// <summary>
        /// Remplit une ListView avec les sous-dossiers d'un répertoire, triés par nom.
        /// </summary>
        private static void RefreshListView(ListView lv, string folderPath)
        {
            lv.Items.Clear();
            if (!Directory.Exists(folderPath)) return;

            foreach (string dir in Directory.GetDirectories(folderPath).OrderBy(Path.GetFileName))
                lv.Items.Add(new ListViewItem(Path.GetFileName(dir)!));
        }

        /// <summary>
        /// Supprime définitivement les dossiers sélectionnés après confirmation de l'utilisateur.
        /// </summary>
        private async Task DeleteSelectedAsync(ListView lv, string rootPath)
        {
            var selected = lv.SelectedItems.Cast<ListViewItem>().ToList();
            if (selected.Count == 0)
            {
                MessageBox.Show("Sélectionnez au moins un datapack.", "Aucune sélection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string names = string.Join("\n  • ", selected.Select(i => i.Text));
            if (MessageBox.Show($"Supprimer définitivement :\n  • {names}\n\nCette action est irréversible.",
                "Confirmer la suppression", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            var errors = new List<string>();
            foreach (var item in selected)
            {
                try   { await Task.Run(() => Directory.Delete(Path.Combine(rootPath, item.Text), recursive: true)); }
                catch (Exception ex) { errors.Add($"  • {item.Text} : {ex.Message}"); }
            }

            if (errors.Count > 0)
                MessageBox.Show($"Erreur(s) lors de la suppression :\n{string.Join("\n", errors)}",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            RefreshDatapackLists();
        }

        /// <summary>
        /// Renomme chaque dossier sélectionné via une boîte de dialogue individuelle
        /// pré-remplie avec le nom courant.
        /// </summary>
        private async Task RenameSelectedAsync(ListView lv, string rootPath)
        {
            var selected = lv.SelectedItems.Cast<ListViewItem>().ToList();
            if (selected.Count == 0)
            {
                MessageBox.Show("Sélectionnez au moins un datapack.", "Aucune sélection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool anyChange = false;

            foreach (var item in selected)
            {
                string oldName = item.Text;
                string? newName = ShowInputDialog("Renommer", $"Nouveau nom pour « {oldName} » :", oldName);

                // Annulé, inchangé ou vide → on passe au suivant
                if (string.IsNullOrWhiteSpace(newName) || newName == oldName) continue;

                if (newName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    MessageBox.Show($"« {newName} » contient des caractères invalides.",
                        "Nom invalide", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }

                string newPath = Path.Combine(rootPath, newName);
                if (Directory.Exists(newPath))
                {
                    MessageBox.Show($"Un dossier nommé « {newName} » existe déjà.",
                        "Conflit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }

                try
                {
                    await Task.Run(() => Directory.Move(Path.Combine(rootPath, oldName), newPath));
                    anyChange = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors du renommage de « {oldName} » :\n{ex.Message}",
                        "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (anyChange) RefreshDatapackLists();
        }

        /// <summary>
        /// Copie les dossiers sélectionnés de sourceRoot vers destRoot.
        /// Propose d'écraser si un dossier de même nom existe déjà à destination.
        /// </summary>
        private async Task SendPacksAsync(ListView lv, string sourceRoot, string destRoot)
        {
            var selected = lv.SelectedItems.Cast<ListViewItem>().ToList();
            if (selected.Count == 0)
            {
                MessageBox.Show("Sélectionnez au moins un datapack.", "Aucune sélection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Directory.CreateDirectory(destRoot);
            var errors = new List<string>();

            foreach (var item in selected)
            {
                string packName = item.Text;
                string srcPath  = Path.Combine(sourceRoot, packName);
                string dstPath  = Path.Combine(destRoot,   packName);

                if (Directory.Exists(dstPath))
                {
                    if (MessageBox.Show($"« {packName} » existe déjà dans la destination.\nÉcraser ?",
                        "Conflit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        continue;

                    try   { await Task.Run(() => Directory.Delete(dstPath, recursive: true)); }
                    catch (Exception ex) { errors.Add($"  • {packName} (suppression préalable) : {ex.Message}"); continue; }
                }

                try   { await Task.Run(() => CopyDirectory(srcPath, dstPath)); }
                catch (Exception ex) { errors.Add($"  • {packName} : {ex.Message}"); }
            }

            if (errors.Count > 0)
                MessageBox.Show($"Erreur(s) lors de l'envoi :\n{string.Join("\n", errors)}",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            RefreshDatapackLists();
        }

        /// <summary>Ouvre un dossier dans l'Explorateur Windows. Le crée si demandé et absent.</summary>
        private static void OpenFolder(string path, bool create = false)
        {
            if (create) Directory.CreateDirectory(path);
            if (!Directory.Exists(path))
            {
                MessageBox.Show($"Le dossier est introuvable :\n{path}",
                    "Dossier manquant", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        /// <summary>
        /// Affiche une boîte de dialogue de saisie modale stylisée avec le thème de l'application.
        /// Retourne le texte saisi (trimé), ou null si l'utilisateur annule.
        /// </summary>
        private string? ShowInputDialog(string title, string prompt, string defaultValue)
        {
            using var form = new Form
            {
                Text            = title,
                Size            = new Size(380, 152),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition   = FormStartPosition.CenterParent,
                MaximizeBox     = false,
                MinimizeBox     = false
            };

            var label   = new Label  { Text = prompt,        Location = new Point(12, 14), AutoSize = true };
            var textBox = new TextBox { Text = defaultValue,  Location = new Point(12, 38), Width = 344 };
            var btnOk   = new Button  { Text = "OK",          Location = new Point(192, 74), Size = new Size(80, 28), DialogResult = DialogResult.OK };
            var btnAnn  = new Button  { Text = "Annuler",     Location = new Point(278, 74), Size = new Size(80, 28), DialogResult = DialogResult.Cancel };

            form.Controls.AddRange(new Control[] { label, textBox, btnOk, btnAnn });
            form.AcceptButton = btnOk;
            form.CancelButton = btnAnn;

            ThemeManager.Apply(form);
            ThemeManager.ApplyPrimaryButton(btnOk);

            // Sélectionne tout le texte pour faciliter la frappe directe
            form.Shown += (_, _) => { textBox.SelectAll(); textBox.Focus(); };

            return form.ShowDialog(this) == DialogResult.OK ? textBox.Text.Trim() : null;
        }
    }
}