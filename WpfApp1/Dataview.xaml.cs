using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Dataview.xaml
    /// </summary>
    public partial class Dataview : Window
    {
        public Dataview()
        {
            InitializeComponent();
        }

        List<string> HseName = new List<string>()
            {
                "Natl Res Univ, Higher Sch Econ",
                "Natl Univ, Higher Sch Econ, Moscow Inst Elect&Math"
            };
        //internal static DataSet ds;
        //internal static DataTable dt;
        public void buttonInsert_Click(object sender, RoutedEventArgs e)
        {
            var upWnd = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is UploadWindow) as UploadWindow;
            string res = upWnd.boxDataSource.SelectedItem.ToString();
            DataSet ds = UploadWindow.dataSet;
            DataTable dt = UploadWindow.dataTable;
            DataTable newDt = dt.Clone();

            int result = -1;
            if (res == "WoS")
            {
                //поиск в строке упоминания ВШЭ                       
                if (dt.Rows[5]["Авторы с аффилиациями"].ToString().IndexOf("Higher") > -1)            
                    result = dt.Rows[5]["Авторы с аффилиациями"].ToString().IndexOf("Higher");            
                else if (dt.Rows[5]["Авторы с аффилиациями"].ToString().IndexOf("HSE") > -1)            
                    result = dt.Rows[5]["Авторы с аффилиациями"].ToString().IndexOf("HSE");


            
                int end = -1;
                int start = -1;
                for (int i = result; i > 0; i--)
                {
                    if (dt.Rows[5]["Авторы с аффилиациями"].ToString()[i] == ']')
                    {
                        end = i;
                        break;
                    }
                }
                for (int i = end; i >= 0; i--)
                {
                    if (dt.Rows[5]["Авторы с аффилиациями"].ToString()[i] == '[')
                    {
                        start = i;
                        break;
                    }
                }
                string authors = dt.Rows[5]["Авторы с аффилиациями"].ToString();
                authors = authors.Substring(start + 1, end - start - 1); //получили строку со всеми авторами, аффилированными с ВШЭ
                start = 0;
                end = -1;
                string person = "";
                int k = 0;
                //вытаскиваем по одному автору из набора и добавляем его со всей остальной информацией по публикации в новую таблицу
               
                for (int i = 0; i < authors.Length; i++)
                {
                    if ((authors[i] == ';') || (i == authors.Length - 1 && k == 0))
                    {
                        newDt.ImportRow(dt.Rows[5]);
                        end = i;
                        person = authors.Substring(start, end - start);
                        start = end;
                        newDt.Rows[k]["Авторы"] = "";
                        MessageBox.Show(newDt.Rows[k]["Авторы"].ToString());
                        newDt.Rows[k]["Авторы"] = person;
                        MessageBox.Show(newDt.Rows[k]["Авторы"].ToString());
                        k++;
                    }
                }
            }
            

            if (res=="Scopus")
            {
                
            }
        }
    }
}
