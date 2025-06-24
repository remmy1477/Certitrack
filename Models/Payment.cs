namespace Certitrack.Models
{
    public class Payment
    {
        public long Id { get; set; } 

        public string Reference { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }

        public Decimal Amount { get; set; } // in kobo
        public bool Paid { get; set; }

        public string PaymentChannel { get; set; }
        public string GatewayResponse { get; set; }
        public string Currency { get; set; }

        public DateTime TransactionDate { get; set; }
       
        public string PaymentType { get; set; } // Credential or Transcript

        public long TypeId { get; set; } // optional
                                         // public Guid? TranscriptId { get; set; } // optional

        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
