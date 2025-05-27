using Certitrack.Models;

namespace Certitrack.Services
{
    public interface ITitleService
    {
        Task<IEnumerable<Tittle>> GetAllTitlesAsync();
        Task<Tittle> GetTitleByIdAsync(long Id);
    }
}
