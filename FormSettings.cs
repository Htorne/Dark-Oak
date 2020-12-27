using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dark_Oak
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        public void GetSettings()
        {
            // Reads string from Settings and populates textboxes with data.
            TextboxSettingsSQLUser.Text = Properties.Settings.Default.SQLUser;
            TextboxSettingsSQLPassword.Text = Properties.Settings.Default.SQLPassword;
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            //When the form Settings load run code below this point
            GetSettings();
        }

        private void SaveSettings(string SQLUser, string SQLPassword)
        {
            //When a user clicks Apply save strings to settings
            Properties.Settings.Default.SQLUser = SQLUser;
            Properties.Settings.Default.SQLPassword = SQLPassword;
            Properties.Settings.Default.Save();
        }

        private void SettingsButtonApply_Click(object sender, EventArgs e)
        {
            //When clicking the apply button run function below
            SaveSettings(TextboxSettingsSQLUser.Text, TextboxSettingsSQLPassword.Text);
            Database.Test_SQL_Connection();
        }
    }
}
