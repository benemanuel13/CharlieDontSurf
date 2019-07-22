using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

using CharlieDontSurfAdmin.Helpers;
using CharlieDontSurfAdmin.Models.Orders;

namespace CharlieDontSurfAdmin.Infrastructure
{
    public class Database
    {
        private static SqlConnection connection = new SqlConnection();
        //private static string conString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=CharlieDontSurf; Integrated Security=True";
        private static string conString = "Data Source=.\\CHARLIE; Initial Catalog=CharlieDontSurf; Integrated Security=True";

        static Database()
        {
            connection.ConnectionString = conString;
        }

        public static List<Order> GetOrders()
        {
            List<Order> orders = new List<Order>();

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "dbo.GetUnfulfilledOrders";

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string userId = Helper.StripSpaces(reader.GetString(1));
                string recipient = Helper.StripSpaces(reader.GetString(2));
                string addressLine1 = Helper.StripSpaces(reader.GetString(3));
                string addressLine2 = Helper.StripSpaces(reader.GetString(4));
                string city = Helper.StripSpaces(reader.GetString(5));
                string county = Helper.StripSpaces(reader.GetString(6));
                string postcode = Helper.StripSpaces(reader.GetString(7));
                string country = Helper.StripSpaces(reader.GetString(8));

                Order newOrder = new Order(id, userId, recipient, addressLine1, addressLine2, city, county, postcode, country);
                orders.Add(newOrder);
            }

            connection.Close();

            return orders;
        }

        public static void SaveOrder(Order order)
        {
            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "dbo.SaveOrder";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = order.Id;
            command.Parameters.Add(parameter);

            SqlParameter parameter1 = command.CreateParameter();
            parameter1.ParameterName = "@userId";
            parameter1.Value = order.UserId;
            command.Parameters.Add(parameter1);

            SqlParameter parameter2 = command.CreateParameter();
            parameter2.ParameterName = "@recipient";
            parameter2.Value = order.Recipient;
            command.Parameters.Add(parameter2);

            SqlParameter parameter3 = command.CreateParameter();
            parameter3.ParameterName = "@addressLine1";
            parameter3.Value = order.AddressLine1;
            command.Parameters.Add(parameter3);

            SqlParameter parameter4 = command.CreateParameter();
            parameter4.ParameterName = "@addressLine2";
            parameter4.Value = order.AddressLine2;
            command.Parameters.Add(parameter4);

            SqlParameter parameter5 = command.CreateParameter();
            parameter5.ParameterName = "@city";
            parameter5.Value = order.City;
            command.Parameters.Add(parameter5);

            SqlParameter parameter6 = command.CreateParameter();
            parameter6.ParameterName = "@county";
            parameter6.Value = order.County;
            command.Parameters.Add(parameter6);

            SqlParameter parameter7 = command.CreateParameter();
            parameter7.ParameterName = "@postcode";
            parameter7.Value = order.Postcode;
            command.Parameters.Add(parameter7);

            SqlParameter parameter8 = command.CreateParameter();
            parameter8.ParameterName = "@country";
            parameter8.Value = order.Country;
            command.Parameters.Add(parameter8);

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}
