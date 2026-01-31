using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ScrcpyGUI
{
    public class SettingsForm : Form
    {
        private readonly Color bgMain = Color.FromArgb(26, 27, 38);
        private readonly Color bgPanel = Color.FromArgb(36, 40, 59);
        private readonly Color bgInput = Color.FromArgb(52, 56, 77);
        private readonly Color accentPrimary = Color.FromArgb(124, 58, 237);
        private readonly Color textPrimary = Color.FromArgb(248, 250, 252);
        private readonly Color textSecondary = Color.FromArgb(148, 163, 184);

        private ComboBox cmbLanguage, cmbCodec;
        private CheckBox chkShowTouches, chkTurnScreenOff, chkPowerOffOnClose;
        public event EventHandler LanguageChanged;

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = Lang.Get("settings_title");
            this.Size = new Size(400, 380);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = bgMain;
            this.ForeColor = textPrimary;
            this.Font = new Font("Segoe UI", 9F);

            int y = 20;

            // Language section
            AddLabel(Lang.Get("lbl_language"), 20, y);
            cmbLanguage = new ComboBox
            {
                Location = new Point(150, y - 3),
                Size = new Size(200, 24),
                BackColor = bgInput,
                ForeColor = textPrimary,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var lang in Lang.GetAvailableLanguages())
            {
                cmbLanguage.Items.Add(Lang.GetLanguageName(lang));
            }
            cmbLanguage.SelectedIndex = Lang.Current == "vi" ? 1 : 0;
            this.Controls.Add(cmbLanguage);
            y += 40;

            // Video Codec
            AddLabel(Lang.Get("settings_codec"), 20, y);
            cmbCodec = new ComboBox
            {
                Location = new Point(150, y - 3),
                Size = new Size(200, 24),
                BackColor = bgInput,
                ForeColor = textPrimary,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCodec.Items.AddRange(new[] { "Auto", "h264", "h265", "av1" });
            cmbCodec.SelectedIndex = 0;
            this.Controls.Add(cmbCodec);
            y += 50;

            // Separator
            var sep = new Panel
            {
                Location = new Point(20, y),
                Size = new Size(340, 1),
                BackColor = bgPanel
            };
            this.Controls.Add(sep);
            y += 20;

            // Options
            AddLabel(Lang.Get("settings_advanced"), 20, y);
            y += 30;

            chkShowTouches = AddCheckBox(Lang.Get("chk_show_touches"), 20, y, false);
            y += 28;

            chkTurnScreenOff = AddCheckBox(Lang.Get("chk_turn_screen_off"), 20, y, false);
            y += 28;

            chkPowerOffOnClose = AddCheckBox("Power off on close", 20, y, false);
            y += 50;

            // Buttons
            var btnSave = new Button
            {
                Text = Lang.Get("btn_save"),
                Location = new Point(150, y),
                Size = new Size(100, 32),
                BackColor = accentPrimary,
                ForeColor = textPrimary,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) => SaveSettings();
            this.Controls.Add(btnSave);

            var btnCancel = new Button
            {
                Text = Lang.Get("btn_cancel"),
                Location = new Point(260, y),
                Size = new Size(90, 32),
                BackColor = bgPanel,
                ForeColor = textPrimary,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();
            this.Controls.Add(btnCancel);
        }

        private Label AddLabel(string text, int x, int y)
        {
            var lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                ForeColor = textSecondary
            };
            this.Controls.Add(lbl);
            return lbl;
        }

        private CheckBox AddCheckBox(string text, int x, int y, bool isChecked)
        {
            var chk = new CheckBox
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Checked = isChecked,
                ForeColor = textPrimary
            };
            this.Controls.Add(chk);
            return chk;
        }

        private void SaveSettings()
        {
            string newLang = cmbLanguage.SelectedIndex == 1 ? "vi" : "en";
            if (newLang != Lang.Current)
            {
                Lang.Current = newLang;
                if (LanguageChanged != null)
                    LanguageChanged(this, EventArgs.Empty);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string VideoCodec
        {
            get { return cmbCodec.SelectedIndex == 0 ? "" : cmbCodec.Text; }
        }

        public bool ShowTouches
        {
            get { return chkShowTouches.Checked; }
        }

        public bool TurnScreenOff
        {
            get { return chkTurnScreenOff.Checked; }
        }
    }
}
