using Labs.WPF.TvShowOrganizer.Data.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Labs.WPF.TvShowOrganizer.Services.Contracts
{
    public interface ITorrentService
    {
        Task<List<TorrentInfoDTO>> GetLinks(string tvShowName, string season, string number);
    }
}
