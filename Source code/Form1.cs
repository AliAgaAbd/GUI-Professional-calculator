using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Professional_calculator;
using Timer = System.Windows.Forms.Timer;
using Keys = System.Windows.Forms.Keys;
using NAudio;
using Microsoft.Win32;
using System.Security.Policy;
using System.Xml.Linq;
using System.Text.Json;
using OpenQA.Selenium.DevTools.V131.Network;
using System.Diagnostics;
namespace Professional_calculator
{
    public partial class Form1 : Form
    {
        private MusicPlayer musicPlayer;
        public Button soundButton;
        private Button volumeArrowButton;
        private TrackBar volumeTrackBar;
        private bool isVolumeSliderVisible = false;
        private Timer autoPlayTimer;
        private bool isAutoPlayEnabled = true;
        double weightMain = 0;
        double tallMain = 0;
        double mainBMI = 0;
        string AI = "https://chat.deepseek.com/";
        private WebView2 webView;
        private ComboBox ModelSelect2;
        private Button StartAI;
        private Label Connection;
        private Label Connection2;
        private Label help;
        private LibVLC libVLC;
        private MediaPlayer mediaPlayer;
        private Timer timer; 
        private LibVLCSharp.WinForms.VideoView videoView;
        private Label BMIshow;
        private TextBox BMIweight;
        private Label BMIweight2;
        private TextBox BMItall;
        private Label BMItall2;
        private Button BMIbutton;
        private Label BMIshow2;
        private ComboBox cbUnitCategory, cbUnitFrom, cbUnitTo;
        private TextBox txtUnitInput, txtUnitOutput;
        private Button btnConvertUnits;
        private Label lblUnitCategory, lblUnitFrom, lblUnitTo;
        private string currentExpression = "";
        private TextBox txtCalcDisplay;
        private ComboBox cbCalcMode; 
        private TextBox txtNthRootDegree; 
        private Button btnAdd, btnSubtract, btnMultiply, btnDivide, btnEquals, btnClear, btnClearAll, btnPower, btnNthRoot;
        private Button btnSin, btnCos, btnTan, btnLog, btnLn, btnFact, btnPi, btnE, btnPercent;
        private Button btnDerivative, btnIntegral, btnSolveQuad, btnMatrixAdd, btnMatrixMultiply;
        private Button btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9, btnDecimal;
        private Timer Best;
        private Timer ConnectionCheck;
        private static Timer UpdateProgram;
        public Form1()
        {
            InitializeComponent();
            InitializeComponentVideo();
            WEB();
            CalculatorButton.MouseClick += CalculatorButton_MouseClick;
            UnitConversationButton.MouseClick += UnitConversationButton_MouseClick;
            SettingButton.MouseEnter += Mouse_hand;
            SettingButton.MouseLeave += Mouse_leave;
            SettingButton.MouseClick += SettingButton_MouseClick;
            this.Shown += (s, e) => LoadSettingsOnStartup();
            InitializeMusicControls();
            Show();
            MemoryClean();
            Best = new Timer();
            Best.Interval = 10000;
            Best.Start();
            Best.Tick += MemoryClean2;
        }
        private void WEB()
        {
            webView = new WebView2();
            webView.Height = 820;
            webView.Width = 1330;
            webView.Left = 15;
            webView.Top = 60;
            this.Controls.Add(webView);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeAsync();
            LoadSettingsOnStartup();
            InitializeMusicControls();
            Show();
            Best = new Timer();
            Best.Interval = 10000;
            Best.Start();
            Best.Tick += MemoryClean2;
            ConnectionCheck = new Timer();
            ConnectionCheck.Interval = 1000;
            ConnectionCheck.Tick += Ping_Tick;
            CenterToScreen();
        }
        private async void InitializeAsync()
        {
            await webView.EnsureCoreWebView2Async(null);
        }
        private void AIButton_MouseClick(object sender, MouseEventArgs e)
        {
            this.ClientSize = new System.Drawing.Size(1360, 900);
            HomeButton.Location = new Point(1245, 12);
            CenterToScreen();
            SettingButton.Visible = false;
            Connection = new Label();
            Connection.Height = 24;
            Connection.Width = 500;
            Connection.Top = 20;
            Connection.Left = 510;
            Connection.BringToFront();
            Connection.Font = new Font(Connection.Font.FontFamily, 15);
            Connection2 = new Label();
            Connection2.Height = 24;
            Connection2.Width = 250;
            Connection2.Top = 20;
            Connection2.Left = 985;
            Connection2.RightToLeft = RightToLeft.Yes;
            Connection2.Font = new Font(Font.FontFamily, 15);
            CalculatorButton.Visible = false;
            UnitConversationButton.Visible = false;
            BMIButton.Visible = false;
            AIButton.Visible = false;
            label1.Visible = false;
            pictureBox1.Visible = false;
            ModelSelect2 = new ComboBox();
            ModelSelect2.Height = 24;   
            ModelSelect2.Width = 146;
            ModelSelect2.Left = 182;
            ModelSelect2.Top = 22;
            ModelSelect2.Text = "مدل خود را انتخاب کنید";
            ModelSelect2.Items.Add("DeepSeek");
            ModelSelect2.Items.Add("Copilot");
            ModelSelect2.Items.Add("Chat GPT (VPN)");
            ModelSelect2.Items.Add("Grok3 (VPN)");
            ModelSelect2.Items.Add("Duck");
            ModelSelect2.Font = new Font(Font.FontFamily, 12);
            ModelSelect2.Visible = true;
            ModelSelect2.SelectedIndexChanged += ModelSelect2_SelectedIndexChanged;
            ModelSelect2.BringToFront();
            StartAI = new Button();
            StartAI.Text = "اجرا";
            StartAI.Height = 30;
            StartAI.Width = 100;
            StartAI.Top = 22;
            StartAI.Left = 400;
            StartAI.MouseClick += StartAI_MouseClick;
            StartAI.MouseEnter += StartAI_MouseClick2;
            StartAI.MouseLeave += StartAI_MouseClick3;
            StartAI.Font = new Font(Font.FontFamily, 13);
            this.Controls.Add(StartAI);
            this.Controls.Add(ModelSelect2);
            this.Controls.Add(Connection2);
            this.Controls.Add(Connection);
            StartAI.Visible = true;
            ModelSelect2.Visible = true;
            webView.Visible = true;
            Connection.Visible = true;
            Connection2.Visible = true;
            volumeArrowButton.Visible = false;
            volumeTrackBar.Visible = false;
            soundButton.Visible = false;
            Connection.Text = "در حال برسی";
            Connection2.Text = "در حال برسی";
            ConnectionCheck.Start();
        }
        private void ModelSelect2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ModelSelect2.SelectedItem.ToString() == "Copilot")
            {
                AI = "https://copilot.microsoft.com/";
            }
            if (ModelSelect2.SelectedItem.ToString() == "DeepSeek")
            {
                AI = "https://chat.deepseek.com/";
            }
            if (ModelSelect2.SelectedItem.ToString() == "Chat GPT (VPN)")
            {
                AI = "https://chatgpt.com/";
            }
            if (ModelSelect2.SelectedItem.ToString() == "Grok3 (VPN)")
            {
                AI = "https://grok.com/?referrer=website";
            }
            if (ModelSelect2.SelectedItem.ToString() == "Duck")
            {
                AI = "https://duckduckgo.com/?q=DuckDuckGo+AI+Chat&ia=chat&duckai=1";
                MessageBox.Show("این یه مدلی که میتونی باهاش به\nChat GPT\nو چند تا مدل دیگه بدون نیاز به فیلترشکن و ثبت نام دسترسی داشته باشی");
            }
        }
        private void StartAI_MouseClick(object sender, MouseEventArgs e)
        {
            webView.CoreWebView2.Navigate(AI);
        }
        private void HomeButton_MouseClick(object sender, MouseEventArgs e)
        {
            this.ClientSize = new Size(1193, 712);
            HomeButton.Location = new Point(1079, 11);
            CenterToScreen();
            ConnectionCheck.Start();
            ConnectionCheck.Stop();
            foreach (Control control in this.Controls)
            {
                if (control != sender)
                {
                    control.Visible = false;
                }
            }
            HelpButton.Visible = true;
            CalculatorButton.Visible = true;
            UnitConversationButton.Visible = true;
            BMIButton.Visible = true;
            AIButton.Visible = true;
            label1.Visible = true;
            pictureBox1.Visible = true;
            SettingButton.Visible = true;
            soundButton.Visible = true;
            volumeArrowButton.Visible = true;
            volumeArrowButton.Text = "↓";
            volumeTrackBar.Visible = false;
            MemoryClean();
        }
        private void StartAI_MouseClick2(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
            MemoryClean();
        }
        private void StartAI_MouseClick3(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }
        private void Ping_Tick(object sender, EventArgs e)
        {
            StartAI.Enabled = true;
            try
            {
                var ping = new Ping().Send("8.8.8.8");
                if (ping.Status == IPStatus.TimedOut)
                {
                    Connection.Text = "به اینترنت متصل نیستید! برای استفاده لطفا به اینترنت متصل شوید";
                    StartAI.Enabled = false;
                }
                else
                {
                    Connection.Text = "شما به اینترنت متصل هستید";
                }
                if (ping.RoundtripTime <= 80 && ping.RoundtripTime >= 1)
                {
                    Connection2.Text = $"کیفیت اینترنت: خیلی خوب ({ping.RoundtripTime}ms)";
                }
                else if (ping.RoundtripTime > 80 && ping.RoundtripTime < 130)
                {
                    Connection2.Text = $"کیفیت اینترنت: خوب ({ping.RoundtripTime}ms)";
                }
                else if (ping.RoundtripTime >= 130 && ping.RoundtripTime < 170)
                {
                    Connection2.Text = $"کیفیت اینترنت: متوسط ({ping.RoundtripTime}ms)";
                }
                else if (ping.RoundtripTime >= 170 && ping.RoundtripTime < 250)
                {
                    Connection2.Text = $"کیفیت اینترنت: بد ({ping.RoundtripTime}ms)";
                }
                else if (ping.RoundtripTime >= 250)
                {
                    Connection2.Text = $"کیفیت اینترنت: خیلی بد ({ping.RoundtripTime}ms)";
                }
                else
                {
                    Connection2.Text = ".";
                    Connection2.Visible = false;
                }
            }
            catch
            {
                Connection.Text = "به اینترنت متصل نیستید! برای استفاده لطفا به اینترنت متصل شوید";
                Connection2.Text = "";
                StartAI.Enabled = false;
            }
        }
        private void HelpButton_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control != sender)
                {
                    control.Visible = false;
                }
            }
            this.ClientSize = new Size(1193, 712);
            HomeButton.Location = new Point(1079, 11);
            CenterToScreen();
            volumeArrowButton.Visible = true;
            volumeArrowButton.Text = "↓";
            volumeTrackBar.Visible = false;
            soundButton.Visible = true;
            SettingButton.Visible = false;
            HomeButton.Visible = true;
            help = new Label();
            help.Visible = true;
            help.Font = new Font(Font.FontFamily, 19);
            help.Text = "این یه برنامه ماشین حساب پیشرفته هست.\nتوی منو ماشین حساب توی صفحه اصلی، یه سری قابیلت ها به درد بخور برای کار با اعداد در نظر گرفته شده.\n\nتوی منو ماشین حساب میتونی به چهار عمل اصلی، مثلثات، توان، رادیکال، درصد و میانگین گیری دسترسی داشته باشی.\n\nتوی منو تبدیل واحد ها، میتونی همه واحد های طول، مساحت، حجم و وزن رو به همدیگه تبدیل کنی.\n\nتوی قسمت شاخص توده بدن BMI میتونی میزان چاقی خودتو محاسبه کنی.\n\nتوی قسمت هوش مصنوعی اگه چیزی هست که توی منو ها نیست یا هر سوال دیگه ای داری، میتونی از سه مدل هوش مصنوعی DeepSeek، Copilot،Grok و Chat GPT بپرسی.\nالبته نکته مهم اینه که با توجه به اینکه  Chat GPT,Grok  ایرانی ها رو تحریم کرده باید یه ابزار تغییر آیپی یا VPN روشن کنی تا بتونی واردش بشی، اما اگه VPN نداری میتونی از دو تا مدل دیگه استفاده کنی.\nبرای استفاده از این قسمت هم باید به اینترنت وصل باشی.\nیه نکته مهم و اینکه توی قسمت هوش مصنوعی یه مدل هست به نام Duck که میتونی باهاش بدون vpn به حالت بدون محدودیت Chat GPT و چند تا مدل دیگه هم دسترسی داشته باشی.\n\nاگه باگ یا مشکلی توی برنامه دیدی یا پیشنهاد و انتقادی داری میتونی اونو گیت هاب من به آدرس https://github.com/AliAgaAbd/GUI-Professional-calculator گزارش کنی!";
            help.Height = 700;
            help.Width = 1103;
            help.Left = 50;
            help.Top = 100;
            this.Controls.Add(help);
            help.BringToFront();
            help.Visible = true;
            help.RightToLeft = RightToLeft.Yes;
        }
        private void InitializeComponentVideo()
        {
            timer = new Timer(); 
            HelpButton.Visible = false;
            HomeButton.Visible = false;
            CalculatorButton.Visible = false;
            BMIButton.Visible = false;
            AIButton.Visible = false;
            label1.Visible = false;
            pictureBox1.Visible = false;
            UnitConversationButton.Visible = false;
            SettingButton.Visible = false;
            Core.Initialize();
            this.libVLC = new LibVLC();
            this.mediaPlayer = new MediaPlayer(libVLC);
            var videoView = new LibVLCSharp.WinForms.VideoView
            {
                MediaPlayer = mediaPlayer,
                Dock = DockStyle.Fill
            };
            this.Load += MainForm_Load;
            this.Controls.Add(videoView);
            timer.Start();
            timer.Interval = 6700;
            timer.Tick += Close_tick;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            using (var media = new Media(libVLC, $"{AppDomain.CurrentDomain.BaseDirectory}\\start animation.mp4", FromType.FromPath))
            {
                mediaPlayer.Play(media);
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
        }
        private void Close_tick(object sender, EventArgs e)
        {
            timer.Stop();
            this.Controls.Remove(videoView);
            foreach (Control control in this.Controls)
            {
                if (control != sender)
                {
                    control.Visible = false;
                }
            }
            HelpButton.Visible = true;
            CalculatorButton.Visible = true;
            UnitConversationButton.Visible = true;
            BMIButton.Visible = true;
            AIButton.Visible = true;
            label1.Visible = true;
            pictureBox1.Visible = true;
            HomeButton.Visible = true;
            SettingButton.Visible = true;
            soundButton.Visible = true;
            volumeArrowButton.Visible = true;
            volumeArrowButton.Text = "↓";
            volumeTrackBar.Visible = false;
            mediaPlayer.Dispose();
            libVLC.Dispose();
            MemoryClean();
            UpdateCheck();
        }
        private void BMIButton_MouseClick(object sender, MouseEventArgs e)
        {
            SettingButton.Visible = false;
            CalculatorButton.Visible = false;
            UnitConversationButton.Visible = false;
            BMIButton.Visible = false;
            AIButton.Visible = false;
            label1.Visible = false;
            pictureBox1.Visible = false;
            BMIshow = new Label();
            BMIshow.Text = "خب اینجا باید وزن خودت رو بر حسب کیلوگرم و قد خودت رو بر حسب متر وارد کنی تا بگم چاقی یا لاغر";
            BMIshow.Font = new Font(BMIshow.Font.FontFamily, 20);
            BMIshow.Height = 100;
            BMIshow.Width = 1000;
            BMIshow.Top = 100;
            BMIshow.Left = 200;
            this.Controls.Add(BMIshow);
            BMIweight = new TextBox();
            BMIweight.Height = 200;
            BMIweight.Width = 100;
            BMIweight.Top = 200;
            BMIweight.Left = 400;
            BMIweight.Font = new Font(BMIweight.Font.FontFamily, 15);
            this.Controls.Add(BMIweight);
            BMIweight2 = new Label();
            BMIweight2.Height = 100;
            BMIweight2.Width = 200;
            BMIweight2.Top = 200;
            BMIweight2.Left = 150;
            BMIweight2.Text = "وزن شما بر حسب کیلوگرم";
            BMIweight2.RightToLeft = RightToLeft.Yes;
            BMIweight2.Font = new Font(BMIweight2.Font.FontFamily, 15);
            this.Controls.Add(BMIweight2);
            BMItall = new TextBox();
            BMItall.Height = 200;
            BMItall.Width = 100;
            BMItall.Top = 300;
            BMItall.Left = 400;
            BMItall.Font = new Font(BMItall.Font.FontFamily, 15);
            this.Controls.Add(BMItall);
            BMItall2 = new Label();
            BMItall2.Height = 100;
            BMItall2.Width = 200;
            BMItall2.Top = 300;
            BMItall2.Left = 150;
            BMItall2.Text = "قد شما بر حسب سانتیمتر";
            BMItall2.RightToLeft = RightToLeft.Yes;
            BMItall2.Font = new Font(BMItall2.Font.FontFamily, 15);
            this.Controls.Add(BMItall2);
            BMIbutton = new Button();
            BMIbutton.Height = 40;
            BMIbutton.Width = 100;
            BMIbutton.Left = 600;
            BMIbutton.Top = 250;
            BMIbutton.Text = "محاسبه BMI";
            BMIbutton.Font = new Font(BMIbutton.Font.FontFamily, 15);
            BMIbutton.RightToLeft = RightToLeft.Yes;
            BMIbutton.TextAlign = ContentAlignment.MiddleCenter;
            BMIbutton.MouseEnter += MouseHand;
            BMIbutton.MouseLeave += MouseDefualt;
            BMIbutton.MouseClick += BMIcalculate;
            this.Controls.Add(BMIbutton);
            BMIshow2 = new Label();
            BMIshow2.Height = 200;
            BMIshow2.Width = 300;
            BMIshow2.Left = 700;
            BMIshow2.Top = 250;
            BMIshow2.RightToLeft = RightToLeft.Yes;
            BMIshow2.Font = new Font(Font.FontFamily, 15);
            this.Controls.Add(BMIshow2);
        }
        private void MouseHand(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }
        private void MouseDefualt(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }
        private void BMIcalculate(object sender, MouseEventArgs e)
        {
            try
            {
                weightMain = Convert.ToDouble(double.Parse(BMIweight.Text.ToString()));
                tallMain = Convert.ToDouble(double.Parse(BMItall.Text.ToString()));
                mainBMI = (float)(weightMain / ((float)(tallMain / 100) * (float)(tallMain / 100)));
                if (mainBMI >= 0 && mainBMI < 18.5)
                {
                    BMIshow2.Text = $"BMI شما برابر است با : {(float)mainBMI}\nوضعیت چاقی : کمبود وزن";
                }
                if (mainBMI >= 18.5 && mainBMI < 25)
                {
                    BMIshow2.Text = $"BMI شما برابر است با : {(float)mainBMI}\nوضعیت چاقی : نرمال";
                }
                if (mainBMI >= 25 && mainBMI < 30)
                {
                    BMIshow2.Text = $"BMI شما برابر است با : {(float)mainBMI}\nوضعیت چاقی : اضافه وزن";
                }
                if (mainBMI >= 30 && mainBMI < 35)
                {
                    BMIshow2.Text = $"BMI شما برابر است با : {(float)mainBMI}\nوضعیت چاقی : چاقی درجه یک";
                }
                if (mainBMI >= 35 && mainBMI < 40)
                {
                    BMIshow2.Text = $"BMI شما برابر است با : {(float)mainBMI}\nوضعیت چاقی : چاقی درجه دو";
                }
                if (mainBMI >= 40)
                {
                    BMIshow2.Text = $"BMI شما برابر است با : {(float)mainBMI}\nوضعیت چاقی : چاقی درجه سه";
                }
            }
            catch { BMIshow2.Text = "ورودی نامعتبر"; }
        }
        private void CalculatorButton_MouseClick(object sender, MouseEventArgs e)
        {
            SettingButton.Visible = false;
            CalculatorButton.Visible = false;
            UnitConversationButton.Visible = false;
            BMIButton.Visible = false;
            AIButton.Visible = false;
            label1.Visible = false;
            pictureBox1.Visible = false;
            txtCalcDisplay = new TextBox { Location = new Point(300, 50), Width = 600, Height = 70, Font = new Font("Arial", 17), Text = "0", RightToLeft = RightToLeft.No, TextAlign = HorizontalAlignment.Right };
            txtCalcDisplay.KeyPress += TxtCalcDisplay_KeyPress;
            cbCalcMode = new ComboBox { Location = new Point(300, 100), Width = 170, Font = new Font("Arial", 15) };
            cbCalcMode.Items.AddRange(new string[] { "ساده", "مهندسی", "پیشرفته" });
            cbCalcMode.SelectedIndex = 0;
            cbCalcMode.SelectedIndexChanged += CbCalcMode_SelectedIndexChanged;
            btnClear = new Button { Text = "CE", Location = new Point(300, 150), Width = 60, Height = 60, Font = new Font("Arial", 14) };
            btnClearAll = new Button { Text = "C", Location = new Point(360, 150), Width = 60, Height = 60, Font = new Font("Arial", 14) };
            btnPercent = new Button { Text = "%", Location = new Point(420, 150), Width = 60, Height = 60, Font = new Font("Arial", 14) };
            btn7 = new Button { Text = "7", Location = new Point(300, 210), Width = 60, Height = 60, Font = new Font("Arial", 14) };
            btn8 = new Button { Text = "8", Location = new Point(360, 210), Width = 60, Height = 60, Font = new Font("Arial", 14) };
            btn9 = new Button { Text = "9", Location = new Point(420, 210), Width = 60, Height = 60, Font = new Font("Arial", 14) };
            btnDivide = new Button { Text = "÷", Location = new Point(480, 210), Width = 60, Height = 60, Font = new Font("Arial", 14) };
            btnMultiply = new Button { Text = "×", Location = new Point(540, 210), Width = 60, Height = 60, Font = new Font("Arial", 14) };
            btn4 = new Button { Text = "4", Location = new Point(300, 270), Width = 60, Height = 60, Font = new Font("Arial", 14) };
            btn5 = new Button { Text = "5", Location = new Point(360, 270), Width = 60, Height = 60, Font = new Font("Arial", 14) };
            btn6 = new Button { Text = "6", Location = new Point(420, 270), Width = 60, Height = 60, Font = new Font("Arial", 14) };
            btnSubtract = new Button { Text = "-", Location = new Point(480, 270), Width = 60, Height = 60, Font = new Font("Arial", 14) };
            btnAdd = new Button { Text = "+", Location = new Point(540, 270), Width = 60, Height = 60, Font = new Font("Arial", 14) };
            btn1 = new Button { Text = "1", Location = new Point(300, 330), Width = 60, Height = 60, Font = new Font("Arial", 14) };
            btn2 = new Button { Text = "2", Location = new Point(360, 330), Width = 60, Height = 60, Font = new Font("Arial", 14) };
            btn3 = new Button { Text = "3", Location = new Point(420, 330), Width = 60, Height = 60, Font = new Font("Arial", 14) };
            btnEquals = new Button { Text = "=", Location = new Point(480, 330), Width = 120, Height = 60, Font = new Font("Arial", 14) };
            btn0 = new Button { Text = "0", Location = new Point(300, 390), Width = 120, Height = 60, Font = new Font("Arial", 14) };
            btnDecimal = new Button { Text = ".", Location = new Point(420, 390), Width = 60, Height = 60, Font = new Font("Arial", 14) };
            btnSin = new Button { Text = "sin", Location = new Point(600, 150), Width = 60, Height = 60, Font = new Font("Arial", 14), Visible = false };
            btnCos = new Button { Text = "cos", Location = new Point(660, 150), Width = 60, Height = 60, Font = new Font("Arial", 14), Visible = false };
            btnTan = new Button { Text = "tan", Location = new Point(720, 150), Width = 60, Height = 60, Font = new Font("Arial", 14), Visible = false };
            btnLog = new Button { Text = "log", Location = new Point(600, 210), Width = 60, Height = 60, Font = new Font("Arial", 14), Visible = false };
            btnLn = new Button { Text = "ln", Location = new Point(660, 210), Width = 60, Height = 60, Font = new Font("Arial", 14), Visible = false };
            btnFact = new Button { Text = "!", Location = new Point(720, 210), Width = 60, Height = 60, Font = new Font("Arial", 14), Visible = false };
            btnPi = new Button { Text = "π", Location = new Point(600, 270), Width = 60, Height = 60, Font = new Font("Arial", 14), Visible = false };
            btnE = new Button { Text = "e", Location = new Point(660, 270), Width = 60, Height = 60, Font = new Font("Arial", 14), Visible = false };
            btnPower = new Button { Text = "x^y", Location = new Point(720, 270), Width = 60, Height = 60, Font = new Font("Arial", 14), Visible = false };
            btnNthRoot = new Button { Text = "√n", Location = new Point(600, 330), Width = 60, Height = 60, Font = new Font("Arial", 14), Visible = false };
            txtNthRootDegree = new TextBox { Location = new Point(660, 330), Width = 60, Height = 60, Font = new Font("Arial", 14), Text = "2", Visible = false };
            btnDerivative = new Button { Text = "d/dx", Location = new Point(600, 390), Width = 60, Height = 60, Font = new Font("Arial", 14), Visible = false };
            btnIntegral = new Button { Text = "∫dx", Location = new Point(660, 390), Width = 60, Height = 60, Font = new Font("Arial", 14), Visible = false };
            btnSolveQuad = new Button { Text = "حل درجه 2", Location = new Point(720, 390), Width = 80, Height = 60, Font = new Font("Arial", 14), Visible = false };
            btnMatrixAdd = new Button { Text = "جمع ماتریس", Location = new Point(600, 450), Width = 80, Height = 60, Font = new Font("Arial", 14), Visible = false };
            btnMatrixMultiply = new Button { Text = "ضرب ماتریس", Location = new Point(680, 450), Width = 80, Height = 60, Font = new Font("Arial", 14), Visible = false };
            btnClear.MouseClick += (s, ev) => { currentExpression = "0"; txtCalcDisplay.Text = "0"; };
            btnClearAll.MouseClick += (s, ev) => { currentExpression = "0"; txtCalcDisplay.Text = "0"; };
            btnPercent.MouseClick += (s, ev) => { currentExpression = (double.Parse(currentExpression) / 100).ToString(); txtCalcDisplay.Text = currentExpression; };
            btnNthRoot.MouseClick += (s, ev) => { currentExpression += " √(" + txtNthRootDegree.Text + ") "; txtCalcDisplay.Text = currentExpression; };
            btnPower.MouseClick += (s, ev) => { currentExpression += " ^ "; txtCalcDisplay.Text = currentExpression; };
            btnEquals.MouseClick += BtnEquals_Click;
            btnAdd.MouseClick += (s, ev) => { currentExpression += " + "; txtCalcDisplay.Text = currentExpression; };
            btnSubtract.MouseClick += (s, ev) => { currentExpression += " - "; txtCalcDisplay.Text = currentExpression; };
            btnMultiply.MouseClick += (s, ev) => { currentExpression += " * "; txtCalcDisplay.Text = currentExpression; };
            btnDivide.MouseClick += (s, ev) => { currentExpression += " / "; txtCalcDisplay.Text = currentExpression; };
            btn0.MouseClick += (s, ev) => { AppendNumber("0"); };
            btn1.MouseClick += (s, ev) => { AppendNumber("1"); };
            btn2.MouseClick += (s, ev) => { AppendNumber("2"); };
            btn3.MouseClick += (s, ev) => { AppendNumber("3"); };
            btn4.MouseClick += (s, ev) => { AppendNumber("4"); };
            btn5.MouseClick += (s, ev) => { AppendNumber("5"); };
            btn6.MouseClick += (s, ev) => { AppendNumber("6"); };
            btn7.MouseClick += (s, ev) => { AppendNumber("7"); };
            btn8.MouseClick += (s, ev) => { AppendNumber("8"); };
            btn9.MouseClick += (s, ev) => { AppendNumber("9"); };
            btnDecimal.MouseClick += (s, ev) => { AppendNumber("."); };
            btnSin.MouseClick += (s, ev) => { currentExpression = Math.Sin(double.Parse(currentExpression) * Math.PI / 180).ToString(); txtCalcDisplay.Text = currentExpression; };
            btnCos.MouseClick += (s, ev) => { currentExpression = Math.Cos(double.Parse(currentExpression) * Math.PI / 180).ToString(); txtCalcDisplay.Text = currentExpression; };
            btnTan.MouseClick += (s, ev) => { currentExpression = Math.Tan(double.Parse(currentExpression) * Math.PI / 180).ToString(); txtCalcDisplay.Text = currentExpression; };
            btnLog.MouseClick += (s, ev) => { currentExpression = Math.Log10(double.Parse(currentExpression)).ToString(); txtCalcDisplay.Text = currentExpression; };
            btnLn.MouseClick += (s, ev) => { currentExpression = Math.Log(double.Parse(currentExpression)).ToString(); txtCalcDisplay.Text = currentExpression; };
            btnFact.MouseClick += (s, ev) => { currentExpression = Factorial((int)double.Parse(currentExpression)).ToString(); txtCalcDisplay.Text = currentExpression; };
            btnPi.MouseClick += (s, ev) => { currentExpression = Math.PI.ToString(); txtCalcDisplay.Text = currentExpression; };
            btnE.MouseClick += (s, ev) => { currentExpression = Math.E.ToString(); txtCalcDisplay.Text = currentExpression; };
            btnDerivative.MouseClick += BtnDerivative_Click;
            btnIntegral.MouseClick += BtnIntegral_Click;
            btnSolveQuad.MouseClick += BtnSolveQuad_Click;
            btnMatrixAdd.MouseClick += BtnMatrixAdd_Click;
            btnMatrixMultiply.MouseClick += BtnMatrixMultiply_Click;
            btnClear.MouseEnter += MouseHand; btnClear.MouseLeave += MouseDefualt;
            btnClearAll.MouseEnter += MouseHand; btnClearAll.MouseLeave += MouseDefualt;
            btnPercent.MouseEnter += MouseHand; btnPercent.MouseLeave += MouseDefualt;
            btnNthRoot.MouseEnter += MouseHand; btnNthRoot.MouseLeave += MouseDefualt;
            btnPower.MouseEnter += MouseHand; btnPower.MouseLeave += MouseDefualt;
            btnEquals.MouseEnter += MouseHand; btnEquals.MouseLeave += MouseDefualt;
            btnAdd.MouseEnter += MouseHand; btnAdd.MouseLeave += MouseDefualt;
            btnSubtract.MouseEnter += MouseHand; btnSubtract.MouseLeave += MouseDefualt;
            btnMultiply.MouseEnter += MouseHand; btnMultiply.MouseLeave += MouseDefualt;
            btnDivide.MouseEnter += MouseHand; btnDivide.MouseLeave += MouseDefualt;
            btn0.MouseEnter += MouseHand; btn0.MouseLeave += MouseDefualt;
            btn1.MouseEnter += MouseHand; btn1.MouseLeave += MouseDefualt;
            btn2.MouseEnter += MouseHand; btn2.MouseLeave += MouseDefualt;
            btn3.MouseEnter += MouseHand; btn3.MouseLeave += MouseDefualt;
            btn4.MouseEnter += MouseHand; btn4.MouseLeave += MouseDefualt;
            btn5.MouseEnter += MouseHand; btn5.MouseLeave += MouseDefualt;
            btn6.MouseEnter += MouseHand; btn6.MouseLeave += MouseDefualt;
            btn7.MouseEnter += MouseHand; btn7.MouseLeave += MouseDefualt;
            btn8.MouseEnter += MouseHand; btn8.MouseLeave += MouseDefualt;
            btn9.MouseEnter += MouseHand; btn9.MouseLeave += MouseDefualt;
            btnDecimal.MouseEnter += MouseHand; btnDecimal.MouseLeave += MouseDefualt;
            btnSin.MouseEnter += MouseHand; btnSin.MouseLeave += MouseDefualt;
            btnCos.MouseEnter += MouseHand; btnCos.MouseLeave += MouseDefualt;
            btnTan.MouseEnter += MouseHand; btnTan.MouseLeave += MouseDefualt;
            btnLog.MouseEnter += MouseHand; btnLog.MouseLeave += MouseDefualt;
            btnLn.MouseEnter += MouseHand; btnLn.MouseLeave += MouseDefualt;
            btnFact.MouseEnter += MouseHand; btnFact.MouseLeave += MouseDefualt;
            btnPi.MouseEnter += MouseHand; btnPi.MouseLeave += MouseDefualt;
            btnE.MouseEnter += MouseHand; btnE.MouseLeave += MouseDefualt;
            btnDerivative.MouseEnter += MouseHand; btnDerivative.MouseLeave += MouseDefualt;
            btnIntegral.MouseEnter += MouseHand; btnIntegral.MouseLeave += MouseDefualt;
            btnSolveQuad.MouseEnter += MouseHand; btnSolveQuad.MouseLeave += MouseDefualt;
            btnMatrixAdd.MouseEnter += MouseHand; btnMatrixAdd.MouseLeave += MouseDefualt;
            btnMatrixMultiply.MouseEnter += MouseHand; btnMatrixMultiply.MouseLeave += MouseDefualt;
            this.Controls.Add(txtCalcDisplay);
            this.Controls.Add(cbCalcMode);
            this.Controls.Add(btnClear);
            this.Controls.Add(btnClearAll);
            this.Controls.Add(btnPercent);
            this.Controls.Add(btnEquals);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnSubtract);
            this.Controls.Add(btnMultiply);
            this.Controls.Add(btnDivide);
            this.Controls.Add(btn0);
            this.Controls.Add(btn1);
            this.Controls.Add(btn2);
            this.Controls.Add(btn3);
            this.Controls.Add(btn4);
            this.Controls.Add(btn5);
            this.Controls.Add(btn6);
            this.Controls.Add(btn7);
            this.Controls.Add(btn8);
            this.Controls.Add(btn9);
            this.Controls.Add(btnDecimal);
            this.Controls.Add(btnSin);
            this.Controls.Add(btnCos);
            this.Controls.Add(btnTan);
            this.Controls.Add(btnLog);
            this.Controls.Add(btnLn);
            this.Controls.Add(btnFact);
            this.Controls.Add(btnPi);
            this.Controls.Add(btnE);
            this.Controls.Add(btnPower);
            this.Controls.Add(btnNthRoot);
            this.Controls.Add(txtNthRootDegree);
            this.Controls.Add(btnDerivative);
            this.Controls.Add(btnIntegral);
            this.Controls.Add(btnSolveQuad);
            this.Controls.Add(btnMatrixAdd);
            this.Controls.Add(btnMatrixMultiply);
        }
        private void AppendNumber(string number)
        {
            if (currentExpression == "0")
                currentExpression = number;
            else
                currentExpression += number;
            txtCalcDisplay.Text = currentExpression;
        }
        private void TxtCalcDisplay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnEquals_Click(this, null);
                e.Handled = true;
            }
            else if (e.KeyChar == '+')
            {
                currentExpression += " + ";
                txtCalcDisplay.Text = currentExpression;
                e.Handled = true;
            }
            else if (e.KeyChar == '-')
            {
                currentExpression += " - ";
                txtCalcDisplay.Text = currentExpression;
                e.Handled = true;
            }
            else if (e.KeyChar == '*')
            {
                currentExpression += " * ";
                txtCalcDisplay.Text = currentExpression;
                e.Handled = true;
            }
            else if (e.KeyChar == '/')
            {
                currentExpression += " / ";
                txtCalcDisplay.Text = currentExpression;
                e.Handled = true;
            }
            else if (e.KeyChar == '^')
            {
                currentExpression += " ^ ";
                txtCalcDisplay.Text = currentExpression;
                e.Handled = true;
            }
            else if (char.IsDigit(e.KeyChar) || e.KeyChar == '.')
            {
                AppendNumber(e.KeyChar.ToString());
                e.Handled = true;
            }
            else if (e.KeyChar == (char)Keys.Back)
            {
                if (currentExpression.Length > 1)
                    currentExpression = currentExpression.Substring(0, currentExpression.Length - 1);
                else
                    currentExpression = "0";
                txtCalcDisplay.Text = currentExpression;
                e.Handled = true;
            }
        }
        private void CbCalcMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string mode = cbCalcMode.SelectedItem.ToString();
            if (mode == "ساده")
            {
                btnSin.Visible = false;
                btnCos.Visible = false;
                btnTan.Visible = false;
                btnLog.Visible = false;
                btnLn.Visible = false;
                btnFact.Visible = false;
                btnPi.Visible = false;
                btnE.Visible = false;
                btnPower.Visible = false;
                btnNthRoot.Visible = false;
                txtNthRootDegree.Visible = false;
                btnDerivative.Visible = false;
                btnIntegral.Visible = false;
                btnSolveQuad.Visible = false;
                btnMatrixAdd.Visible = false;
                btnMatrixMultiply.Visible = false;
            }
            else if (mode == "مهندسی")
            {
                btnSin.Visible = true;
                btnCos.Visible = true;
                btnTan.Visible = true;
                btnLog.Visible = true;
                btnLn.Visible = true;
                btnFact.Visible = true;
                btnPi.Visible = true;
                btnE.Visible = true;
                btnPower.Visible = true;
                btnNthRoot.Visible = true;
                txtNthRootDegree.Visible = true;
                btnDerivative.Visible = false;
                btnIntegral.Visible = false;
                btnSolveQuad.Visible = false;
                btnMatrixAdd.Visible = false;
                btnMatrixMultiply.Visible = false;
            }
            else if (mode == "پیشرفته")
            {
                btnSin.Visible = true;
                btnCos.Visible = true;
                btnTan.Visible = true;
                btnLog.Visible = true;
                btnLn.Visible = true;
                btnFact.Visible = true;
                btnPi.Visible = true;
                btnE.Visible = true;
                btnPower.Visible = true;
                btnNthRoot.Visible = true;
                txtNthRootDegree.Visible = true;
                btnDerivative.Visible = true;
                btnIntegral.Visible = true;
                btnSolveQuad.Visible = true;
                btnMatrixAdd.Visible = true;
                btnMatrixMultiply.Visible = true;
            }
        }
        private void BtnEquals_Click(object sender, MouseEventArgs e)
        {
            try
            {
                string expression = currentExpression;
                double result = 0;
                while (expression.Contains("√"))
                {
                    int rootIndex = expression.IndexOf("√");
                    int degreeStart = expression.IndexOf("(", rootIndex);
                    int degreeEnd = expression.IndexOf(")", degreeStart);
                    if (degreeStart == -1 || degreeEnd == -1)
                        throw new Exception("فرجه رادیکال نامعتبر است");
                    string degreeStr = expression.Substring(degreeStart + 1, degreeEnd - degreeStart - 1);
                    double degree = double.Parse(degreeStr);
                    string numberStr = "";
                    int i = rootIndex - 1;
                    while (i >= 0 && (char.IsDigit(expression[i]) || expression[i] == '.' || expression[i] == ' '))
                    {
                        if (expression[i] != ' ')
                            numberStr = expression[i] + numberStr;
                        i--;
                    }
                    int numberStart = i + 1;
                    if (string.IsNullOrEmpty(numberStr))
                        throw new Exception("عدد قبل از رادیکال یافت نشد");
                    double number = double.Parse(numberStr);
                    result = Math.Pow(number, 1.0 / degree);
                    expression = expression.Substring(0, numberStart) + result.ToString() + expression.Substring(degreeEnd + 1);
                }
                while (expression.Contains("^"))
                {
                    int powerIndex = expression.IndexOf("^");
                    int baseEnd = powerIndex;
                    int exponentStart = powerIndex + 1;
                    string baseStr = "";
                    int i = powerIndex - 1;
                    while (i >= 0 && (char.IsDigit(expression[i]) || expression[i] == '.' || expression[i] == ' '))
                    {
                        if (expression[i] != ' ')
                            baseStr = expression[i] + baseStr;
                        i--;
                    }
                    baseEnd = i + 1;
                    if (string.IsNullOrEmpty(baseStr))
                        throw new Exception("پایه توان یافت نشد");
                    double baseNum = double.Parse(baseStr);
                    string exponentStr = "";
                    i = exponentStart;
                    while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.' || expression[i] == ' '))
                    {
                        if (expression[i] != ' ')
                            exponentStr += expression[i];
                        i++;
                    }
                    if (string.IsNullOrEmpty(exponentStr))
                        throw new Exception("توان یافت نشد");
                    double exponent = double.Parse(exponentStr);
                    result = Math.Pow(baseNum, exponent);
                    expression = expression.Substring(0, baseEnd) + result.ToString() + expression.Substring(i);
                }
                System.Data.DataTable table = new System.Data.DataTable();
                result = Convert.ToDouble(table.Compute(expression, ""));
                txtCalcDisplay.Text = result.ToString();
                currentExpression = result.ToString();
            }
            catch (Exception ex)
            {
                txtCalcDisplay.Text = "خطا: " + ex.Message;
                currentExpression = "0";
            }
        }
        private double Factorial(int n)
        {
            if (n <= 1) return 1;
            double result = 1;
            for (int i = 2; i <= n; i++)
                result *= i;
            return result;
        }
        private void BtnDerivative_Click(object sender, MouseEventArgs e)
        {
            try
            {
                double x = double.Parse(currentExpression);
                double h = 0.0001;
                double derivative = (Math.Pow(x + h, 2) - Math.Pow(x, 2)) / h;
                currentExpression = derivative.ToString();
                txtCalcDisplay.Text = currentExpression;
            }
            catch (Exception ex)
            {
                txtCalcDisplay.Text = "خطا در مشتق: " + ex.Message;
            }
        }
        private void BtnIntegral_Click(object sender, MouseEventArgs e)
        {
            try
            {
                double x = double.Parse(currentExpression);
                double result = 0;
                double dx = 0.0001;
                for (double i = 0; i <= x; i += dx)
                {
                    result += Math.Pow(i, 2) * dx;
                }
                currentExpression = result.ToString();
                txtCalcDisplay.Text = currentExpression;
            }
            catch (Exception ex)
            {
                txtCalcDisplay.Text = "خطا در انتگرال: " + ex.Message;
            }
        }
        private void BtnSolveQuad_Click(object sender, MouseEventArgs e)
        {
            try
            {
                string[] parts = currentExpression.Split(',');
                if (parts.Length != 3)
                    throw new Exception("لطفاً a,b,c رو با کاما جدا کنید");
                double a = double.Parse(parts[0]);
                double b = double.Parse(parts[1]);
                double c = double.Parse(parts[2]);
                double discriminant = b * b - 4 * a * c;
                if (discriminant < 0)
                    throw new Exception("معادله جواب حقیقی ندارد");
                double root1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                double root2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
                currentExpression = $"x1 = {root1}, x2 = {root2}";
                txtCalcDisplay.Text = currentExpression;
            }
            catch (Exception ex)
            {
                txtCalcDisplay.Text = "خطا در حل معادله: " + ex.Message;
            }
        }
        private void BtnMatrixAdd_Click(object sender, MouseEventArgs e)
        {
            try
            {
                string[] matrices = currentExpression.Split('|');
                if (matrices.Length != 2)
                    throw new Exception("لطفاً دو ماتریس با | جدا کنید");
                var rows1 = matrices[0].Split(';');
                var rows2 = matrices[1].Split(';');
                if (rows1.Length != rows2.Length)
                    throw new Exception("ابعاد ماتریس‌ها باید برابر باشد");
                double[,] resultMatrix = new double[rows1.Length, rows1[0].Split(',').Length];
                for (int i = 0; i < rows1.Length; i++)
                {
                    var cols1 = rows1[i].Split(',');
                    var cols2 = rows2[i].Split(',');
                    if (cols1.Length != cols2.Length)
                        throw new Exception("ابعاد ماتریس‌ها باید برابر باشد");
                    for (int j = 0; j < cols1.Length; j++)
                    {
                        resultMatrix[i, j] = double.Parse(cols1[j]) + double.Parse(cols2[j]);
                    }
                }
                string resultStr = "";
                for (int i = 0; i < resultMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < resultMatrix.GetLength(1); j++)
                    {
                        resultStr += resultMatrix[i, j] + ",";
                    }
                    resultStr = resultStr.TrimEnd(',') + ";";
                }
                currentExpression = resultStr.TrimEnd(';');
                txtCalcDisplay.Text = currentExpression;
            }
            catch (Exception ex)
            {
                txtCalcDisplay.Text = "خطا در جمع ماتریس: " + ex.Message;
            }
        }
        private void BtnMatrixMultiply_Click(object sender, MouseEventArgs e)
        {
            try
            {
                string[] matrices = currentExpression.Split('|');
                if (matrices.Length != 2)
                    throw new Exception("لطفاً دو ماتریس با | جدا کنید");
                var rows1 = matrices[0].Split(';');
                var rows2 = matrices[1].Split(';');
                var cols1 = rows1[0].Split(',');
                var cols2 = rows2[0].Split(',');
                if (cols1.Length != rows2.Length)
                    throw new Exception("تعداد ستون‌های ماتریس اول باید برابر تعداد سطرهای ماتریس دوم باشد");
                double[,] resultMatrix = new double[rows1.Length, cols2.Length];
                for (int i = 0; i < rows1.Length; i++)
                {
                    var row1 = rows1[i].Split(',');
                    for (int j = 0; j < cols2.Length; j++)
                    {
                        double sum = 0;
                        for (int k = 0; k < cols1.Length; k++)
                        {
                            sum += double.Parse(row1[k]) * double.Parse(rows2[k].Split(',')[j]);
                        }
                        resultMatrix[i, j] = sum;
                    }
                }
                string resultStr = "";
                for (int i = 0; i < resultMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < resultMatrix.GetLength(1); j++)
                    {
                        resultStr += resultMatrix[i, j] + ",";
                    }
                    resultStr = resultStr.TrimEnd(',') + ";";
                }
                currentExpression = resultStr.TrimEnd(';');
                txtCalcDisplay.Text = currentExpression;
            }
            catch (Exception ex)
            {
                txtCalcDisplay.Text = "خطا در ض(rb ماتریس: " + ex.Message;
            }
        }
        private void UnitConversationButton_MouseClick(object sender, MouseEventArgs e)
        {
            SettingButton.Visible = false;
            CalculatorButton.Visible = false;
            UnitConversationButton.Visible = false;
            BMIButton.Visible = false;
            AIButton.Visible = false;
            label1.Visible = false;
            pictureBox1.Visible = false;
            lblUnitCategory = new Label { Text = "دسته‌بندی:", Location = new Point(150, 50), Width = 100, Font = new Font("Arial", 15), RightToLeft = RightToLeft.Yes };
            cbUnitCategory = new ComboBox { Location = new Point(400, 50), Width = 150, Font = new Font("Arial", 15) };
            cbUnitCategory.Items.AddRange(new string[] { "طول", "مساحت", "حجم", "وزن", "دما", "سرعت", "زمان", "انرژی", "فشار", "چگالی" });
            cbUnitCategory.SelectedIndex = 0;
            cbUnitCategory.SelectedIndexChanged += CbUnitCategory_SelectedIndexChanged;
            lblUnitFrom = new Label { Text = "مبدا:", Location = new Point(150, 100), Width = 100, Font = new Font("Arial", 15), RightToLeft = RightToLeft.Yes };
            cbUnitFrom = new ComboBox { Location = new Point(400, 100), Width = 150, Font = new Font("Arial", 15) };
            cbUnitFrom.Items.AddRange(new string[] { "متر", "سانتی‌متر", "کیلومتر", "میلی‌متر", "مایل", "فوت", "اینچ" });
            cbUnitFrom.SelectedIndex = 0;
            lblUnitTo = new Label { Text = "مقصد:", Location = new Point(150, 150), Width = 100, Font = new Font("Arial", 15), RightToLeft = RightToLeft.Yes };
            cbUnitTo = new ComboBox { Location = new Point(400, 150), Width = 150, Font = new Font("Arial", 15) };
            cbUnitTo.Items.AddRange(new string[] { "متر", "سانتی‌متر", "کیلومتر", "میلی‌متر", "مایل", "فوت", "اینچ" });
            cbUnitTo.SelectedIndex = 1;
            txtUnitInput = new TextBox { Location = new Point(400, 200), Width = 250, Font = new Font("Arial", 15) };
            txtUnitOutput = new TextBox { Location = new Point(700, 200), Width = 250, Font = new Font("Arial", 15), ReadOnly = true };
            btnConvertUnits = new Button { Text = "تبدیل", Location = new Point(400, 250), Width = 200, Height = 40, Font = new Font("Arial", 15), RightToLeft = RightToLeft.Yes };
            btnConvertUnits.MouseClick += BtnConvertUnits_Click;
            btnConvertUnits.MouseEnter += MouseHand;
            btnConvertUnits.MouseLeave += MouseDefualt;
            this.Controls.Add(lblUnitCategory);
            this.Controls.Add(cbUnitCategory);
            this.Controls.Add(lblUnitFrom);
            this.Controls.Add(cbUnitFrom);
            this.Controls.Add(lblUnitTo);
            this.Controls.Add(cbUnitTo);
            this.Controls.Add(txtUnitInput);
            this.Controls.Add(txtUnitOutput);
            this.Controls.Add(btnConvertUnits);
        }
        private void CbUnitCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbUnitFrom.Items.Clear();
            cbUnitTo.Items.Clear();
            string category = cbUnitCategory.SelectedItem.ToString();
            switch (category)
            {
                case "طول":
                    cbUnitFrom.Items.AddRange(new string[] { "متر", "سانتی‌متر", "کیلومتر", "میلی‌متر", "مایل", "فوت", "اینچ" });
                    cbUnitTo.Items.AddRange(new string[] { "متر", "سانتی‌متر", "کیلومتر", "میلی‌متر", "مایل", "فوت", "اینچ" });
                    break;
                case "مساحت":
                    cbUnitFrom.Items.AddRange(new string[] { "متر مربع", "سانتی‌متر مربع", "کیلومتر مربع", "هکتار", "مایل مربع", "فوت مربع", "اینچ مربع" });
                    cbUnitTo.Items.AddRange(new string[] { "متر مربع", "سانتی‌متر مربع", "کیلومتر مربع", "هکتار", "مایل مربع", "فوت مربع", "اینچ مربع" });
                    break;
                case "حجم":
                    cbUnitFrom.Items.AddRange(new string[] { "متر مکعب", "سانتی‌متر مکعب", "لیتر", "میلی‌لیتر", "گالن", "فوت مکعب", "اینچ مکعب" });
                    cbUnitTo.Items.AddRange(new string[] { "متر مکعب", "سانتی‌متر مکعب", "لیتر", "میلی‌لیتر", "گالن", "فوت مکعب", "اینچ مکعب" });
                    break;
                case "وزن":
                    cbUnitFrom.Items.AddRange(new string[] { "کیلوگرم", "گرم", "تن", "پوند", "اونس" });
                    cbUnitTo.Items.AddRange(new string[] { "کیلوگرم", "گرم", "تن", "پوند", "اونس" });
                    break;
                case "دما":
                    cbUnitFrom.Items.AddRange(new string[] { "سانتی‌گراد", "فارنهایت", "کلوین" });
                    cbUnitTo.Items.AddRange(new string[] { "سانتی‌گراد", "فارنهایت", "کلوین" });
                    break;
                case "سرعت":
                    cbUnitFrom.Items.AddRange(new string[] { "متر بر ثانیه", "کیلومتر بر ساعت", "مایل بر ساعت", "گره" });
                    cbUnitTo.Items.AddRange(new string[] { "متر بر ثانیه", "کیلومتر بر ساعت", "مایل بر ساعت", "گره" });
                    break;
                case "زمان":
                    cbUnitFrom.Items.AddRange(new string[] { "ثانیه", "دقیقه", "ساعت", "روز", "هفته", "ماه", "سال" });
                    cbUnitTo.Items.AddRange(new string[] { "ثانیه", "دقیقه", "ساعت", "روز", "هفته", "ماه", "سال" });
                    break;
                case "انرژی":
                    cbUnitFrom.Items.AddRange(new string[] { "ژول", "کیلوژول", "کالری", "کیلوکالری", "وات-ساعت" });
                    cbUnitTo.Items.AddRange(new string[] { "ژول", "کیلوژول", "کالری", "کیلوکالری", "وات-ساعت" });
                    break;
                case "فشار":
                    cbUnitFrom.Items.AddRange(new string[] { "پاسکال", "کیلوپاسکال", "بار", "پوند بر اینچ مربع", "اتمسفر" });
                    cbUnitTo.Items.AddRange(new string[] { "پاسکال", "کیلوپاسکال", "بار", "پوند بر اینچ مربع", "اتمسفر" });
                    break;
                case "چگالی":
                    cbUnitFrom.Items.AddRange(new string[] { "کیلوگرم بر متر مکعب", "گرم بر سانتی‌متر مکعب", "پوند بر فوت مکعب" });
                    cbUnitTo.Items.AddRange(new string[] { "کیلوگرم بر متر مکعب", "گرم بر سانتی‌متر مکعب", "پوند بر فوت مکعب" });
                    break;
            }
            cbUnitFrom.SelectedIndex = 0;
            cbUnitTo.SelectedIndex = 1;
        }
        private void BtnConvertUnits_Click(object sender, MouseEventArgs e)
        {
            try
            {
                double value = double.Parse(txtUnitInput.Text);
                double result = 0;
                string category = cbUnitCategory.SelectedItem.ToString();
                switch (category)
                {
                    case "طول":
                        if (cbUnitFrom.SelectedItem.ToString() == "متر")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "سانتی‌متر") result = value * 100;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلومتر") result = value / 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "میلی‌متر") result = value * 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "مایل") result = value / 1609.344;
                            else if (cbUnitTo.SelectedItem.ToString() == "فوت") result = value * 3.28084;
                            else if (cbUnitTo.SelectedItem.ToString() == "اینچ") result = value * 39.3701;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "سانتی‌متر")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "متر") result = value / 100;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلومتر") result = value / 100000;
                            else if (cbUnitTo.SelectedItem.ToString() == "میلی‌متر") result = value * 10;
                            else if (cbUnitTo.SelectedItem.ToString() == "مایل") result = value / 160934.4;
                            else if (cbUnitTo.SelectedItem.ToString() == "فوت") result = value / 30.48;
                            else if (cbUnitTo.SelectedItem.ToString() == "اینچ") result = value / 2.54;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "کیلومتر")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "متر") result = value * 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "سانتی‌متر") result = value * 100000;
                            else if (cbUnitTo.SelectedItem.ToString() == "میلی‌متر") result = value * 1000000;
                            else if (cbUnitTo.SelectedItem.ToString() == "مایل") result = value / 1.609344;
                            else if (cbUnitTo.SelectedItem.ToString() == "فوت") result = value * 3280.84;
                            else if (cbUnitTo.SelectedItem.ToString() == "اینچ") result = value * 39370.1;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "میلی‌متر")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "متر") result = value / 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "سانتی‌متر") result = value / 10;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلومتر") result = value / 1000000;
                            else if (cbUnitTo.SelectedItem.ToString() == "مایل") result = value / 1609344;
                            else if (cbUnitTo.SelectedItem.ToString() == "فوت") result = value / 304.8;
                            else if (cbUnitTo.SelectedItem.ToString() == "اینچ") result = value / 25.4;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "مایل")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "متر") result = value * 1609.344;
                            else if (cbUnitTo.SelectedItem.ToString() == "سانتی‌متر") result = value * 160934.4;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلومتر") result = value * 1.609344;
                            else if (cbUnitTo.SelectedItem.ToString() == "میلی‌متر") result = value * 1609344;
                            else if (cbUnitTo.SelectedItem.ToString() == "فوت") result = value * 5280;
                            else if (cbUnitTo.SelectedItem.ToString() == "اینچ") result = value * 63360;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "فوت")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "متر") result = value / 3.28084;
                            else if (cbUnitTo.SelectedItem.ToString() == "سانتی‌متر") result = value * 30.48;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلومتر") result = value / 3280.84;
                            else if (cbUnitTo.SelectedItem.ToString() == "میلی‌متر") result = value * 304.8;
                            else if (cbUnitTo.SelectedItem.ToString() == "مایل") result = value / 5280;
                            else if (cbUnitTo.SelectedItem.ToString() == "اینچ") result = value * 12;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "اینچ")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "متر") result = value / 39.3701;
                            else if (cbUnitTo.SelectedItem.ToString() == "سانتی‌متر") result = value * 2.54;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلومتر") result = value / 39370.1;
                            else if (cbUnitTo.SelectedItem.ToString() == "میلی‌متر") result = value * 25.4;
                            else if (cbUnitTo.SelectedItem.ToString() == "مایل") result = value / 63360;
                            else if (cbUnitTo.SelectedItem.ToString() == "فوت") result = value / 12;
                            else result = value;
                        }
                        break;
                    case "مساحت":
                        if (cbUnitFrom.SelectedItem.ToString() == "متر مربع")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "سانتی‌متر مربع") result = value * 10000;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلومتر مربع") result = value / 1000000;
                            else if (cbUnitTo.SelectedItem.ToString() == "هکتار") result = value / 10000;
                            else if (cbUnitTo.SelectedItem.ToString() == "مایل مربع") result = value / 2589988;
                            else if (cbUnitTo.SelectedItem.ToString() == "فوت مربع") result = value * 10.7639;
                            else if (cbUnitTo.SelectedItem.ToString() == "اینچ مربع") result = value * 1550;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "سانتی‌متر مربع")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "متر مربع") result = value / 10000;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلومتر مربع") result = value / 10000000000;
                            else if (cbUnitTo.SelectedItem.ToString() == "هکتار") result = value / 100000000;
                            else if (cbUnitTo.SelectedItem.ToString() == "مایل مربع") result = value / 25899881103;
                            else if (cbUnitTo.SelectedItem.ToString() == "فوت مربع") result = value / 929.03;
                            else if (cbUnitTo.SelectedItem.ToString() == "اینچ مربع") result = value / 6.4516;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "کیلومتر مربع")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "متر مربع") result = value * 1000000;
                            else if (cbUnitTo.SelectedItem.ToString() == "سانتی‌متر مربع") result = value * 10000000000;
                            else if (cbUnitTo.SelectedItem.ToString() == "هکتار") result = value * 100;
                            else if (cbUnitTo.SelectedItem.ToString() == "مایل مربع") result = value / 2.589988;
                            else if (cbUnitTo.SelectedItem.ToString() == "فوت مربع") result = value * 10763910;
                            else if (cbUnitTo.SelectedItem.ToString() == "اینچ مربع") result = value * 1550003100;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "هکتار")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "متر مربع") result = value * 10000;
                            else if (cbUnitTo.SelectedItem.ToString() == "سانتی‌متر مربع") result = value * 100000000;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلومتر مربع") result = value / 100;
                            else if (cbUnitTo.SelectedItem.ToString() == "مایل مربع") result = value / 258.9988;
                            else if (cbUnitTo.SelectedItem.ToString() == "فوت مربع") result = value * 107639;
                            else if (cbUnitTo.SelectedItem.ToString() == "اینچ مربع") result = value * 15500031;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "مایل مربع")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "متر مربع") result = value * 2589988;
                            else if (cbUnitTo.SelectedItem.ToString() == "سانتی‌متر مربع") result = value * 25899881103;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلومتر مربع") result = value * 2.589988;
                            else if (cbUnitTo.SelectedItem.ToString() == "هکتار") result = value * 258.9988;
                            else if (cbUnitTo.SelectedItem.ToString() == "فوت مربع") result = value * 27878400;
                            else if (cbUnitTo.SelectedItem.ToString() == "اینچ مربع") result = value * 4014489600;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "فوت مربع")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "متر مربع") result = value / 10.7639;
                            else if (cbUnitTo.SelectedItem.ToString() == "سانتی‌متر مربع") result = value * 929.03;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلومتر مربع") result = value / 10763910;
                            else if (cbUnitTo.SelectedItem.ToString() == "هکتار") result = value / 107639;
                            else if (cbUnitTo.SelectedItem.ToString() == "مایل مربع") result = value / 27878400;
                            else if (cbUnitTo.SelectedItem.ToString() == "اینچ مربع") result = value * 144;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "اینچ مربع")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "متر مربع") result = value / 1550;
                            else if (cbUnitTo.SelectedItem.ToString() == "سانتی‌متر مربع") result = value * 6.4516;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلومتر مربع") result = value / 1550003100;
                            else if (cbUnitTo.SelectedItem.ToString() == "هکتار") result = value / 15500031;
                            else if (cbUnitTo.SelectedItem.ToString() == "مایل مربع") result = value / 4014489600;
                            else if (cbUnitTo.SelectedItem.ToString() == "فوت مربع") result = value / 144;
                            else result = value;
                        }
                        break;
                    case "حجم":
                        if (cbUnitFrom.SelectedItem.ToString() == "متر مکعب")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "سانتی‌متر مکعب") result = value * 1000000;
                            else if (cbUnitTo.SelectedItem.ToString() == "لیتر") result = value * 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "میلی‌لیتر") result = value * 1000000;
                            else if (cbUnitTo.SelectedItem.ToString() == "گالن") result = value * 264.172;
                            else if (cbUnitTo.SelectedItem.ToString() == "فوت مکعب") result = value * 35.3147;
                            else if (cbUnitTo.SelectedItem.ToString() == "اینچ مکعب") result = value * 61023.7;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "سانتی‌متر مکعب")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "متر مکعب") result = value / 1000000;
                            else if (cbUnitTo.SelectedItem.ToString() == "لیتر") result = value / 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "میلی‌لیتر") result = value;
                            else if (cbUnitTo.SelectedItem.ToString() == "گالن") result = value / 3785.41;
                            else if (cbUnitTo.SelectedItem.ToString() == "فوت مکعب") result = value / 28316.8;
                            else if (cbUnitTo.SelectedItem.ToString() == "اینچ مکعب") result = value / 16.3871;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "لیتر")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "متر مکعب") result = value / 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "سانتی‌متر مکعب") result = value * 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "میلی‌لیتر") result = value * 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "گالن") result = value / 3.78541;
                            else if (cbUnitTo.SelectedItem.ToString() == "فوت مکعب") result = value / 28.3168;
                            else if (cbUnitTo.SelectedItem.ToString() == "اینچ مکعب") result = value * 61.0237;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "میلی‌لیتر")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "متر مکعب") result = value / 1000000;
                            else if (cbUnitTo.SelectedItem.ToString() == "سانتی‌متر مکعب") result = value;
                            else if (cbUnitTo.SelectedItem.ToString() == "لیتر") result = value / 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "گالن") result = value / 3785.41;
                            else if (cbUnitTo.SelectedItem.ToString() == "فوت مکعب") result = value / 28316.8;
                            else if (cbUnitTo.SelectedItem.ToString() == "اینچ مکعب") result = value / 16.3871;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "گالن")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "متر مکعب") result = value / 264.172;
                            else if (cbUnitTo.SelectedItem.ToString() == "سانتی‌متر مکعب") result = value * 3785.41;
                            else if (cbUnitTo.SelectedItem.ToString() == "لیتر") result = value * 3.78541;
                            else if (cbUnitTo.SelectedItem.ToString() == "میلی‌لیتر") result = value * 3785.41;
                            else if (cbUnitTo.SelectedItem.ToString() == "فوت مکعب") result = value / 7.48052;
                            else if (cbUnitTo.SelectedItem.ToString() == "اینچ مکعب") result = value * 231;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "فوت مکعب")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "متر مکعب") result = value / 35.3147;
                            else if (cbUnitTo.SelectedItem.ToString() == "سانتی‌متر مکعب") result = value * 28316.8;
                            else if (cbUnitTo.SelectedItem.ToString() == "لیتر") result = value * 28.3168;
                            else if (cbUnitTo.SelectedItem.ToString() == "میلی‌لیتر") result = value * 28316.8;
                            else if (cbUnitTo.SelectedItem.ToString() == "گالن") result = value * 7.48052;
                            else if (cbUnitTo.SelectedItem.ToString() == "اینچ مکعب") result = value * 1728;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "اینچ مکعب")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "متر مکعب") result = value / 61023.7;
                            else if (cbUnitTo.SelectedItem.ToString() == "سانتی‌متر مکعب") result = value * 16.3871;
                            else if (cbUnitTo.SelectedItem.ToString() == "لیتر") result = value / 61.0237;
                            else if (cbUnitTo.SelectedItem.ToString() == "میلی‌لیتر") result = value * 16.3871;
                            else if (cbUnitTo.SelectedItem.ToString() == "گالن") result = value / 231;
                            else if (cbUnitTo.SelectedItem.ToString() == "فوت مکعب") result = value / 1728;
                            else result = value;
                        }
                        break;
                    case "وزن":
                        if (cbUnitFrom.SelectedItem.ToString() == "کیلوگرم")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "گرم") result = value * 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "تن") result = value / 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "پوند") result = value * 2.20462;
                            else if (cbUnitTo.SelectedItem.ToString() == "اونس") result = value * 35.274;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "گرم")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "کیلوگرم") result = value / 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "تن") result = value / 1000000;
                            else if (cbUnitTo.SelectedItem.ToString() == "پوند") result = value / 453.592;
                            else if (cbUnitTo.SelectedItem.ToString() == "اونس") result = value / 28.3495;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "تن")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "کیلوگرم") result = value * 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "گرم") result = value * 1000000;
                            else if (cbUnitTo.SelectedItem.ToString() == "پوند") result = value * 2204.62;
                            else if (cbUnitTo.SelectedItem.ToString() == "اونس") result = value * 35274;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "پوند")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "کیلوگرم") result = value / 2.20462;
                            else if (cbUnitTo.SelectedItem.ToString() == "گرم") result = value * 453.592;
                            else if (cbUnitTo.SelectedItem.ToString() == "تن") result = value / 2204.62;
                            else if (cbUnitTo.SelectedItem.ToString() == "اونس") result = value * 16;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "اونس")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "کیلوگرم") result = value / 35.274;
                            else if (cbUnitTo.SelectedItem.ToString() == "گرم") result = value * 28.3495;
                            else if (cbUnitTo.SelectedItem.ToString() == "تن") result = value / 35274;
                            else if (cbUnitTo.SelectedItem.ToString() == "پوند") result = value / 16;
                            else result = value;
                        }
                        break;
                    case "دما":
                        if (cbUnitFrom.SelectedItem.ToString() == "سانتی‌گراد")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "فارنهایت") result = (value * 9 / 5) + 32;
                            else if (cbUnitTo.SelectedItem.ToString() == "کلوین") result = value + 273.15;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "فارنهایت")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "سانتی‌گراد") result = (value - 32) * 5 / 9;
                            else if (cbUnitTo.SelectedItem.ToString() == "کلوین") result = (value - 32) * 5 / 9 + 273.15;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "کلوین")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "سانتی‌گراد") result = value - 273.15;
                            else if (cbUnitTo.SelectedItem.ToString() == "فارنهایت") result = (value - 273.15) * 9 / 5 + 32;
                            else result = value;
                        }
                        break;
                    case "سرعت":
                        if (cbUnitFrom.SelectedItem.ToString() == "متر بر ثانیه")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "کیلومتر بر ساعت") result = value * 3.6;
                            else if (cbUnitTo.SelectedItem.ToString() == "مایل بر ساعت") result = value * 2.23694;
                            else if (cbUnitTo.SelectedItem.ToString() == "گره") result = value * 1.94384;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "کیلومتر بر ساعت")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "متر بر ثانیه") result = value / 3.6;
                            else if (cbUnitTo.SelectedItem.ToString() == "مایل بر ساعت") result = value / 1.609344;
                            else if (cbUnitTo.SelectedItem.ToString() == "گره") result = value / 1.852;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "مایل بر ساعت")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "متر بر ثانیه") result = value / 2.23694;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلومتر بر ساعت") result = value * 1.609344;
                            else if (cbUnitTo.SelectedItem.ToString() == "گره") result = value / 1.15078;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "گره")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "متر بر ثانیه") result = value / 1.94384;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلومتر بر ساعت") result = value * 1.852;
                            else if (cbUnitTo.SelectedItem.ToString() == "مایل بر ساعت") result = value * 1.15078;
                            else result = value;
                        }
                        break;
                    case "زمان":
                        if (cbUnitFrom.SelectedItem.ToString() == "ثانیه")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "دقیقه") result = value / 60;
                            else if (cbUnitTo.SelectedItem.ToString() == "ساعت") result = value / 3600;
                            else if (cbUnitTo.SelectedItem.ToString() == "روز") result = value / 86400;
                            else if (cbUnitTo.SelectedItem.ToString() == "هفته") result = value / 604800;
                            else if (cbUnitTo.SelectedItem.ToString() == "ماه") result = value / 2629800;
                            else if (cbUnitTo.SelectedItem.ToString() == "سال") result = value / 31557600;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "دقیقه")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "ثانیه") result = value * 60;
                            else if (cbUnitTo.SelectedItem.ToString() == "ساعت") result = value / 60;
                            else if (cbUnitTo.SelectedItem.ToString() == "روز") result = value / 1440;
                            else if (cbUnitTo.SelectedItem.ToString() == "هفته") result = value / 10080;
                            else if (cbUnitTo.SelectedItem.ToString() == "ماه") result = value / 43830;
                            else if (cbUnitTo.SelectedItem.ToString() == "سال") result = value / 525960;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "ساعت")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "ثانیه") result = value * 3600;
                            else if (cbUnitTo.SelectedItem.ToString() == "دقیقه") result = value * 60;
                            else if (cbUnitTo.SelectedItem.ToString() == "روز") result = value / 24;
                            else if (cbUnitTo.SelectedItem.ToString() == "هفته") result = value / 168;
                            else if (cbUnitTo.SelectedItem.ToString() == "ماه") result = value / 730.5;
                            else if (cbUnitTo.SelectedItem.ToString() == "سال") result = value / 8766;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "روز")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "ثانیه") result = value * 86400;
                            else if (cbUnitTo.SelectedItem.ToString() == "دقیقه") result = value * 1440;
                            else if (cbUnitTo.SelectedItem.ToString() == "ساعت") result = value * 24;
                            else if (cbUnitTo.SelectedItem.ToString() == "هفته") result = value / 7;
                            else if (cbUnitTo.SelectedItem.ToString() == "ماه") result = value / 30.417;
                            else if (cbUnitTo.SelectedItem.ToString() == "سال") result = value / 365;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "هفته")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "ثانیه") result = value * 604800;
                            else if (cbUnitTo.SelectedItem.ToString() == "دقیقه") result = value * 10080;
                            else if (cbUnitTo.SelectedItem.ToString() == "ساعت") result = value * 168;
                            else if (cbUnitTo.SelectedItem.ToString() == "روز") result = value * 7;
                            else if (cbUnitTo.SelectedItem.ToString() == "ماه") result = value / 4.345;
                            else if (cbUnitTo.SelectedItem.ToString() == "سال") result = value / 52.143;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "ماه")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "ثانیه") result = value * 2629800;
                            else if (cbUnitTo.SelectedItem.ToString() == "دقیقه") result = value * 43830;
                            else if (cbUnitTo.SelectedItem.ToString() == "ساعت") result = value * 730.5;
                            else if (cbUnitTo.SelectedItem.ToString() == "روز") result = value * 30.417;
                            else if (cbUnitTo.SelectedItem.ToString() == "هفته") result = value * 4.345;
                            else if (cbUnitTo.SelectedItem.ToString() == "سال") result = value / 12;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "سال")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "ثانیه") result = value * 31557600;
                            else if (cbUnitTo.SelectedItem.ToString() == "دقیقه") result = value * 525960;
                            else if (cbUnitTo.SelectedItem.ToString() == "ساعت") result = value * 8766;
                            else if (cbUnitTo.SelectedItem.ToString() == "روز") result = value * 365;
                            else if (cbUnitTo.SelectedItem.ToString() == "هفته") result = value * 52.143;
                            else if (cbUnitTo.SelectedItem.ToString() == "ماه") result = value * 12;
                            else result = value;
                        }
                        break;
                    case "انرژی":
                        if (cbUnitFrom.SelectedItem.ToString() == "ژول")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "کیلوژول") result = value / 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "کالری") result = value / 4.184;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلوکالری") result = value / 4184;
                            else if (cbUnitTo.SelectedItem.ToString() == "وات-ساعت") result = value / 3600;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "کیلوژول")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "ژول") result = value * 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "کالری") result = value * 239;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلوکالری") result = value / 4.184;
                            else if (cbUnitTo.SelectedItem.ToString() == "وات-ساعت") result = value / 3.6;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "کالری")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "ژول") result = value * 4.184;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلوژول") result = value / 239;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلوکالری") result = value / 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "وات-ساعت") result = value / 860;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "کیلوکالری")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "ژول") result = value * 4184;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلوژول") result = value * 4.184;
                            else if (cbUnitTo.SelectedItem.ToString() == "کالری") result = value * 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "وات-ساعت") result = value / 0.86;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "وات-ساعت")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "ژول") result = value * 3600;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلوژول") result = value * 3.6;
                            else if (cbUnitTo.SelectedItem.ToString() == "کالری") result = value * 860;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلوکالری") result = value * 0.86;
                            else result = value;
                        }
                        break;
                    case "فشار":
                        if (cbUnitFrom.SelectedItem.ToString() == "پاسکال")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "کیلوپاسکال") result = value / 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "بار") result = value / 100000;
                            else if (cbUnitTo.SelectedItem.ToString() == "پوند بر اینچ مربع") result = value / 6894.76;
                            else if (cbUnitTo.SelectedItem.ToString() == "اتمسفر") result = value / 101325;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "کیلوپاسکال")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "پاسکال") result = value * 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "بار") result = value / 100;
                            else if (cbUnitTo.SelectedItem.ToString() == "پوند بر اینچ مربع") result = value / 6.89476;
                            else if (cbUnitTo.SelectedItem.ToString() == "اتمسفر") result = value / 101.325;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "بار")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "پاسکال") result = value * 100000;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلوپاسکال") result = value * 100;
                            else if (cbUnitTo.SelectedItem.ToString() == "پوند بر اینچ مربع") result = value * 14.5038;
                            else if (cbUnitTo.SelectedItem.ToString() == "اتمسفر") result = value / 1.01325;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "پوند بر اینچ مربع")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "پاسکال") result = value * 6894.76;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلوپاسکال") result = value * 6.89476;
                            else if (cbUnitTo.SelectedItem.ToString() == "بار") result = value / 14.5038;
                            else if (cbUnitTo.SelectedItem.ToString() == "اتمسفر") result = value / 14.6959;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "اتمسفر")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "پاسکال") result = value * 101325;
                            else if (cbUnitTo.SelectedItem.ToString() == "کیلوپاسکال") result = value * 101.325;
                            else if (cbUnitTo.SelectedItem.ToString() == "بار") result = value * 1.01325;
                            else if (cbUnitTo.SelectedItem.ToString() == "پوند بر اینچ مربع") result = value * 14.6959;
                            else result = value;
                        }
                        break;
                    case "چگالی":
                        if (cbUnitFrom.SelectedItem.ToString() == "کیلوگرم بر متر مکعب")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "گرم بر سانتی‌متر مکعب") result = value / 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "پوند بر فوت مکعب") result = value / 16.0185;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "گرم بر سانتی‌متر مکعب")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "کیلوگرم بر متر مکعب") result = value * 1000;
                            else if (cbUnitTo.SelectedItem.ToString() == "پوند بر فوت مکعب") result = value * 62.42796;
                            else result = value;
                        }
                        else if (cbUnitFrom.SelectedItem.ToString() == "پوند بر فوت مکعب")
                        {
                            if (cbUnitTo.SelectedItem.ToString() == "کیلوگرم بر متر مکعب") result = value * 16.0185;
                            else if (cbUnitTo.SelectedItem.ToString() == "گرم بر سانتی‌متر مکعب") result = value / 62.42796;
                            else result = value;
                        }
                        break;
                }
                txtUnitOutput.Text = result.ToString();
            }
            catch (Exception ex)
            {
                txtUnitOutput.Text = "خطا: " + ex.Message;
            }
        }
        private void Mouse_hand(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }
        private void Mouse_leave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }
        private void SettingButton_MouseClick(object sender, MouseEventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm(this);
            settingsForm.ShowDialog();
        }
        private void LoadSettingsOnStartup()
        {
            try
            {
                if (System.IO.File.Exists("settings.json"))
                {
                    string json = System.IO.File.ReadAllText("settings.json");
                    var settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    if (settings.TryGetValue("Opacity", out string opacityStr) && double.TryParse(opacityStr, out double opacity))
                    {
                        this.Opacity = opacity;
                    }
                    else
                    {
                        this.Opacity = 1.0; 
                    }
                    if (settings.TryGetValue("TopMost", out string topMostStr) && bool.TryParse(topMostStr, out bool topMost))
                    {
                        this.TopMost = topMost;
                    }
                    else
                    {
                        this.TopMost = false; 
                    }
                    if (settings.TryGetValue("BackgroundColor", out string bgColorStr) && int.TryParse(bgColorStr, out int bgColorArgb))
                    {
                        this.BackColor = Color.FromArgb(bgColorArgb);
                    }
                    else
                    {
                        this.BackColor = SystemColors.Control; 
                    }
                    if (settings.TryGetValue("ForegroundColor", out string fgColorStr) && int.TryParse(fgColorStr, out int fgColorArgb))
                    {
                        Color fgColor = Color.FromArgb(fgColorArgb);
                        foreach (Control control in this.Controls)
                        {
                            control.ForeColor = fgColor;
                        }
                    }
                    else
                    {
                        foreach (Control control in this.Controls)
                        {
                            control.ForeColor = Color.Black; 
                        }
                    }
                }
                else
                {
                    this.Opacity = 1.0;
                    this.TopMost = false;
                    this.BackColor = SystemColors.Control;
                    foreach (Control control in this.Controls)
                    {
                        control.ForeColor = Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Opacity = 1.0;
                this.TopMost = false;
                this.BackColor = SystemColors.Control;
                foreach (Control control in this.Controls)
                {
                    control.ForeColor = Color.Black;
                }
                MessageBox.Show("خطا در لود تنظیمات: " + ex.Message);
            }
        }
        private void InitializeMusicControls()
        {
            try
            {
                musicPlayer = new MusicPlayer();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در مقداردهی اولیه پخش‌کننده موزیک: " + ex.Message);
                return;
            }
            soundButton = new Button
            {
                Text = "پخش صدا",
                Location = new Point(900, 8),
                Size = new Size(100, 40),
                Font = new Font("Arial", 15),
                BackColor = Color.FromArgb(100,250,60,40),
                Visible = true
            };
            soundButton.MouseClick += SoundButton_MouseClick;
            soundButton.MouseEnter += MouseHand;
            soundButton.MouseLeave += MouseDefualt;
            volumeArrowButton = new Button
            {
                Text = "↓",
                Location = new Point(soundButton.Right + 5, soundButton.Top),
                Size = new Size(30, 40),
                Font = new Font("Arial", 15),
                BackColor = Color.FromArgb(100, 250, 60, 40),
                Visible = true
            };
            volumeArrowButton.MouseClick += VolumeArrowButton_MouseClick;
            volumeArrowButton.MouseEnter += MouseHand;
            volumeArrowButton.MouseLeave += MouseDefualt;
            volumeTrackBar = new TrackBar
            {
                Location = new Point(soundButton.Left, soundButton.Bottom + 5),
                Size = new Size(100, 30),
                Minimum = 0,
                Maximum = 100,
                Value = musicPlayer.GetVolume(),
                Visible = false
            };
            volumeTrackBar.Scroll += VolumeTrackBar_Scroll;
            autoPlayTimer = new Timer();
            autoPlayTimer.Interval = 8000; 
            autoPlayTimer.Tick += (s, e) =>
            {
                if (isAutoPlayEnabled && !musicPlayer.IsPlaying())
                {
                    musicPlayer.PlayRandomSong(true);
                    soundButton.Text = "توقف صدا";
                    Console.WriteLine("پخش خودکار بعد از 8 ثانیه اجرا شد");
                }
                autoPlayTimer.Stop();
            };
            autoPlayTimer.Start();
            soundButton.Visible = true;
            volumeTrackBar.Visible = true;
            volumeArrowButton.Visible = true;
            this.Controls.Add(soundButton);
            this.Controls.Add(volumeArrowButton);
            this.Controls.Add(volumeTrackBar);
            soundButton.Visible = true;
            volumeTrackBar.Visible = true;
            volumeArrowButton.Visible = true;
        }
        private void SoundButton_MouseClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine($"مقدار فعلی ولوم: {musicPlayer.GetVolume()}"); 
            Console.WriteLine($"مقدار TrackBar: {volumeTrackBar.Value}");
            if (soundButton.Text == "توقف صدا")
            {
                musicPlayer.Stop();
                soundButton.Text = "پخش صدا";
                isAutoPlayEnabled = false;
                musicPlayer.SetAutoPlayEnabled(false);
                Console.WriteLine("پخش متوقف شد");
            }
            else
            {
                isAutoPlayEnabled = true;
                musicPlayer.PlayRandomSong(true); 
                soundButton.Text = "توقف صدا";
                Console.WriteLine("پخش دستی شروع شد");
            }
        }
        private void VolumeArrowButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (volumeArrowButton.Text.ToString() == "↑")
            {
                volumeArrowButton.Text = "↓";
                volumeTrackBar.Visible = false;
            }
            else if (volumeArrowButton.Text.ToString() == "↓")
            {
                volumeArrowButton.Text = "↑";
                volumeTrackBar.Visible = true;
            }
        }
        private void VolumeTrackBar_Scroll(object sender, EventArgs e)
        {
            musicPlayer.SetVolume(volumeTrackBar.Value);
            Console.WriteLine($"ولوم تنظیم شد: {volumeTrackBar.Value}%");
            Console.WriteLine($"ولوم واقعی بعد از تنظیم: {musicPlayer.GetVolume()}%");
        }
        private new void Show()
        {
            soundButton.Visible = true;
            volumeArrowButton.Visible = true;
            volumeTrackBar.Visible = true;
        }
        private void MemoryClean()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        private void MemoryClean2(object sender, EventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        static async Task UpdateCheck()
        {
            string repoUrl = "https://api.github.com/repos/AliAgaAbd/GUI-Professional-calculator/releases/latest";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");
                HttpResponseMessage response = await client.GetAsync(repoUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JsonDocument json = JsonDocument.Parse(responseBody);
                    string latestVersion = json.RootElement.GetProperty("tag_name").GetString();
                    if (latestVersion != "V1.3.10")
                    {
                        MessageBox.Show($"نرم افزار شما بروز نیست لطفا بروزرسانی فرمایید.\nآخرین نسخه: {latestVersion}\nباز کردن مرورگر بعد از 3 ثانیه");
                        UpdateVersion();
                    }
                }
                else
                {
                    Console.WriteLine("خطا در دریافت نسخه!");
                }
            }
        }
        private static void UpdateVersion()
        {
            UpdateProgram = new Timer();
            UpdateProgram.Interval = 3000;
            UpdateProgram.Tick += Browser_open;
            UpdateProgram.Start();
        }
        private static void Browser_open(object sender, EventArgs e)
        {
            string UpdateURL = "https://github.com/AliAgaAbd/GUI-Professional-calculator/releases/latest";
            Process.Start(new ProcessStartInfo
            {
                FileName = UpdateURL,
                UseShellExecute = true
            });
            UpdateProgram.Stop();
        }
    }
}