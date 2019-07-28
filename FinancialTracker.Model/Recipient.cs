using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialTracker.Model
{
    public class Recipient
    {
        [Key]
        public int ID { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Required]
        [RegularExpression("[\\w ]{1,50}", ErrorMessage = "Invalid Name")]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        [RegularExpression("[\\w\\d.,# ]{0,100}", ErrorMessage = "Invalid Address")]
        public string Address { get; set; }

        [NotMapped]
        public DateTime? FirstPaymentDate { get; set; }

        [NotMapped]
        public DateTime? LastPaymentDate { get; set; }

        public ICollection<Payment> Payments { get; set; }
    }
}
