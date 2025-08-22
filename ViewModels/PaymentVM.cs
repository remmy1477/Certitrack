namespace Certitrack.ViewModels
{
    public class PaymentVM
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
    }
}
