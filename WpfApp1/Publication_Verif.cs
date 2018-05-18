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
        public static void PublVerif (DataRow publiction)
        {
            verif = publiction;
            string publName = publiction["Название публикации"].ToString(); // оригинальное название 
            //извлекаем из названия только буквы и пробелы 
            string publicName = "";
            string publToLower = publName.ToLower();
            for (int i=0; i<publToLower.Length; i++)
            {
                if (char.IsLetter(publToLower[i]) || publToLower[i]==' ')
                    publicName = publicName + publToLower[i];
            }
            MessageBox.Show(publicName);
            string ssqlconnectionstring = "Data Source=LAPTOP-LCJH6N9V;Initial Catalog=dip;Integrated Security=SSPI";
            SqlConnection conn = new SqlConnection(ssqlconnectionstring);
            conn.Open();

            string sql = "SELECT * FROM [dip].[dbo].[Publ] WHERE [Название публикации (преобразованное)] = '" + publicName + "'";
            SqlDataAdapter daPubl = new SqlDataAdapter(sql, conn);
            DataSet dsPubl = new DataSet("publications");
            daPubl.FillSchema(dsPubl, SchemaType.Source, "[dbo].[Publ]");
            daPubl.Fill(dsPubl, "[dbo].[Publ]");
            //DataTable dtPubl;
            dtPubl = dsPubl.Tables["[dbo].[Publ]"];

            
            //посчитали количество найденных строк
            int count = dtPubl.Rows.Count;
            //если нашли такую строку/строки 
            if (count!=0)
            {
                foreach (DataRow drPubl in dtPubl.Rows)
                {
                    if (publiction["Авторы"].ToString() == drPubl["Авторы"].ToString())
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
                                        //если все параметры равны, то это значит, что публикация уже существует и добавлять ее не нужно
                                        break;
                                    }
                                    else
                                    {
                                        PublChoose publChooseView = new PublChoose();
                                        publChooseView.ShowDialog();
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
                                            //если все параметры равны, то это значит, что публикация уже существует и добавлять ее не нужно
                                            break;
                                        }
                                        else
                                        {
                                            PublChoose publChooseView = new PublChoose();
                                            publChooseView.ShowDialog();
                                        }
                                    }
                                    else
                                    {
                                        //вывести окно выбора
                                        PublChoose publChooseView = new PublChoose();
                                        publChooseView.ShowDialog();
                                    }
                                }
                            }
                            else
                            {
                                PublChoose publChooseView = new PublChoose();
                                publChooseView.ShowDialog();
                            }
                        }
                        else
                        {
                      
                        }
                    }
                    
                }
            }
            //строк с посимвольно одинаковыми названиями нет
            else
            {
                int countWord = 0;
                for (int i=0; i<publicName.Length;i++)
                {
                    if (publicName[i] == ' ')
                        countWord++;
                }
                countWord++;
                char first = publicName[0];
                string sqlWords = "SELECT * FROM [dip].[dbo].[Publ] WHERE [Название публикации (преобразованное)] LIKE '" + first + "%'";
                SqlDataAdapter daWords = new SqlDataAdapter(sql, conn);
                DataSet dsWords = new DataSet("publications");
                daWords.FillSchema(dsWords, SchemaType.Source, "[dbo].[Publ]");
                daWords.Fill(dsWords, "[dbo].[Publ]");
                DataTable dtWords = dsWords.Tables["[dbo].[Publ]"];
                DataView dvWords = new DataView(dtWords);
                dvWords.Sort = "[Название публикации (преобразованное)] ASC";

                foreach (DataRowView row in dvWords)
                {
                    Console.WriteLine(" {0} \t {1}", row["[Название публикации (преобразованное)]"]);
                }


                
                /*foreach (DataRow drCur in dtWords.Rows)
                {
                    string str = drCur["Название публикации"].ToString();
                    
                }*/

            }


      
        }
    }
}
