public class RecommendationTests
{
    [Fact]
    public async Task LocalRecommendation_ReturnsPrefixMatches()
    {
        var repo = new InMemoryRepo(new[] {"leite","pão","leite condensado"});
        var svc = new LocalRecommendationService(repo);

        var res = await svc.SuggestItemsAsync("lei", new string[0]);
        Assert.Contains("leite", res);
    }
}