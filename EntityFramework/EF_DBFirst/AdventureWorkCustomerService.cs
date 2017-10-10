using EF_DBFirst.MSSQL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_DBFirst
{
    public class AdventureWorkCustomerService
    {
        private AdventureWorksEntities _dbContext;
        public AdventureWorksEntities AdventureWorkDB
        {
            get
            {
                if (_dbContext == null)
                    _dbContext = new AdventureWorksEntities();
                return _dbContext;
            }
        }

        public IEnumerable<Customer> GetAllCustomer()
        {
            try
            {
                return AdventureWorkDB.Customer;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu!{ex.Message}");
                throw;
            }

        }

        public void CreateCustomer(Customer addedCustomer)
        {
            try
            {
                AdventureWorkDB.Customer.Add(addedCustomer);
                AdventureWorkDB.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu!{ex.Message}");
                throw;
            }

        }

        public void UpdateCustomer(Customer updatedCustomer)
        {
            try
            {
                AdventureWorkDB.Customer.AddOrUpdate(updatedCustomer);
                AdventureWorkDB.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu!{ex.Message}");
                throw;
            }

        }

        public void DeleteCustomer(Customer deletedCustomer)
        {
            try
            {
                AdventureWorkDB.Customer.Remove(deletedCustomer);
                AdventureWorkDB.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu!{ex.Message}");
                throw;
            }
        }

        public Customer GetCustomerByID(int customerID)
        {
            try
            {
                return _dbContext.Customer.FirstOrDefault(x => x.CustomerID == customerID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu!{ex.Message}");
                throw;
            }

        }
    }
}
