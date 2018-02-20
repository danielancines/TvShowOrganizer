using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Labs.WPF.TvShowOrganizer.Services.Contracts
{
    public interface ITorrentService
    {
        Task<List<Tuple<string, string>>> GetLinks(string tvShowName, string season, string number);
    }
}
