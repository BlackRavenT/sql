using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для PublChoose.xaml
    /// </summary>
    /// 
    public class NewPublication
    {
        public string auth;
        public string publNameIns;
        public string year;
        public string journ;
        public string authCount;
        public string doi;
        public string publType;
        public string snip;
        public string quartileSc;
    }


    public partial class PublChoose : Window
    {
        public PublChoose()
        {
            InitializeComponent();
            ObservableCollection<NewPublication> coll = new ObservableCollection<NewPublication>();
            coll.Add(new NewPublication() { auth = Publication_Verif.verif["Авторы"].ToString(),
                publNameIns = Publication_Verif.verif["Название публикации"].ToString(),
                year = Publication_Verif.verif["Год"].ToString(),
                journ = Publication_Verif.verif["Название журнала"].ToString(),
                authCount = Publication_Verif.verif["Кол-во авторов"].ToString(),
                doi = Publication_Verif.verif["DOI"].ToString(),
                publType = Publication_Verif.verif["Вид публикации"].ToString(),
                snip = Publication_Verif.verif["SNIP"].ToString(),
                quartileSc = Publication_Verif.verif["Квартили по Scopus (SJR)"].ToString()
        });
            dataGridNew.ItemsSource = coll;
            dataGridNew.Items.Refresh();

            dataGridOld.ItemsSource = Publication_Verif.dtPubl.DefaultView;
            /*DataSet dsNew = new DataSet("new");
            DataTable table = new DataTable();
            table = Dataview.newDt.Clone();
            //DataRow row = 
            table = Publication_Verif.dtPubl.Clone();
            DataRow row = table.NewRow();
            row = Publication_Verif.verif;

            table.Rows.Add(row);
            dataGridNew.ItemsSource = table.DefaultView;
            


             dataGridNew.Items.Add(Publication_Verif.verif);
            dataGridNew.Items.Insert(1, Publication_Verif.verif);
            MessageBox.Show(Publication_Verif.verif[1].ToString());*/
           /* string ssqlconnectionstring = "Data Source=LAPTOP-LCJH6N9V;Initial Catalog=dip;Integrated Security=SSPI";
            SqlConnection conn = new SqlConnection(ssqlconnectionstring);
            conn.Open();

            string sql = "SELECT * FROM [dip].[dbo].[Publ] WHERE [Название публикации] = '" + Publication_Verif.publDB[""] + "'";
            SqlDataAdapter daPubl = new SqlDataAdapter(sql, conn);
            DataSet dsPubl = new DataSet("publications");
            daPubl.FillSchema(dsPubl, SchemaType.Source, "[dbo].[Publ]");
            daPubl.Fill(dsPubl, "[dbo].[Publ]");
            DataTable dtPubl;
            dtPubl = dsPubl.Tables["[dbo].[Publ]"];

            string sql = "SELECT * FROM [dip].[dbo].[Employees] WHERE translit_name LIKE '" + first + "%'";
            SqlDataAdapter daEmpl = new SqlDataAdapter(sql, conn);
            DataSet dsEmpl = new DataSet("dip");
            daEmpl.FillSchema(dsEmpl, SchemaType.Source, "[dbo].[Employees]");
            daEmpl.Fill(dsEmpl, "[dbo].[Employees]");
            DataTable dtSearch;
            dtSearch = dsEmpl.Tables["[dbo].[Employees]"];

            string s = dataGridViewAuth_0.CurrentCell.ToString();
            dataGridViewAuth_0.ItemsSource = dtSearch.DefaultView;*/

        }

        private void buttonOld_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonAddNew_Click(object sender, RoutedEventArgs e)
        {
            string ssqlconnectionstring = "Data Source=LAPTOP-LCJH6N9V;Initial Catalog=dip;Integrated Security=SSPI";
            SqlConnection conn = new SqlConnection(ssqlconnectionstring);
            conn.Open();


            string auth = Publication_Verif.dtPubl.Rows[0]["Авторы"].ToString();
            string publName = Publication_Verif.dtPubl.Rows[0]["Название публикации"].ToString();
            string year = Publication_Verif.dtPubl.Rows[0]["Год"].ToString();
            string journ = Publication_Verif.dtPubl.Rows[0]["Название журнала"].ToString();
            string countQuote = Publication_Verif.dtPubl.Rows[0]["Кол-во цитирований"].ToString();
            string authCount = Publication_Verif.dtPubl.Rows[0]["Кол-во авторов"].ToString();
            string doi = Publication_Verif.dtPubl.Rows[0]["DOI"].ToString();
            string publType = Publication_Verif.dtPubl.Rows[0]["Вид публикации"].ToString();
            string delStr = "";
            string insStr = "";
            if (Dataview.res == "Scopus")
            {
                string snip = Publication_Verif.dtPubl.Rows[0]["SNIP"].ToString();
                string quartileSc = Publication_Verif.dtPubl.Rows[0]["Квартили по Scopus (SJR)"].ToString();
                delStr = "[SNIP] = '" +snip + "' AND [Квартили по Scopus(SJR)] = '"+quartileSc +"'";
                insStr = "[SNIP], [Квартили по Scopus (SJR)])";
            };
            if (Dataview.res == "WoS")
            {
                string JIF = Publication_Verif.dtPubl.Rows[0]["Journal Impact Factor"].ToString();
                string AIS = Publication_Verif.dtPubl.Rows[0]["Article Influence Score"].ToString();
                string quartileWos = Publication_Verif.dtPubl.Rows[0]["Квартиль по WoS (JCR)"].ToString();
                delStr = "[Journal Impact Factor] = '" + JIF + "' AND [Article Influence Score] = '"+ AIS + "' AND [Квартиль по WoS (JCR)] = '"+quartileWos+"'";
                insStr = "[Journal Impact Factor], [Article Influence Score], [Квартиль по WoS (JCR)])";
            };


            string sql = "DELETE FROM [dip].[dbo].[Publ] WHERE [Авторы] = '"+ auth + "' AND [Название публикации] = '"+ publName +
                "' AND [Год] = '"+year+ "' AND [Название журнала] ='" +journ + "AND [Кол-во цитирований] = '"+ countQuote + "' AND [Кол-во авторов] = '" + authCount + "' AND [DOI] = '" + 
                doi + "' AND [Вид публикации] = '" + publType + "' AND "+ delStr;
            SqlCommand cmdDel = new SqlCommand(sql, conn);
            int count = cmdDel.ExecuteNonQuery();

            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Clear();

            auth = Publication_Verif.verif["Авторы"].ToString();
            cmd.Parameters.Add("@auth", SqlDbType.NVarChar).Value = auth;
            publName = Publication_Verif.verif["Название публикации"].ToString();
            cmd.Parameters.Add("@publName", SqlDbType.NVarChar).Value = publName;
            year = Publication_Verif.verif["Год"].ToString();
            cmd.Parameters.Add("@year", SqlDbType.Int).Value = year;
            journ = Publication_Verif.verif["Название журнала"].ToString();
            cmd.Parameters.Add("@journ", SqlDbType.NVarChar).Value = journ;

            countQuote = Publication_Verif.verif["Кол-во цитирований"].ToString();
            cmd.Parameters.Add("@countQuote", SqlDbType.Int).Value = countQuote;

            authCount = Publication_Verif.verif["Кол-во авторов"].ToString();
            cmd.Parameters.Add("@authCount", SqlDbType.NVarChar).Value = authCount;
            doi = Publication_Verif.verif["DOI"].ToString();
            cmd.Parameters.Add("@doi", SqlDbType.NVarChar).Value = doi;
            publType = Publication_Verif.verif["Вид публикации"].ToString();
            cmd.Parameters.Add("@publType", SqlDbType.NVarChar).Value = publType;
            delStr = "";
            insStr = "";
            string insVal = "";

            

            if (Dataview.res == "Scopus")
            {
                string snip = Publication_Verif.verif["SNIP"].ToString();
                string quartileSc = Publication_Verif.verif["Квартили по Scopus (SJR)"].ToString();
                insVal = "@snip, @quartileSc)";               
                insStr = "[SNIP], [Квартили по Scopus (SJR)])";
                //cmd.Parameters.Clear();
                cmd.Parameters.Add("@snip", SqlDbType.NVarChar).Value = snip;
                cmd.Parameters.Add("@quartileSc", SqlDbType.NVarChar).Value = quartileSc;
            };
            if (Dataview.res == "WoS")
            {
                string JIF = Publication_Verif.dtPubl.Rows[0]["Journal Impact Factor"].ToString();
                string AIS = Publication_Verif.dtPubl.Rows[0]["Article Influence Score"].ToString();
                string quartileWos = Publication_Verif.dtPubl.Rows[0]["Квартиль по WoS (JCR)"].ToString();
                insStr = "[Journal Impact Factor], [Article Influence Score], [Квартиль по WoS (JCR)])";
                insVal = "@JIF, @AIS, @quartileWoS)";
                
                cmd.Parameters.Add("@JIF", SqlDbType.NVarChar).Value = JIF;
                cmd.Parameters.Add("@AIS", SqlDbType.NVarChar).Value = AIS;
                cmd.Parameters.Add("@quartileWos", SqlDbType.NVarChar).Value = quartileWos;
            };

            string sqlIns = "INSERT INTO [dip].[dbo].[Publ] ([Авторы], [Название публикации],  [Год], [Название журнала],[Кол-во цитирований], [Кол-во авторов], [DOI], [Вид публикации], " + 
                insStr + "VALUES (@auth, @publName, @year, @journ, @countQuote, @authCount, @doi, @publType, " + insVal;
            
            cmd.Connection = conn;
            cmd.CommandText = sqlIns;
            int inscount = cmd.ExecuteNonQuery();
        }
    }
}
