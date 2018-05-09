using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows;

namespace WpfApp1
{
    class Translit
    {
        public static bool flagInit = false; //флаг заполнения словаря
        private static System.Collections.Generic.Dictionary<string, string> transliter = new System.Collections.Generic.Dictionary<string, string>();
        private static void prepareTranslit()
        {
            flagInit = true;
            transliter.Add("а", "a");
            transliter.Add("б", "b");
            transliter.Add("в", "v");
            transliter.Add("г", "g");
            transliter.Add("д", "d");
            transliter.Add("е", "e");
            transliter.Add("ё", "yo");
            transliter.Add("ж", "zh");
            transliter.Add("з", "z");
            transliter.Add("и", "i");
            transliter.Add("й", "y");
            transliter.Add("к", "k");
            transliter.Add("л", "l");
            transliter.Add("м", "m");
            transliter.Add("н", "n");
            transliter.Add("о", "o");
            transliter.Add("п", "p");
            transliter.Add("р", "r");
            transliter.Add("с", "s");
            transliter.Add("т", "t");
            transliter.Add("у", "u");
            transliter.Add("ф", "f");
            transliter.Add("х", "kh");
            transliter.Add("ц", "c");
            transliter.Add("ч", "ch");
            transliter.Add("ш", "sh");
            transliter.Add("щ", "shch");
            transliter.Add("ъ", "j");
            transliter.Add("ы", "i");
            transliter.Add("ь", "");
            transliter.Add("э", "e");
            transliter.Add("ю", "yu");
            transliter.Add("я", "ya");
            transliter.Add("А", "A");
            transliter.Add("Б", "B");
            transliter.Add("В", "V");
            transliter.Add("Г", "G");
            transliter.Add("Д", "D");
            transliter.Add("Е", "E");
            transliter.Add("Ё", "Yo");
            transliter.Add("Ж", "Zh");
            transliter.Add("З", "Z");
            transliter.Add("И", "I");
            transliter.Add("Й", "Y");
            transliter.Add("К", "K");
            transliter.Add("Л", "L");
            transliter.Add("М", "M");
            transliter.Add("Н", "N");
            transliter.Add("О", "O");
            transliter.Add("П", "P");
            transliter.Add("Р", "R");
            transliter.Add("С", "S");
            transliter.Add("Т", "T");
            transliter.Add("У", "U");
            transliter.Add("Ф", "F");
            transliter.Add("Х", "Kh");
            transliter.Add("Ц", "C");
            transliter.Add("Ч", "Ch");
            transliter.Add("Ш", "Sh");
            transliter.Add("Щ", "Shch");
            transliter.Add("Ъ", "J");
            transliter.Add("Ы", "I");
            transliter.Add("Ь", "");
            transliter.Add("Э", "E");
            transliter.Add("Ю", "Yu");
            transliter.Add("Я", "Ya");
        }
        public static string GetTranslit(string sourceText)
        {
            StringBuilder ans = new StringBuilder();
            for (int i = 0; i < sourceText.Length; i++)
            {
                if (transliter.ContainsKey(sourceText[i].ToString()))
                {
                    ans.Append(transliter[sourceText[i].ToString()]);
                }
                else
                {
                    ans.Append(sourceText[i].ToString());
                }
            }
            return ans.ToString();
        }
        public static void createTranslitName ()
        {
            string ssqlconnectionstring = "Data Source=LAPTOP-LCJH6N9V;Initial Catalog=dip;Integrated Security=SSPI";
            SqlConnection conn = new SqlConnection(ssqlconnectionstring);
            conn.Open();

            string sql = "SELECT empl_name FROM [dip].[dbo].[Employees]";
            SqlDataAdapter daEmpl = new SqlDataAdapter(sql, conn);
            DataSet dsEmpl = new DataSet("dip");
            daEmpl.FillSchema(dsEmpl, SchemaType.Source, "[dbo].[Employees]");
            daEmpl.Fill(dsEmpl, "[dbo].[Employees]");
            DataTable dtEmpl;
            dtEmpl = dsEmpl.Tables["[dbo].[Employees]"];

            if (!flagInit)
                prepareTranslit();

            string sqlUpdate = "UPDATE [dip].[dbo].[Employees] SET translit_name = @newName WHERE empl_name = @cur";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sqlUpdate;
            string cur = ""; //хранение текущего имени кириллицей
            string newName = "";
            foreach (DataRow drCurrent in dtEmpl.Rows)
            {
                //if (drCurrent["empl_name"].ToString()!="") //только для тех записей, для которых еще не добавили транслит
                {
                    cur = "";
                    newName = "";
                    cur = drCurrent["empl_name"].ToString();
                    newName = GetTranslit(cur);
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@newName", SqlDbType.NVarChar).Value = newName;
                    cmd.Parameters.Add("@cur", SqlDbType.NVarChar).Value = cur;
                    int updateRow = cmd.ExecuteNonQuery();
                }         
            }
        }
        
}
    

}