using W_ORM.MSSQL;
using W_ORM.Test.MSSQL.BankEntities;

namespace W_ORM.Test.MSSQL
{
    public class Bank
    {
        public MSSQLProviderContext<Customer> Customer { get { return new MSSQLProviderContext<Customer>(); } }
        public MSSQLProviderContext<Account> Account { get { return new MSSQLProviderContext<Account>(); } }
        public MSSQLProviderContext<Loan> Loan { get { return new MSSQLProviderContext<Loan>(); } }
    }
}
