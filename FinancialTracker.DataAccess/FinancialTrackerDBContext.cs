using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using FinancialTracker.Model;

namespace FinancialTracker.DataAccess
{
    public class FinancialTrackerDBContext : DbContext
    {
        public FinancialTrackerDBContext() : base("FinancialTrackerDB")
        {
            
        }

        public DbSet<Recipient> Recipients { get; set; }

        public DbSet<PaymentPurpose> PaymentPurposes { get; set; }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
