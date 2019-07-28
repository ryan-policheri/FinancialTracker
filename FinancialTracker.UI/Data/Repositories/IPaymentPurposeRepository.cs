using FinancialTracker.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialTracker.UI.Data.Repositories
{
    public interface IPaymentPurposeRepository : IGenericRepository<PaymentPurpose>
    {
        Task<IEnumerable<LookupItem>> GetPaymentLookupByPaymentPurposeIDAsync(int paymentPurposeID);

        Task<bool> HasPaymentsAsync(int paymentPurposeID);
        Task ReloadPaymentAsync(int paymentID);
    }
}
