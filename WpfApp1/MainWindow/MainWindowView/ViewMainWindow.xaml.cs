//основное окно
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
//using WpfApp1.MainWindow.MainWindowViewModel;
using Excel = Microsoft.Office.Interop.Excel;


namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class ViewMainWindow : System.Windows.Window
    {
        private Range xlSheetRange;

        public ViewMainWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModelMainWindow();
        }
        

       
        //загрузка данных в БД
        public void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            UploadWindow upWindow = new UploadWindow();
            upWindow.Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Window1 dwWindow = new Window1();
            dwWindow.Show();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            KPI_PA kpiWindow = new KPI_PA();
            kpiWindow.Show();
        }
    }
    
}
