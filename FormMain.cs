using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dark_Oak
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettings form = new FormSettings();
            form.Show();
        }
        private void datagridview1_SelectionChanged(object sender, EventArgs e)
        {
            try { 
            if (mTGCardsDataGridView.SelectedCells.Count > 0)
            {
                int selectedrowindex = mTGCardsDataGridView.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = mTGCardsDataGridView.Rows[selectedrowindex];
                string a = Convert.ToString(selectedRow.Cells["web_scraper_order"].Value);
                byte[] result = Database.GetImage(a);
                MemoryStream stream = new MemoryStream(result);
                pictureBox1.Image = Image.FromStream(stream);

            }
            } catch (Exception ed)
            {
                MessageBox.Show(Convert.ToString(("{0} Exception caught.", ed)),"Harmless Error #1 - Safe to ignore"); 
            }
            
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'darkOakDBDataSet.MTGCards' table. You can move, or remove it, as needed.
            this.mTGCardsTableAdapter.Fill(this.darkOakDBDataSet.MTGCards);
            // TODO: This line of code loads data into the 'darkOakDBDataSet.Ikoria' table. You can move, or remove it, as needed.
            //this.ikoriaTableAdapter.Fill(this.darkOakDBDataSet.MTGCards);

        }
        private void button1_Click_1(object sender, EventArgs e)
        {

            byte[] result = Database.GetImage("1608664530-425");
            MemoryStream stream = new MemoryStream(result);
            pictureBox1.Image = Image.FromStream(stream);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int selectedrowindex = mTGCardsDataGridView.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = mTGCardsDataGridView.Rows[selectedrowindex];
            string a = Convert.ToString(selectedRow.Cells["web_scraper_order"].Value);
        }
    }
}
