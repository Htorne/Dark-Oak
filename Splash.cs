using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dark_Oak
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
        }



        private void Splash_Shown(object sender, EventArgs e)
        {
            Thread.Sleep(8000);
            this.Close();
        }
    }
}
