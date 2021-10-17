using Newtonsoft.Json.Linq;
using RestAPITester;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// Code is organized in Regions - Collaps them for better overview
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
            PullDataFromCollection();
            PullDataFromSortBoard();
        }

        // Dataframe and Database connection ingress region
        #region Data Ingress
        // Pulls data from database to different dataframes
        public void PullData() 
        {
                try

                {   // This datatable is the default 
                    DataTable dt = new DataTable(); //declare a new datatable
                    using (SqlConnection conn = new SqlConnection // Create a new connection
                       (Properties.Settings.Default.DarkOakDBConnectionString)) //Using settings - I need to make this editable from within the program via settings at some point
                    {
                        string query = "select " +
                            "           [collector_number_1_1] as [#]," +
                            "           name as [Name], " +                        
                            "           type_line_1 as  [Type 1], " +                
                            "           type_line_2_1 as [Type 2], " +                
                            "           type_line_2_2_1 as [Type 3], " +                
                            "           type_line_2_2_2 as [Type 4], " +                
                            "           [set_name] as [Set], " +
                            "           [set] as [Set Code], " +
                            "           [id] as [ScryFallID]," +                   
                            "           prices_eur as [NM Price Eur]," +            
                            "           prices_eur_foil as [Foil Price Eur]," +     
                            "           reserved as [Reserve List]," +             
                            "           digital as [Digital]," +                     
                            "           released_at as [print]" +
                            "           from MTGCardsDatabase";

                        SqlCommand cmd = new SqlCommand(query, conn);
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
                }
                catch (Exception ex) { MessageBox.Show("Unable BOOM 2 to open a connection to the database ::::" + ex); }
        }
        public void PullDataFromSortBoard()
        {
            DataTable dtsort = new DataTable();
            SqlConnection conn = new SqlConnection(Properties.Settings.Default.DarkOakDBConnectionString);
            //please connect to SQL using the information provided by user and stored in settings, mykay.
            string query = "select [collector_number_1_1] as [#],[name],[set_name],[id],[released_at] as [Print] from [dbo].[MTGCardsSortBoard]";

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
        public void PullDataFromCollection()
        {

            //Lets pull some data shall we
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlConnection conn = new SqlConnection(Properties.Settings.Default.DarkOakDBConnectionString);
                    string query = "select " +
                                                "amount_owned, " +
                                                "           [collector_number_1_1] as [#]," +
                                                "           name as [Name], " +                 //0
                                                "           [set_name] as [Set], " +            //1
                                                "           [id] as [ScryFallID]," +            //2
                                                "           reserved as [Reserve List]," +
                                                "           digital as [Digital]" +            //5
                                                "           from MTGCardsCollection";

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
                    mTGCollectionDataGridView.DataSource = dt;
                }
                catch (Exception ex) { MessageBox.Show("Unable BOOM 33 to open a connection to the database ::::" + ex); }

            }


        }
        #endregion
        // Menus
        #region Menus
        // Opens the settings menu vis the Strip menu
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettings form = new FormSettings();
            form.Show();
        }
        #endregion
        // Various half-done feautures (WIP)
        #region Beta features
        // Picture click hook into database - can perhaps be used for more interesting features in the future
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int selectedrowindex = mTGCardsDataGridView.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = mTGCardsDataGridView.Rows[selectedrowindex];
            MessageBox.Show(Convert.ToString(selectedrowindex));
        }
        #endregion
        // Search functions and filters
        #region filters
        // MTG Master filters
        public void filter_mtg_collector_number(object sender, EventArgs e)
        {
            PullData();
            try
            {
                if (filter_mtg_collectornumber.Text == "")
                {
                    (mTGCardsDataGridView.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
                }
                else
                {
                    (mTGCardsDataGridView.DataSource as DataTable).DefaultView.RowFilter = string.Format("[#] = '{0}'", filter_mtg_collectornumber.Text);
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
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            filterstuff();
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            filterstuff();
        }
        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            filterstuff();
        }
        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            filterstuff();
        }
        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            filterstuff();
        }
        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            filterstuff();
        }

        // Collection filters below
        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            filterstuffInCollection();
        }
        // Filter functions
        public void filterstuff()
        {

            BindingSource bs = new BindingSource();
            bs.DataSource = mTGCardsDataGridView.DataSource;
            string cardname = textBox2.Text;
            string set = textBox3.Text;
            string originalType = textBox4.Text;
            string type2 = textBox11.Text;
            string cardtext = textBox5.Text;
            string setcode = textBox14.Text;
            string type3 = textBox12.Text;
            string type4 = textBox13.Text;
            var filt = "";

            if (!string.IsNullOrEmpty(type4))  //if there is text then
            {
                if (filt == "")
                    filt += "[Type 4] LIKE '%" + type4 + "%'";
                else
                    filt += " And [Type 4] LIKE '%" + type4 + "%' ";
            }

            if (!string.IsNullOrEmpty(type3))  //if there is text then
            {
                if (filt == "")
                    filt += "[Type 3] LIKE '%" + type3 + "%'";
                else
                    filt += " And [Type 3] LIKE '%" + type3 + "%' ";
            }


            if (!string.IsNullOrEmpty(type2))  //if there is text then
            {
                if (filt == "")
                    filt += "[Type 2] LIKE '%" + type2 + "%'";
                else
                    filt += " And [Type 2] LIKE '%" + type2 + "%' ";
            }


            if (!string.IsNullOrEmpty(setcode))  //if there is text then
            {
                if (filt == "")
                    filt += "[Set Code] = '" + setcode + "'";
                else
                    filt += " And [Set Code] = '" + setcode + "' ";
            }

            if (!string.IsNullOrEmpty(cardname))  //if there is text then
            {
                if (filt == "")
                    filt += "[name] LIKE '%" + cardname + "%'";
                else
                    filt += " And [name] LIKE '%" + cardname + "%' ";
            }


            if (!string.IsNullOrEmpty(set))
            {
                if (filt == "")
                    filt += "[Set] LIKE '%" + set + "%'";
                else
                    filt += " And [Set] LIKE '%" + set + "%' ";
            }


            if (!string.IsNullOrEmpty(originalType)) //orginalType is old name for the data field type_line
            {
                if (filt == "")
                    filt += "[Type 1] LIKE '%" + originalType + "%'";
                else
                    filt += " And [Type 1] LIKE '%" + originalType + "%' ";
            }

            if (!string.IsNullOrEmpty(cardtext))
            {
                if (filt == "")
                    filt += "[originalText] LIKE '%" + cardtext + "%'";
                else
                    filt += " And [originalText] LIKE '%" + cardtext + "%' ";
            }

            try
            {

                bs.Filter = filt;
                mTGCardsDataGridView.DataSource = bs;
            }
            catch (Exception ed)
            {
                MessageBox.Show(Convert.ToString(("{0} Exception caught.", ed)), "Harmless Error #1 - Safe to ignore");
            }

        }
        public void filterstuffInCollection()
        {



            BindingSource bs = new BindingSource();
            bs.DataSource = mTGCollectionDataGridView.DataSource;
            string cardname = textBox10.Text;
            // string set = textBox10.Text;
            //  string originalType = textBox4.Text;
            // string cardtext = textBox5.Text;
            var filt = "";


            if (!string.IsNullOrEmpty(cardname))
            {
                if (filt == "")
                    filt += "[name] LIKE '%" + cardname + "%'";
                else
                    filt += " And [name] LIKE '%" + cardname + "%' ";
            }
            try
            {

                bs.Filter = filt;
                mTGCollectionDataGridView.DataSource = bs;
            }
            catch (Exception ed)
            {
                MessageBox.Show(Convert.ToString(("{0} Exception caught.", ed)), "Harmless Error #1 - Safe to ignore");
            }

        }
        #endregion
        // Dataframe grid formatting region
        #region GridFormatting 
        private void mTGCardsDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                this.mTGCardsDataGridView.Columns["scryfallid"].Visible = false;
                // this.mTGCardsDataGridView.Columns["isReserved"].Visible = false;
                //  this.mTGCardsDataGridView.Columns["isOnlineOnly"].Visible = false;
                this.mTGCardsDataGridView.Columns[0].HeaderText = "#";
                //  this.mTGCardsDataGridView.Columns[1].HeaderText = "Card Name";
                //  this.mTGCardsDataGridView.Columns[2].HeaderText = "Set";
                //   this.mTGCardsDataGridView.Columns[3].HeaderText = "Type";
                //   this.mTGCardsDataGridView.Columns[4].HeaderText = "Card Text";
                //   this.mTGCardsDataGridView.Columns[6].HeaderText = "Normal";
                //   this.mTGCardsDataGridView.Columns[7].HeaderText = "Foil";
                // this.mTGCardsDataGridView.Columns[4].HeaderText = "Card Text";
                //    this.mTGCardsDataGridView.AllowUserToResizeColumns = false;
                //    this.mTGCardsDataGridView.AllowUserToResizeRows = false;
                this.mTGCardsDataGridView.Columns[0].Width = 45;
                this.mTGCardsDataGridView.Columns[1].Width = 145;
                this.mTGCardsDataGridView.Columns[2].Width = 84;
                this.mTGCardsDataGridView.Columns[3].Width = 90;
                this.mTGCardsDataGridView.Columns[4].Width = 80;
                this.mTGCardsDataGridView.Columns[5].Width = 84;
                this.mTGCardsDataGridView.Columns[6].Width = 130;
                //    this.mTGCardsDataGridView.Columns[7].Width = 60;
                //   this.mTGCardsDataGridView.AllowUserToAddRows = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex));
            }
        }
        private void mTGCollectionDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                //  this.mTGCollectionDataGridView.Columns["isonlineonly"].Visible = false;
                //  this.mTGCollectionDataGridView.Columns["isReserved"].Visible = false; 
                //this.mTGCollectionDataGridView.Columns["id"].Visible = false;
                //  this.mTGCollectionDataGridView.Columns[0].HeaderText = "#";
                //  this.mTGCollectionDataGridView.Columns[1].HeaderText = "Card Name";
                //  this.mTGCollectionDataGridView.Columns[2].HeaderText = "Set";
                //  this.mTGCollectionDataGridView.Columns[3].HeaderText = "Type";
                //  this.mTGCollectionDataGridView.Columns[4].HeaderText = "Card Text";
                //  this.mTGCollectionDataGridView.Columns[6].HeaderText = "Normal";
                //  this.mTGCollectionDataGridView.Columns[7].HeaderText = "Foil";
                //  this.mTGCollectionDataGridView.Columns[8].HeaderText = "Owned";
                //  this.mTGCollectionDataGridView.Columns[9].HeaderText = "Foil";
                // this.mTGCardsDataGridView.Columns[4].HeaderText = "Card Text";
                this.mTGCollectionDataGridView.AllowUserToResizeColumns = false;
                this.mTGCollectionDataGridView.AllowUserToResizeRows = false;
                this.mTGCollectionDataGridView.Columns[0].Width = 45;
                //   this.mTGCollectionDataGridView.Columns[1].Width = 200;
                //   this.mTGCollectionDataGridView.Columns[2].Width = 60;
                //   this.mTGCollectionDataGridView.Columns[3].Width = 225;
                //   this.mTGCollectionDataGridView.Columns[4].Width = 500;
                //   this.mTGCollectionDataGridView.Columns[6].Width = 60;
                //   this.mTGCollectionDataGridView.Columns[7].Width = 60;
                //   this.mTGCollectionDataGridView.Columns[8].Width = 60;
                //   this.mTGCollectionDataGridView.Columns[9].Width = 60;
                //this.mTGCollectionDataGridView.Columns[10].Width = 45;
                this.mTGCollectionDataGridView.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex));
            }
        }
        private void mtgSortingBoardDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                this.mtgSortingBoardDataGridView.Columns["id"].Visible = false;
                this.mtgSortingBoardDataGridView.Columns[0].HeaderText = "#";
                this.mtgSortingBoardDataGridView.Columns[1].HeaderText = "Name";
                this.mtgSortingBoardDataGridView.Columns[2].HeaderText = "Set";
                this.mtgSortingBoardDataGridView.AllowUserToResizeColumns = false;
                this.mtgSortingBoardDataGridView.AllowUserToResizeRows = false;
                this.mtgSortingBoardDataGridView.Columns[0].Width = 45;
                this.mtgSortingBoardDataGridView.Columns[1].Width = 200;
                this.mtgSortingBoardDataGridView.Columns[2].Width = 60;
                this.mtgSortingBoardDataGridView.AllowUserToAddRows = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex));
            }
        }

        // Datagrid Colors
        private void mTGCardsDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Compare the column to the column you want to format
            if (mTGCardsDataGridView.Columns[e.ColumnIndex].HeaderText == "Type 3" && mTGCardsDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            // if the column is bool_badge and check null value for the extra row at dgv
            {
                if (mTGCardsDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Knight")
                {
                    mTGCardsDataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Blue;
                }
                if (mTGCardsDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Cleric")
                {
                    mTGCardsDataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.ForestGreen;
                }
            }
        }
        #endregion
        // Datagrid Interactions
        #region Datagrid Interatctions
        private void mTGCardsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedrowindex = mTGCardsDataGridView.SelectedCells[0].RowIndex; //
            DataGridViewRow selectedRow = mTGCardsDataGridView.Rows[selectedrowindex];
            string scryfallid = Convert.ToString(selectedRow.Cells["ScryFallID"].Value);
            string set_name = Convert.ToString(selectedRow.Cells["Set"].Value);
            string name = Convert.ToString(selectedRow.Cells["Name"].Value);

            name = name.Replace("'", $"{(char)39}");
            // string Command = "";
            string Command = "INSERT INTO dbo.MTGCardsSortBoard SELECT * FROM [MTGCardsDatabase] where " +
                "      [id] like '" + scryfallid +
                "' and [set_name] like '" + set_name.Replace("'", "''") +
                "' and [name] like '" + name.Replace("'", "''") + "'";
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
        // When you use the mouse to select a card in the table
        private void mTGCardsDataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            pull_Images_from_Internet();
        }
        // Navigating Datagrid using keyboard and support for + button to add to sorting board
        private void mTGCardsDataGridView_KeyUp(object sender, KeyEventArgs e)
        {
            // If you press the plus sign on the numpad it adds a card to the collection board.
            {
                try
                {
                    //Get 
                    int selectedrowindex2 = mTGCardsDataGridView.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow2 = mTGCardsDataGridView.Rows[selectedrowindex2];
                    string scryfallid2 = Convert.ToString(selectedRow2.Cells["ScryFallID"].Value);
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Dreameater\"; // Define path as C:\Users\user\Documents\Dreameater
                    label6.Text = scryfallid2;
                    label11.Text = Convert.ToString(selectedRow2.Cells["NM Price Eur"].Value);
                    bool is_reserved = Convert.ToBoolean(selectedRow2.Cells["Reserve List"].Value);
                    bool is_online = Convert.ToBoolean(selectedRow2.Cells["digital"].Value);
                    if (is_reserved == true) { pictureBox2.Visible = true; } else { pictureBox2.Visible = false; }
                    if (is_online == true) { pictureBox3.Visible = true; } else { pictureBox3.Visible = false; }
                    label12.Text = Convert.ToString(selectedRow2.Cells["Foil Price Eur"].Value);
                    if (File.Exists(path + scryfallid2 + ".jpeg")) //Testing to see if image has allready been downloaded.
                    {
                        pictureBox1.ImageLocation = (path + scryfallid2 + ".jpeg");

                    }
                    else
                    { //If the image is not downloaded - go online and download it

                        try
                        {
                            int selectedrowindex = mTGCardsDataGridView.SelectedCells[0].RowIndex;
                            DataGridViewRow selectedRow = mTGCardsDataGridView.Rows[selectedrowindex];
                            string scryfallid = Convert.ToString(selectedRow.Cells["scryfallid"].Value);
                            RestClient rClient = new RestClient();
                            rClient.endPoint = "https://api.scryfall.com/cards/" + scryfallid;
                            //Get data from Scryfall API

                            string strResponse = string.Empty;
                            //Initiate variable

                            strResponse = rClient.makeRequest();
                            // MessageBox.Show(Convert.ToString(strResponse));
                            try
                            {
                                JObject jsonObj = JObject.Parse(strResponse);

                                foreach (JProperty obj in jsonObj.Properties())
                                {
                                    if (obj.Name == "image_uris")
                                    {

                                        String text = Convert.ToString(obj);

                                        var stringliste = new List<string> { };

                                        string[] image_uris = text.Split();
                                        foreach (string info in image_uris)
                                        {
                                            stringliste.Add(info);
                                        }

                                        string image_uris_result = (stringliste[11]); // Get string from list in position 11 
                                        image_uris_result = image_uris_result.Remove(0, 1); // Clean the string and remove the first "
                                        image_uris_result = image_uris_result.Substring(0, image_uris_result.Length - 2); // Clean the string and remove the two last chars ",

                                        var wc = new WebClient(); // Create a new webclient
                                        Image x = Image.FromStream(wc.OpenRead(image_uris_result)); // Use webclient to read datastream as an image and save to variable x
                                        pictureBox1.Image = x; // Assign picturebox image as x


                                        if (Directory.Exists(path))
                                        { // If that path exisits do nothing yet
                                        }
                                        else { CreateFolder(path); } // If that path does NOT exist create the folder
                                        x.Save(path + scryfallid + ".jpeg", ImageFormat.Jpeg); // Save data from variable x to path + \ + the scryfallid + jpeg as a image format jpeg.


                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Program most likely had a 404 error see debug log or messageboxes for details " + Convert.ToString(ex));
                            }




                        }
                        catch (Exception ed)
                        {
                            MessageBox.Show(Convert.ToString(("{0} Exception caught.", ed)), "Harmless Error #1 - Safe to ignore");
                        }

                        // With the first part above done, where we got the image using the scryfall scryfallid, we now 
                        // attempt to get market data using the mcmid information to get the regular and foil prices. 

                    }
                }
                catch (Exception ed)
                {
                    MessageBox.Show(Convert.ToString(("{0} Exception caught.", ed)), "Mr.Fix it");
                }
                if (e.KeyCode == Keys.Add)
                {
                    int selectedrowindex = mTGCardsDataGridView.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = mTGCardsDataGridView.Rows[selectedrowindex];
                    string scryfallid = Convert.ToString(selectedRow.Cells["scryfallid"].Value);
                    string set_name = Convert.ToString(selectedRow.Cells["Set"].Value);
                    string name = Convert.ToString(selectedRow.Cells["name"].Value);

                    name = name.Replace("'", $"{(char)39}");
                    // string Command = "";
                    string Command = "INSERT INTO dbo.MTGCardsSortBoard SELECT * FROM [MTGCardsDatabase] where [id] like '" + scryfallid + "'";
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
        private void mtgSortingBoardDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {

                int selectedrowindex = mtgSortingBoardDataGridView.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = mtgSortingBoardDataGridView.Rows[selectedrowindex];
                string scryfallid = Convert.ToString(selectedRow.Cells["scryfallid"].Value);


                string Command = "Delete from MTGCardsSortBoard where [scryfallid] like '" + scryfallid + "'";
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
        }
        private void mTGCollectionDataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                //Get 
                int selectedrowindex2 = mTGCollectionDataGridView.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow2 = mTGCollectionDataGridView.Rows[selectedrowindex2];
                string scryfallid2 = Convert.ToString(selectedRow2.Cells["ScryFallID"].Value);
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Dreameater\"; // Define path as C:\Users\user\Documents\Dreameater
                label6.Text = scryfallid2;
                //label11.Text = Convert.ToString(selectedRow2.Cells["NM Price Eur"].Value);
                bool is_reserved = Convert.ToBoolean(selectedRow2.Cells["Reserve List"].Value);
                bool is_online = Convert.ToBoolean(selectedRow2.Cells["digital"].Value);
                if (is_reserved == true) { pictureBox2.Visible = true; } else { pictureBox2.Visible = false; }
                if (is_online == true) { pictureBox3.Visible = true; } else { pictureBox3.Visible = false; }
                //label12.Text = Convert.ToString(selectedRow2.Cells["Foil Price Eur"].Value);
                if (File.Exists(path + scryfallid2 + ".jpeg")) //Testing to see if image has allready been downloaded.
                {
                    pictureBox1.ImageLocation = (path + scryfallid2 + ".jpeg");

                }
                else
                { //If the image is not downloaded - go online and download it

                    try
                    {
                        int selectedrowindex = mTGCollectionDataGridView.SelectedCells[0].RowIndex;
                        DataGridViewRow selectedRow = mTGCollectionDataGridView.Rows[selectedrowindex];
                        string scryfallid = Convert.ToString(selectedRow.Cells["id"].Value);
                        RestClient rClient = new RestClient();
                        rClient.endPoint = "https://api.scryfall.com/cards/" + scryfallid;
                        //Get data from Scryfall API

                        string strResponse = string.Empty;
                        //Initiate variable

                        strResponse = rClient.makeRequest();
                        // MessageBox.Show(Convert.ToString(strResponse));
                        try
                        {
                            JObject jsonObj = JObject.Parse(strResponse);

                            foreach (JProperty obj in jsonObj.Properties())
                            {
                                if (obj.Name == "image_uris") //select list of links
                                {

                                    String text = Convert.ToString(obj);

                                    var stringliste = new List<string> { }; //create a empty list

                                    string[] image_uris = text.Split(); //split into strings
                                    foreach (string info in image_uris)
                                    {
                                        stringliste.Add(info); //and each substring
                                    }

                                    string image_uris_result = (stringliste[11]); // Get string from list in position 11 
                                    image_uris_result = image_uris_result.Remove(0, 1); // Clean the string and remove the first "
                                    image_uris_result = image_uris_result.Substring(0, image_uris_result.Length - 2); // Clean the string and remove the two last chars ",

                                    var wc = new WebClient(); // Create a new webclient
                                    Image x = Image.FromStream(wc.OpenRead(image_uris_result)); // Use webclient to read datastream as an image and save to variable x
                                    pictureBox1.Image = x; // Assign picturebox image as x

                                    if (Directory.Exists(path))
                                    { // If that path exisits do nothing yet
                                    }
                                    else { CreateFolder(path); } // If that path does NOT exist create the folder
                                    x.Save(path + scryfallid + ".jpeg", ImageFormat.Jpeg); // Save data from variable x to path + \ + the scryfallid + jpeg as a image format jpeg.
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Program most likely had a 404 error see debug log or messageboxes for details " + Convert.ToString(ex));
                        }
                    }
                    catch (Exception ed)
                    {
                        MessageBox.Show(Convert.ToString(("{0} Exception caught.", ed)), "Harmless Error #1 - Safe to ignore");
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("No connection to data found, check your data source::::" + ex); }
        }
        private void mtgSortingBoardDataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                //Get 
                int selectedrowindex2 = mtgSortingBoardDataGridView.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow2 = mtgSortingBoardDataGridView.Rows[selectedrowindex2];
                string scryfallid2 = Convert.ToString(selectedRow2.Cells["id"].Value);
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Dreameater\"; // Define path as C:\Users\user\Documents\Dreameater
                label6.Text = scryfallid2;
                //label11.Text = Convert.ToString(selectedRow2.Cells["NM Price Eur"].Value);
                //int is_reserved = Convert.ToInt32(selectedRow2.Cells["isReserved"].Value);
                //int is_online = Convert.ToInt32(selectedRow2.Cells["isonlineonly"].Value);
                // if (is_reserved == 1) { pictureBox2.Visible = true; } else { pictureBox2.Visible = false; }
                // if (is_online == 1) { pictureBox3.Visible = true; } else { pictureBox3.Visible = false; }
                //label12.Text = Convert.ToString(selectedRow2.Cells["Foil Price Eur"].Value);
                if (File.Exists(path + scryfallid2 + ".jpeg")) //Testing to see if image has allready been downloaded.
                {
                    pictureBox1.ImageLocation = (path + scryfallid2 + ".jpeg");

                }
                else
                { //If the image is not downloaded - go online and download it

                    try
                    {
                        int selectedrowindex = mtgSortingBoardDataGridView.SelectedCells[0].RowIndex;
                        DataGridViewRow selectedRow = mtgSortingBoardDataGridView.Rows[selectedrowindex];
                        string scryfallid = Convert.ToString(selectedRow.Cells["scryfallid"].Value);
                        RestClient rClient = new RestClient();
                        rClient.endPoint = "https://api.scryfall.com/cards/" + scryfallid;
                        //Get data from Scryfall API

                        string strResponse = string.Empty;
                        //Initiate variable

                        strResponse = rClient.makeRequest();
                        // MessageBox.Show(Convert.ToString(strResponse));
                        try
                        {
                            JObject jsonObj = JObject.Parse(strResponse);

                            foreach (JProperty obj in jsonObj.Properties())
                            {
                                if (obj.Name == "image_uris") //select list of links
                                {

                                    String text = Convert.ToString(obj);

                                    var stringliste = new List<string> { }; //create a empty list

                                    string[] image_uris = text.Split(); //split into strings
                                    foreach (string info in image_uris)
                                    {
                                        stringliste.Add(info); //and each substring
                                    }

                                    string image_uris_result = (stringliste[11]); // Get string from list in position 11 
                                    image_uris_result = image_uris_result.Remove(0, 1); // Clean the string and remove the first "
                                    image_uris_result = image_uris_result.Substring(0, image_uris_result.Length - 2); // Clean the string and remove the two last chars ",

                                    var wc = new WebClient(); // Create a new webclient
                                    Image x = Image.FromStream(wc.OpenRead(image_uris_result)); // Use webclient to read datastream as an image and save to variable x
                                    pictureBox1.Image = x; // Assign picturebox image as x

                                    if (Directory.Exists(path))
                                    { // If that path exisits do nothing yet
                                    }
                                    else { CreateFolder(path); } // If that path does NOT exist create the folder
                                    x.Save(path + scryfallid + ".jpeg", ImageFormat.Jpeg); // Save data from variable x to path + \ + the scryfallid + jpeg as a image format jpeg.
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Program most likely had a 404 error see debug log or messageboxes for details " + Convert.ToString(ex));
                        }
                    }
                    catch (Exception ed)
                    {
                        MessageBox.Show(Convert.ToString(("{0} Exception caught.", ed)), "Harmless Error #1 - Safe to ignore");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("bonkers42 " + Convert.ToString(ex));
            }
        }
        #endregion
        // Buttons
        #region Buttons
        // Empty sort board database
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
        // Show pictures 
        private void button3_Click_1(object sender, EventArgs e)
        {
            string path =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Process.Start(path + @"\Dreameater");
        }
        private void button_send_to_sortingboard_Click(object sender, EventArgs e)
        {
            int selectedrowindex = mTGCardsDataGridView.SelectedCells[0].RowIndex; //
            DataGridViewRow selectedRow = mTGCardsDataGridView.Rows[selectedrowindex];
            string scryfallid = Convert.ToString(selectedRow.Cells["ScryFallID"].Value);
            string set_name = Convert.ToString(selectedRow.Cells["Set"].Value);
            string name = Convert.ToString(selectedRow.Cells["Name"].Value);

            name = name.Replace("'", $"{(char)39}");
            // string Command = "";
            string Command = "INSERT INTO dbo.MTGCardsSortBoard SELECT * FROM [MTGCardsDatabase] where " +
                "      [id] like '" + scryfallid +
                "' and [set_name] like '" + set_name.Replace("'", "''") +
                "' and [name] like '" + name.Replace("'", "''") + "'";
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
        private void button5_Click(object sender, EventArgs e)
        {
            string Command = "DECLARE @sortboard int DECLARE MY_CURSOR CURSOR " +
                              "LOCAL STATIC READ_ONLY FORWARD_ONLY " +
                              "FOR SELECT DISTINCT sortboard " +
                              "FROM DarkOakDB.dbo.MTGCardsSortBoard " +
                              "OPEN MY_CURSOR " +
                              "FETCH NEXT FROM MY_CURSOR INTO @sortboard " +
                 "WHILE @@FETCH_STATUS = 0 " +
                 "BEGIN " +
                 "DECLARE @number_of_cards_in_collection int " +
                 "DECLARE @id nvarchar(max) " +
                 "set @id = (select id from DarkOakDB.dbo.MTGCardsSortBoard where sortboard = @sortboard) " +
                 "if exists(SELECT * FROM DarkOakDB.dbo.MTGCardsCollection WHERE id = @id) " +
                 "begin " +
                 "set @number_of_cards_in_collection = (select amount_owned from DarkOakDB.dbo.MTGCardsCollection where id = @id) " +
                 "set @number_of_cards_in_collection = @number_of_cards_in_collection + 1 " +
                 "update DarkOakDB.dbo.MTGCardsCollection set amount_owned = @number_of_cards_in_collection where id = @id " +
                 "end " +
                 "else " +
                 "begin " +
                 "insert into DarkOakDB.dbo.MTGCardsCollection " +
                 "select *,1,0,'','','',0,0  from DarkOakDB.dbo.MTGCardsDatabase " +
                 "where id = @id " +
                 "end " +
                 "FETCH NEXT FROM MY_CURSOR INTO @sortboard " +
                 "end " +
                 "CLOSE MY_CURSOR " +
                 "DEALLOCATE MY_CURSOR " +
                 "delete from[DarkOakDB].[dbo].[MTGCardsSortBoard] ";
            using (SqlConnection myConnection = new SqlConnection(Properties.Settings.Default.DarkOakDBConnectionString))
            {
                myConnection.Open();
                using (SqlCommand myCommand = new SqlCommand(Command, myConnection))
                {
                    myCommand.ExecuteScalar(); //runs Command string hopefully
                }
                myConnection.Close();
                //  MessageBox.Show(Command);
                PullDataFromSortBoard();
                PullDataFromCollection();
            }

        }

        #endregion
        // REST API and Internet Data Ingress
        #region Aceess data from Internet
        private void pull_Images_from_Internet()
        {
            try
            {
                //Get 
                int selectedrowindex2 = mTGCardsDataGridView.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow2 = mTGCardsDataGridView.Rows[selectedrowindex2];
                string scryfallid2 = Convert.ToString(selectedRow2.Cells["scryfallid"].Value);
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Dreameater\"; // Define path as C:\Users\user\Documents\Dreameater
                label6.Text = scryfallid2;
                label11.Text = Convert.ToString(selectedRow2.Cells["NM Price Eur"].Value);
                //int is_reserved = Convert.ToInt32(selectedRow2.Cells["isReserved"].Value);
                //int is_online = Convert.ToInt32(selectedRow2.Cells["isonlineonly"].Value);
                // if (is_reserved == 1) { pictureBox2.Visible = true; } else { pictureBox2.Visible = false; }
                // if (is_online == 1) { pictureBox3.Visible = true; } else { pictureBox3.Visible = false; }
                label12.Text = Convert.ToString(selectedRow2.Cells["Foil Price Eur"].Value);
                if (File.Exists(path + scryfallid2 + ".jpeg")) //Testing to see if image has allready been downloaded.
                {
                    pictureBox1.ImageLocation = (path + scryfallid2 + ".jpeg");

                }
                else
                { //If the image is not downloaded - go online and download it

                    try
                    {
                        int selectedrowindex = mTGCardsDataGridView.SelectedCells[0].RowIndex;
                        DataGridViewRow selectedRow = mTGCardsDataGridView.Rows[selectedrowindex];
                        string scryfallid = Convert.ToString(selectedRow.Cells["scryfallid"].Value);
                        RestClient rClient = new RestClient();
                        rClient.endPoint = "https://api.scryfall.com/cards/" + scryfallid;
                        //Get data from Scryfall API

                        string strResponse = string.Empty;
                        //Initiate variable

                        strResponse = rClient.makeRequest();
                        // MessageBox.Show(Convert.ToString(strResponse));
                        try
                        {
                            JObject jsonObj = JObject.Parse(strResponse);

                            foreach (JProperty obj in jsonObj.Properties())
                            {
                                if (obj.Name == "image_uris") //select list of links
                                {

                                    String text = Convert.ToString(obj);

                                    var stringliste = new List<string> { }; //create a empty list

                                    string[] image_uris = text.Split(); //split into strings
                                    foreach (string info in image_uris)
                                    {
                                        stringliste.Add(info); //and each substring
                                    }

                                    string image_uris_result = (stringliste[11]); // Get string from list in position 11 
                                    image_uris_result = image_uris_result.Remove(0, 1); // Clean the string and remove the first "
                                    image_uris_result = image_uris_result.Substring(0, image_uris_result.Length - 2); // Clean the string and remove the two last chars ",

                                    var wc = new WebClient(); // Create a new webclient
                                    Image x = Image.FromStream(wc.OpenRead(image_uris_result)); // Use webclient to read datastream as an image and save to variable x
                                    pictureBox1.Image = x; // Assign picturebox image as x

                                    if (Directory.Exists(path))
                                    { // If that path exisits do nothing yet
                                    }
                                    else { CreateFolder(path); } // If that path does NOT exist create the folder
                                    x.Save(path + scryfallid + ".jpeg", ImageFormat.Jpeg); // Save data from variable x to path + \ + the scryfallid + jpeg as a image format jpeg.
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Program most likely had a 404 error see debug log or messageboxes for details " + Convert.ToString(ex));
                        }
                    }
                    catch (Exception ed)
                    {
                        MessageBox.Show(Convert.ToString(("{0} Exception caught.", ed)), "Harmless Error #1 - Safe to ignore");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("bonkers42 " + Convert.ToString(ex));
            }
        }
        #endregion
        // Benched Functions that might be usefull later
        #region Abandoned Functions
        public void runsqlquery(string query)
        {

            SqlConnection conn = new SqlConnection(@"Data Source=sqlsrv-mtgdb.database.windows.net;Initial Catalog=DarkOakDB;User ID=htorne;Password=Ia3#qFJz");
            //SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.ExecuteScalar(); //runs Command string hopefully
            }
            conn.Close();

        }
        #endregion
        // Filesystem operations and such
        #region Backend Functions
        public static void CreateFolder(string path)
        {
            DirectoryInfo di = Directory.CreateDirectory(path);
        }
        #endregion
        // Code activly being worked on
        #region WORK AREA
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            PullData();
            filterstuff();
        }
        #endregion
    }
}