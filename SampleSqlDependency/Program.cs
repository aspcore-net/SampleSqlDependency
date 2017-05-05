using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleSqlDependency
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
        }

        public async static void Test()
        {
            try
            {
                var sqlConnection =
                    new SqlConnection(
                        @"Data Source=ANUPAM-PC\MSSQLSERVER2012; Initial Catalog=TestDb; User Id=sa; Password=123");
                SqlDependency.Start(sqlConnection.ConnectionString);
                var sqlCommand = new SqlCommand("SELECT * FROM tbl_SysConfig", sqlConnection);
                sqlConnection.Open();
                //await sqlCommand.Connection.OpenAsync();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Notification = null;
                var sqlDependency = new SqlDependency(sqlCommand);
                sqlDependency.OnChange += SqlDependencyOnChange;
                var dr = await sqlCommand.ExecuteReaderAsync();
                while (dr.Read())
                {
                    Console.WriteLine(dr[0]);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private static void SqlDependencyOnChange(object sender, SqlNotificationEventArgs eventArgs)
        {
            if (eventArgs.Info == SqlNotificationInfo.Invalid)
            {
                Console.WriteLine("The above notification query is not valid.");
            }
            else
            {
                Console.WriteLine("Notification Info: " + eventArgs.Info);
                Console.WriteLine("Notification source: " + eventArgs.Source);
                Console.WriteLine("Notification type: " + eventArgs.Type);
            }
        }
    }
}
