using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 Note and legal stuff 
https://company.wizards.com/fancontentpolicy
 
 */
namespace Dark_Oak
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            PullData();

        }

        public void PullData()
        //Lets pull some data shall we
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(@"Data Source=192.168.1.117\Razorback;Initial Catalog=DarkOakDB;User ID=Max;Password=Ia3#qFJz");
            //please connect to SQL using the information provided by user and stored in settings, mykay.
            string query = "select [card_number],[web_scraper_order],[card_name],[creature_type] as [Type],[card_rules2],[set_name],[rareity_code],[note] as [Artist], [card_type] as [Color] from [dbo].[MTGCards]";
           
            //Just grab whatever is written above from the SQL server
            SqlCommand cmd = new SqlCommand(query, conn);
            //Make a new fancy command 
            conn.Open();
            //Connect to SQL Server
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            //Do the disco
            da.Fill(dt);
            //Using SQLdataAdapter fill datatable with result set from the cmd.
            conn.Close();
            //Close SQL Server connection.
            da.Dispose();
            //Hide the evidence; dispose data adapter 
            mTGCardsDataGridView.DataSource = dt;
            
        }

        public void PullDataFromSortBoard()
        //Lets pull some data shall we
        {
            DataTable dtsort = new DataTable();
            SqlConnection conn = new SqlConnection(@"Data Source=192.168.1.117\Razorback;Initial Catalog=DarkOakDB;User ID=Max;Password=Ia3#qFJz");
            //please connect to SQL using the information provided by user and stored in settings, mykay.
            string query = "select [card_number] as [#],[card_name],[set_name],[web_scraper_order] from [dbo].[MTGCardsSortBoard]";

            //Just grab whatever is written above from the SQL server
            SqlCommand cmd = new SqlCommand(query, conn);
            //Make a new fancy command 
            conn.Open();
            //Connect to SQL Server
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            //Do the disco
            da.Fill(dtsort);
            //Using SQLdataAdapter fill datatable with result set from the cmd.
            conn.Close();
            //Close SQL Server connection.
            da.Dispose();
            //Hide the evidence; dispose data adapter 
            mtgSortingBoardDataGridView.DataSource = dtsort;

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettings form = new FormSettings();
            form.Show();
        }
        public void datagridview1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (mTGCardsDataGridView.SelectedCells.Count > 0)
                {
                    int selectedrowindex = mTGCardsDataGridView.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = mTGCardsDataGridView.Rows[selectedrowindex];
                    string a = Convert.ToString(selectedRow.Cells["web_scraper_order"].Value);
                    string b = Convert.ToString(selectedRow.Cells["card_name"].Value);
                    byte[] result = Database.GetImage(a);
                    MemoryStream stream = new MemoryStream(result);
                    pictureBox1.Image = Image.FromStream(stream);
                    label6.Text = b;

                }
            }
            catch (Exception ed)
            {
                MessageBox.Show(Convert.ToString(("{0} Exception caught.", ed)), "Harmless Error #1 - Safe to ignore");
            }

        }
        public void FormMain_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'darkOakDBDataSet.MTGCards' table. You can move, or remove it, as needed.
            this.mTGCardsTableAdapter.Fill(this.darkOakDBDataSet.MTGCards);
            // TODO: This line of code loads data into the 'darkOakDBDataSet.Ikoria' table. You can move, or remove it, as needed.
            //this.ikoriaTableAdapter.Fill(this.darkOakDBDataSet.MTGCards);

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int selectedrowindex = mTGCardsDataGridView.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = mTGCardsDataGridView.Rows[selectedrowindex];
            string a = Convert.ToString(selectedRow.Cells["web_scraper_order"].Value);
        }
        public void textBox1_TextChanged(object sender, EventArgs e)
        {
            PullData();
            try { 
            if (textBox1.Text == "")
            {
                (mTGCardsDataGridView.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
            }
            else { 
                (mTGCardsDataGridView.DataSource as DataTable).DefaultView.RowFilter = string.Format("[card_number] = '{0}'", textBox1.Text);
            }
            }
            catch (Exception ed)
            {
                MessageBox.Show(Convert.ToString(("{0} Exception caught.", ed)), "Harmless Error #1 - Safe to ignore");
            }
        }
        public void textBox2_TextChanged(object sender, EventArgs e)
        {
            filterstuff();
        }
        public void textBox3_TextChanged(object sender, EventArgs e)
        {

            filterstuff();

        }
        private void mTGCardsDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.mTGCardsDataGridView.Columns["web_scraper_order"].Visible = false;
            this.mTGCardsDataGridView.Columns[0].HeaderText = "#";
            this.mTGCardsDataGridView.Columns[4].HeaderText = "Card Text";
            this.mTGCardsDataGridView.AllowUserToResizeColumns = false;
            this.mTGCardsDataGridView.AllowUserToResizeRows = false;
            this.mTGCardsDataGridView.Columns[0].Width = 40;
            this.mTGCardsDataGridView.Columns[2].Width = 150;
            this.mTGCardsDataGridView.Columns[3].Width = 150;
            this.mTGCardsDataGridView.Columns[4].Width = 400;
            this.mTGCardsDataGridView.AllowUserToAddRows = false;
        }

     
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            filterstuff();
        }
        public void filterstuff()
        {
            
            BindingSource bs = new BindingSource();
            bs.DataSource = mTGCardsDataGridView.DataSource;
            string cardname = textBox2.Text;
            string type = textBox3.Text;
            string cardrules = textBox4.Text;
            var filt = "";


            if (!string.IsNullOrEmpty(cardname))
            {
                if (filt == "")
                    filt += "[card_name] LIKE '%" + cardname + "%'";
                else
                    filt += " And [card_name] LIKE '%" + cardname + "%' ";
            }


            if (!string.IsNullOrEmpty(type))
            {
                if (filt == "")
                    filt += "[Type] LIKE '%" + type + "%'";
                else
                    filt += " And [Type] LIKE '%" + type + "%' ";
            }

            
            if (!string.IsNullOrEmpty(cardrules))
            {
                if (filt == "")
                    filt += "[card_rules2] LIKE '%" + cardrules + "%'";
                else
                    filt += " And [card_rules2] LIKE '%" + cardrules + "%' ";
            }

            try { 

            bs.Filter = filt;
            mTGCardsDataGridView.DataSource = bs;
            } catch (Exception ed)
            {
                MessageBox.Show(Convert.ToString(("{0} Exception caught.", ed)), "Harmless Error #1 - Safe to ignore");
            }

        }

        private void mTGCardsDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        { /*
            foreach (DataGridViewRow Myrow in mTGCardsDataGridView.Rows)
            {            //Here 2 cell is target value and 1 cell is Volume
                if (Convert.ToString(Myrow.Cells[8].Value) == "G")// Or your condition 
                {
                    Myrow.DefaultCellStyle.ForeColor = Color.Green;
                }
                else
                {
                    Myrow.DefaultCellStyle.ForeColor = Color.Black;
                }
                if (Convert.ToString(Myrow.Cells[8].Value) == "U")// Or your condition 
                {
                    Myrow.DefaultCellStyle.ForeColor = Color.Blue;
                }
                else
                {
                    Myrow.DefaultCellStyle.ForeColor = Color.Black;
                }
            }*/
        }

        private void mTGCardsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            int selectedrowindex = mTGCardsDataGridView.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = mTGCardsDataGridView.Rows[selectedrowindex];
            string card_number = Convert.ToString(selectedRow.Cells["card_number"].Value);
            string set_name = Convert.ToString(selectedRow.Cells["set_name"].Value);
            string card_name = Convert.ToString(selectedRow.Cells["card_name"].Value);
           // string full_card_id = card_number + " " + set_name + " " + card_name;
           // MessageBox.Show(full_card_id);

            card_name = card_name.Replace("'", $"{(char)39}");
            string Command = "INSERT INTO dbo.MTGCardsSortBoard SELECT * FROM [MTGCards] where [card_number] like '" + card_number+"' and [set_name] like '"+set_name+"' and [card_name] like '"+card_name.Replace("'", "''") +"'" ; 
            using (SqlConnection myConnection = new SqlConnection(Properties.Settings.Default.DarkOakDBConnectionString))
                {
                    myConnection.Open();
                    using (SqlCommand myCommand = new SqlCommand(Command, myConnection))
                    {
                       myCommand.ExecuteScalar(); //runs Command string hopefully
                    }
                    myConnection.Close();
                }
            //    MessageBox.Show(Command);
            PullDataFromSortBoard();
        }

        private void mtgSortingBoardDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.mtgSortingBoardDataGridView.Columns["web_scraper_order"].Visible = false;
            this.mtgSortingBoardDataGridView.Columns[0].HeaderText = "#";
            this.mtgSortingBoardDataGridView.Columns[1].HeaderText = "Name";
            this.mtgSortingBoardDataGridView.Columns[2].HeaderText = "Set";
            this.mtgSortingBoardDataGridView.AllowUserToResizeColumns = false;
            this.mtgSortingBoardDataGridView.AllowUserToResizeRows = false;
            this.mtgSortingBoardDataGridView.Columns[0].Width = 35;
            this.mtgSortingBoardDataGridView.Columns[1].Width = 212;
            this.mtgSortingBoardDataGridView.Columns[2].Width = 150;
            this.mtgSortingBoardDataGridView.AllowUserToAddRows = false;
        }
        private void mtgSortingBoardDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (mTGCardsDataGridView.SelectedCells.Count > 0)
                {
                    int selectedrowindex = mtgSortingBoardDataGridView.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = mtgSortingBoardDataGridView.Rows[selectedrowindex];
                    string a = Convert.ToString(selectedRow.Cells["web_scraper_order"].Value);
                    string b = Convert.ToString(selectedRow.Cells["card_name"].Value);
                    byte[] result = Database.GetImage(a);
                    MemoryStream stream = new MemoryStream(result);
                    pictureBox1.Image = Image.FromStream(stream);
                    label6.Text = b;
                }
            }
            catch (Exception ed)
            {
              //  MessageBox.Show(Convert.ToString(("{0} Exception caught.", ed)), "Harmless Error #1 - Safe to ignore");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string Command = "Delete from MTGCardsSortBoard";
            using (SqlConnection myConnection = new SqlConnection(Properties.Settings.Default.DarkOakDBConnectionString))
            {
                myConnection.Open();
                using (SqlCommand myCommand = new SqlCommand(Command, myConnection))
                {
                    myCommand.ExecuteScalar(); //runs Command string hopefully
                }
                myConnection.Close();
            }
            //  MessageBox.Show(Command);
            PullDataFromSortBoard();
        }



        private void mTGCardsDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Add)
            {
                int selectedrowindex = mTGCardsDataGridView.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = mTGCardsDataGridView.Rows[selectedrowindex];
                string card_number = Convert.ToString(selectedRow.Cells["card_number"].Value);
                string set_name = Convert.ToString(selectedRow.Cells["set_name"].Value);
                string card_name = Convert.ToString(selectedRow.Cells["card_name"].Value);
                // string full_card_id = card_number + " " + set_name + " " + card_name;
                // MessageBox.Show(full_card_id);

                card_name = card_name.Replace("'", $"{(char)39}");
                string Command = "INSERT INTO dbo.MTGCardsSortBoard SELECT * FROM [MTGCards] where [card_number] like '" + card_number + "' and [set_name] like '" + set_name + "' and [card_name] like '" + card_name.Replace("'", "''") + "'";
                using (SqlConnection myConnection = new SqlConnection(Properties.Settings.Default.DarkOakDBConnectionString))
                {
                    myConnection.Open();
                    using (SqlCommand myCommand = new SqlCommand(Command, myConnection))
                    {
                        myCommand.ExecuteScalar(); //runs Command string hopefully
                    }
                    myConnection.Close();
                }
               // MessageBox.Show(Command);
                PullDataFromSortBoard();
            }
            else { }
        }
    }
}