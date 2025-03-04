namespace Professional_calculator
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CalculatorButton = new System.Windows.Forms.Button();
            this.UnitConversationButton = new System.Windows.Forms.Button();
            this.BMIButton = new System.Windows.Forms.Button();
            this.AIButton = new System.Windows.Forms.Button();
            this.HelpButton = new System.Windows.Forms.Button();
            this.HomeButton = new System.Windows.Forms.Button();
            this.SettingButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // CalculatorButton
            // 
            this.CalculatorButton.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.CalculatorButton, "CalculatorButton");
            this.CalculatorButton.Name = "CalculatorButton";
            this.CalculatorButton.UseVisualStyleBackColor = true;
            // 
            // UnitConversationButton
            // 
            this.UnitConversationButton.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.UnitConversationButton, "UnitConversationButton");
            this.UnitConversationButton.Name = "UnitConversationButton";
            this.UnitConversationButton.UseVisualStyleBackColor = true;
            // 
            // BMIButton
            // 
            this.BMIButton.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.BMIButton, "BMIButton");
            this.BMIButton.Name = "BMIButton";
            this.BMIButton.UseVisualStyleBackColor = true;
            this.BMIButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BMIButton_MouseClick);
            // 
            // AIButton
            // 
            this.AIButton.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.AIButton, "AIButton");
            this.AIButton.Name = "AIButton";
            this.AIButton.UseVisualStyleBackColor = true;
            this.AIButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.AIButton_MouseClick);
            // 
            // HelpButton
            // 
            this.HelpButton.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.HelpButton, "HelpButton");
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.UseVisualStyleBackColor = true;
            this.HelpButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.HelpButton_MouseClick);
            // 
            // HomeButton
            // 
            this.HomeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.HomeButton, "HomeButton");
            this.HomeButton.Name = "HomeButton";
            this.HomeButton.UseVisualStyleBackColor = true;
            this.HomeButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.HomeButton_MouseClick);
            // 
            // SettingButton
            // 
            resources.ApplyResources(this.SettingButton, "SettingButton");
            this.SettingButton.Name = "SettingButton";
            this.SettingButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.SettingButton);
            this.Controls.Add(this.HomeButton);
            this.Controls.Add(this.HelpButton);
            this.Controls.Add(this.AIButton);
            this.Controls.Add(this.BMIButton);
            this.Controls.Add(this.UnitConversationButton);
            this.Controls.Add(this.CalculatorButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CalculatorButton;
        private System.Windows.Forms.Button UnitConversationButton;
        private System.Windows.Forms.Button BMIButton;
        private System.Windows.Forms.Button AIButton;
        private System.Windows.Forms.Button HelpButton;
        private System.Windows.Forms.Button HomeButton;
        public System.Windows.Forms.ComboBox ModelSelect;
        private System.Windows.Forms.Button SettingButton;
    }
}

