using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для NewEmpl.xaml
    /// </summary>
    public partial class NewEmpl : Window
    {
        public NewEmpl()
        {
            InitializeComponent();
        }

        private void buttonInsert_Click(object sender, RoutedEventArgs e)
        {
            string ssqlconnectionstring = "Data Source=LAPTOP-LCJH6N9V;Initial Catalog=dip;Integrated Security=SSPI";
            SqlConnection conn = new SqlConnection(ssqlconnectionstring);
            conn.Open();
           // string sql = "INSERT INTO [dip].[dbo].[Employees] (employees_id, empl_name, translit_name)";
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
