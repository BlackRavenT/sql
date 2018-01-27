using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для UploadWindow.xaml
    /// </summary>
    public partial class UploadWindow : System.Windows.Window
    {
        private Range xlSheetRange;

        public UploadWindow()
        {
            InitializeComponent();
            string ssqlconnectionstring = "Data Source=LAPTOP-LCJH6N9V;Initial Catalog=test;Integrated Security=SSPI";
            SqlConnection conn = new SqlConnection(ssqlconnectionstring);
            conn.Open();
            string sql = "SELECT TABLE_NAME FROM information_schema.TABLES WHERE TABLE_TYPE != 'VIEW'";

            SqlCommand command = new SqlCommand(sql, conn);

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
            /*OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Exel Files|*.xls;*.xlsx;*.xlsm";
            if (myDialog.ShowDialog() == true)
            {
                textBox1.Text = myDialog.FileName;
            }*/
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
        //выгрузка из Excel в БД
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //  OpenFileDialog ope = new OpenFileDialog();
            //  ope.FileName=textBox1.Text;
            //  string excelFilePath = ope.FileName;
            string excelFilePath = textBox1.Text;

            //string ssqltable = "test";
            string ssqltable = boxDataTable.SelectedItem.ToString();
            string myexceldataquery = "select * from [" + boxListExcel.SelectedItem + "$]";

            try
            {
                string sexcelconnectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelFilePath +
               ";Extended Properties='Excel 12.0 xml; HDR=NO;'";
                string ssqlconnectionstring = "Data Source=LAPTOP-LCJH6N9V;Initial Catalog=test;Integrated Security=SSPI";
                /*string sclearsql = "delete from " + ssqltable;
                SqlConnection sqlconn = new SqlConnection(ssqlconnectionstring);
                SqlCommand sqlcmd = new SqlCommand(sclearsql, sqlconn);
                sqlconn.Open();
                sqlcmd.ExecuteNonQuery();
                sqlconn.Close(); */
                OleDbConnection oledbconn = new OleDbConnection(sexcelconnectionstring);
                OleDbCommand oledbcmd = new OleDbCommand(myexceldataquery, oledbconn);
                oledbconn.Open();
                OleDbDataReader dr = oledbcmd.ExecuteReader();
                SqlBulkCopy bulkcopy = new SqlBulkCopy(ssqlconnectionstring);
                bulkcopy.DestinationTableName = ssqltable;
                while (dr.Read())
                {
                    bulkcopy.WriteToServer(dr);
                }
                dr.Close();
                oledbconn.Close();
                MessageBox.Show("File imported into sql server.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        

        
    }
}
