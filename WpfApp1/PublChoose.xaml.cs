using System;
using System.Collections.Generic;
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
    public partial class PublChoose : Window
    {
        public PublChoose()
        {
            InitializeComponent();
            

            dataGridOld.ItemsSource = Publication_Verif.dtPubl.DefaultView;
            dataGridNew.Items.Add(Publication_Verif.verif);

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
                "' AND [Год] = '"+year+ "' AND [Название журнала] ='" +journ + "' AND [Кол-во авторов] = '" + authCount + "' AND [DOI] = '" + 
                doi + "' AND [Вид публикации] = '" + publType + "' AND "+ delStr;
            SqlCommand cmdDel = new SqlCommand(sql, conn);
            int count = cmdDel.ExecuteNonQuery();

            auth = Publication_Verif.verif["Авторы"].ToString();
            publName = Publication_Verif.verif["Название публикации"].ToString();
            year = Publication_Verif.verif["Год"].ToString();
            journ = Publication_Verif.verif["Название журнала"].ToString();
            authCount = Publication_Verif.verif["Кол-во авторов"].ToString();
            doi = Publication_Verif.verif["DOI"].ToString();
            publType = Publication_Verif.verif["Вид публикации"].ToString();
            delStr = "";
            insStr = "";
            string insVal = "";

            SqlCommand cmd = new SqlCommand();

            if (Dataview.res == "Scopus")
            {
                string snip = Publication_Verif.verif["SNIP"].ToString();
                string quartileSc = Publication_Verif.verif["Квартили по Scopus (SJR)"].ToString();
                insVal = "@snip, @quartileSc";               
                insStr = "[SNIP], [Квартили по Scopus (SJR)])";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@snip", SqlDbType.NVarChar).Value = snip;
                cmd.Parameters.Add("@quartileSc", SqlDbType.NVarChar).Value = quartileSc;
            };
            if (Dataview.res == "WoS")
            {
                string JIF = Publication_Verif.dtPubl.Rows[0]["Journal Impact Factor"].ToString();
                string AIS = Publication_Verif.dtPubl.Rows[0]["Article Influence Score"].ToString();
                string quartileWos = Publication_Verif.dtPubl.Rows[0]["Квартиль по WoS (JCR)"].ToString();
                insStr = "[Journal Impact Factor], [Article Influence Score], [Квартиль по WoS (JCR)])";
                insVal = "@JIF, @AIS, @quartileWoS";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@JIF", SqlDbType.NVarChar).Value = JIF;
                cmd.Parameters.Add("@AIS", SqlDbType.NVarChar).Value = AIS;
                cmd.Parameters.Add("@quartileWos", SqlDbType.NVarChar).Value = quartileWos;
            };

            string sqlIns = "INSERT INTO [dip].[dbo].[Publ] ([Авторы], [Название публикации],  [Год], [Название журнала], [Кол-во авторов], [DOI], [Вид публикации], " + 
                insStr + "VALUES @auth, @publName, @year, @journ, @authCount, @doi, @publType, "+insVal;

            
            cmd.Connection = conn;
            cmd.CommandText = sqlIns;
            int inscount = cmd.ExecuteNonQuery();
        }
    }
}
