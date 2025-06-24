
using Certitrack.Data;
using Certitrack.Models;
using Microsoft.EntityFrameworkCore;

namespace Certitrack.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }   
        public async Task AddAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();  
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<Payment> GetPaymentIdAsync(long Id)
        {
            return await _context.Payments.FindAsync(Id);
        }

        public async  Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async  Task UpdateAsync(Payment payment)
        {
            _context.Update(payment);
            await _context.SaveChangesAsync();
        }
    }
}
