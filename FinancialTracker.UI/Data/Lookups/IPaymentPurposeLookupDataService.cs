using FinancialTracker.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialTracker.UI.Data.Lookups
{
    public interface IPaymentPurposeLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetPaymentPurposeLookupAsync();
    }
}
