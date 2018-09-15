namespace ZBilling
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.systemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsTransaction = new System.Windows.Forms.ToolStripMenuItem();
            this.createTransactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsManageDB = new System.Windows.Forms.ToolStripMenuItem();
            this.usersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.roomsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssDatabaseStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssUserlogin = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel9 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssUserRole = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel11 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssLicenseStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssToday = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.tssTransaction = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.tssManageDB = new System.Windows.Forms.ToolStripSplitButton();
            this.userRoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usersToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.customerProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tenantProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chartOfAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.roomsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.roomAssignmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.tmrToday = new System.Windows.Forms.Timer(this.components);
            this.dateConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tssPayment = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemToolStripMenuItem,
            this.tsTransaction,
            this.tsManageDB,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(898, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // systemToolStripMenuItem
            // 
            this.systemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loginToolStripMenuItem,
            this.toolStripSeparator1,
            this.settingsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.systemToolStripMenuItem.Name = "systemToolStripMenuItem";
            this.systemToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.systemToolStripMenuItem.Text = "System";
            // 
            // loginToolStripMenuItem
            // 
            this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            this.loginToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.loginToolStripMenuItem.Text = "&Login";
            this.loginToolStripMenuItem.Click += new System.EventHandler(this.loginToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(113, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.settingsToolStripMenuItem.Text = "&Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // tsTransaction
            // 
            this.tsTransaction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createTransactionToolStripMenuItem});
            this.tsTransaction.Name = "tsTransaction";
            this.tsTransaction.Size = new System.Drawing.Size(81, 20);
            this.tsTransaction.Text = "Transaction";
            this.tsTransaction.Visible = false;
            // 
            // createTransactionToolStripMenuItem
            // 
            this.createTransactionToolStripMenuItem.Name = "createTransactionToolStripMenuItem";
            this.createTransactionToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.createTransactionToolStripMenuItem.Text = "Create &Transaction";
            this.createTransactionToolStripMenuItem.Click += new System.EventHandler(this.createTransactionToolStripMenuItem_Click);
            // 
            // tsManageDB
            // 
            this.tsManageDB.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usersToolStripMenuItem,
            this.userToolStripMenuItem,
            this.roomsToolStripMenuItem1});
            this.tsManageDB.Name = "tsManageDB";
            this.tsManageDB.Size = new System.Drawing.Size(62, 20);
            this.tsManageDB.Text = "Manage";
            this.tsManageDB.Visible = false;
            // 
            // usersToolStripMenuItem
            // 
            this.usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            this.usersToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.usersToolStripMenuItem.Text = "Roles";
            this.usersToolStripMenuItem.Click += new System.EventHandler(this.usersToolStripMenuItem_Click);
            // 
            // userToolStripMenuItem
            // 
            this.userToolStripMenuItem.Name = "userToolStripMenuItem";
            this.userToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.userToolStripMenuItem.Text = "User";
            this.userToolStripMenuItem.Click += new System.EventHandler(this.userToolStripMenuItem_Click);
            // 
            // roomsToolStripMenuItem1
            // 
            this.roomsToolStripMenuItem1.Name = "roomsToolStripMenuItem1";
            this.roomsToolStripMenuItem1.Size = new System.Drawing.Size(111, 22);
            this.roomsToolStripMenuItem1.Text = "Rooms";
            this.roomsToolStripMenuItem1.Click += new System.EventHandler(this.roomsToolStripMenuItem1_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem,
            this.toolStripSeparator2,
            this.aboutToolStripMenuItem1});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(104, 6);
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem1.Text = "&About";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.tssDatabaseStatus,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4,
            this.tssUserlogin,
            this.toolStripStatusLabel9,
            this.tssUserRole,
            this.toolStripStatusLabel6,
            this.toolStripStatusLabel11,
            this.tssLicenseStatus,
            this.toolStripStatusLabel7,
            this.tssToday});
            this.statusStrip1.Location = new System.Drawing.Point(0, 447);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(898, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(42, 17);
            this.toolStripStatusLabel1.Text = "Status:";
            // 
            // tssDatabaseStatus
            // 
            this.tssDatabaseStatus.Name = "tssDatabaseStatus";
            this.tssDatabaseStatus.Size = new System.Drawing.Size(79, 17);
            this.tssDatabaseStatus.Text = "Disconnected";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(109, 17);
            this.toolStripStatusLabel3.Spring = true;
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(36, 17);
            this.toolStripStatusLabel4.Text = "User: ";
            // 
            // tssUserlogin
            // 
            this.tssUserlogin.Name = "tssUserlogin";
            this.tssUserlogin.Size = new System.Drawing.Size(58, 17);
            this.tssUserlogin.Text = "Unknown";
            this.tssUserlogin.TextChanged += new System.EventHandler(this.tssUserlogin_TextChanged);
            // 
            // toolStripStatusLabel9
            // 
            this.toolStripStatusLabel9.Name = "toolStripStatusLabel9";
            this.toolStripStatusLabel9.Size = new System.Drawing.Size(109, 17);
            this.toolStripStatusLabel9.Spring = true;
            // 
            // tssUserRole
            // 
            this.tssUserRole.Name = "tssUserRole";
            this.tssUserRole.Size = new System.Drawing.Size(93, 17);
            this.tssUserRole.Text = "Role: Not Found";
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(109, 17);
            this.toolStripStatusLabel6.Spring = true;
            // 
            // toolStripStatusLabel11
            // 
            this.toolStripStatusLabel11.Name = "toolStripStatusLabel11";
            this.toolStripStatusLabel11.Size = new System.Drawing.Size(49, 17);
            this.toolStripStatusLabel11.Text = "License:";
            // 
            // tssLicenseStatus
            // 
            this.tssLicenseStatus.Name = "tssLicenseStatus";
            this.tssLicenseStatus.Size = new System.Drawing.Size(109, 17);
            this.tssLicenseStatus.Spring = true;
            this.tssLicenseStatus.Text = "Invalid";
            this.tssLicenseStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(34, 17);
            this.toolStripStatusLabel7.Text = "Date:";
            // 
            // tssToday
            // 
            this.tssToday.Name = "tssToday";
            this.tssToday.Size = new System.Drawing.Size(54, 17);
            this.tssToday.Text = "Today is ";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(50, 50);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.tssTransaction,
            this.tssPayment,
            this.toolStripButton2,
            this.toolStripButton4,
            this.tssManageDB,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(898, 72);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(54, 69);
            this.toolStripButton1.Text = "Login";
            this.toolStripButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton1.ToolTipText = "Login User";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // tssTransaction
            // 
            this.tssTransaction.Image = ((System.Drawing.Image)(resources.GetObject("tssTransaction.Image")));
            this.tssTransaction.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssTransaction.Name = "tssTransaction";
            this.tssTransaction.Size = new System.Drawing.Size(73, 69);
            this.tssTransaction.Text = "Transaction";
            this.tssTransaction.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tssTransaction.Visible = false;
            this.tssTransaction.Click += new System.EventHandler(this.tssTransaction_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(54, 69);
            this.toolStripButton2.Text = "Settings";
            this.toolStripButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(54, 69);
            this.toolStripButton4.Text = "Help";
            this.toolStripButton4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // tssManageDB
            // 
            this.tssManageDB.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userRoleToolStripMenuItem,
            this.usersToolStripMenuItem1,
            this.customerProfileToolStripMenuItem,
            this.chartOfAccountToolStripMenuItem,
            this.roomsToolStripMenuItem,
            this.roomAssignmentToolStripMenuItem,
            this.dateConfigurationToolStripMenuItem});
            this.tssManageDB.Image = ((System.Drawing.Image)(resources.GetObject("tssManageDB.Image")));
            this.tssManageDB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssManageDB.Name = "tssManageDB";
            this.tssManageDB.Size = new System.Drawing.Size(84, 69);
            this.tssManageDB.Text = "Manage DB";
            this.tssManageDB.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tssManageDB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tssManageDB.Visible = false;
            // 
            // userRoleToolStripMenuItem
            // 
            this.userRoleToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("userRoleToolStripMenuItem.Image")));
            this.userRoleToolStripMenuItem.Name = "userRoleToolStripMenuItem";
            this.userRoleToolStripMenuItem.Size = new System.Drawing.Size(209, 56);
            this.userRoleToolStripMenuItem.Text = "User Role";
            this.userRoleToolStripMenuItem.Click += new System.EventHandler(this.userRoleToolStripMenuItem_Click);
            // 
            // usersToolStripMenuItem1
            // 
            this.usersToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("usersToolStripMenuItem1.Image")));
            this.usersToolStripMenuItem1.Name = "usersToolStripMenuItem1";
            this.usersToolStripMenuItem1.Size = new System.Drawing.Size(209, 56);
            this.usersToolStripMenuItem1.Text = "Users";
            this.usersToolStripMenuItem1.Click += new System.EventHandler(this.usersToolStripMenuItem1_Click);
            // 
            // customerProfileToolStripMenuItem
            // 
            this.customerProfileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tenantProfileToolStripMenuItem});
            this.customerProfileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("customerProfileToolStripMenuItem.Image")));
            this.customerProfileToolStripMenuItem.Name = "customerProfileToolStripMenuItem";
            this.customerProfileToolStripMenuItem.Size = new System.Drawing.Size(209, 56);
            this.customerProfileToolStripMenuItem.Text = "&Owner Profile";
            this.customerProfileToolStripMenuItem.Click += new System.EventHandler(this.customerProfileToolStripMenuItem_Click);
            // 
            // tenantProfileToolStripMenuItem
            // 
            this.tenantProfileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("tenantProfileToolStripMenuItem.Image")));
            this.tenantProfileToolStripMenuItem.Name = "tenantProfileToolStripMenuItem";
            this.tenantProfileToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.tenantProfileToolStripMenuItem.Text = "&Tenant Profile";
            this.tenantProfileToolStripMenuItem.Click += new System.EventHandler(this.tenantProfileToolStripMenuItem_Click);
            // 
            // chartOfAccountToolStripMenuItem
            // 
            this.chartOfAccountToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("chartOfAccountToolStripMenuItem.Image")));
            this.chartOfAccountToolStripMenuItem.Name = "chartOfAccountToolStripMenuItem";
            this.chartOfAccountToolStripMenuItem.Size = new System.Drawing.Size(209, 56);
            this.chartOfAccountToolStripMenuItem.Text = "Chart of Account";
            this.chartOfAccountToolStripMenuItem.Click += new System.EventHandler(this.chartOfAccountToolStripMenuItem_Click);
            // 
            // roomsToolStripMenuItem
            // 
            this.roomsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("roomsToolStripMenuItem.Image")));
            this.roomsToolStripMenuItem.Name = "roomsToolStripMenuItem";
            this.roomsToolStripMenuItem.Size = new System.Drawing.Size(209, 56);
            this.roomsToolStripMenuItem.Text = "Rooms";
            this.roomsToolStripMenuItem.Click += new System.EventHandler(this.roomsToolStripMenuItem_Click);
            // 
            // roomAssignmentToolStripMenuItem
            // 
            this.roomAssignmentToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("roomAssignmentToolStripMenuItem.Image")));
            this.roomAssignmentToolStripMenuItem.Name = "roomAssignmentToolStripMenuItem";
            this.roomAssignmentToolStripMenuItem.Size = new System.Drawing.Size(209, 56);
            this.roomAssignmentToolStripMenuItem.Text = "Room Assignment";
            this.roomAssignmentToolStripMenuItem.Click += new System.EventHandler(this.roomAssignmentToolStripMenuItem_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(54, 69);
            this.toolStripButton3.Text = "Exit";
            this.toolStripButton3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolStripButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton3.ToolTipText = "Exit Application";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // tmrToday
            // 
            this.tmrToday.Enabled = true;
            this.tmrToday.Tick += new System.EventHandler(this.tmrToday_Tick);
            // 
            // dateConfigurationToolStripMenuItem
            // 
            this.dateConfigurationToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("dateConfigurationToolStripMenuItem.Image")));
            this.dateConfigurationToolStripMenuItem.Name = "dateConfigurationToolStripMenuItem";
            this.dateConfigurationToolStripMenuItem.Size = new System.Drawing.Size(209, 56);
            this.dateConfigurationToolStripMenuItem.Text = "Date Configuration";
            this.dateConfigurationToolStripMenuItem.Click += new System.EventHandler(this.dateConfigurationToolStripMenuItem_Click);
            // 
            // tssPayment
            // 
            this.tssPayment.Image = ((System.Drawing.Image)(resources.GetObject("tssPayment.Image")));
            this.tssPayment.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssPayment.Name = "tssPayment";
            this.tssPayment.Size = new System.Drawing.Size(58, 69);
            this.tssPayment.Text = "Payment";
            this.tssPayment.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tssPayment.Visible = false;
            this.tssPayment.Click += new System.EventHandler(this.tssPayment_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 469);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "s";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem systemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loginToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tssDatabaseStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel tssUserlogin;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.ToolStripStatusLabel tssToday;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel9;
        private System.Windows.Forms.ToolStripStatusLabel tssUserRole;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel11;
        private System.Windows.Forms.ToolStripStatusLabel tssLicenseStatus;
        private System.Windows.Forms.Timer tmrToday;
        private System.Windows.Forms.ToolStripMenuItem tsManageDB;
        private System.Windows.Forms.ToolStripMenuItem usersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton tssManageDB;
        private System.Windows.Forms.ToolStripMenuItem userRoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usersToolStripMenuItem1;
        private System.Windows.Forms.ToolStripButton tssTransaction;
        private System.Windows.Forms.ToolStripMenuItem tsTransaction;
        private System.Windows.Forms.ToolStripMenuItem createTransactionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customerProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chartOfAccountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem roomsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem roomsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem roomAssignmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tenantProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dateConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tssPayment;
    }
}

