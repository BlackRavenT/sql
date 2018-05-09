using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace WpfApp1
{
    class Author_Verif
    {
        internal static DataTable dtEmplSearch;
        internal static DataRow AuthorRow;
        
        public static int CountAuthor (DataRow author)
        {
            AuthorRow = author;
            string ssqlconnectionstring = "Data Source=LAPTOP-LCJH6N9V;Initial Catalog=dip;Integrated Security=SSPI";
            SqlConnection conn = new SqlConnection(ssqlconnectionstring);
            conn.Open();
            string author_name = author["Авторы"].ToString();
            //выделить фамилию
            string last_name = "";
            int i = 0;
            while (author_name[i]!=',')
            {
                last_name = last_name + author_name[i];
                i++;
            }
            i++; // пропускаем запятую в строке 
            last_name = last_name + author_name[i] + author_name[i + 1]; //добавили к строке поиска первую букву имени 
            string sql = "SELECT * FROM [dip].[dbo].[Employees] WHERE translit_name LIKE '"+ last_name + "%'";
            SqlDataAdapter daEmpl = new SqlDataAdapter(sql, conn);
            DataSet dsEmpl = new DataSet("dip");
            daEmpl.FillSchema(dsEmpl, SchemaType.Source, "[dbo].[Employees]");
            daEmpl.Fill(dsEmpl, "[dbo].[Employees]");
            dtEmplSearch = dsEmpl.Tables["[dbo].[Employees]"];


            return dtEmplSearch.Rows.Count;
        }
    }
}
