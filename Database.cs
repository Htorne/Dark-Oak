using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace Dark_Oak
{
    class Database
    {


        public static void Test_SQL_Connection()
        {
            string SQLUser = Properties.Settings.Default.SQLUser;
            string SQLPassword = Properties.Settings.Default.SQLPassword;
            string connectionString = Properties.Settings.Default.DarkOakDBConnectionString;
            SqlConnection cnn;
            cnn = new SqlConnection(connectionString);
            try
            {
                cnn.Open();
                MessageBox.Show("Connection Open !");
                cnn.Close();
                
            } catch (Exception ex)
            {
                MessageBox.Show("Unable to connect to SQL");
                MessageBox.Show(Convert.ToString(ex));
            }


        }
        public static byte[] GetImage(string a)
        {
            byte[] result = null;

             string query = "SELECT ImageData from MTGCardsImages where ImageID like '"+a +"'";
 
            using (SqlConnection conn = new SqlConnection
                      ("Data Source=sqlsrv-mtgdb.database.windows.net;Initial Catalog=DarkOakDB;User ID=htorne;Password=Ia3#qFJz"))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    result = (byte[])dr["ImageData"];
                }
            }
            return result;
        }

       
    }
}
