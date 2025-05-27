using Certitrack.Models;

namespace Certitrack.Repositories
{
    public interface ITitleRepository
    {
        Task<IEnumerable<Tittle>> GetAllTitlesAsync();
        Task<Tittle> GetTitleByIdAsync(long Id);
    }
}
