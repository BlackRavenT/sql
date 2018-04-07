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
        internal static DataSet ds;
        internal static DataTable dt;
        public void buttonInsert_Click(object sender, RoutedEventArgs e)
        {
            string res="";
            dt = new DataTable();
            dt = UploadWindow.dataTable;
            res = UploadWindow.dataSource;
            if (UploadWindow.dataSource.ToString()=="WoS")
            {
                dt.Rows[1]["Авторы с аффилиациями"] = res;
                MessageBox.Show(res);
            }
        }
    }
}
