using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialTracker.Model;

namespace FinancialTracker.UI.Data.Repositories
{
    public interface IRecipientRepository : IGenericRepository<Recipient>
    {
        Task<IEnumerable<LookupItem>> GetPaymentLookupByRecipientIDAsync(int recipientID);

        Task<bool> HasPaymentsAsync(int paymentPurposeID);
        Task ReloadPaymentAsync(int paymentID);
    }
}