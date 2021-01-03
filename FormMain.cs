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
            //string query = "select [card_number],[web_scraper_order],[card_name],[creature_type] as [Type],[card_rules2],[set_name],[rareity_code],[note] as [Artist], [card_type] as [Color] from [dbo].[MTGCards]";
            string query = "select [number] as [#],[name],[setCode] as [Set Name],[originalType],[originalText],[scryfallid],[regularprice],[foilprice],[isReserved] from dbo.MTGCardsDatabase";
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
        // Opens the settings menu vis the Strip menu
        public void FormMain_Load(object sender, EventArgs e)
        {


        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int selectedrowindex = mTGCardsDataGridView.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = mTGCardsDataGridView.Rows[selectedrowindex];
            // [OLD] string a = Convert.ToString(selectedRow.Cells["web_scraper_order"].Value);
        }
        #region filters
        public void textBox1_TextChanged(object sender, EventArgs e)
        {
            PullData();
            try
            {
                if (textBox1.Text == "")
                {
                    (mTGCardsDataGridView.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
                }
                else
                {
                    (mTGCardsDataGridView.DataSource as DataTable).DefaultView.RowFilter = string.Format("[#] = '{0}'", textBox1.Text);
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
        public void filterstuff()
        {

            BindingSource bs = new BindingSource();
            bs.DataSource = mTGCardsDataGridView.DataSource;
            string cardname = textBox2.Text;
            string set = textBox3.Text;
            string originalType = textBox4.Text;
            var filt = "";


            if (!string.IsNullOrEmpty(cardname))
            {
                if (filt == "")
                    filt += "[name] LIKE '%" + cardname + "%'";
                else
                    filt += " And [name] LIKE '%" + cardname + "%' ";
            }


            if (!string.IsNullOrEmpty(set))
            {
                if (filt == "")
                    filt += "[Set Name] LIKE '%" + set + "%'";
                else
                    filt += " And [Set Name] LIKE '%" + set + "%' ";
            }


            if (!string.IsNullOrEmpty(originalType))
            {
                if (filt == "")
                    filt += "[originalType] LIKE '%" + originalType + "%'";
                else
                    filt += " And [originalType] LIKE '%" + originalType + "%' ";
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
        #endregion
        #region GridFormatting 
        private void mTGCardsDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.mTGCardsDataGridView.Columns["scryfallid"].Visible = false;
            //this.mTGCardsDataGridView.Columns["mcmid"].Visible = false;
            this.mTGCardsDataGridView.Columns[0].HeaderText = "#";
            this.mTGCardsDataGridView.Columns[1].HeaderText = "Card Name";
            this.mTGCardsDataGridView.Columns[2].HeaderText = "Set";
            this.mTGCardsDataGridView.Columns[3].HeaderText = "Type";
            this.mTGCardsDataGridView.Columns[4].HeaderText = "Card Text";
            this.mTGCardsDataGridView.Columns[6].HeaderText = "Normal";
            this.mTGCardsDataGridView.Columns[7].HeaderText = "Foil";
            // this.mTGCardsDataGridView.Columns[4].HeaderText = "Card Text";
            this.mTGCardsDataGridView.AllowUserToResizeColumns = false;
            this.mTGCardsDataGridView.AllowUserToResizeRows = false;
            this.mTGCardsDataGridView.Columns[0].Width = 45;
            this.mTGCardsDataGridView.Columns[1].Width = 200;
            this.mTGCardsDataGridView.Columns[2].Width = 60;
            this.mTGCardsDataGridView.Columns[3].Width = 225;
            this.mTGCardsDataGridView.Columns[4].Width = 500;
            this.mTGCardsDataGridView.Columns[6].Width = 60;
            this.mTGCardsDataGridView.Columns[7].Width = 60;


            this.mTGCardsDataGridView.AllowUserToAddRows = false;
        }
        private void mtgSortingBoardDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.mtgSortingBoardDataGridView.Columns["scryfallid"].Visible = false;
            this.mtgSortingBoardDataGridView.Columns["mcmid"].Visible = false;
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
        #endregion

        private void mTGCardsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            int selectedrowindex = mTGCardsDataGridView.SelectedCells[0].RowIndex; //
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
            //    MessageBox.Show(Command);
            PullDataFromSortBoard();
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

                    Image img = Image.FromStream(stream);
                    pictureBox1.Image = img;
                    
                    string path =
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    MessageBox.Show(path + @"\Dreameater");
                    img.Save(Path.GetTempPath() + path, ImageFormat.Jpeg);
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
        // If you press the plus sign on the numpad it adds a card to the collection board.
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
        private void mTGCardsDataGridView_MouseClick(object sender, MouseEventArgs e)
        // When you use the mouse to select a card in the table
        {

            int selectedrowindex2 = mTGCardsDataGridView.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow2 = mTGCardsDataGridView.Rows[selectedrowindex2];
            string scryfallid2 = Convert.ToString(selectedRow2.Cells["scryfallid"].Value);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Dreameater\"; // Define path as C:\Users\user\Documents\Dreameater
            label6.Text = scryfallid2;
            if (File.Exists(path+scryfallid2+".jpeg")) //Testing to see if image has allready been downloaded.
            { 
                pictureBox1.ImageLocation = (path + scryfallid2 + ".jpeg");
            
            } else { 

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

                            
                            if (Directory.Exists(path)) { // If that path exisits do nothing yet
                            } else { CreateFolder(path); } // If that path does NOT exist create the folder
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
                //MessageBox.Show(Convert.ToString(("{0} Exception caught.", ed)), "Harmless Error #1 - Safe to ignore");
            }

            // With the first part above done, where we got the image using the scryfall scryfallid, we now 
            // attempt to get market data using the mcmid information to get the regular and foil prices. 
            try
            {

                int selectedrowindex = mTGCardsDataGridView.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = mTGCardsDataGridView.Rows[selectedrowindex];
                string mcmid = Convert.ToString(selectedRow.Cells["mcmid"].Value);
                RestClient rClient = new RestClient();
                rClient.endPoint = "https://api.scryfall.com/cards/cardmarket/" + mcmid;
                // Here we retrive the card mcmid information from the database and preform an
                // API call to check to see if the card has any market data. There are serval
                // cards which does not have any market data on scryfall.

                string strResponse = string.Empty;
                strResponse = rClient.makeRequest();
                //MessageBox.Show(Convert.ToString(strResponse));
                try
                {
                    JObject jsonObj = JObject.Parse(strResponse);

                    foreach (JProperty obj in jsonObj.Properties())
                    {
                        if (obj.Name == "prices")
                        {
                            String text = Convert.ToString(obj);
                            var stringliste = new List<string> { };
                            string[] jsonarray = text.Split();
                            foreach (string priceinfo in jsonarray)
                            {
                                stringliste.Add(priceinfo);
                            }
                            string foilprice = (stringliste[11]);
                            //MessageBox.Show(foilprice);
                            if (foilprice == "null,") { } else {
                                foilprice = foilprice.Remove(0, 1);
                                foilprice = foilprice.Substring(0, foilprice.Length - 2);
                                decimal foilrealprice = Convert.ToDecimal(foilprice);
                            }
                        }
                    }
                    foreach (JProperty obj in jsonObj.Properties())
                    {
                        if (obj.Name == "prices")
                        {
                            //string gogo = (string)obj[0]["small"][0];
                            String text = Convert.ToString(obj);
                            //MessageBox.Show(text);
                            // MessageBox.Show("Type is " + Convert.ToString(obj.GetType()));
                            var stringliste = new List<string> { };
                            string[] jsonarray = text.Split();
                            foreach (string priceinfo in jsonarray)
                            {
                                // MessageBox.Show(info);
                                stringliste.Add(priceinfo);
                            }
                            string Nonfoilprice = (stringliste[6]);
                            Nonfoilprice = Nonfoilprice.Remove(0, 1);
                            Nonfoilprice = Nonfoilprice.Substring(0, Nonfoilprice.Length - 2);
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
                //MessageBox.Show(Convert.ToString(("{0} Exception caught.", ed)), "Harmless Error #1 - Safe to ignore");
            }
            }
        }
        private void updatePricesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int lenght = retrieveMCMids().Count;
            MessageBox.Show(Convert.ToString(lenght));
            int counter = 0;

            foreach (var card in retrieveMCMids())
            {
                counter++;
                try
                {

                    System.Threading.Thread.Sleep(200);
                    string mcmid = card;
                    RestClient rClient = new RestClient();
                    rClient.endPoint = "https://api.scryfall.com/cards/cardmarket/" + mcmid;
                    // Here we retrive the card mcmid information from the database and preform an
                    // API call to check to see if the card has any market data. There are serval
                    // cards which does not have any market data on scryfall.

                    string strResponse = string.Empty;
                    strResponse = rClient.makeRequest();
                    //MessageBox.Show(Convert.ToString(strResponse));
                    try
                    {
                        decimal ext_Nonfoilprice = 0;
                        decimal ext_foilprice = 0;
                        JObject jsonObj = JObject.Parse(strResponse);
                        foreach (JProperty obj in jsonObj.Properties())
                        {
                            if (obj.Name == "prices")
                            {
                                String text = Convert.ToString(obj);
                                var stringliste = new List<string> { };
                                string[] jsonarray = text.Split();
                                foreach (string priceinfo in jsonarray)
                                {
                                    stringliste.Add(priceinfo);
                                }
                                string foilprice = (stringliste[11]);
                                //MessageBox.Show(foilprice);
                                if (foilprice == "null,") { }
                                else
                                {
                                    foilprice = foilprice.Remove(0, 1);
                                    foilprice = foilprice.Substring(0, foilprice.Length - 2);
                                    decimal foilrealprice = Convert.ToDecimal(foilprice);
                                    ext_foilprice = foilrealprice;


                                }
                            }
                        }
                        foreach (JProperty obj in jsonObj.Properties())
                        {
                            if (obj.Name == "prices")
                            {
                                //string gogo = (string)obj[0]["small"][0];
                                String text = Convert.ToString(obj);
                                //MessageBox.Show(text);
                                // MessageBox.Show("Type is " + Convert.ToString(obj.GetType()));
                                var stringliste = new List<string> { };
                                string[] jsonarray = text.Split();
                                foreach (string priceinfo in jsonarray)
                                {
                                    // MessageBox.Show(info);
                                    stringliste.Add(priceinfo);
                                }

                                string Nonfoilprice = (stringliste[6]);
                                if (Nonfoilprice == "null,") { }
                                else {
                                    Nonfoilprice = Nonfoilprice.Remove(0, 1);
                                    Nonfoilprice = Nonfoilprice.Substring(0, Nonfoilprice.Length - 2);
                                    decimal nonfoilprice = Convert.ToDecimal(Nonfoilprice);
                                    ext_Nonfoilprice = nonfoilprice;
                                }


                            }

                        }
                        System.Diagnostics.Debug.WriteLine("FOIL = " + ext_foilprice + " : Regular = " + ext_Nonfoilprice + " [MCMid = " + card + "]");
                        System.Diagnostics.Debug.WriteLine(counter);
                        runsqlquery("insert into mcmcards values(" + card + "," + ext_foilprice + "," + ext_Nonfoilprice + ")");


                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(Convert.ToString(ex));
                    }




                }
                catch (Exception ed)
                {
                    //MessageBox.Show(Convert.ToString(("{0} Exception caught.", ed)), "Harmless Error #1 - Safe to ignore");
                }
                //System.Diagnostics.Debug.WriteLine(card);
            }

        }
        public static List<string> retrieveMCMids()
        {
            List<string> columnData = new List<string>();

            using (SqlConnection connection = new SqlConnection(@"Data Source=192.168.1.117\Razorback;Initial Catalog=DarkOakDB;User ID=Max;Password=Ia3#qFJz"))
            {
                connection.Open();
                string query = "SELECT mcmid FROM dbo.cards where mcmid is not null";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            columnData.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return columnData;
        }
        public void runsqlquery(string query)
        {

            SqlConnection conn = new SqlConnection(@"Data Source=192.168.1.117\Razorback;Initial Catalog=DarkOakDB;User ID=Max;Password=Ia3#qFJz");
            //SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.ExecuteScalar(); //runs Command string hopefully
            }
            conn.Close();

        }
        private void button3_Click(object sender, EventArgs e)
        {
            string path =
             Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            MessageBox.Show(path + @"\Dreameater");
        }
        public static void CreateFolder(string path){
            DirectoryInfo di = Directory.CreateDirectory(path);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string path =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Process.Start(path + @"\Dreameater");
        }
    }
}

