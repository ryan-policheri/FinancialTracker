using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialTracker.Model
{
    public class PaymentPurpose
    {
        [Key]
        public int ID { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression("[\\w ]{1,50}", ErrorMessage = "Invalid Name")]
        public string Purpose { get; set; }

        [NotMapped]
        public DateTime? FirstPaymentDate { get; set; }

        [NotMapped]
        public DateTime? LastPaymentDate { get; set; }

        public ICollection<Payment> Payments { get; set; }
    }
}
