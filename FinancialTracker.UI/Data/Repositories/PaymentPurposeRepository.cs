using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using FinancialTracker.DataAccess;
using FinancialTracker.Model;

namespace FinancialTracker.UI.Data.Repositories
{
    public class PaymentPurposeRepository : GenericRepository<PaymentPurpose, FinancialTrackerDBContext>, IPaymentPurposeRepository
    {
        public PaymentPurposeRepository(FinancialTrackerDBContext context) : base(context)
        {          
        }

        public override async Task<PaymentPurpose> GetByIDAsync(int paymentPurposeID)
        {
            PaymentPurpose paymentPurpose = await Context.PaymentPurposes.SingleAsync(p => p.ID == paymentPurposeID);

            Payment firstPayment = await Context.Payments
                .Where(p => p.PaymentPurposeID == paymentPurposeID)
                .OrderBy(p => p.Date)
                .FirstOrDefaultAsync();

            Payment lastPayment = await Context.Payments
                .Where(p => p.PaymentPurposeID == paymentPurposeID)
                .OrderByDescending(p => p.Date)
                .FirstOrDefaultAsync();

            paymentPurpose.FirstPaymentDate = firstPayment?.Date;
            paymentPurpose.LastPaymentDate = lastPayment?.Date;

            return paymentPurpose;
        }

        public async Task<IEnumerable<LookupItem>> GetPaymentLookupByPaymentPurposeIDAsync(int paymentPurposeID)
        {
            return await Context.Payments.AsNoTracking()
                .Where(p => p.PaymentPurposeID == paymentPurposeID)
                .OrderByDescending(p => p.Date)
                .Select(p =>
                    new LookupItem
                    {
                        ID = p.ID,
                        DisplayMember = (SqlFunctions.DatePart("month", p.Date) + "/" +
                                         SqlFunctions.DatePart("day", p.Date) + "/" +
                                         SqlFunctions.DatePart("year", p.Date) +
                                         " " + p.Recipient.Name)
                    })
                .ToListAsync();
        }

        public async Task<bool> HasPaymentsAsync(int paymentPurposeID)
        {
            return await Context.Payments.AsNoTracking()
                .Include(p => p.PaymentPurposeID)
                .AnyAsync(p => p.PaymentPurposeID == paymentPurposeID);
        }

        public async Task ReloadPaymentAsync(int paymentID)
        {
            var dbEntityEntry = Context.ChangeTracker.Entries<Payment>()
                .SingleOrDefault(db => db.Entity.ID == paymentID);
            if (dbEntityEntry != null)
            {
                await dbEntityEntry.ReloadAsync();
            }
        }
    }
}
