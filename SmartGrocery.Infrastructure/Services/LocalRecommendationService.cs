using SmartGrocery.Core.Interfaces;
using SmartGrocery.Core.Models;

namespace SmartGrocery.Infrastructure.Services;

public class LocalRecommendationService : IRecommendationService
{
    private readonly IRepository _repo;

    public LocalRecommendationService(IRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<string>> SuggestItemsAsync(string query, IEnumerable<string> recentItems)
    {
        // Query não nulo
        var q = query?.Trim() ?? string.Empty;

        // Buscar todas as listas e seus itens
        var lists = await _repo.GetAllListsAsync();
        var allItems = lists.SelectMany(l => l.Items).ToList();

        // Filtrar por prefixo
        var suggestions = allItems
            .Where(i => i.ItemName.StartsWith(q, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(i => i.Frequency) // ordenar por frequência
            .Select(i => i.ItemName)
            .Distinct()
            .Take(10);

        return suggestions;
    }
}

