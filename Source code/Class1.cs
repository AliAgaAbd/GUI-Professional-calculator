using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
namespace Professional_calculator
{
    public class SettingsForm : Form
    {
        private Form1 mainForm;
        private TrackBar opacityTrackBar;
        private CheckBox alwaysOnTopCheckBox;
        private Button backgroundColorButton;
        private Button foregroundColorButton;
        private Label opacityLabel;
        private Label currentOpacityLabel;
        private Dictionary<string, string> settings;
        public SettingsForm(Form1 parentForm)
        {
            mainForm = parentForm;
            LoadSettings();
            InitializeComponents();
        }
        private void LoadSettings()
        {
            try
            {
                if (File.Exists("settings.json"))
                {
                    string json = File.ReadAllText("settings.json");
                    settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                }
                else
                {
                    settings = new Dictionary<string, string>
                    {
                        { "Opacity", "1.0" },
                        { "TopMost", "False" },
                        { "BackgroundColor", ColorTranslator.ToHtml(SystemColors.Control) },
                        { "ForegroundColor", ColorTranslator.ToHtml(Color.Black) }
                    };
                    SaveSettings();
                }
            }
            catch
            {
                settings = new Dictionary<string, string>
                {
                    { "Opacity", "1.0" },
                    { "TopMost", "False" },
                    { "BackgroundColor", ColorTranslator.ToHtml(SystemColors.Control) },
                    { "ForegroundColor", ColorTranslator.ToHtml(Color.Black) }
                };
                SaveSettings();
            }
        }
        private void SaveSettings()
        {
            try
            {
                string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText("settings.json", json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در ذخیره تنظیمات: " + ex.Message);
            }
        }
        private void InitializeComponents()
        {
            this.Text = "تنظیمات";
            this.Size = new Size(400, 300);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            opacityLabel = new Label
            {
                Text = "شفافیت فرم:",
                Location = new Point(20, 20),
                Font = new Font("Arial", 12),
                Size = new Size(100, 30),
                RightToLeft = RightToLeft.Yes
            };
            currentOpacityLabel = new Label
            {
                Text = $"{(int)(mainForm.Opacity * 100)}%",
                Location = new Point(320, 20),
                Font = new Font("Arial", 12),
                Size = new Size(50, 30),
                RightToLeft = RightToLeft.Yes
            };
            opacityTrackBar = new TrackBar
            {
                Location = new Point(120, 20),
                Size = new Size(190, 30),
                Minimum = 20,
                Maximum = 100,
                Value = (int)(mainForm.Opacity * 100),
                TickFrequency = 10
            };
            opacityTrackBar.Scroll += OpacityTrackBar_Scroll;
            alwaysOnTopCheckBox = new CheckBox
            {
                Text = "همیشه بالای پنجره‌های دیگر",
                Location = new Point(20, 70),
                Font = new Font("Arial", 12),
                Size = new Size(250, 30),
                RightToLeft = RightToLeft.Yes,
                Checked = mainForm.TopMost
            };
            alwaysOnTopCheckBox.CheckedChanged += AlwaysOnTopCheckBox_CheckedChanged;
            backgroundColorButton = new Button
            {
                Text = "انتخاب رنگ پس‌زمینه",
                Location = new Point(20, 120),
                Size = new Size(150, 40),
                Font = new Font("Arial", 12),
                RightToLeft = RightToLeft.Yes
            };
            backgroundColorButton.Click += BackgroundColorButton_Click;
            backgroundColorButton.MouseEnter += MouseHand;
            backgroundColorButton.MouseLeave += MouseDefualt;
            foregroundColorButton = new Button
            {
                Text = "انتخاب رنگ نوشته‌ها",
                Location = new Point(200, 120),
                Size = new Size(150, 40),
                Font = new Font("Arial", 12),
                RightToLeft = RightToLeft.Yes
            };
            foregroundColorButton.Click += ForegroundColorButton_Click;
            foregroundColorButton.MouseEnter += MouseHand;
            foregroundColorButton.MouseLeave += MouseDefualt;
            this.Controls.Add(opacityLabel);
            this.Controls.Add(currentOpacityLabel);
            this.Controls.Add(opacityTrackBar);
            this.Controls.Add(alwaysOnTopCheckBox);
            this.Controls.Add(backgroundColorButton);
            this.Controls.Add(foregroundColorButton);
        }
        private void OpacityTrackBar_Scroll(object sender, EventArgs e)
        {
            mainForm.Opacity = opacityTrackBar.Value / 100.0;
            currentOpacityLabel.Text = $"{opacityTrackBar.Value}%";
            settings["Opacity"] = mainForm.Opacity.ToString();
            SaveSettings();
        }
        private void AlwaysOnTopCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            mainForm.TopMost = alwaysOnTopCheckBox.Checked;
            settings["TopMost"] = mainForm.TopMost.ToString();
            SaveSettings();
        }
        private void BackgroundColorButton_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    mainForm.BackColor = colorDialog.Color;
                    settings["BackgroundColor"] = colorDialog.Color.ToArgb().ToString();
                    SaveSettings();
                }
            }
        }
        private void ForegroundColorButton_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (Control control in mainForm.Controls)
                    {
                        if (control is Label || control is TextBox || control is Button || control is ComboBox)
                        {
                            control.ForeColor = colorDialog.Color;
                        }
                    }
                    settings["ForegroundColor"] = colorDialog.Color.ToArgb().ToString();
                    SaveSettings();
                }
            }
        }
        private void MouseHand(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }
        private void MouseDefualt(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }
    }
}