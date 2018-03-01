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
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : System.Windows.Window
    {
        private Range xlSheetRange;

        public Window1()
        {
            InitializeComponent();
            string ssqlconnectionstring = "Data Source=LAPTOP-LCJH6N9V;Initial Catalog=dip;Integrated Security=SSPI";
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
        private System.Data.DataTable GetData()
        {
            string ssqlconnectionstring = "Data Source=LAPTOP-LCJH6N9V;Initial Catalog=test;Integrated Security=SSPI";
            SqlConnection sqlconn = new SqlConnection(ssqlconnectionstring);
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                /*string query = @"SELECT     Customers.CompanyName, Customers.ContactName, Orders.ShipVia, Orders.Freight, Orders.ShipName, Orders.ShipAddress, Orders.ShipCity, Orders.ShipRegion, Orders.ShipPostalCode, Orders.ShipCountry
                    FROM         Customers INNER JOIN
                      Orders ON Customers.CustomerID = Orders.CustomerID";*/
                string query = @"SELECT * from dbo." + boxDataTable.SelectedItem;
                SqlCommand comm = new SqlCommand(query, sqlconn);

                sqlconn.Open();
                SqlDataAdapter da = new SqlDataAdapter(comm);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
            }
            finally
            {
                sqlconn.Close();
                sqlconn.Dispose();
            }
            return dt;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Excel.Application xlApp = new Excel.Application();
            try
            {
                //добавляем книгу
                xlApp.Workbooks.Add(Type.Missing);

                //делаем временно неактивным документ
                xlApp.Interactive = false;
                xlApp.EnableEvents = false;
                Worksheet

                                //выбираем лист на котором будем работать (Лист 1)
                                xlSheet = (Excel.Worksheet)xlApp.Sheets[1];

                //Название листа
                xlSheet.Name = "Данные";

                //Выгрузка данных
                System.Data.DataTable dt = GetData();

                int collInd = 0;
                int rowInd = 0;
                string data = "";

                //называем колонки
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    data = dt.Columns[i].ColumnName.ToString();
                    xlSheet.Cells[1, i + 1] = data;
                    Range

                                        //выделяем первую строку
                                        xlSheetRange = xlSheet.get_Range("A1:Z1", Type.Missing);

                    //делаем полужирный текст и перенос слов
                    xlSheetRange.WrapText = true;
                    xlSheetRange.Font.Bold = true;
                }

                //заполняем строки
                for (rowInd = 0; rowInd < dt.Rows.Count; rowInd++)
                {
                    for (collInd = 0; collInd < dt.Columns.Count; collInd++)
                    {
                        data = dt.Rows[rowInd].ItemArray[collInd].ToString();
                        xlSheet.Cells[rowInd + 2, collInd + 1] = data;
                    }
                }

                //выбираем всю область данных
                xlSheetRange = xlSheet.UsedRange;

                //выравниваем строки и колонки по их содержимому
                xlSheetRange.Columns.AutoFit();
                xlSheetRange.Rows.AutoFit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                //Показываем ексель
                xlApp.Visible = true;

                xlApp.Interactive = true;
                xlApp.ScreenUpdating = true;
                xlApp.UserControl = true;

                //Отсоединяемся от Excel
                // releaseObject(xlSheetRange);
                //releaseObject(xlSheet);
                //releaseObject(xlApp);
            }
        }
    }
}
