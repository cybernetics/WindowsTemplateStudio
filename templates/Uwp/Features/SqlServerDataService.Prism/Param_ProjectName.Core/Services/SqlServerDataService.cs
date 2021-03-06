﻿using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Param_RootNamespace.Core.Models;

namespace Param_RootNamespace.Core.Services
{
    // This class holds sample data used by some generated pages to show how they can be used.
    // TODO WTS: Change your code to use this instead of the SampleDataService.
    public static class SqlServerDataService
    {
        // TODO WTS: Specify the connection string in a config file or below.
        private static string GetConnectionString()
        {
            // Attempt to get the connection string from a config file
            // Learn more about specifying the connection string in a config file at https://docs.microsoft.com/en-us/dotnet/api/system.configuration.configurationmanager?view=netframework-4.7.2
            var conStr = ConfigurationManager.ConnectionStrings["MyAppConnectionString"]?.ConnectionString;

            if (!string.IsNullOrWhiteSpace(conStr))
            {
                return conStr;
            }
            else
            {
                // If no connection string is specified in a config file, use this as a fallback.
                return @"Data Source=*server*\*instance*;Initial Catalog=*dbname*;Integrated Security=SSPI";
            }
        }

        // This method returns data with the same structure as the SampleDataService but based on the NORTHWIND database.
        // Use this as an alternative to the sample data to test using a different datasource without changing the page code.
        // Alternatively, use this as a base for your ow data retrieval methods.
        // TODO WTS: Remove this when or if it isn't needed.
        public static async Task<ObservableCollection<SampleOrder>> AllOrders()
        {
            const string getSampleOrdersQuery = @"
SELECT Orders.OrderID,
       Orders.OrderDate,
       Customers.CompanyName,
       Orders.ShipName,
       SUM([Order Details].UnitPrice * [Order Details].Quantity) as OrderTotal,
       ISNULL(CHOOSE(CAST(RAND(CHECKSUM(NEWID())) * 3 as INT), 'Shipped', 'Closed'), 'New') as Status,
       CAST(RAND(CHECKSUM(NEWID())) * 200 as INT) + 57600 as Symbol
FROM dbo.Orders
     inner join dbo.[Order Details] on Orders.OrderID = [Order Details].OrderID
     inner join dbo.Customers ON Orders.CustomerID = Customers.CustomerID
Group by Orders.OrderID, Orders.OrderDate, Customers.CompanyName, Orders.ShipName, Orders.CustomerID
Order BY Orders.OrderID";

            var sampleOrders = new ObservableCollection<SampleOrder>();

            try
            {
                using (var conn = new SqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = getSampleOrdersQuery;

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    var order = new SampleOrder
                                    {
                                        OrderId = reader.GetInt32(0),
                                        OrderDate = reader.GetDateTime(1),
                                        Company = reader.GetString(2),
                                        ShipTo = reader.GetString(3),
                                        OrderTotal = double.Parse(reader.GetDecimal(4).ToString()),
                                        Status = reader.GetString(5),
                                        Symbol = (char)reader.GetInt32(6)
                                    };
                                    sampleOrders.Add(order);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }

            return sampleOrders;
        }
    }
}
