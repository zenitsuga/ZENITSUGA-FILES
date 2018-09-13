namespace RPOSWI
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
            this.pnlLogin = new System.Windows.Forms.Panel();
            this.pnlLoginUser = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.keyboardShortcutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlSideMenu = new System.Windows.Forms.Panel();
            this.pnlLogin.SuspendLayout();
            this.pnlLoginUser.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLogin
            // 
            this.pnlLogin.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlLogin.Controls.Add(this.pnlSideMenu);
            this.pnlLogin.Controls.Add(this.pnlLoginUser);
            this.pnlLogin.Controls.Add(this.menuStrip1);
            this.pnlLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLogin.Location = new System.Drawing.Point(0, 0);
            this.pnlLogin.Name = "pnlLogin";
            this.pnlLogin.Size = new System.Drawing.Size(870, 404);
            this.pnlLogin.TabIndex = 0;
            // 
            // pnlLoginUser
            // 
            this.pnlLoginUser.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pnlLoginUser.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pnlLoginUser.Controls.Add(this.groupBox1);
            this.pnlLoginUser.Location = new System.Drawing.Point(248, 277);
            this.pnlLoginUser.Name = "pnlLoginUser";
            this.pnlLoginUser.Size = new System.Drawing.Size(377, 124);
            this.pnlLoginUser.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(371, 118);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Login";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(151, 77);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 28);
            this.button1.TabIndex = 4;
            this.button1.Text = "Login";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(85, 45);
            this.textBox2.Name = "textBox2";
            this.textBox2.PasswordChar = '*';
            this.textBox2.Size = new System.Drawing.Size(263, 20);
            this.textBox2.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(85, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(263, 20);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keyboardShortcutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(870, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // keyboardShortcutToolStripMenuItem
            // 
            this.keyboardShortcutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurationSettingsToolStripMenuItem});
            this.keyboardShortcutToolStripMenuItem.Name = "keyboardShortcutToolStripMenuItem";
            this.keyboardShortcutToolStripMenuItem.Size = new System.Drawing.Size(117, 20);
            this.keyboardShortcutToolStripMenuItem.Text = "Keyboard Shortcut";
            // 
            // configurationSettingsToolStripMenuItem
            // 
            this.configurationSettingsToolStripMenuItem.Name = "configurationSettingsToolStripMenuItem";
            this.configurationSettingsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.configurationSettingsToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.configurationSettingsToolStripMenuItem.Text = "Configuration Settings";
            this.configurationSettingsToolStripMenuItem.Click += new System.EventHandler(this.configurationSettingsToolStripMenuItem_Click);
            // 
            // pnlSideMenu
            // 
            this.pnlSideMenu.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pnlSideMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSideMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlSideMenu.Name = "pnlSideMenu";
            this.pnlSideMenu.Size = new System.Drawing.Size(219, 404);
            this.pnlSideMenu.TabIndex = 2;
            this.pnlSideMenu.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 404);
            this.Controls.Add(this.pnlLogin);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "[CoName] System";
            this.pnlLogin.ResumeLayout(false);
            this.pnlLogin.PerformLayout();
            this.pnlLoginUser.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLogin;
        private System.Windows.Forms.Panel pnlLoginUser;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem keyboardShortcutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configurationSettingsToolStripMenuItem;
        private System.Windows.Forms.Panel pnlSideMenu;
    }
}

