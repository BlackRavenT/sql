//окно загрузки данных из Excel в БД

//using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections;
 

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для UploadWindow.xaml
    /// </summary>
    public partial class UploadWindow : System.Windows.Window
    {
        private Excel.Range xlSheetRange;
        string dataSource;
        internal static DataSet dataSet;
        internal static DataTable dataTable;

        public UploadWindow()
        {
            InitializeComponent();
            boxDataSource.Items.Add("WoS");
            boxDataSource.Items.Add("Scopus");
            string ssqlconnectionstring = "Data Source=LAPTOP-LCJH6N9V;Initial Catalog=dip;Integrated Security=SSPI";
            SqlConnection conn = new SqlConnection(ssqlconnectionstring);
            conn.Open();
            string sql = "SELECT TABLE_NAME FROM information_schema.TABLES WHERE TABLE_TYPE != 'VIEW'";

            SqlCommand command = new SqlCommand(sql, conn);

            /*DataSet dataEmpl;
            DataTable tableEmpl;

            string sqlEmpl = "SELECT empl_name FROM [dbo].[Employees]";
            SqlCommand comEmpl = new SqlCommand(sqlEmpl, conn);
            SqlDataReader readerEmpl = comEmpl.ExecuteReader();
            while (readerEmpl.Read())
            {
                
            }
            */
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string table = reader.GetString(0);
                    boxDataTable.Items.Add(table);
                }
               
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ошибка:" + ex);
            }
            conn.Close();
            
        }
        //выбор файла
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ope = new OpenFileDialog();
            ope.Filter = "Exel Files|*.xls;*.xlsx;*.xlsm";
            if (ope.ShowDialog() == true)
            {
                textBox1.Text = ope.FileName;
            }
            
            //из выбранной книги выводим все названия листов в combobox
            Excel.Workbook xlWB;
            Excel.Application xlApp = new Excel.Application();
            xlWB = xlApp.Workbooks.Open(textBox1.Text);
            boxListExcel.Items.Clear(); // очистить combobox 
            //добавить название листов из книги
            for (int i = 0; i < xlWB.Sheets.Count; i++)
            {
                boxListExcel.Items.Add(xlWB.Worksheets[i + 1].Name);
            }
            

        }
        //выгрузка ИЗ Excel в БД
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            string res;
            UploadWindow upWnd = new UploadWindow();
            res = boxDataSource.SelectedItem.ToString();
            upWnd.dataSource = res;

            OpenFileDialog ope = new OpenFileDialog();
            ope.FileName=textBox1.Text;
            
            string excelFilePath = textBox1.Text;
            
            string ssqltable = boxDataTable.SelectedItem.ToString();
            string sheet1 = boxListExcel.SelectedItem.ToString();
            string myexceldataquery = "select * from [" + boxListExcel.SelectedItem + "$]"; // select * into dbo.tablename - создаст новую таблицу при запросе

            try
            {
                // Командная строка "подключения к Excel"
                string sexcelconnectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelFilePath + ";Extended Properties='Excel 12.0 xml; HDR=YES;'";
                // Строка подключения к SQL
                string ssqlconnectionstring = "Data Source=LAPTOP-LCJH6N9V;Initial Catalog=dip;Integrated Security=SSPI";
                // Создаем новый DataSet
                UploadWindow.dataSet = new DataSet("Tables"); 
                // Открываем соединение с Excel
                OleDbConnection oledbconn = new OleDbConnection(sexcelconnectionstring);
                oledbconn.Open();
                 // Получаем список листов в файле
                DataTable schemaTable = oledbconn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                string select;
              
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(myexceldataquery, oledbconn);
                UploadWindow.dataTable = new DataTable();
                dataAdapter.Fill(UploadWindow.dataTable); // Заполняем таблицу
                UploadWindow.dataTable.TableName = sheet1.Substring(0, sheet1.Length - 1); // В конце от Экселя стоит символ '$'
                UploadWindow.dataSet.Tables.Add(UploadWindow.dataTable);
                Dataview dvWindow = new Dataview();
                dvWindow.dataGridView1.ItemsSource = UploadWindow.dataTable.DefaultView;
                    //MessageBox.Show(UploadWindow.dataTable.Rows[2]["Авторы с аффилиациями"].ToString());
            
                oledbconn.Close();
                //MessageBox.Show("File imported into sql server.");
                
                dvWindow.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        

        
    }
}

//}
/*
string sclearsql = "delete from " + ssqltable;
SqlConnection sqlconn = new SqlConnection(ssqlconnectionstring);
SqlCommand sqlcmd = new SqlCommand(sclearsql, sqlconn);
sqlconn.Open();
sqlcmd.ExecuteNonQuery();
sqlconn.Close(); 
*/

//OleDbCommand oledbcmd = new OleDbCommand(myexceldataquery, oledbconn);

// OleDbDataReader dr = oledbcmd.ExecuteReader();


/*OleDbDataAdapter da = new OleDbDataAdapter(myexceldataquery, oledbconn);
DataTable dt = new DataTable();
da.Fill(dt);  */


/* SqlBulkCopy bulkcopy = new SqlBulkCopy(ssqlconnectionstring);
                bulkcopy.DestinationTableName = ssqltable;
                //bulkcopy.DestinationTableName = "dbo.buffer";
                while (dr.Read())
                 {
                    bulkcopy.ColumnMappings.Add("Авторы","Авторы");
                    bulkcopy.ColumnMappings.Add("Название публикации", "Название публикации");
                    bulkcopy.ColumnMappings.Add("Год", "Год");
                    bulkcopy.ColumnMappings.Add("Название журнала", "Название журнала");
                    //bulkcopy.ColumnMappings.Add("Кол-во цитирований на 20.01.2018", "Кол-во цитирований на 20.01.2018");
                    bulkcopy.ColumnMappings.Add("Кол-во авторов", "Кол-во авторов");
                    bulkcopy.ColumnMappings.Add("Дата внесения в базу", "Дата внесения в базу");
                    bulkcopy.ColumnMappings.Add("DOI", "DOI");
                    bulkcopy.ColumnMappings.Add("Авторы с аффилиациями", "Авторы с аффилиациями");
                    bulkcopy.ColumnMappings.Add("Вид публикации", "Вид публикации");
                    bulkcopy.ColumnMappings.Add("EID", "EID");
                    bulkcopy.ColumnMappings.Add("Язык публикации", "Язык публикации");
                    bulkcopy.ColumnMappings.Add("SNIP", "SNIP");
                    bulkcopy.ColumnMappings.Add("Квартили по Scopus (SJR)", "Квартили по Scopus (SJR)");                    
                    bulkcopy.ColumnMappings.Add("Авторы ВШЭ (поиск вручную)", "Авторы ВШЭ (поиск вручную)");
                    bulkcopy.ColumnMappings.Add("Авторы НИУ ВШЭ", "Авторы НИУ ВШЭ");
                    bulkcopy.ColumnMappings.Add("российский журнал", "российский журнал");*/

/*bulkcopy.ColumnMappings.Add(0,0);
bulkcopy.ColumnMappings.Add(1,1);
bulkcopy.ColumnMappings.Add(2,2);
bulkcopy.ColumnMappings.Add(3,3);
bulkcopy.ColumnMappings.Add(4,4);
bulkcopy.ColumnMappings.Add(5,5);
bulkcopy.ColumnMappings.Add(6,6);
bulkcopy.ColumnMappings.Add(7,7);
bulkcopy.ColumnMappings.Add(8,8);
bulkcopy.ColumnMappings.Add(9,9);
bulkcopy.ColumnMappings.Add(10,10);
bulkcopy.ColumnMappings.Add(11,11);
bulkcopy.ColumnMappings.Add(12,12);
bulkcopy.ColumnMappings.Add(13,13);
bulkcopy.ColumnMappings.Add(14,14);
bulkcopy.ColumnMappings.Add(15,15);
bulkcopy.ColumnMappings.Add(16,16);
bulkcopy.WriteToServer(dr); 
}*/
