using Certitrack.Models;
using Certitrack.Repositories;

namespace Certitrack.Services
{
    public class TitleService : ITitleService
    {
        private readonly ITitleRepository _titleRepository;
        public TitleService(ITitleRepository titleRepository)
        {
            _titleRepository = titleRepository;
        }

        public async Task<IEnumerable<Tittle>> GetAllTitlesAsync()
        {
            return await _titleRepository.GetAllTitlesAsync();  
        }

        public async Task<Tittle> GetTitleByIdAsync(long Id)
        {
            return await _titleRepository.GetTitleByIdAsync(Id);
        }
    }
}
