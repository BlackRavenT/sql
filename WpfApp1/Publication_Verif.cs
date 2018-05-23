using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    class Publication_Verif
    {

        internal static DataRow verif; //копия строки из файла, которую проверяем
        internal static DataTable dtPubl; //сет данных из базы по запросу 
        //internal static string publicName;

        public static string UpdatePublName (string str)
        {
            string publicName = "";
            string publToLower = str.ToLower();
            for (int i = 0; i < publToLower.Length; i++)
            {
                if (char.IsLetter(publToLower[i]) || publToLower[i] == ' ')
                    publicName = publicName + publToLower[i];
            }
            return publicName;
        }
       
        public static void PublVerif (DataRow publiction, string NameTable)
        {
            verif = publiction;
            string publName = publiction["Название публикации"].ToString(); // оригинальное название 
                                                                             
            string publicName = UpdatePublName(publName); //извлекаем из названия только буквы и пробелы
           // MessageBox.Show(publicName);
            string ssqlconnectionstring = "Data Source=LAPTOP-LCJH6N9V;Initial Catalog=dip;Integrated Security=SSPI";
            SqlConnection conn = new SqlConnection(ssqlconnectionstring);
            conn.Open();

            string sql = "SELECT * FROM " + NameTable + " WHERE [Название публикации (преобразованное)] = '" + publicName + "'";
            SqlDataAdapter daPubl = new SqlDataAdapter(sql, conn);
            DataSet dsPubl = new DataSet("publications");
            daPubl.FillSchema(dsPubl, SchemaType.Source, NameTable);
            daPubl.Fill(dsPubl, NameTable);
            //DataTable dtPubl;
            dtPubl = dsPubl.Tables[NameTable];

            string sqlSyn = "SELECT * FROM [dip].[dbo].[Employees] WHERE synonym LIKE '%" + publiction["Авторы"].ToString() + "%'";
            SqlDataAdapter daSyn = new SqlDataAdapter(sqlSyn, conn);
            DataSet dsSyn = new DataSet("synonym");
            daSyn.FillSchema(dsSyn, SchemaType.Source, "[dbo].[Employees]");
            daSyn.Fill(dsSyn, "[dbo].[Employees]");
            DataTable dtSyn;
            dtSyn = dsSyn.Tables["[dbo].[Employees]"];
            int countSyn = dtSyn.Rows.Count;

            //посчитали количество найденных строк
            int count = dtPubl.Rows.Count;
            bool flagIns = true; //нужно добавлять публикацию в БД или нет
            //если нашли такую строку/строки 
            if (count!=0)
            {
                foreach (DataRow drPubl in dtPubl.Rows)
                {
                    if (publiction["Авторы"].ToString() == drPubl["Авторы"].ToString() || countSyn>0)
                    {
                        if (publiction["Название публикации"].ToString() == drPubl["Название публикации"].ToString())
                        {
                            if (publiction["Год"].ToString() == drPubl["Год"].ToString()
                                && publiction["Название журнала"].ToString() == drPubl["Название журнала"].ToString()
                                && publiction["Кол-во авторов"].ToString() == drPubl["Кол-во авторов"].ToString()
                                && publiction["DOI"].ToString() == drPubl["DOI"].ToString()
                                && publiction["Вид публикации"].ToString() == drPubl["Вид публикации"].ToString())
                            {
                                if (Dataview.res == "Scopus")
                                {
                                    if (publiction["SNIP"].ToString() == drPubl["SNIP"].ToString()
                                        && publiction["Квартили по Scopus (SJR)"].ToString() == drPubl["Квартили по Scopus (SJR)"].ToString())
                                    {
                                        flagIns = false;//если все параметры равны, то это значит, что публикация уже существует и добавлять ее не нужно
                                        break;
                                    }
                                    else
                                    {
                                        PublChoose publChooseView = new PublChoose();
                                        publChooseView.ShowDialog();
                                        flagIns = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (Dataview.res == "WoS")
                                    {
                                        if (publiction["Journal Impact Factor"].ToString() == drPubl["Journal Impact Factor"].ToString()
                                            && publiction["Article Influence Score"].ToString() == drPubl["Article Influence Score"].ToString()
                                            && publiction["Квартиль по WoS (JCR)"].ToString() == drPubl["Квартиль по WoS (JCR)"].ToString())
                                        {
                                            flagIns = false;//если все параметры равны, то это значит, что публикация уже существует и добавлять ее не нужно
                                            break;
                                        }
                                        else
                                        {
                                            PublChoose publChooseView = new PublChoose();
                                            publChooseView.ShowDialog();
                                            flagIns = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        //вывести окно выбора
                                        PublChoose publChooseView = new PublChoose();
                                        publChooseView.ShowDialog();
                                        flagIns = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                /*PublChoose publChooseView = new PublChoose();
                                publChooseView.ShowDialog();
                                flagIns = false;
                                break;*/
                            }
                        }
                        
                    }
                     
                }
                if (flagIns)//публикация с таким названием уже есть, но нет данного автора
                    InsRow(publiction, NameTable);


            }
            //строк с посимвольно одинаковыми названиями нет
            else
            {
                int countWord = 0;
                for (int i=0; i<publicName.Length;i++)
                {
                    if (publicName[i] == ' ')
                    {
                        countWord++;
                    }                        
                }
                countWord++;
                string[] wordsPublicName = publicName.Split(' ');  //массив слов из строки, которую проверяем
                char first = publicName[0];
                string sqlWords = "SELECT * FROM " + NameTable + " WHERE [Название публикации (преобразованное)] LIKE '" + first + "%'";
                SqlDataAdapter daWords = new SqlDataAdapter(sql, conn);
                DataSet dsWords = new DataSet("publications");
                daWords.FillSchema(dsWords, SchemaType.Source, NameTable);
                daWords.Fill(dsWords, NameTable);
                DataTable dtWords = dsWords.Tables[NameTable];
                DataView dvWords = new DataView(dtWords);
                dvWords.Sort = "[Название публикации (преобразованное)] ASC";
                bool flag = false; //несовпадение символов в слове
                bool approve = false;
                foreach (DataRowView row in dvWords)
                {
                    string[] wordsDvWords = row["[Название публикации (преобразованное)]"].ToString().Split(' ');
                    int minWordCount = Math.Min(wordsPublicName.GetLength(0), wordsDvWords.GetLength(0));
                    for (int i=0; i<minWordCount; i++)
                    {
                        int minWordLengh = Math.Min(wordsPublicName[i].Length, wordsDvWords[i].Length);
                        for (int j=0; j<minWordLengh; j++)
                        {
                            if (wordsDvWords[i][j]!=wordsPublicName[i][j])
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (wordsPublicName.GetLength(0)== wordsDvWords.GetLength(0) && i==minWordCount && flag == false)
                        {
                            //такая публикация уже есть в базе 
                        }
                        else
                        if (i==minWordCount && wordsPublicName.GetLength(0) != wordsDvWords.GetLength(0))
                        {
                            approve = true;
                        }
                        if (flag) break;
                    }
                    if (flag) break;
                }
                if (approve)
                {
                    PublChoose dlg = new PublChoose();
                    dlg.ShowDialog();
                }
                if (!flag)
                {
                    InsRow(verif, NameTable);
                   
                }
                                
            }
            
        }

        public static void InsRow (DataRow insertRow, string NameTable)
        {
            string ssqlconnectionstring = "Data Source=LAPTOP-LCJH6N9V;Initial Catalog=dip;Integrated Security=SSPI";
            SqlConnection conn = new SqlConnection(ssqlconnectionstring);
            conn.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Clear();
            string sqlIns = "INSERT INTO "+ NameTable;
            string insStr = "";
            string insVal = "";


            if (insertRow["Авторы"].ToString() != "")
            {
                string auth = insertRow["Авторы"].ToString();
                cmd.Parameters.Add("@auth", SqlDbType.NVarChar).Value = auth;
                insStr = insStr + "[Авторы]";
                insVal = insVal + "@auth";

            };
            if (insertRow["Название публикации"].ToString() != "")
            {
                string publNameIns = insertRow["Название публикации"].ToString();
                cmd.Parameters.Add("@publNameIns", SqlDbType.NVarChar).Value = publNameIns;
                insStr = insStr + ", [Название публикации]";
                insVal = insVal + ", @publNameIns";
            };
            if (insertRow["Год"].ToString() != "")
            {
                string year = insertRow["Год"].ToString();
                cmd.Parameters.Add("@year", SqlDbType.Int).Value = year;
                insStr = insStr + ", [Год]";
                insVal = insVal + ", @year";
            }
            if (insertRow["Название журнала"].ToString() != "")
            {
                string journ = insertRow["Название журнала"].ToString();
                cmd.Parameters.Add("@journ", SqlDbType.NVarChar).Value = journ;
                insStr = insStr + ", [Название журнала]";
                insVal = insVal + ", @journ";
            }


            //string countQuote = Publication_Verif.verif["Кол-во цитирований"].ToString();
            //cmd.Parameters.Add("@countQuote", SqlDbType.Int).Value = countQuote;
            if (insertRow["Кол-во авторов"].ToString() != "")
            {
                string authCount = insertRow["Кол-во авторов"].ToString();
                cmd.Parameters.Add("@authCount", SqlDbType.NVarChar).Value = authCount;
                insStr = insStr + ", [Кол-во авторов]";
                insVal = insVal + ", @authCount";
            }


            if (insertRow["DOI"].ToString() != "")
            {
                string doi = insertRow["DOI"].ToString();
                cmd.Parameters.Add("@doi", SqlDbType.NVarChar).Value = doi;
                insStr = insStr + ", [DOI]";
                insVal = insVal + ", @doi";
            }

            if (insertRow["Вид публикации"].ToString() != "")
            {
                string publType = insertRow["Вид публикации"].ToString();
                cmd.Parameters.Add("@publType", SqlDbType.NVarChar).Value = publType;
                insStr = insStr + ", [Вид публикации]";
                insVal = insVal + ", @publType";
            }

            if (Dataview.res == "Scopus")
            {
                if (insertRow["SNIP"].ToString() != "")
                {
                    string snip = insertRow["SNIP"].ToString();
                    cmd.Parameters.Add("@snip", SqlDbType.Float).Value = snip;
                    insStr = insStr + ", [SNIP]";
                    insVal = insVal + ", @snip";
                }
                if (insertRow["Квартили по Scopus (SJR)"].ToString() != "")
                {
                    string quartileSc = insertRow["Квартили по Scopus (SJR)"].ToString();
                    cmd.Parameters.Add("@quartileSc", SqlDbType.NVarChar).Value = quartileSc;
                    insStr = insStr + ", [Квартили по Scopus (SJR)]";
                    insVal = insVal + ", @quartileSc";
                }
            };
            if (Dataview.res == "WoS")
            {
                if (insertRow["Journal Impact Factor"].ToString() != "")
                {
                    string JIF = insertRow["Journal Impact Factor"].ToString();
                    cmd.Parameters.Add("@JIF", SqlDbType.Float).Value = JIF;
                    insStr = insStr + ", [Journal Impact Factor]";
                    insVal = insVal + ", @JIF";
                }
                if (insertRow["Article Influence Score"].ToString() != "")
                {
                    string AIS = insertRow["Article Influence Score"].ToString();
                    cmd.Parameters.Add("@AIS", SqlDbType.Float).Value = AIS;
                    insStr = insStr + ", [Article Influence Score]";
                    insVal = insVal + ", @AIS";
                }
                if (insertRow["Квартиль по WoS (JCR)"].ToString() != "")
                {
                    string quartileWos = insertRow["Квартиль по WoS (JCR)"].ToString();
                    cmd.Parameters.Add("@quartileWos", SqlDbType.NVarChar).Value = quartileWos;
                    insStr = insStr + ", [Квартиль по WoS (JCR)]";
                    insVal = insVal + ", @quartileWos";
                }

            };
            cmd.Parameters.Add("@publNameCh", SqlDbType.NVarChar).Value = UpdatePublName(insertRow["Название публикации"].ToString());
            //string sqlIns = "INSERT INTO [dip].[dbo].[Publ] ([Авторы], [Название публикации],  [Год], [Название журнала], [Кол-во авторов], [DOI], [Вид публикации], " +
            //    insStr + "VALUES ( @auth, @publNameIns, @year, @journ,  @authCount, @doi, @publType, " + insVal;

            sqlIns = sqlIns + "(" + insStr + ", [Название публикации (преобразованное)]) VALUES (" + insVal + ", @publNameCh)";
            cmd.Connection = conn;
            cmd.CommandText = sqlIns;
            int inscount = cmd.ExecuteNonQuery();
        }
    }
}
