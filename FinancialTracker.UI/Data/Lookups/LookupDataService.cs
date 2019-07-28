using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using FinancialTracker.DataAccess;
using FinancialTracker.Model;

namespace FinancialTracker.UI.Data.Lookups
{
    public class LookupDataService : IPaymentLookupDataService, IRecipientLookupDataService, IPaymentPurposeLookupDataService
    {
        private Func<FinancialTrackerDBContext> _contextCreator;

        public LookupDataService(Func<FinancialTrackerDBContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetPaymentLookupAsync()
        {
            using (var context = _contextCreator())
            {
                return await context.Payments.AsNoTracking()
                    .OrderByDescending(p => p.Date)
                    .Select(p =>
                        new LookupItem
                        {
                            ID = p.ID,
                            DisplayMember = (SqlFunctions.DatePart("month",p.Date) + "/" +
                                             SqlFunctions.DatePart("day", p.Date) + "/" +
                                             SqlFunctions.DatePart("year", p.Date) +
                                             " " + p.PaymentPurpose.Purpose)
                        })
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetRecipientLookupAsync()
        {
            using (var context = _contextCreator())
            {
                return await context.Recipients.AsNoTracking()
                    .Select(p =>
                        new LookupItem
                        {
                            ID = p.ID,
                            DisplayMember = p.Name
                        })
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetPaymentPurposeLookupAsync()
        {
            using (var context = _contextCreator())
            {
                return await context.PaymentPurposes.AsNoTracking()
                    .Select(p =>
                        new LookupItem
                        {
                            ID = p.ID,
                            DisplayMember = p.Purpose
                        })
                    .ToListAsync();
            }
        }

    }
}
