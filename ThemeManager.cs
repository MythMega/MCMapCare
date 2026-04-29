using System.Runtime.InteropServices;

namespace MCMapCare
{
    /// <summary>
    /// Gestionnaire de thème sombre et flat design pour l'application.
    /// S'applique à un formulaire et à tous ses contrôles récursivement,
    /// sans aucune dépendance externe.
    /// </summary>
    public static class ThemeManager
    {
        // ===========================
        // Palette de couleurs
        // ===========================

        /// <summary>Fond principal des formulaires.</summary>
        public static readonly Color BackgroundDeep = Color.FromArgb(13, 13, 15);

        /// <summary>Fond des panneaux, onglets et groupes.</summary>
        public static readonly Color BackgroundSurface = Color.FromArgb(22, 22, 26);

        /// <summary>Fond des contrôles interactifs (boutons, grilles).</summary>
        public static readonly Color BackgroundControl = Color.FromArgb(36, 36, 42);

        /// <summary>Fond légèrement plus clair pour les en-têtes.</summary>
        public static readonly Color BackgroundElevated = Color.FromArgb(46, 46, 54);

        /// <summary>Couleur d'accentuation principale (boutons primaires, onglet actif).</summary>
        public static readonly Color Accent = Color.FromArgb(88, 130, 255);

        /// <summary>Variante sombre de l'accentuation.</summary>
        public static readonly Color AccentDark = Color.FromArgb(60, 100, 215);

        /// <summary>Texte principal.</summary>
        public static readonly Color TextPrimary = Color.FromArgb(218, 218, 224);

        /// <summary>Texte secondaire (labels, en-têtes de grille).</summary>
        public static readonly Color TextSecondary = Color.FromArgb(120, 120, 138);

        /// <summary>Couleur des bordures et séparateurs.</summary>
        public static readonly Color Border = Color.FromArgb(50, 50, 60);

        /// <summary>Couleur de succès (fusion terminée).</summary>
        public static readonly Color Success = Color.FromArgb(72, 199, 116);

        /// <summary>Couleur d'erreur.</summary>
        public static readonly Color Error = Color.FromArgb(240, 78, 84);

        /// <summary>Fond de la sélection dans les listes et grilles.</summary>
        public static readonly Color SelectionBackground = Color.FromArgb(55, 98, 200);

        // ===========================
        // Win32 — couleur de la barre de progression
        // ===========================
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private const int PBM_SETBARCOLOR = 0x0409;
        private const int PBM_SETBKCOLOR = 0x2001;

        // ===========================
        // Application du thème
        // ===========================

        /// <summary>
        /// Applique le thème sombre à un formulaire et à tous ses contrôles enfants.
        /// À appeler dans l'événement Load du formulaire.
        /// </summary>
        public static void Apply(Form form)
        {
            form.BackColor = BackgroundDeep;
            form.ForeColor = TextPrimary;

            ApplyToControls(form.Controls);
        }

        /// <summary>
        /// Applique le style de bouton primaire (fond accentuation) à un bouton donné.
        /// À appeler après Apply() pour les boutons d'action principale.
        /// </summary>
        public static void ApplyPrimaryButton(Button btn)
        {
            btn.BackColor = Accent;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.BorderColor = AccentDark;
            btn.Cursor = Cursors.Hand;
        }

        // ===========================
        // Récursion sur les contrôles
        // ===========================

        private static void ApplyToControls(Control.ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                ApplyToControl(c);
                if (c.Controls.Count > 0)
                    ApplyToControls(c.Controls);
            }
        }

        private static void ApplyToControl(Control c)
        {
            switch (c)
            {
                case Label lbl:
                    lbl.ForeColor = TextPrimary;
                    lbl.BackColor = Color.Transparent;
                    break;

                case Button btn:
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderColor = Border;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.BackColor = BackgroundControl;
                    btn.ForeColor = TextPrimary;
                    btn.Cursor = Cursors.Hand;
                    btn.UseVisualStyleBackColor = false;
                    break;

                case CheckBox chk:
                    chk.FlatStyle = FlatStyle.Flat;
                    // FlatAppearance.BorderColor n'accepte pas Transparent sur ButtonBase —
                    // on utilise la couleur de fond pour simuler l'absence de bordure système.
                    // Le dessin réel est entièrement géré par PaintCheckBox.
                    chk.FlatAppearance.BorderColor = BackgroundSurface;
                    chk.FlatAppearance.CheckedBackColor = BackgroundSurface;
                    chk.FlatAppearance.MouseOverBackColor = BackgroundSurface;
                    chk.FlatAppearance.MouseDownBackColor = BackgroundSurface;
                    chk.BackColor = Color.Transparent;
                    chk.ForeColor = TextPrimary;
                    // Abonnement idempotent (évite les doublons si Apply est rappelé)
                    chk.Paint -= PaintCheckBox;
                    chk.Paint += PaintCheckBox;
                    // Force le redessin lors du clic (état Checked change)
                    chk.CheckedChanged -= InvalidateCheckBox;
                    chk.CheckedChanged += InvalidateCheckBox;
                    chk.Width += 5;
                    break;

                case GroupBox grp:
                    grp.BackColor = BackgroundSurface;
                    grp.ForeColor = TextSecondary;
                    break;

                case ListView list:
                    list.BackColor = BackgroundSurface;
                    list.ForeColor = TextSecondary;
                    break;

                case TabPage tp:
                    tp.BackColor = BackgroundSurface;
                    tp.ForeColor = TextPrimary;
                    break;

                case TableLayoutPanel tlp:
                    tlp.BackColor = BackgroundSurface;
                    break;

                case Panel pnl:
                    pnl.BackColor = BackgroundSurface;
                    break;

                case TabControl tc:
                    ApplyToTabControl(tc);
                    break;

                case ProgressBar pb:
                    pb.Style = ProgressBarStyle.Continuous;
                    pb.BackColor = BackgroundControl;
                    pb.ForeColor = Accent;
                    // Force la couleur via Win32 (les visual styles l'ignorent sinon)
                    pb.HandleCreated += (_, _) => SetProgressBarColor(pb);
                    if (pb.IsHandleCreated) SetProgressBarColor(pb);
                    break;

                case DataGridView dgv:
                    ApplyToDataGridView(dgv);
                    break;

                case CheckedListBox clb:
                    clb.BackColor = BackgroundControl;
                    clb.ForeColor = TextPrimary;
                    clb.BorderStyle = BorderStyle.None;
                    break;

                case ListBox lb:
                    lb.BackColor = BackgroundControl;
                    lb.ForeColor = TextPrimary;
                    lb.BorderStyle = BorderStyle.None;
                    break;

                case MenuStrip ms:
                    ms.BackColor = BackgroundElevated;
                    ms.ForeColor = TextPrimary;
                    ms.Renderer = new DarkMenuRenderer();
                    ApplyToToolStripItems(ms.Items);
                    break;

                case TextBox tb:
                    tb.BackColor = BackgroundControl;
                    tb.ForeColor = TextPrimary;
                    tb.BorderStyle = BorderStyle.FixedSingle;
                    break;

                case ComboBox cb:
                    cb.BackColor = BackgroundControl;
                    cb.ForeColor = TextPrimary;
                    cb.FlatStyle = FlatStyle.Flat;
                    break;
            }
        }

        // ===========================
        // TabControl — dessin personnalisé
        // ===========================

        private static void ApplyToTabControl(TabControl tc)
        {
            tc.BackColor = BackgroundSurface;
            tc.ForeColor = TextPrimary;
            tc.DrawMode = TabDrawMode.OwnerDrawFixed;
            tc.SizeMode = TabSizeMode.Fixed;
            tc.ItemSize = new Size(130, 34);
            tc.Padding = new Point(0, 0);

            // Supprime le listener précédent si Apply est appelé plusieurs fois
            tc.DrawItem -= DrawTabItem;
            tc.DrawItem += DrawTabItem;
        }

        private static void DrawTabItem(object? sender, DrawItemEventArgs e)
        {
            if (sender is not TabControl tc) return;

            bool isSelected = e.Index == tc.SelectedIndex;

            // Fond de l'onglet
            Color bgColor = isSelected ? BackgroundSurface : BackgroundDeep;
            using var bgBrush = new SolidBrush(bgColor);
            e.Graphics.FillRectangle(bgBrush, e.Bounds);

            // Barre d'accent en bas de l'onglet actif
            if (isSelected)
            {
                using var accentBrush = new SolidBrush(Accent);
                e.Graphics.FillRectangle(accentBrush,
                    e.Bounds.Left, e.Bounds.Bottom - 3, e.Bounds.Width, 3);
            }

            // Texte de l'onglet
            string title = tc.TabPages[e.Index].Text;
            Color fgColor = isSelected ? TextPrimary : TextSecondary;
            using var font = new Font("Segoe UI", 9.5F,
                isSelected ? FontStyle.Bold : FontStyle.Regular);
            using var fgBrush = new SolidBrush(fgColor);

            var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            e.Graphics.DrawString(title, font, fgBrush,
                new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height - 3), sf);
        }

        // ===========================
        // DataGridView
        // ===========================

        private static void ApplyToDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = BackgroundSurface;
            dgv.GridColor = Border;
            dgv.BorderStyle = BorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;

            // Cellules normales
            dgv.DefaultCellStyle.BackColor = BackgroundSurface;
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.SelectionBackColor = SelectionBackground;
            dgv.DefaultCellStyle.SelectionForeColor = TextPrimary;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9F);

            // Lignes alternées (zèbre) pour la lisibilité
            dgv.AlternatingRowsDefaultCellStyle.BackColor = BackgroundControl;
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = TextPrimary;
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = SelectionBackground;
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = TextPrimary;

            // En-têtes de colonnes
            dgv.ColumnHeadersDefaultCellStyle.BackColor = BackgroundElevated;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = TextSecondary;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = BackgroundElevated;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // En-têtes de lignes
            dgv.RowHeadersDefaultCellStyle.BackColor = BackgroundElevated;
            dgv.RowHeadersDefaultCellStyle.ForeColor = TextSecondary;
            dgv.RowHeadersDefaultCellStyle.SelectionBackColor = BackgroundElevated;
        }

        // ===========================
        // MenuStrip
        // ===========================

        private static void ApplyToToolStripItems(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                item.BackColor = BackgroundElevated;
                item.ForeColor = TextPrimary;
                if (item is ToolStripMenuItem mi && mi.DropDownItems.Count > 0)
                    ApplyToToolStripItems(mi.DropDownItems);
            }
        }

        // ===========================
        // ProgressBar — couleur Win32
        // ===========================

        private static void SetProgressBarColor(ProgressBar pb)
        {
            SendMessage(pb.Handle, PBM_SETBARCOLOR, IntPtr.Zero,
                (IntPtr)ColorTranslator.ToWin32(Accent));
            SendMessage(pb.Handle, PBM_SETBKCOLOR, IntPtr.Zero,
                (IntPtr)ColorTranslator.ToWin32(BackgroundControl));
        }

        // ===========================
        // CheckBox — dessin personnalisé complet
        // ===========================

        /// <summary>
        /// Dessine entièrement une CheckBox : fond, case, coche et texte.
        /// Remplace le rendu système pour garantir un contraste optimal sur fond sombre.
        /// </summary>
        private static void PaintCheckBox(object? sender, PaintEventArgs e)
        {
            if (sender is not CheckBox chk) return;

            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Efface le fond avec la couleur du parent pour masquer le rendu système résiduel
            Color parentBg = GetEffectiveBackColor(chk);
            using (var bgBrush = new SolidBrush(parentBg))
                g.FillRectangle(bgBrush, chk.ClientRectangle);

            // Dimensions et position de la case à cocher
            const int BoxSize = 16;
            int boxX = 1;
            int boxY = (chk.Height - BoxSize) / 2;
            var boxRect = new RectangleF(boxX + 0.5f, boxY + 0.5f, BoxSize - 1, BoxSize - 1);

            // Fond de la case : couleur d'accentuation si coché, sinon contrôle sombre
            Color boxBg = chk.Checked ? Accent : BackgroundControl;
            using (var boxBrush = new SolidBrush(boxBg))
                g.FillRectangle(boxBrush, boxX, boxY, BoxSize, BoxSize);

            // Bordure de la case : accent si coché, sinon bordure standard
            Color borderColor = chk.Checked ? Accent : Border;
            using (var borderPen = new Pen(borderColor, 1.5f))
                g.DrawRectangle(borderPen, boxRect.X, boxRect.Y, boxRect.Width, boxRect.Height);

            // Coche blanche (toujours visible sur fond accent)
            if (chk.Checked)
            {
                using var checkPen = new Pen(Color.White, 2.2f)
                {
                    StartCap = System.Drawing.Drawing2D.LineCap.Round,
                    EndCap = System.Drawing.Drawing2D.LineCap.Round,
                    LineJoin = System.Drawing.Drawing2D.LineJoin.Round
                };

                // Trois points formant la coche en V
                var pts = new PointF[]
                {
                    new(boxX + 3f,              boxY + BoxSize / 2f),
                    new(boxX + BoxSize / 2f - 1, boxY + BoxSize - 4f),
                    new(boxX + BoxSize - 3f,    boxY + 3.5f)
                };
                g.DrawLines(checkPen, pts);
            }

            // Texte du label à droite de la case
            var textRect = new Rectangle(boxX + BoxSize + 5, 0,
                chk.Width - BoxSize - boxX - 5, chk.Height);
            TextRenderer.DrawText(g, chk.Text, chk.Font, textRect,
                chk.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.SingleLine);
        }

        /// <summary>
        /// Force le redessin de la CheckBox lors d'un changement d'état (cochée / décochée).
        /// </summary>
        private static void InvalidateCheckBox(object? sender, EventArgs e)
            => (sender as CheckBox)?.Invalidate();

        /// <summary>
        /// Remonte la hiérarchie des parents pour trouver la première couleur de fond non transparente.
        /// Utilisé pour effacer le fond des CheckBox transparentes.
        /// </summary>
        private static Color GetEffectiveBackColor(Control control)
        {
            Control? current = control.Parent;
            while (current != null)
            {
                if (current.BackColor != Color.Transparent)
                    return current.BackColor;
                current = current.Parent;
            }
            return BackgroundSurface;
        }
    }

    // ===========================
    // Renderer sombre pour MenuStrip
    // ===========================

    /// <summary>
    /// Renderer de menu sombre utilisé par le MenuStrip du formulaire Load.
    /// </summary>
    internal class DarkMenuRenderer : ToolStripProfessionalRenderer
    {
        public DarkMenuRenderer() : base(new DarkColorTable())
        {
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = ThemeManager.TextPrimary;
            base.OnRenderItemText(e);
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.ArrowColor = ThemeManager.TextSecondary;
            base.OnRenderArrow(e);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            // Fond de l'élément survolé
            Color bg = e.Item.Selected
                ? ThemeManager.BackgroundControl
                : ThemeManager.BackgroundElevated;
            using var brush = new SolidBrush(bg);
            e.Graphics.FillRectangle(brush, new Rectangle(Point.Empty, e.Item.Size));
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            using var brush = new SolidBrush(ThemeManager.BackgroundElevated);
            e.Graphics.FillRectangle(brush, e.AffectedBounds);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            int y = e.Item.Height / 2;
            using var pen = new Pen(ThemeManager.Border);
            e.Graphics.DrawLine(pen, 0, y, e.Item.Width, y);
        }
    }

    /// <summary>
    /// Table de couleurs sombres pour ToolStripProfessionalRenderer.
    /// </summary>
    internal class DarkColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected => ThemeManager.BackgroundControl;
        public override Color MenuItemSelectedGradientBegin => ThemeManager.BackgroundControl;
        public override Color MenuItemSelectedGradientEnd => ThemeManager.BackgroundControl;
        public override Color MenuItemPressedGradientBegin => ThemeManager.BackgroundElevated;
        public override Color MenuItemPressedGradientEnd => ThemeManager.BackgroundElevated;
        public override Color MenuBorder => ThemeManager.Border;
        public override Color MenuItemBorder => ThemeManager.Border;
        public override Color ToolStripDropDownBackground => ThemeManager.BackgroundSurface;
        public override Color ImageMarginGradientBegin => ThemeManager.BackgroundSurface;
        public override Color ImageMarginGradientMiddle => ThemeManager.BackgroundSurface;
        public override Color ImageMarginGradientEnd => ThemeManager.BackgroundSurface;
        public override Color ToolStripBorder => ThemeManager.Border;
        public override Color ToolStripContentPanelGradientBegin => ThemeManager.BackgroundSurface;
        public override Color ToolStripContentPanelGradientEnd => ThemeManager.BackgroundSurface;
        public override Color SeparatorDark => ThemeManager.Border;
        public override Color SeparatorLight => ThemeManager.BackgroundControl;
        public override Color ToolStripGradientBegin => ThemeManager.BackgroundElevated;
        public override Color ToolStripGradientMiddle => ThemeManager.BackgroundElevated;
        public override Color ToolStripGradientEnd => ThemeManager.BackgroundElevated;
        public override Color CheckBackground => ThemeManager.Accent;
        public override Color CheckPressedBackground => ThemeManager.AccentDark;
        public override Color CheckSelectedBackground => ThemeManager.Accent;
        public override Color ButtonCheckedGradientBegin => ThemeManager.SelectionBackground;
        public override Color ButtonCheckedGradientEnd => ThemeManager.SelectionBackground;
    }
}