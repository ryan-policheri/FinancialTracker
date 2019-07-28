namespace FinancialTracker.DataAccess.Migrations
{
    using FinancialTracker.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FinancialTracker.DataAccess.FinancialTrackerDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(FinancialTracker.DataAccess.FinancialTrackerDBContext context)
        {
            //context.PaymentPurposes.AddOrUpdate(
            //    p => p.Purpose,
            //    new PaymentPurpose { Purpose = "Car Lease" },
            //    new PaymentPurpose { Purpose = "Apartment Rent" },
            //    new PaymentPurpose { Purpose = "Student Loans" }
            //);

            //context.Recipients.AddOrUpdate(
            //    r => r.Name,
            //    new Recipient { Name = "Toyota Financial Services" },
            //    new Recipient { Name = "Winding Hills Apartments", Address = "1825 Winding Hill Rd" },
            //    new Recipient { Name = "Navient" }
            //);

            //context.Payments.AddOrUpdate(
            //    p => new { p.Date, p.Recipient, p.AmountInDollars },
            //    new Payment { AmountInDollars = 399.94, Date = new DateTime(2018, 11, 15), PaymentPurposeID = 1, RecipientID = 1 },
            //    new Payment { AmountInDollars = 845, Date = new DateTime(2019, 2, 5), PaymentPurposeID = 2, RecipientID = 2 },
            //    new Payment { AmountInDollars = 720, Date = new DateTime(2019, 1, 20), PaymentPurposeID = 3, RecipientID = 3 }
            //);
        }
    }
}
