using Certitrack.Models;

namespace Certitrack.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllPaymentAsync();

        Task<Payment> GetPaymentByIdAsync(long Id);

       

        Task<string> InsertPaymentAsync(Payment payment);

        Task<string> UpdatePaymentAsync(Payment payment);
    }
}
