using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        internal static DataTable newDt; //таблица с разделенными по авторам публикациями
        internal static DataTable errDt; //таблица для публикаций, в ходе обработки которых возникли исключения
        internal static int er = 0;
        internal static string AuthVerifName;
        internal static string res; //строка с выбором типа файла WoS/Scopus

        public Dataview()
        {
            InitializeComponent();
            Translit.createTranslitName();
        }

        List<string> HseName = new List<string>()
            {
                "Natl Res Univ, Higher Sch Econ",
                "Natl Univ, Higher Sch Econ, Moscow Inst Elect&Math"
            };

        
//предпросмотр добавляемых записей: вывод разбитых по авторам публикаций 
        public void buttonInsert_Click(object sender, RoutedEventArgs e)
        {
            var upWnd = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is UploadWindow) as UploadWindow;
            res = upWnd.boxDataSource.SelectedItem.ToString();
            DataSet ds = UploadWindow.dataSet;
            DataTable dt = UploadWindow.dataTable;
            newDt = dt.Clone();
            //DataTable newDt = dt.Clone(); //таблица для обработанных публикаций, разбитых по авторам 
            errDt = dt.Clone(); //таблица для публикаций, в ходе обработки которых возникли исключения 

            int result = -1;

            //блок обработки файлов WoS

            if (res == "WoS")
            {
                //int j = 0;
                int k = 0;
                for (int j=0; j<dt.Rows.Count; j++)
                {
                    //int er = 0; //счетчик строк в файле с ошибками
                    //поиск в строке упоминания ВШЭ                       
                    if (dt.Rows[j]["Авторы с аффилиациями"].ToString().IndexOf("Higher") > -1)
                        result = dt.Rows[j]["Авторы с аффилиациями"].ToString().IndexOf("Higher");
                    else if (dt.Rows[j]["Авторы с аффилиациями"].ToString().IndexOf("HSE") > -1)
                        result = dt.Rows[j]["Авторы с аффилиациями"].ToString().IndexOf("HSE");

                    int end = -1;
                    int start = -1;
                    for (int i = result; i > 0; i--)
                    {
                        if (dt.Rows[j]["Авторы с аффилиациями"].ToString()[i] == ']')
                        {
                            end = i;
                            break;
                        }
                    }
                    for (int i = end; i >= 0; i--)
                    {
                        if (dt.Rows[j]["Авторы с аффилиациями"].ToString()[i] == '[')
                        {
                            start = i;
                            break;
                        }
                    }
                    if (start < 0 && end < 0)
                    {
                        errDt.ImportRow(dt.Rows[j]);
                        er++;
                    }
                    else
                    {
                        string authors = dt.Rows[j]["Авторы с аффилиациями"].ToString();
                        authors = authors.Substring(start + 1, end - start-1); //получили строку со всеми авторами, аффилированными с ВШЭ
                        start = 0;
                        end = -1;
                        string person = "";
                        //string translit_pers = "";
                        //вытаскиваем по одному автору из набора и добавляем его со всей остальной информацией по публикации в новую таблицу

                        for (int i = 0; i < authors.Length; i++)
                        {
                            if ((authors[i] == ';') || (i == authors.Length - 1))
                            {
                                newDt.ImportRow(dt.Rows[j]);
                                end = i+1;
                                //if (k > 0)
                                //    start += 2;
                                person = authors.Substring(start, end - start);
                                start = end;
                                newDt.Rows[k]["Авторы"] = "";
                                if (person[0] == ' ')
                                    person = person.Substring(1, person.Length - 1);
                                if (person[person.Length-1] == ';')
                                    person = person.Substring(0, person.Length-1);
                                //MessageBox.Show(newDt.Rows[k]["Авторы"].ToString());
                                newDt.Rows[k]["Авторы"] = person;
                                
                                //MessageBox.Show(newDt.Rows[k]["Авторы"].ToString());
                                k++;
                            }
                        }
                    }
                    
                }
            }
            dataGridView1.ItemsSource = newDt.DefaultView;

            //блок обработки файлов Scopus

            if (res=="Scopus")
            {
                int z = 0;
                //перебор всех строк в таблице
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    int i = 0;
                    int last = 0;
                    result = 0;
                    string aff = dt.Rows[j]["Авторы с аффилиациями"].ToString();
                    for (i=last+1; i < aff.Length; i++)
                    {
                        if (aff.IndexOf("Higher") > -1)
                            result = aff.IndexOf("Higher");
                        else if (aff.IndexOf("HSE") > -1)
                            result = aff.IndexOf("HSE");
                        else
                            break;
                        int end = -1;
                        int start = -1;

                        int pos = result;
                        //прокручиваем строку до конца текущего упоминания автора, для того чтобы начать поиск в дальнейшем с этой позиции, а не с начала
                        while (aff[pos] != ';' && pos < aff.Length-1)
                        {
                            pos++;
                        }
                        last = pos;
                        int k = result;
                        while (aff[k] != ';' && k>0)
                        {
                            k--;
                        }
                        start = k;
                        int kol = 0; //количество запятых
                        for (k=start; k < result; k++)
                        {
                            if (kol==2)
                            {
                                end = k;
                                break;
                            }
                            if (aff[k] == ',')
                                kol++;                            
                        }
                        string authors = aff;
                        authors = authors.Substring(start, end - start - 1);
                        //удаляем лишние символы в начале строки
                        if (authors[0]==';' && authors[1]==' ')
                            {
                                authors=authors.Substring(2, authors.Length-2);
                            }
                        
                        newDt.ImportRow(dt.Rows[j]);
                        newDt.Rows[z]["Авторы"] = "";                        
                        newDt.Rows[z]["Авторы"] = authors;
                        //MessageBox.Show(newDt.Rows[z]["Авторы"].ToString());

                        z++; //счетчик добавленных строк с разделенными публикациями по авторам
                        //удаляем обработанную часть строки 
                        aff = aff.Substring(last, aff.Length-last);
                    }
                    
                }
                dataGridView1.ItemsSource = newDt.DefaultView;
            }
            
        }
        //проверка и добавление новых
        private void buttonInsertNew_Click(object sender, RoutedEventArgs e)
        {
            int i;
            foreach (DataRow drNewDt in newDt.Rows)
            {
                AuthVerifName = drNewDt["Авторы"].ToString();
                bool flag = false; //наличие автора в таблице с ошибками
                //i = Author_Verif.CountAuthor(drNewDt["Авторы"].ToString());
                i = Author_Verif.CountAuthor(drNewDt); //считаем, сколько авторов подходит под маску по текущей строке
                                                       //если не нашли такого автора в таблице сотрудников
                //если нашли ровно одного подходящего 
                if (i == 1)
                {
                    Publication_Verif.PublVerif(drNewDt);
                    //тут функция проверки публикации
                }
                                
                else
                //если нашли больше одного или ни одного 
                {
                    int j = 0;
                    while (j<er)
                    {
                        if (errDt.Rows[j]["Авторы"].ToString()==AuthVerifName)
                        {
                            flag = true;
                            errDt.ImportRow(drNewDt); //добавили в файл с ошибками строку, увеличили счетчик строк в этом файле
                            Console.WriteLine(errDt.Rows[er]);
                            MessageBox.Show(errDt.Rows[j]["Авторы"].ToString());
                            er++;
                            break;
                        }
                        j++;
                    }
                    if (flag==false)
                    {
                        Author_0_matches author_view = new Author_0_matches();
                        author_view.ShowDialog();
                    }
                    
                }
            }
        }
    }

}
