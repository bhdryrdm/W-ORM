using EF_DBFirst.MSSQL;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EF_DBFirst
{
    public class Program
    {
        static void Main(string[] args)
        {
            #region NorthWind
            Customers addedCustomer = new Customers
            {
                CustomerID = "AB999",
                Address = "Test Adress",
                City = "İstanbul",
                CompanyName = "NetBT",
                ContactName = "Bahadır Yardım",
                ContactTitle = "Technology Consultant",
                Country = "Türkiye",
                Fax = "054612356",
                Phone = "054623891623",
                PostalCode = "54900",
                Region = "Marmara"
            };

            NorthWindCustomerService customerService = new NorthWindCustomerService();
            customerService.CreateCustomer(addedCustomer);

            List<Customers> customers = customerService.GetAllCustomer().ToList();

            using (NORTHWNDEntities dbContext = new NORTHWNDEntities())
            {
                IQueryable<Orders> queryableOrders = dbContext.Orders;
                IEnumerable<Orders> enumarableOrders = dbContext.Orders;

                IQueryable queryableOrder = queryableOrders.Where(x => x.CustomerID == "TOMSP");
                IEnumerable enumerableOrder = enumarableOrders.Where(x => x.CustomerID == "TOMSP");
            }
            #endregion
            #region AdventureWorks
            Customer addedCustomerAdvntr = new Customer
            {
                CustomerID = 123456789,
                AccountNumber = "1234"
            };

            AdventureWorkCustomerService customerServiceAdvntr = new AdventureWorkCustomerService();
            customerServiceAdvntr.CreateCustomer(addedCustomerAdvntr);

            List<Customer> customersAdvntr = customerServiceAdvntr.GetAllCustomer().ToList();

            using (AdventureWorksEntities dbContext = new AdventureWorksEntities())
            {
                IQueryable<SalesOrderDetail> queryableOrders = dbContext.SalesOrderDetail;
                IEnumerable<SalesOrderDetail> enumarableOrders = dbContext.SalesOrderDetail;

                IQueryable queryableOrder = queryableOrders.Where(x => x.ProductID == 777);
                IEnumerable enumerableOrder = enumarableOrders.Where(x => x.ProductID == 777);
            }
            #endregion

        }
    }
}
