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
        
        public static string LastName (string str)
        {
            string lastName = "";
            int i = 0;
            while (str[i] != ',')
            {
                if (str[i]!='\'')
                {
                    lastName = lastName + str[i];
                    i++;
                }
            }
            i++; // пропускаем запятую в строке 
            lastName = lastName + str[i] + str[i + 1];
            return lastName;
        }

        public static int CountAuthor (DataRow author)
        {
            AuthorRow = author;
            string ssqlconnectionstring = "Data Source=LAPTOP-LCJH6N9V;Initial Catalog=dip;Integrated Security=SSPI";
            SqlConnection conn = new SqlConnection(ssqlconnectionstring);
            conn.Open();
            string author_name = author["Авторы"].ToString();
            //выделить фамилию
            string last_name = LastName(author_name);
            
            //добавили к строке поиска первую букву имени 
            string sql = "SELECT * FROM [dip].[dbo].[Employees] WHERE translit_name LIKE '"+ last_name + "%' OR synonym LIKE '%"+ last_name+"%'";
            SqlDataAdapter daEmpl = new SqlDataAdapter(sql, conn);
            DataSet dsEmpl = new DataSet("dip");
            
            daEmpl.FillSchema(dsEmpl, SchemaType.Source, "[dbo].[Employees]");
            daEmpl.Fill(dsEmpl, "[dbo].[Employees]");
            dtEmplSearch = dsEmpl.Tables["[dbo].[Employees]"];


            return dtEmplSearch.Rows.Count;
        }
    }
}
