using Certitrack.Models;

namespace Certitrack.Repositories
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllPaymentAsync();

       // Task<Payment> GetCertificateDetailByIdAsync(long Id);

        Task<Payment> GetPaymentIdAsync(long Id);

        Task AddAsync(Payment payment);

        Task SaveChangesAsync();

        Task UpdateAsync(Payment payment);
    }
}
