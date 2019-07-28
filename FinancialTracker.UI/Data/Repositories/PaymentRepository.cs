using System.Data.Entity;
using System.Threading.Tasks;
using FinancialTracker.DataAccess;
using FinancialTracker.Model;

namespace FinancialTracker.UI.Data.Repositories
{

    public class PaymentRepository : GenericRepository<Payment,FinancialTrackerDBContext>, IPaymentRepository
    {
        public PaymentRepository(FinancialTrackerDBContext context) : base(context)
        {
        }

        public override async Task<Payment> GetByIDAsync(int paymentID)
        {
            return await Context.Payments.SingleAsync(p => p.ID == paymentID);
        }
    }
}
