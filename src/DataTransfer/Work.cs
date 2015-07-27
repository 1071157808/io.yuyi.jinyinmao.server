using DataTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace DataTransfer
{
    public class Work
    {
        private static string connStr = new OldDBContext().Database.Connection.ConnectionString;
        public static void Run()
        {
            var table = new DataTable();
            DataColumn colName = new DataColumn("Name");
            DataColumn colAge = new DataColumn("Age");
            table.Columns.Add(colName);
            table.Columns.Add(colAge);
            for (int i = 0; i < 5; i++)
            {
                var row = table.NewRow();
                row["Name"] = "king" + i;
                row["Age"] = 20 + i;
                table.Rows.Add(row);
            }

            
            Console.WriteLine(JsonConvert.SerializeObject(table));
            Console.ReadKey();
        }

        private static DataTable GetDataSet(string sql)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataTable table = new DataTable();
                    conn.Open();
                    adapter.Fill(table);
                    return table;
                }
            }
        }
    }
}
