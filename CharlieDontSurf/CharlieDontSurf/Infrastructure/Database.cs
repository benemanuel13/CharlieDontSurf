using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;

using CharlieDontSurf.Models.Basket;
using CharlieDontSurf.Models.Inventory;
using CharlieDontSurf.Models.Shipping;
using CharlieDontSurf.Models.Orders;

using CharlieDontSurf.Helpers;

namespace CharlieDontSurf.Infrastructure
{
    public static class Database
    {
        
        private static string conString = "Data Source=TBENSERVER\\SQLEXPRESS; Initial Catalog=CharlieDontSurf; Integrated Security=True";

        static Database()
        {
            //connection.ConnectionString = conString;
        }

        public static Basket GetCurrentBasket(string userId, bool deep)
        {
            Basket basket = new Basket();
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "GetBasketId";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@userId";
            parameter.Value = userId;

            command.Parameters.Add(parameter);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
                basket.Id = reader.GetInt32(0);
            else
            {
                reader.Close();
                connection.Close();

                basket.Id = CreateBasket(userId);
                return basket;
            }

            reader.Close();

            if (!deep)
            {
                connection.Close();
                return basket;
            }

            command.CommandText = "GetBasketItems";
            command.Parameters.Clear();

            SqlParameter itemsParameter = command.CreateParameter();
            itemsParameter.ParameterName = "@parentId";
            itemsParameter.Value = basket.Id;
            command.Parameters.Add(itemsParameter);

            SqlParameter parentTypeParameter = command.CreateParameter();
            parentTypeParameter.ParameterName = "@parentType";
            parentTypeParameter.Value = 0;
            command.Parameters.Add(parentTypeParameter);

            reader = command.ExecuteReader();

            while(reader.Read())
            {
                int basketItemId = reader.GetInt32(0);
                int itemId = reader.GetInt32(1);
                int sizeId = reader.GetInt32(2);
                int styleId = reader.GetInt32(3);
                int quantity = reader.GetInt32(4);
                decimal price = reader.GetDecimal(5);

                Item item = Database.GetItem(itemId, true);

                BasketItem basketItem = new BasketItem(basketItemId, item.Id, sizeId, styleId, quantity, price);
                basketItem.IsSubItem = false;
                basketItem.SetPrice(false);

                GetBasketSubitems(ref basketItem);

                basket.AddItem(basketItem);
            }

            reader.Close();
            connection.Close();

            return basket;
        }

        private static void GetBasketSubitems(ref BasketItem basketItem)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "GetBasketItems";

            SqlParameter itemsParameter = command.CreateParameter();
            itemsParameter.ParameterName = "@parentId";
            itemsParameter.Value = basketItem.Id;
            command.Parameters.Add(itemsParameter);

            SqlParameter parentTypeParameter = command.CreateParameter();
            parentTypeParameter.ParameterName = "@parentType";
            parentTypeParameter.Value = 1;
            command.Parameters.Add(parentTypeParameter);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int basketItemId = reader.GetInt32(0);
                int itemId = reader.GetInt32(1);
                int sizeId = reader.GetInt32(2);
                int styleId = reader.GetInt32(3);
                int quantity = reader.GetInt32(4);
                decimal price = reader.GetDecimal(5);

                Item item = Database.GetItem(itemId, true);

                BasketItem newBasketItem = new BasketItem(basketItemId, item.Id, sizeId, styleId, quantity, price);
                newBasketItem.IsSubItem = true;
                newBasketItem.SetPrice(true);

                basketItem.AddItem(newBasketItem);
            }

            reader.Close();
            connection.Close();
        }

        public static Item GetItem(int itemId, bool deep)
        {
            SqlConnection connection2 = new SqlConnection();
            connection2.ConnectionString = conString;

            connection2.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection2;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "GetItem";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@itemId";
            parameter.Value = itemId;

            command.Parameters.Add(parameter);

            SqlDataReader reader = command.ExecuteReader();
            reader.Read();

            string name = Helper.StripSpaces(reader.GetString(0));
            string description = Helper.StripSpaces(reader.GetString(1));
            string fullDescription = Helper.StripSpaces(reader.GetString(2));
            decimal price = reader.GetDecimal(3);
            decimal bundlePrice = reader.GetDecimal(4);

            Item newItem = new Item(itemId, name, description, fullDescription, price, bundlePrice);
            GetItemSizes(ref newItem);

            if(!deep)
            {
                reader.Close();
                connection2.Close();

                return newItem;
            }

            reader.Close();

            command.Parameters.Clear();

            command.CommandText = "GetSubItems";

            SqlParameter subParameter = command.CreateParameter();
            subParameter.ParameterName = "@parentItemId";
            subParameter.Value = itemId;

            command.Parameters.Add(subParameter);

            reader = command.ExecuteReader();

            while(reader.Read())
            {
                int id = reader.GetInt32(0);
                name = Helper.StripSpaces(reader.GetString(1));
                description = Helper.StripSpaces(reader.GetString(2));
                fullDescription = Helper.StripSpaces(reader.GetString(3));
                price = reader.GetDecimal(4);
                bundlePrice = reader.GetDecimal(5);

                Item subItem = new Item(id, name, description, fullDescription, price, bundlePrice);
                GetItemSizes(ref subItem);

                newItem.AddSubItem(subItem);
            }

            reader.Close();
            connection2.Close();

            return newItem;
        }

        private static void GetItemSizes(ref Item item)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "GetItemSizes";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@itemId";
            parameter.Value = item.Id;

            command.Parameters.Add(parameter);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string description = Helper.StripSpaces(reader.GetString(1));

                ItemSize size = new ItemSize(id, description);

                item.AddSize(size);
            }

            reader.Close();
            connection.Close();
        }

        private static int CreateBasket(string userId)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "CreateBasket";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@userId";
            parameter.Value = userId;

            command.Parameters.Add(parameter);

            SqlDataReader reader = command.ExecuteReader();

            reader.Read();

            int basketId = reader.GetInt32(0);

            connection.Close();

            return basketId;
        }

        public static List<ShippingAddress> GetShippingAddresses(string userId)
        {
            List<ShippingAddress> addresses = new List<ShippingAddress>();

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "GetShippingAddresses";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@userId";
            parameter.Value = userId;

            command.Parameters.Add(parameter);

            SqlDataReader reader = command.ExecuteReader();

            while(reader.Read())
            {
                int id = reader.GetInt32(0);
                string recipient = Helper.StripSpaces(reader.GetString(1));
                string addressLine1 = Helper.StripSpaces(reader.GetString(2));
                string addressLine2;

                try
                {
                    addressLine2 = Helper.StripSpaces(reader.GetString(3));
                }
                catch
                {
                    addressLine2 = "";
                }

                string city = Helper.StripSpaces(reader.GetString(4));
                string county = Helper.StripSpaces(reader.GetString(5));
                string postcode = Helper.StripSpaces(reader.GetString(6));
                string country = Helper.StripSpaces(reader.GetString(7));

                ShippingAddress address = new ShippingAddress(id, recipient, addressLine1 , addressLine2, city, county, postcode, country);
                addresses.Add(address);
            }

            connection.Close();

            return addresses;
        }

        public static int GetShippingAddressCount(string userId)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "GetShippingAddressCount";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@userId";
            parameter.Value = userId;

            command.Parameters.Add(parameter);

            SqlDataReader reader = command.ExecuteReader();
            reader.Read();

            int count = reader.GetInt32(0);

            reader.Close();
            connection.Close();

            return count;
        }

        public static int SaveShippingAddress(ShippingAddress address, string userId)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "ClearShippingAddressesCurrent";
            command.ExecuteNonQuery();

            command.CommandText = "SaveShippingAddress";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@userId";
            parameter.Value = userId;
            command.Parameters.Add(parameter);

            SqlParameter parameter2 = command.CreateParameter();
            parameter2.ParameterName = "@recipient";
            parameter2.Value = address.Recipient;
            command.Parameters.Add(parameter2);

            SqlParameter parameter3 = command.CreateParameter();
            parameter3.ParameterName = "@addressLine1";
            parameter3.Value = address.AddressLine1;
            command.Parameters.Add(parameter3);

            SqlParameter parameter4 = command.CreateParameter();
            parameter4.ParameterName = "@addressLine2";

            if(address.AddressLine2 == null)
                parameter4.Value = "";
            else
                parameter4.Value = address.AddressLine2;

            command.Parameters.Add(parameter4);

            SqlParameter parameter5 = command.CreateParameter();
            parameter5.ParameterName = "@city";
            parameter5.Value = address.City;
            command.Parameters.Add(parameter5);

            SqlParameter parameter6 = command.CreateParameter();
            parameter6.ParameterName = "@county";
            parameter6.Value = address.County;
            command.Parameters.Add(parameter6);

            SqlParameter parameter7 = command.CreateParameter();
            parameter7.ParameterName = "@postcode";
            parameter7.Value = address.Postcode;
            command.Parameters.Add(parameter7);

            SqlParameter parameter8 = command.CreateParameter();
            parameter8.ParameterName = "@country";
            parameter8.Value = address.Country;
            command.Parameters.Add(parameter8);

            SqlDataReader reader = command.ExecuteReader();
            
            reader.Read();
            int newId = reader.GetInt32(0);

            reader.Close();
            connection.Close();

            return newId;
        }

        public static void UpdateShippingAddress(ShippingAddress address)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "ClearShippingAddressesCurrent";
            command.ExecuteNonQuery();

            command.CommandText = "UpdateShippingAddress";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = address.Id;
            command.Parameters.Add(parameter);

            SqlParameter parameter2 = command.CreateParameter();
            parameter2.ParameterName = "@recipient";
            parameter2.Value = address.Recipient;
            command.Parameters.Add(parameter2);

            SqlParameter parameter3 = command.CreateParameter();
            parameter3.ParameterName = "@addressLine1";
            parameter3.Value = address.AddressLine1;
            command.Parameters.Add(parameter3);

            SqlParameter parameter4 = command.CreateParameter();
            parameter4.ParameterName = "@addressLine2";
            if (address.AddressLine2 == null)
                parameter4.Value = "";
            else
                parameter4.Value = address.AddressLine2;

            command.Parameters.Add(parameter4);

            SqlParameter parameter5 = command.CreateParameter();
            parameter5.ParameterName = "@city";
            parameter5.Value = address.City;
            command.Parameters.Add(parameter5);

            SqlParameter parameter6 = command.CreateParameter();
            parameter6.ParameterName = "@county";
            parameter6.Value = address.County;
            command.Parameters.Add(parameter6);

            SqlParameter parameter7 = command.CreateParameter();
            parameter7.ParameterName = "@postcode";
            parameter7.Value = address.Postcode;
            command.Parameters.Add(parameter7);

            SqlParameter parameter8 = command.CreateParameter();
            parameter8.ParameterName = "@country";
            parameter8.Value = address.Country;
            command.Parameters.Add(parameter8);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public static ShippingAddress GetCurrentShippingAddress(string userId)
        {
            ShippingAddress address;

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "GetCurrentShippingAddress";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@userId";
            parameter.Value = userId;

            command.Parameters.Add(parameter);

            SqlDataReader reader = command.ExecuteReader();

            if(reader.Read())
            {
                int id = reader.GetInt32(0);
                string recipient = Helper.StripSpaces(reader.GetString(1));
                string addressLine1 = Helper.StripSpaces(reader.GetString(2));
                string addressLine2 = Helper.StripSpaces(reader.GetString(3));
                string city = Helper.StripSpaces(reader.GetString(4));
                string county = Helper.StripSpaces(reader.GetString(5));
                string postcode = Helper.StripSpaces(reader.GetString(6));
                string country = Helper.StripSpaces(reader.GetString(7));

                address = new ShippingAddress(id, recipient, addressLine1, addressLine2, city, county, postcode, country);
            }
            else
                address = null;

            reader.Close();
            connection.Close();

            return address;
        }

        public static ShippingAddress GetShippingAddress(int id)
        {
            ShippingAddress address;

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "GetShippingAddress";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = id;

            command.Parameters.Add(parameter);

            SqlDataReader reader = command.ExecuteReader();
            reader.Read();

            string recipient = Helper.StripSpaces(reader.GetString(0));
            string addressLine1 = Helper.StripSpaces(reader.GetString(1));
            string addressLine2 = Helper.StripSpaces(reader.GetString(2));
            string city = Helper.StripSpaces(reader.GetString(3));
            string county = Helper.StripSpaces(reader.GetString(4));
            string postcode = Helper.StripSpaces(reader.GetString(5));
            string country = Helper.StripSpaces(reader.GetString(6));

            address = new ShippingAddress(id, recipient, addressLine1, addressLine2, city, county, postcode, country);

            reader.Close();
            connection.Close();

            return address;
        }

        public static void SetShippingAddressCurrent(int id)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "ClearShippingAddressesCurrent";
            command.ExecuteNonQuery();

            command.CommandText = "SetShippingAddressCurrent";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = id;
            command.Parameters.Add(parameter);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public static void DeleteShippingAddress(int id)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "DeleteShippingAddress";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = id;

            command.Parameters.Add(parameter);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public static List<ItemType> GetItemTypes()
        {
            List<ItemType> itemTypes = new List<ItemType>();

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "GetItemTypes";

            SqlDataReader reader = command.ExecuteReader();

            while(reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);

                ItemType newType = new ItemType(id, name);
                itemTypes.Add(newType);
            }

            reader.Close();
            connection.Close();

            return itemTypes;
        }

        public static List<Item> GetItems(int typeId)
        {
            List<Item> items = new List<Item>();

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "GetItems";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@itemTypeId";
            parameter.Value = typeId;

            command.Parameters.Add(parameter);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                string description = reader.GetString(2);
                string fullDescription = reader.GetString(3);
                decimal price = reader.GetDecimal(4);
                decimal bundlePrice = reader.GetDecimal(5);

                Item newItem = new Item(id, name, description, fullDescription, price, bundlePrice);
                items.Add(newItem);
            }

            reader.Close();
            connection.Close();

            return items;
        }

        public static Item GetItem(int itemId)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "GetItem";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@itemId";
            parameter.Value = itemId;

            command.Parameters.Add(parameter);

            SqlDataReader reader = command.ExecuteReader();
            reader.Read();

            string name = reader.GetString(0);
            string description = reader.GetString(1);
            string fullDescription = reader.GetString(2);
            decimal price = reader.GetDecimal(3);
            decimal bundlePrice = reader.GetDecimal(4);

            Item item = new Item(itemId, name, description, fullDescription, price, bundlePrice);
            GetItemSizes(ref item);

            reader.Close();
            connection.Close();

            return item;
        }

        public static int SaveBasketItem(int parentId, BasketItem basketItem, int parentType)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;

            command.CommandText = "SaveBasketItem";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@parentId";
            parameter.Value = parentId;
            command.Parameters.Add(parameter);

            SqlParameter parameter2 = command.CreateParameter();
            parameter2.ParameterName = "@parentType";
            parameter2.Value = parentType;
            command.Parameters.Add(parameter2);

            SqlParameter parameter3 = command.CreateParameter();
            parameter3.ParameterName = "@itemId";
            parameter3.Value = basketItem.ItemId;
            command.Parameters.Add(parameter3);

            SqlParameter parameter4 = command.CreateParameter();
            parameter4.ParameterName = "@sizeId";
            parameter4.Value = basketItem.SizeId;
            command.Parameters.Add(parameter4);

            SqlParameter parameter5 = command.CreateParameter();
            parameter5.ParameterName = "@styleId";
            parameter5.Value = basketItem.StyleId;
            command.Parameters.Add(parameter5);

            SqlParameter parameter6 = command.CreateParameter();
            parameter6.ParameterName = "@quantity";
            parameter6.Value = basketItem.Quantity;
            command.Parameters.Add(parameter6);

            SqlParameter parameter7 = command.CreateParameter();
            parameter7.ParameterName = "@price";
            parameter7.Value = basketItem.Price;
            command.Parameters.Add(parameter7);

            SqlDataReader reader = command.ExecuteReader();

            reader.Read();
            int newId = reader.GetInt32(0);

            reader.Close();
            connection.Close();

            return newId;
        }

        public static void DeleteBasketItem(BasketItem basketItem)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;

            command.CommandText = "DeleteBasketItem";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@basketItemId";
            parameter.Value = basketItem.Id;
            command.Parameters.Add(parameter);

            command.ExecuteNonQuery();

            foreach (BasketItem item in basketItem.Items)
            {
                command.Parameters.Clear();

                parameter = command.CreateParameter();
                parameter.ParameterName = "@basketItemId";
                parameter.Value = item.Id;
                command.Parameters.Add(parameter);

                command.ExecuteNonQuery();
            }

            connection.Close();
        }

        public static void UpdateBasketItem(BasketItem basketItem)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;

            command.CommandText = "UpdateBasketItem";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@basketItemId";
            parameter.Value = basketItem.Id;
            command.Parameters.Add(parameter);

            SqlParameter parameter2 = command.CreateParameter();
            parameter2.ParameterName = "@itemId";
            parameter2.Value = basketItem.ItemId;
            command.Parameters.Add(parameter2);

            SqlParameter parameter3 = command.CreateParameter();
            parameter3.ParameterName = "@sizeId";
            parameter3.Value = basketItem.SizeId;
            command.Parameters.Add(parameter3);

            SqlParameter parameter4 = command.CreateParameter();
            parameter4.ParameterName = "@styleId";
            parameter4.Value = basketItem.StyleId;
            command.Parameters.Add(parameter4);

            SqlParameter parameter5 = command.CreateParameter();
            parameter5.ParameterName = "@quantity";
            parameter5.Value = basketItem.Quantity;
            command.Parameters.Add(parameter5);

            SqlParameter parameter6 = command.CreateParameter();
            parameter6.ParameterName = "@price";
            parameter6.Value = basketItem.TotalPrice;
            command.Parameters.Add(parameter6);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public static void CreateOrder(string userId)
        {
            ShippingAddress shippingAddress = GetCurrentShippingAddress(userId);
            Basket basket = Database.GetCurrentBasket(userId, true);

            Order newOrder = new Order(userId, shippingAddress.Recipient, shippingAddress.AddressLine1, shippingAddress.AddressLine2, shippingAddress.City, shippingAddress.County, shippingAddress.Postcode, shippingAddress.Country);

            foreach (BasketItem item in basket.Items)
            {
                string size = GetSize(item.SizeId);
                string style = GetStyle(item.StyleId);

                OrderItem newItem = new OrderItem(item.ItemId, item.Name, item.Description, size, style, item.Quantity, item.Price);

                foreach (BasketItem subItem in item.Items)
                {
                    size = GetSize(subItem.SizeId);
                    style = GetStyle(subItem.StyleId);

                    OrderItem newSubItem = new OrderItem(subItem.ItemId, subItem.Name, subItem.Description, size, style, subItem.Quantity, subItem.Price);
                    newItem.AddOrderItem(newSubItem);
                }

                newOrder.AddOrderItem(newItem);
            }

            SaveOrder(userId, newOrder);
        }
        private static void SaveOrder(string userId, Order order)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;

            command.CommandText = "SaveOrder";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@UserId";
            parameter.Value = userId;
            command.Parameters.Add(parameter);

            SqlParameter parameter2 = command.CreateParameter();
            parameter2.ParameterName = "@Recipient";
            parameter2.Value = order.Recipient;
            command.Parameters.Add(parameter2);

            SqlParameter parameter3 = command.CreateParameter();
            parameter3.ParameterName = "@AddressLine1";
            parameter3.Value = order.AddressLine1;
            command.Parameters.Add(parameter3);

            SqlParameter parameter4 = command.CreateParameter();
            parameter4.ParameterName = "@AddressLine2";
            parameter4.Value = order.AddressLine2;
            command.Parameters.Add(parameter4);

            SqlParameter parameter5 = command.CreateParameter();
            parameter5.ParameterName = "@City";
            parameter5.Value = order.City;
            command.Parameters.Add(parameter5);

            SqlParameter parameter6 = command.CreateParameter();
            parameter6.ParameterName = "@County";
            parameter6.Value = order.County;
            command.Parameters.Add(parameter6);

            SqlParameter parameter7 = command.CreateParameter();
            parameter7.ParameterName = "@Postcode";
            parameter7.Value = order.Postcode;
            command.Parameters.Add(parameter7);

            SqlParameter parameter8 = command.CreateParameter();
            parameter8.ParameterName = "@Country";
            parameter8.Value = order.Country;
            command.Parameters.Add(parameter8);

            SqlDataReader reader = command.ExecuteReader();
            reader.Read();

            int newId = reader.GetInt32(0);

            reader.Close();
            connection.Close();

            foreach (OrderItem item in order.Items)
                SaveOrderItem(newId, 0, item);
        }

        private static void SaveOrderItem(int parentId, int parentType, OrderItem item)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;

            command.CommandText = "SaveOrderItem";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@ParentId";
            parameter.Value = parentId;
            command.Parameters.Add(parameter);

            SqlParameter parameter2 = command.CreateParameter();
            parameter2.ParameterName = "@ParentType";
            parameter2.Value = parentType;
            command.Parameters.Add(parameter2);

            SqlParameter parameter3 = command.CreateParameter();
            parameter3.ParameterName = "@ItemId";
            parameter3.Value = item.ItemId;
            command.Parameters.Add(parameter3);

            SqlParameter parameter4 = command.CreateParameter();
            parameter4.ParameterName = "@Name";
            parameter4.Value = item.Name;
            command.Parameters.Add(parameter4);

            SqlParameter parameter5 = command.CreateParameter();
            parameter5.ParameterName = "@Description";
            parameter5.Value = item.Description;
            command.Parameters.Add(parameter5);

            SqlParameter parameter6 = command.CreateParameter();
            parameter6.ParameterName = "@Size";
            parameter6.Value = item.Size;
            command.Parameters.Add(parameter6);

            SqlParameter parameter7 = command.CreateParameter();
            parameter7.ParameterName = "@Style";
            parameter7.Value = item.Style;
            command.Parameters.Add(parameter7);

            SqlParameter parameter8 = command.CreateParameter();
            parameter8.ParameterName = "@Quantity";
            parameter8.Value = item.Quantity;
            command.Parameters.Add(parameter8);

            SqlParameter parameter9 = command.CreateParameter();
            parameter9.ParameterName = "@TotalPrice";
            parameter9.Value = item.TotalPrice;
            command.Parameters.Add(parameter9);

            SqlDataReader reader = command.ExecuteReader();
            reader.Read();

            int newId = reader.GetInt32(0);

            foreach (OrderItem subItem in item.Items)
                SaveOrderItem(newId, 1, subItem);

            reader.Close();
            connection.Close();
        }

        public static List<Order> GetUndownloadedOrders(string storedProcedure)
        {
            List<Order> orders = new List<Order>();

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;

            command.CommandText = storedProcedure;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string userId = Helper.StripSpaces(reader.GetString(1));
                string recipient = Helper.StripSpaces(reader.GetString(2));
                string address1 = Helper.StripSpaces(reader.GetString(3));
                string address2 = Helper.StripSpaces(reader.GetString(4));
                string city = Helper.StripSpaces(reader.GetString(5));
                string county = Helper.StripSpaces(reader.GetString(6));
                string postcode = Helper.StripSpaces(reader.GetString(7));
                string country = Helper.StripSpaces(reader.GetString(8));

                Order newOrder = new Order(id, userId, recipient, address1, address2, city, county, postcode, country);
                newOrder.Items = GetOrderItems(id, 0);

                foreach (OrderItem item in newOrder.Items)
                    item.Items = GetOrderItems(item.Id, 1);

                orders.Add(newOrder);
            }

            connection.Close();
            return orders;
        }

        public static void SetOrderDownloaded(int id, string storedProcedure)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;

            command.CommandText = storedProcedure;

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = id;
            command.Parameters.Add(parameter);

            command.ExecuteNonQuery();

            connection.Close();
        }

        private static List<OrderItem> GetOrderItems(int parentId, int parentType)
        {
            List<OrderItem> items = new List<OrderItem>();

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;

            command.CommandText = "GetOrderItems";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@ParentId";
            parameter.Value = parentId;
            command.Parameters.Add(parameter);

            SqlParameter parameter2 = command.CreateParameter();
            parameter2.ParameterName = "@ParentType";
            parameter2.Value = parentType;
            command.Parameters.Add(parameter2);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                int itemId = reader.GetInt32(1);
                string name = Helper.StripSpaces(reader.GetString(4));
                string description = Helper.StripSpaces(reader.GetString(5));
                string size = Helper.StripSpaces(reader.GetString(6));
                string style = Helper.StripSpaces(reader.GetString(7));
                int quantity = reader.GetInt32(8);
                decimal totalPrice = reader.GetDecimal(9);

                OrderItem newItem = new OrderItem(id, itemId, name, description, size, style, quantity, totalPrice);
                items.Add(newItem);
            }

            reader.Close();
            connection.Close();

            return items;
        }

        private static string GetSize(int id)
        {
            if (id == 0)
                return "No Size";

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "GetItemSize";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = id;
            command.Parameters.Add(parameter);

            SqlDataReader reader = command.ExecuteReader();
            reader.Read();

            string description = reader.GetString(0);

            reader.Close();
            connection.Close();

            return description;
        }

        private static string GetStyle(int id)
        {
            if (id == 0)
                return "No Style";

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = conString;

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "GetItemStyle";

            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = id;
            command.Parameters.Add(parameter);

            SqlDataReader reader = command.ExecuteReader();
            reader.Read();

            string description = reader.GetString(0);

            reader.Close();
            connection.Close();

            return description;
        }
    }
}