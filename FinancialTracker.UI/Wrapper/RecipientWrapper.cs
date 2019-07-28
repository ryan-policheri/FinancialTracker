using System;
using FinancialTracker.Model;

namespace FinancialTracker.UI.Wrapper
{
    public class RecipientWrapper : ModelWrapper<Recipient>
    {
        public RecipientWrapper(Recipient model) : base(model)
        {
        }

        public int ID { get { return Model.ID; } }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Address
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public DateTime? FirstPaymentDate
        {
            get { return GetValue<DateTime?>(); }
        }

        public DateTime? LastPaymentDate
        {
            get { return GetValue<DateTime?>(); }
        }
    }
}
