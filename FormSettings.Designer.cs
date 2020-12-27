
namespace Dark_Oak
{
    partial class FormSettings
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
            this.TextboxSettingsSQLUser = new System.Windows.Forms.TextBox();
            this.TextboxSettingsSQLPassword = new System.Windows.Forms.TextBox();
            this.SettingsButtonApply = new System.Windows.Forms.Button();
            this.LabelSQLUser = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // TextboxSettingsSQLUser
            // 
            this.TextboxSettingsSQLUser.Location = new System.Drawing.Point(72, 112);
            this.TextboxSettingsSQLUser.Name = "TextboxSettingsSQLUser";
            this.TextboxSettingsSQLUser.Size = new System.Drawing.Size(100, 26);
            this.TextboxSettingsSQLUser.TabIndex = 0;
            // 
            // TextboxSettingsSQLPassword
            // 
            this.TextboxSettingsSQLPassword.Location = new System.Drawing.Point(256, 112);
            this.TextboxSettingsSQLPassword.Name = "TextboxSettingsSQLPassword";
            this.TextboxSettingsSQLPassword.Size = new System.Drawing.Size(100, 26);
            this.TextboxSettingsSQLPassword.TabIndex = 1;
            // 
            // SettingsButtonApply
            // 
            this.SettingsButtonApply.Location = new System.Drawing.Point(72, 162);
            this.SettingsButtonApply.Name = "SettingsButtonApply";
            this.SettingsButtonApply.Size = new System.Drawing.Size(75, 30);
            this.SettingsButtonApply.TabIndex = 2;
            this.SettingsButtonApply.Text = "button1";
            this.SettingsButtonApply.UseVisualStyleBackColor = true;
            this.SettingsButtonApply.Click += new System.EventHandler(this.SettingsButtonApply_Click);
            // 
            // LabelSQLUser
            // 
            this.LabelSQLUser.AutoSize = true;
            this.LabelSQLUser.Location = new System.Drawing.Point(68, 89);
            this.LabelSQLUser.Name = "LabelSQLUser";
            this.LabelSQLUser.Size = new System.Drawing.Size(51, 20);
            this.LabelSQLUser.TabIndex = 3;
            this.LabelSQLUser.Text = "label1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(325, 317);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(268, 188);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 692);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.LabelSQLUser);
            this.Controls.Add(this.SettingsButtonApply);
            this.Controls.Add(this.TextboxSettingsSQLPassword);
            this.Controls.Add(this.TextboxSettingsSQLUser);
            this.Name = "FormSettings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextboxSettingsSQLUser;
        private System.Windows.Forms.TextBox TextboxSettingsSQLPassword;
        private System.Windows.Forms.Button SettingsButtonApply;
        private System.Windows.Forms.Label LabelSQLUser;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}