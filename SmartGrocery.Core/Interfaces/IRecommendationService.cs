using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartGrocery.Core.Interfaces
{
    public interface IRecommendationService
    {
        Task<IEnumerable<string>> SuggestItemsAsync(string query, IEnumerable<string> recentItems);
    }
}