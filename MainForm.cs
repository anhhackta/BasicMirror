using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ScrcpyGUI
{
    public class MainForm : Form
    {
        // Modern Dark Theme Colors
        private readonly Color bgMain = Color.FromArgb(26, 27, 38);
        private readonly Color bgPanel = Color.FromArgb(36, 40, 59);
        private readonly Color bgInput = Color.FromArgb(52, 56, 77);
        private readonly Color accentPrimary = Color.FromArgb(124, 58, 237);
        private readonly Color accentSuccess = Color.FromArgb(20, 184, 166);
        private readonly Color accentWarning = Color.FromArgb(245, 158, 11);
        private readonly Color accentDanger = Color.FromArgb(244, 63, 94);
        private readonly Color textPrimary = Color.FromArgb(248, 250, 252);
        private readonly Color textSecondary = Color.FromArgb(148, 163, 184);

        // Controls
        private ListView lstDevices;
        private ComboBox cmbSize, cmbBitrate, cmbFps;
        private CheckBox chkAudio, chkNoControl, chkStayAwake, chkOnTop, chkBorderless, chkFullscreen;
        private CheckBox chkRecord;
        private TextBox txtRecord;
        private Button btnRecStart, btnRecStop;
        private Label lblStatus;
        private ProgressBar progressBar;
        private Timer refreshTimer;
        private List<SavedDevice> savedDevices;
        private Panel headerPanel;
        private Button btnSettings, btnLang;
        private SettingsForm settingsForm;

        public MainForm()
        {
            InitializeComponent();

            if (!ScrcpyDownloader.IsScrcpyInstalled())
            {
                ShowDownloadPrompt();
            }
            else
            {
                StartApp();
            }
        }

        private void ShowDownloadPrompt()
        {
            lblStatus.Text = Lang.Get("msg_scrcpy_not_found");
            progressBar.Visible = true;
            Application.DoEvents();

            string url = ScrcpyDownloader.GetLatestDownloadUrl();
            if (url != null)
            {
                DialogResult result = MessageBox.Show(
                    Lang.Get("msg_download_prompt"),
                    Lang.Get("msg_download_title"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    lblStatus.Text = Lang.Get("status_downloading", 0);
                    Application.DoEvents();

                    bool success = ScrcpyDownloader.DownloadAndExtract(url, 
                        (progress) =>
                        {
                            if (progress >= 0)
                            {
                                progressBar.Value = progress;
                                lblStatus.Text = Lang.Get("status_downloading", progress);
                            }
                            else
                            {
                                progressBar.Style = ProgressBarStyle.Marquee;
                            }
                            Application.DoEvents();
                        },
                        (status) =>
                        {
                            lblStatus.Text = status;
                            Application.DoEvents();
                        }
                    );

                    if (success)
                    {
                        progressBar.Style = ProgressBarStyle.Blocks;
                        progressBar.Value = 100;
                        lblStatus.Text = Lang.Get("status_download_complete");
                        System.Threading.Thread.Sleep(500);
                        StartApp();
                    }
                }
                else
                {
                    progressBar.Visible = false;
                }
            }
            else
            {
                progressBar.Visible = false;
            }
        }

        private void StartApp()
        {
            progressBar.Visible = false;
            savedDevices = DeviceManager.Load();
            AdbHelper.StartServer();
            RefreshDevices();
            refreshTimer.Start();
            UpdateStatus(Lang.Get("status_ready"), accentSuccess);
        }

        private void InitializeComponent()
        {
            this.Text = Lang.Get("app_title") + " " + Lang.Get("app_version");
            this.Size = new Size(580, 640);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = bgMain;
            this.ForeColor = textPrimary;
            this.Font = new Font("Segoe UI", 9F);

            int y = 0;

            // Header Panel with gradient
            headerPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(580, 55),
                BackColor = Color.Transparent
            };
            headerPanel.Paint += HeaderPanel_Paint;
            this.Controls.Add(headerPanel);

            // Logo icon (drawn)
            var pnlLogo = new Panel
            {
                Location = new Point(15, 10),
                Size = new Size(36, 36),
                BackColor = Color.Transparent
            };
            pnlLogo.Paint += Logo_Paint;
            headerPanel.Controls.Add(pnlLogo);

            var lblTitle = new Label
            {
                Text = Lang.Get("app_title"),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = textPrimary,
                Location = new Point(55, 8),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(lblTitle);

            var lblVersion = new Label
            {
                Text = Lang.Get("app_version"),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(200, 200, 200),
                Location = new Point(180, 14),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(lblVersion);

            // Settings & Language buttons
            btnLang = new Button
            {
                Text = Lang.Current.ToUpper(),
                Location = new Point(460, 12),
                Size = new Size(45, 30),
                BackColor = Color.FromArgb(80, 255, 255, 255),
                ForeColor = textPrimary,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnLang.FlatAppearance.BorderSize = 0;
            btnLang.Click += (s, e) => ToggleLanguage();
            headerPanel.Controls.Add(btnLang);

            btnSettings = new Button
            {
                Text = "⚙",
                Location = new Point(510, 12),
                Size = new Size(45, 30),
                BackColor = Color.FromArgb(80, 255, 255, 255),
                ForeColor = textPrimary,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 14)
            };
            btnSettings.FlatAppearance.BorderSize = 0;
            btnSettings.Click += (s, e) => OpenSettings();
            headerPanel.Controls.Add(btnSettings);

            y = 65;

            // Devices Section
            AddSectionLabel(Lang.Get("section_devices"), 15, y, accentPrimary);
            y += 25;

            lstDevices = new ListView
            {
                Location = new Point(15, y),
                Size = new Size(540, 90),
                BackColor = bgPanel,
                ForeColor = textPrimary,
                BorderStyle = BorderStyle.None,
                View = View.Details,
                FullRowSelect = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable
            };
            lstDevices.Columns.Add("Device", 200);
            lstDevices.Columns.Add("Type", 60);
            lstDevices.Columns.Add("Status", 100);
            lstDevices.Columns.Add("IP", 150);
            this.Controls.Add(lstDevices);
            y += 95;

            // Device buttons
            AddModernButton(Lang.Get("btn_refresh"), 15, y, 80, 28, accentPrimary, RefreshDevices);
            AddModernButton(Lang.Get("btn_mirror"), 100, y, 80, 28, accentSuccess, MirrorSelected);
            AddModernButton(Lang.Get("btn_open_wifi"), 185, y, 120, 28, accentWarning, OpenWiFiPort);
            AddModernButton(Lang.Get("btn_add_ip"), 310, y, 80, 28, bgPanel, AddIPDevice);
            AddModernButton(Lang.Get("btn_delete"), 395, y, 70, 28, accentDanger, DeleteDevice);
            y += 45;

            // Settings Panels
            var pnlVideo = AddModernPanel(15, y, 175, 120);
            AddLabel(Lang.Get("section_video"), 12, 8, accentPrimary, pnlVideo);
            AddLabel(Lang.Get("lbl_size"), 12, 38, textSecondary, pnlVideo);
            cmbSize = AddComboInPanel(75, 35, 85, new[] { "Auto", "800", "1024", "1280", "1600", "1920" }, 3, pnlVideo);
            AddLabel(Lang.Get("lbl_bitrate"), 12, 65, textSecondary, pnlVideo);
            cmbBitrate = AddComboInPanel(75, 62, 65, new[] { "Auto", "4", "8", "16", "32", "64" }, 2, pnlVideo);
            AddLabel("M", 145, 65, textSecondary, pnlVideo);
            AddLabel(Lang.Get("lbl_fps"), 12, 92, textSecondary, pnlVideo);
            cmbFps = AddComboInPanel(75, 89, 65, new[] { "Auto", "30", "60", "90", "120" }, 2, pnlVideo);

            var pnlAudio = AddModernPanel(200, y, 175, 120);
            AddLabel(Lang.Get("section_audio_control"), 12, 8, accentSuccess, pnlAudio);
            chkAudio = AddCheckBoxInPanel(Lang.Get("chk_audio"), 12, 35, true, pnlAudio);
            chkNoControl = AddCheckBoxInPanel(Lang.Get("chk_no_control"), 12, 58, false, pnlAudio);
            chkStayAwake = AddCheckBoxInPanel(Lang.Get("chk_stay_awake"), 12, 81, false, pnlAudio);

            var pnlWindow = AddModernPanel(385, y, 170, 120);
            AddLabel(Lang.Get("section_window"), 12, 8, accentWarning, pnlWindow);
            chkOnTop = AddCheckBoxInPanel(Lang.Get("chk_always_top"), 12, 35, true, pnlWindow);
            chkBorderless = AddCheckBoxInPanel(Lang.Get("chk_borderless"), 12, 58, false, pnlWindow);
            chkFullscreen = AddCheckBoxInPanel(Lang.Get("chk_fullscreen"), 12, 81, false, pnlWindow);
            y += 130;

            // Recording Panel
            var pnlRecord = AddModernPanel(15, y, 540, 75);
            AddLabel(Lang.Get("section_recording"), 12, 8, accentDanger, pnlRecord);
            chkRecord = AddCheckBoxInPanel("Enable Recording", 12, 35, false, pnlRecord);
            AddLabel("Output:", 150, 38, textSecondary, pnlRecord);
            txtRecord = new TextBox
            {
                Location = new Point(200, 35),
                Size = new Size(180, 22),
                Text = "record.mp4",
                BackColor = bgInput,
                ForeColor = textPrimary,
                BorderStyle = BorderStyle.None
            };
            pnlRecord.Controls.Add(txtRecord);

            btnRecStart = new Button
            {
                Text = "▶ " + Lang.Get("btn_rec_start"),
                Location = new Point(390, 32),
                Size = new Size(70, 28),
                BackColor = accentSuccess,
                ForeColor = textPrimary,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnRecStart.FlatAppearance.BorderSize = 0;
            btnRecStart.Click += (s, e) => StartRecording();
            pnlRecord.Controls.Add(btnRecStart);

            btnRecStop = new Button
            {
                Text = "⏹ " + Lang.Get("btn_rec_stop"),
                Location = new Point(465, 32),
                Size = new Size(65, 28),
                BackColor = accentDanger,
                ForeColor = textPrimary,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnRecStop.FlatAppearance.BorderSize = 0;
            btnRecStop.Click += (s, e) => StopRecording();
            pnlRecord.Controls.Add(btnRecStop);

            chkRecord.CheckedChanged += (s, e) => { btnRecStart.Enabled = chkRecord.Checked; };
            y += 85;

            // Running instances info
            var pnlRunning = AddModernPanel(15, y, 540, 45);
            AddLabel("Running: 0 instance(s)", 12, 12, textSecondary, pnlRunning);
            var btnStopAll = new Button
            {
                Text = "Stop All",
                Location = new Point(440, 8),
                Size = new Size(90, 28),
                BackColor = accentDanger,
                ForeColor = textPrimary,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnStopAll.FlatAppearance.BorderSize = 0;
            btnStopAll.Click += (s, e) => StopAll();
            pnlRunning.Controls.Add(btnStopAll);
            y += 55;

            // Progress bar
            progressBar = new ProgressBar
            {
                Location = new Point(15, y),
                Size = new Size(540, 6),
                Style = ProgressBarStyle.Blocks,
                Visible = false
            };
            this.Controls.Add(progressBar);

            // Status
            lblStatus = new Label
            {
                Text = "Initializing...",
                Location = new Point(15, y + 10),
                Size = new Size(400, 20),
                ForeColor = textSecondary
            };
            this.Controls.Add(lblStatus);

            var lblScrcpyVer = new Label
            {
                Text = "scrcpy: checking...",
                Location = new Point(420, y + 10),
                Size = new Size(140, 20),
                ForeColor = textSecondary,
                TextAlign = ContentAlignment.TopRight
            };
            this.Controls.Add(lblScrcpyVer);

            // Timer
            refreshTimer = new Timer { Interval = 5000 };
            refreshTimer.Tick += (s, e) => RefreshDevices();

            // Check scrcpy version after load
            this.Load += (s, e) =>
            {
                var ver = ScrcpyDownloader.GetScrcpyVersion();
                lblScrcpyVer.Text = ver != null ? "scrcpy: v" + ver : "";
            };
        }

        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                headerPanel.ClientRectangle,
                Color.FromArgb(124, 58, 237),
                Color.FromArgb(79, 70, 229),
                LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, headerPanel.ClientRectangle);
            }
        }

        private void Logo_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            
            // Phone outline
            using (var pen = new Pen(textPrimary, 2))
            {
                g.DrawRoundedRectangle(pen, 6, 2, 24, 32, 4);
            }
            
            // Screen
            using (var brush = new SolidBrush(accentSuccess))
            {
                g.FillRectangle(brush, 9, 6, 18, 22);
            }
            
            // Home button
            using (var brush = new SolidBrush(textSecondary))
            {
                g.FillEllipse(brush, 15, 29, 6, 3);
            }
        }

        #region UI Helpers
        private void AddSectionLabel(string text, int x, int y, Color color)
        {
            var lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = color
            };
            this.Controls.Add(lbl);
        }

        private Label AddLabel(string text, int x, int y, Color? color = null, Control parent = null)
        {
            var lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                ForeColor = color ?? textPrimary
            };
            (parent ?? this).Controls.Add(lbl);
            return lbl;
        }

        private Button AddModernButton(string text, int x, int y, int w, int h, Color color, Action click)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(w, h),
                BackColor = color,
                ForeColor = textPrimary,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += (s, e) => click();
            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Light(color, 0.15f);
            btn.MouseLeave += (s, e) => btn.BackColor = color;
            this.Controls.Add(btn);
            return btn;
        }

        private Panel AddModernPanel(int x, int y, int w, int h)
        {
            var pnl = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(w, h),
                BackColor = bgPanel
            };
            this.Controls.Add(pnl);
            return pnl;
        }

        private ComboBox AddComboInPanel(int x, int y, int w, string[] items, int selected, Control parent)
        {
            var cmb = new ComboBox
            {
                Location = new Point(x, y),
                Size = new Size(w, 22),
                BackColor = bgInput,
                ForeColor = textPrimary,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmb.Items.AddRange(items);
            if (selected < cmb.Items.Count) cmb.SelectedIndex = selected;
            parent.Controls.Add(cmb);
            return cmb;
        }

        private CheckBox AddCheckBoxInPanel(string text, int x, int y, bool isChecked, Control parent)
        {
            var chk = new CheckBox
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Checked = isChecked,
                ForeColor = textPrimary
            };
            parent.Controls.Add(chk);
            return chk;
        }

        private void UpdateStatus(string text, Color color)
        {
            lblStatus.Text = text;
            lblStatus.ForeColor = color;
        }
        #endregion

        #region Actions
        private void RefreshDevices()
        {
            lstDevices.Items.Clear();
            
            // USB devices
            var usbDevices = AdbHelper.GetUsbDevices();
            foreach (var serial in usbDevices)
            {
                var name = AdbHelper.GetDeviceName(serial);
                var item = new ListViewItem(name);
                item.SubItems.Add(Lang.Get("device_usb"));
                item.SubItems.Add("Connected");
                item.SubItems.Add(serial);
                item.Tag = new DeviceInfo { Serial = serial, Name = name, IsUsb = true };
                lstDevices.Items.Add(item);
            }

            // Saved WiFi devices
            foreach (var sd in savedDevices)
            {
                var item = new ListViewItem(sd.Name);
                item.SubItems.Add(Lang.Get("device_wifi"));
                item.SubItems.Add("Saved");
                item.SubItems.Add(sd.IP);
                item.Tag = new DeviceInfo { IP = sd.IP, Name = sd.Name, IsUsb = false };
                lstDevices.Items.Add(item);
            }

            UpdateStatus(Lang.Get("status_found_devices", lstDevices.Items.Count), textSecondary);
        }

        private void MirrorSelected()
        {
            if (lstDevices.SelectedItems.Count == 0)
            {
                MessageBox.Show(Lang.Get("msg_select_device"), Lang.Get("msg_info"));
                return;
            }

            var item = lstDevices.SelectedItems[0];
            var device = (DeviceInfo)item.Tag;
            var settings = GetSettings();

            if (device.IsUsb)
            {
                settings.Serial = device.Serial;
                settings.Title = device.Name;
            }
            else
            {
                settings.UseTcpIP = true;
                settings.IP = device.IP;
                var ping = GetPing(device.IP);
                settings.Title = ping > 0 ? device.Name + " (" + ping + "ms)" : device.Name;
            }

            try
            {
                ScrcpyLauncher.Launch(settings);
                UpdateStatus(Lang.Get("status_mirroring", device.Name), accentSuccess);
                if (settings.Record)
                {
                    btnRecStop.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Lang.Get("msg_error"));
            }
        }

        private void OpenWiFiPort()
        {
            if (lstDevices.SelectedItems.Count == 0)
            {
                MessageBox.Show(Lang.Get("msg_select_device"), Lang.Get("msg_info"));
                return;
            }

            var item = lstDevices.SelectedItems[0];
            var device = (DeviceInfo)item.Tag;
            if (!device.IsUsb)
            {
                MessageBox.Show("Select a USB device!", Lang.Get("msg_info"));
                return;
            }

            UpdateStatus(Lang.Get("status_opening_port"), accentWarning);
            Application.DoEvents();

            AdbHelper.EnableTcpIP(device.Serial);
            var ip = AdbHelper.GetDeviceIP(device.Serial);

            if (!string.IsNullOrEmpty(ip))
            {
                // Save this device
                DeviceManager.AddDevice(device.Name, ip, savedDevices);
                RefreshDevices();
                UpdateStatus(Lang.Get("status_port_opened", ip), accentSuccess);
            }
            else
            {
                UpdateStatus(Lang.Get("msg_cannot_get_ip"), accentDanger);
            }
        }

        private void AddIPDevice()
        {
            var ip = ShowInputDialog(Lang.Get("lbl_ip"), Lang.Get("btn_add_ip"), "192.168.1.");
            if (string.IsNullOrEmpty(ip)) return;

            var name = ShowInputDialog(Lang.Get("lbl_device_name"), Lang.Get("btn_add_ip"), "Device");
            if (string.IsNullOrEmpty(name)) return;

            DeviceManager.AddDevice(name, ip, savedDevices);
            RefreshDevices();
            UpdateStatus(Lang.Get("status_saved", name), accentSuccess);
        }

        private void DeleteDevice()
        {
            if (lstDevices.SelectedItems.Count == 0) return;

            var item = lstDevices.SelectedItems[0];
            var device = (DeviceInfo)item.Tag;

            if (!device.IsUsb)
            {
                int idx = savedDevices.FindIndex(d => d.IP == device.IP);
                if (idx >= 0)
                {
                    DeviceManager.RemoveDevice(idx, savedDevices);
                    RefreshDevices();
                    UpdateStatus(Lang.Get("status_deleted"), accentWarning);
                }
            }
        }

        private ScrcpySettings GetSettings()
        {
            return new ScrcpySettings
            {
                MaxSize = cmbSize.Text == "Auto" ? "" : cmbSize.Text,
                Bitrate = cmbBitrate.Text == "Auto" ? "" : cmbBitrate.Text,
                Fps = cmbFps.Text == "Auto" ? "" : cmbFps.Text,
                Audio = chkAudio.Checked,
                NoControl = chkNoControl.Checked,
                StayAwake = chkStayAwake.Checked,
                AlwaysOnTop = chkOnTop.Checked,
                Borderless = chkBorderless.Checked,
                Fullscreen = chkFullscreen.Checked,
                Record = chkRecord.Checked,
                RecordPath = txtRecord.Text
            };
        }

        private void StartRecording()
        {
            if (lstDevices.SelectedItems.Count == 0)
            {
                MessageBox.Show(Lang.Get("msg_select_device"), Lang.Get("msg_info"));
                return;
            }

            chkRecord.Checked = true;
            MirrorSelected();
        }

        private void StopRecording()
        {
            ScrcpyLauncher.StopAll();
            btnRecStop.Enabled = false;
            UpdateStatus("Recording stopped", accentWarning);
        }

        private void StopAll()
        {
            ScrcpyLauncher.StopAll();
            UpdateStatus("All instances stopped", accentWarning);
        }

        private int GetPing(string ip)
        {
            try
            {
                var ping = new Ping();
                var reply = ping.Send(ip, 1000);
                if (reply.Status == IPStatus.Success)
                    return (int)reply.RoundtripTime;
            }
            catch { }
            return 0;
        }

        private void ToggleLanguage()
        {
            Lang.Current = Lang.Current == "en" ? "vi" : "en";
            btnLang.Text = Lang.Current.ToUpper();
            // Reload form to apply new language
            MessageBox.Show("Language changed! Restart app to apply.", Lang.Get("msg_info"));
        }

        private void OpenSettings()
        {
            settingsForm = new SettingsForm();
            settingsForm.LanguageChanged += (s, e) =>
            {
                btnLang.Text = Lang.Current.ToUpper();
            };
            settingsForm.ShowDialog(this);
        }

        private string ShowInputDialog(string prompt, string title, string defaultValue)
        {
            Form inputBox = new Form();
            inputBox.Width = 350;
            inputBox.Height = 160;
            inputBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputBox.Text = title;
            inputBox.StartPosition = FormStartPosition.CenterParent;
            inputBox.BackColor = bgMain;
            inputBox.ForeColor = textPrimary;

            Label label = new Label() { Left = 20, Top = 20, Text = prompt, AutoSize = true };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 290, Text = defaultValue, BackColor = bgInput, ForeColor = textPrimary, BorderStyle = BorderStyle.None };
            Button btnOk = new Button() { Text = Lang.Get("btn_ok"), Left = 130, Width = 90, Top = 90, BackColor = accentPrimary, ForeColor = textPrimary, FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.OK };
            btnOk.FlatAppearance.BorderSize = 0;

            inputBox.Controls.Add(label);
            inputBox.Controls.Add(textBox);
            inputBox.Controls.Add(btnOk);
            inputBox.AcceptButton = btnOk;

            return inputBox.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
        #endregion

        private class DeviceInfo
        {
            public string Serial { get; set; }
            public string IP { get; set; }
            public string Name { get; set; }
            public bool IsUsb { get; set; }
        }
    }

    public static class GraphicsExtensions
    {
        public static void DrawRoundedRectangle(this Graphics g, Pen pen, int x, int y, int width, int height, int radius)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(x, y, radius * 2, radius * 2, 180, 90);
                path.AddArc(x + width - radius * 2, y, radius * 2, radius * 2, 270, 90);
                path.AddArc(x + width - radius * 2, y + height - radius * 2, radius * 2, radius * 2, 0, 90);
                path.AddArc(x, y + height - radius * 2, radius * 2, radius * 2, 90, 90);
                path.CloseFigure();
                g.DrawPath(pen, path);
            }
        }
    }
}
