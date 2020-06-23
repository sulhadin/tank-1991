namespace WorldOfTanks
{
    partial class GameMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameMenu));
            this.label2 = new System.Windows.Forms.Label();
            this.lblScore = new System.Windows.Forms.Label();
            this.pbMenuIcon = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lbMenuList = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbMenuIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Courier New", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(185, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(280, 46);
            this.label2.TabIndex = 1;
            this.label2.Text = "BEST SCORE";
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Font = new System.Drawing.Font("Courier New", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScore.ForeColor = System.Drawing.Color.White;
            this.lblScore.Location = new System.Drawing.Point(458, 31);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(332, 46);
            this.lblScore.TabIndex = 1;
            this.lblScore.Text = "000000000000";
            // 
            // pbMenuIcon
            // 
            this.pbMenuIcon.Image = global::WorldOfTanks.Properties.Resources.MenuIcon;
            this.pbMenuIcon.Location = new System.Drawing.Point(213, 326);
            this.pbMenuIcon.Name = "pbMenuIcon";
            this.pbMenuIcon.Size = new System.Drawing.Size(41, 43);
            this.pbMenuIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbMenuIcon.TabIndex = 2;
            this.pbMenuIcon.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::WorldOfTanks.Properties.Resources.menu;
            this.pictureBox1.Location = new System.Drawing.Point(204, 119);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(570, 119);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Courier New", 10F);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(210, 710);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(509, 30);
            this.label6.TabIndex = 1;
            this.label6.Text = "2016 - POWERED BY SULHADİN ÖNEY";
            // 
            // lbMenuList
            // 
            this.lbMenuList.BackColor = System.Drawing.Color.Black;
            this.lbMenuList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbMenuList.Font = new System.Drawing.Font("Courier New", 16.125F);
            this.lbMenuList.ForeColor = System.Drawing.Color.White;
            this.lbMenuList.FormattingEnabled = true;
            this.lbMenuList.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbMenuList.ItemHeight = 46;
            this.lbMenuList.Items.AddRange(new object[] {
            "PLAY CLASSIC ",
            "PLAY CLASSIC WITH VOICE",
            "PLAY MODE EAGLE",
            "EDIT MAP",
            "EDIT MAP EAGLE",
            "EXIT"});
            this.lbMenuList.Location = new System.Drawing.Point(260, 326);
            this.lbMenuList.Name = "lbMenuList";
            this.lbMenuList.Size = new System.Drawing.Size(626, 276);
            this.lbMenuList.TabIndex = 3;
            this.lbMenuList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbMenuList_KeyDown);
            // 
            // GameMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1008, 763);
            this.Controls.Add(this.lbMenuList);
            this.Controls.Add(this.pbMenuIcon);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TANK 1990";
            this.Activated += new System.EventHandler(this.GameMenu_Activated);
            this.Load += new System.EventHandler(this.GameMenu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbMenuIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.PictureBox pbMenuIcon;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox lbMenuList;
    }
}