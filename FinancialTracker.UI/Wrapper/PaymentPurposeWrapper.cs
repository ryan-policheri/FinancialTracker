using FinancialTracker.Model;
using System;

namespace FinancialTracker.UI.Wrapper
{
    public class PaymentPurposeWrapper : ModelWrapper<PaymentPurpose>
    {
        public PaymentPurposeWrapper(PaymentPurpose model) : base(model)
        {           
        }

        public int ID { get { return Model.ID; } }

        public string Purpose
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
