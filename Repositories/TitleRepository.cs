using Certitrack.Data;
using Certitrack.Models;
using Microsoft.EntityFrameworkCore;

namespace Certitrack.Repositories
{
    public class TitleRepository : ITitleRepository
    {
        private readonly AppDbContext _context;

        public TitleRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Tittle>> GetAllTitlesAsync()
        {
            return await _context.Tittles.ToListAsync();
        }

        public async Task<Tittle> GetTitleByIdAsync(long Id)
        {
            return await _context.Tittles.FindAsync(Id);
        }
    }

}
