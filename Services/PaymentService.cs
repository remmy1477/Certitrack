using Certitrack.Models;
using Certitrack.Repositories;

namespace Certitrack.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public async Task<IEnumerable<Payment>> GetAllPaymentAsync()
        {
            return await _paymentRepository.GetAllPaymentAsync();   
        }

        public async Task<Payment> GetPaymentByIdAsync(long Id)
        {
            return await _paymentRepository.GetPaymentIdAsync(Id);
        }

        public async Task<string> InsertPaymentAsync(Payment payment)
        {
            try 
            {
                await _paymentRepository.AddAsync(payment);
                return "Payment Inserted Successfully";
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return $"Error inserting payment: {ex.Message}";
            }
        }

        public async Task<string> UpdatePaymentAsync(Payment payment)
        {
            try
            { 
                await _paymentRepository.UpdateAsync(payment);
                return "Payment Updated Successfully";
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return $"Error updating payment: {ex.Message}";
            }   
        }
    }
}
