using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF_DBFirst.MSSQL;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace EF_DBFirst
{
    public class NorthWindCustomerService : BaseService
    {
        private NORTHWNDEntities _dbContext;
        public NORTHWNDEntities NorthWindDB
        {
            get
            {
                if (_dbContext == null)
                    _dbContext = new NORTHWNDEntities();
                return _dbContext;
            }
        }

        public IEnumerable<Customers> GetAllCustomer()
        {
            try
            {
                return NorthWindDB.Customers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu!{ex.Message}");
                throw;
            }
           
        }

        public void CreateCustomer(Customers addedCustomer)
        {
            try
            {
                NorthWindDB.Customers.Add(addedCustomer);
                NorthWindDB.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu!{ex.Message}");
                throw;
            }

        }

        public void UpdateCustomer(Customers updatedCustomer)
        {
            try
            {
                NorthWindDB.Customers.AddOrUpdate(updatedCustomer);
                NorthWindDB.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu!{ex.Message}");
                throw;
            }

        }

        public void DeleteCustomer(Customers deletedCustomer)
        {
            try
            {
                NorthWindDB.Customers.Remove(deletedCustomer);
                NorthWindDB.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu!{ex.Message}");
                throw;
            }
        }

        public Customers GetCustomerByID(string customerID)
        {
            try
            {
                return _dbContext.Customers.FirstOrDefault(x => x.CustomerID == customerID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu!{ex.Message}");
                throw;
            }

        }
    }
}
