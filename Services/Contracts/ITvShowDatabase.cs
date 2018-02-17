using Labs.WPF.TvShowOrganizer.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Labs.WPF.TvShowOrganizer.Services.Contracts
{
    public interface ITvShowDatabase
    {
        Task<IEnumerable<TvShow>> Search(string term);
    }
}
