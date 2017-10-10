using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_CodeFirst.ContextConfiguration
{
    internal class Configuration : DbMigrationsConfiguration<UniversityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true; // Otomatik olarak migration yapar.Projede çok kişi çalışınca problem oluşturan kısım
            AutomaticMigrationDataLossAllowed = true; // En büyük sıkıntılardan biri veri kaybının önunu acıyor.
            ContextKey = "EF_CodeFirst.ContextConfiguration.UniversityContext";
        }
    }
}
