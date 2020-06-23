using WorldOfTanks.Repository;

namespace WorldOfTanks
{
    partial class GameForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this.GameArea = new System.Windows.Forms.Panel();
            this.PaneHeader = new System.Windows.Forms.Panel();
            this.TimerSR = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // GameArea
            // 
            this.GameArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GameArea.BackColor = System.Drawing.Color.Black;
            this.GameArea.Location = new System.Drawing.Point(0, 160);
            this.GameArea.Margin = new System.Windows.Forms.Padding(6);
            this.GameArea.Name = "GameArea";
            this.GameArea.Size = new System.Drawing.Size(940, 541);
            this.GameArea.TabIndex = 0;
            // 
            // PaneHeader
            // 
            this.PaneHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PaneHeader.BackColor = System.Drawing.Color.RosyBrown;
            this.PaneHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PaneHeader.Location = new System.Drawing.Point(0, 0);
            this.PaneHeader.Name = "PaneHeader";
            this.PaneHeader.Size = new System.Drawing.Size(939, 160);
            this.PaneHeader.TabIndex = 1;
            // 
            // TimerSR
            // 
            this.TimerSR.Tick += new System.EventHandler(this.TimerSR_Tick);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 701);
            this.Controls.Add(this.PaneHeader);
            this.Controls.Add(this.GameArea);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameForm";
            this.Text = "TANK 1990";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameForm_FormClosing);
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        public  System.Windows.Forms.Panel GameArea;
        private System.Windows.Forms.Panel PaneHeader;
        private System.Windows.Forms.Timer TimerSR;
    
    }
}

