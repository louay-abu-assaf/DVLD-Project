namespace DVLD_Project
{
    partial class frmShowLicenseHistory
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
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ctrDrivingLicenses1 = new DVLD_Project.ctrDrivingLicenses();
            this.ctrSearchPersonInfo1 = new DVLD_Project.ctrSearchPersonInfo();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Urdu Typesetting", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(464, -5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(248, 63);
            this.label1.TabIndex = 1;
            this.label1.Text = "License History";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DVLD_Project.Properties.Resources.PersonLicenseHistory_512;
            this.pictureBox1.Location = new System.Drawing.Point(5, 202);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(193, 206);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // ctrDrivingLicenses1
            // 
            this.ctrDrivingLicenses1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(22)))), ((int)(((byte)(33)))));
            this.ctrDrivingLicenses1.Location = new System.Drawing.Point(38, 551);
            this.ctrDrivingLicenses1.Name = "ctrDrivingLicenses1";
            this.ctrDrivingLicenses1.Size = new System.Drawing.Size(1098, 284);
            this.ctrDrivingLicenses1.TabIndex = 3;
            // 
            // ctrSearchPersonInfo1
            // 
            this.ctrSearchPersonInfo1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(22)))), ((int)(((byte)(33)))));
            this.ctrSearchPersonInfo1.Location = new System.Drawing.Point(204, 60);
            this.ctrSearchPersonInfo1.Name = "ctrSearchPersonInfo1";
            this.ctrSearchPersonInfo1.SearchBox = "";
            this.ctrSearchPersonInfo1.SearchType = "National No";
            this.ctrSearchPersonInfo1.Size = new System.Drawing.Size(978, 514);
            this.ctrSearchPersonInfo1.TabIndex = 0;
            // 
            // frmShowLicenseHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(22)))), ((int)(((byte)(33)))));
            this.ClientSize = new System.Drawing.Size(1183, 864);
            this.Controls.Add(this.ctrDrivingLicenses1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ctrSearchPersonInfo1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmShowLicenseHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmShowLicenseHistory";
            this.Load += new System.EventHandler(this.frmShowLicenseHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ctrSearchPersonInfo ctrSearchPersonInfo1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private ctrDrivingLicenses ctrDrivingLicenses1;
    }
}