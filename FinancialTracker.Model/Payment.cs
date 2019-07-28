using System;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace FinancialTracker.Model
{
    public class Payment
    {
        [Key]
        public int ID { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Range(0.0, 999999999999.9)]
        public double AmountInDollars { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public int? PaymentPurposeID { get; set; }

        public PaymentPurpose PaymentPurpose { get; set; }

        [Required]
        public int? RecipientID { get; set; }

        public Recipient Recipient { get; set; }
    }
}
