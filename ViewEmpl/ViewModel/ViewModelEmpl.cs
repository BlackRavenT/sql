using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ViewEmpl.Model;

namespace ViewEmpl.ViewModel
{
    public class ViewModelEmpl 
    {
        public ObservableCollection<ModelEmpl> Employees { get; set; }


        public ViewModelEmpl()
        {
            Employees = new ObservableCollection<ModelEmpl>();

            string ssqlconnectionstring = "Data Source=LAPTOP-LCJH6N9V;Initial Catalog=dip;Integrated Security=SSPI";
            SqlConnection conn = new SqlConnection(ssqlconnectionstring);
            conn.Open();
            SqlCommand comm = conn.CreateCommand();
            comm.CommandType = CommandType.StoredProcedure;
            comm.CommandText = "view_empl";
            //DataTable dt = new DataTable();
            //dt.Load(comm.ExecuteReader());

            SqlDataReader reader = comm.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    try
                    {
                        Employees.Add(new ModelEmpl { EmplName = (string)reader.GetValue(0), Department = (string)reader.GetValue(1), ScienceDegree = (string)reader.GetValue(2), Hours = (double)reader.GetValue(3) });

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
        }
        
    }
}
