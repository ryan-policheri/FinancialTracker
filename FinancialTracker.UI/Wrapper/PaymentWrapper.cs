using FinancialTracker.Model;
using System;
using System.Collections.Generic;

namespace FinancialTracker.UI.Wrapper
{
    public class PaymentWrapper : ModelWrapper<Payment>
    {
        public PaymentWrapper(Payment model) : base(model)
        {
        }

        public int ID
        {
            get { return Model.ID; }
        }

        public double AmountInDollars
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        public DateTime Date
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public int? PaymentPurposeID
        {
            get { return GetValue<int?>(); }
            set { SetValue(value); }
        }

        public int? RecipientID
        {
            get { return GetValue<int?>(); }
            set { SetValue(value); }
        }

        //protected override IEnumerable<string> ValidateProperty(string propertyName)
        //{
        //    //Custom validation goes here
        //    switch (propertyName)
        //    {
        //        case nameof(Context):
        //            if (string.Equals(Context, "Corrupt Politician", StringComparison.OrdinalIgnoreCase))
        //            {
        //                yield return "Should not send money to corrupt politicians";
        //            }
        //            break;
        //    }
        //}
    }
}
