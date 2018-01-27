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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private Range xlSheetRange;

        // private Excel.Application xlApp;
        // private Worksheet xlSheet;

        public MainWindow()
        {
            InitializeComponent();
            
           
        }
        

       
        //загрузка данных в БД
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            UploadWindow upWindow = new UploadWindow();
            upWindow.Show();

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Window1 dwWindow = new Window1();
            dwWindow.Show();
        }
    }
    
}
