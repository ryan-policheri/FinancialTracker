using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialTracker.Model;

namespace FinancialTracker.UI.Data.Lookups
{
    public interface IRecipientLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetRecipientLookupAsync();
    }
}