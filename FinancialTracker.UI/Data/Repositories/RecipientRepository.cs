using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using FinancialTracker.DataAccess;
using FinancialTracker.Model;

namespace FinancialTracker.UI.Data.Repositories
{
    public class RecipientRepository : GenericRepository<Recipient, FinancialTrackerDBContext>, IRecipientRepository
    {
        public RecipientRepository(FinancialTrackerDBContext context) : base(context)
        {
        }

        public override async Task<Recipient> GetByIDAsync(int recipientId)
        {
            Recipient recipient = await Context.Recipients
                .SingleAsync(r => r.ID == recipientId);

            Payment firstPayment = await Context.Payments
                .Where(p => p.RecipientID == recipientId)
                .OrderBy(p => p.Date)
                .FirstOrDefaultAsync();

            Payment lastPayment = await Context.Payments
                .Where(p => p.RecipientID == recipientId)
                .OrderByDescending(p => p.Date)
                .FirstOrDefaultAsync();

            recipient.FirstPaymentDate = firstPayment?.Date;
            recipient.LastPaymentDate = lastPayment?.Date;

            return recipient;
        }

        public async Task<IEnumerable<LookupItem>> GetPaymentLookupByRecipientIDAsync(int recipientID)
        {
            return await Context.Payments.AsNoTracking()
                .Where(p => p.RecipientID == recipientID)
                .OrderByDescending(p => p.Date)
                .Select(p =>
                    new LookupItem
                    {
                        ID = p.ID,
                        DisplayMember = (SqlFunctions.DatePart("month", p.Date) + "/" +
                                         SqlFunctions.DatePart("day", p.Date) + "/" +
                                         SqlFunctions.DatePart("year", p.Date) +
                                         " " + p.PaymentPurpose.Purpose)
                    })
                .ToListAsync();
        }

        public async Task<bool> HasPaymentsAsync(int recipientID)
        {
            return await Context.Payments.AsNoTracking()
                .Include(p => p.RecipientID)
                .AnyAsync(p => p.RecipientID == recipientID);
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
