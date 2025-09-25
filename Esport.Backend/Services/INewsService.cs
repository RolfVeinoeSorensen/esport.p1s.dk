using Esport.Backend.Entities;

namespace Esport.Backend.Services
{
    public interface INewsService
    {
        IEnumerable<News> GetNews(int page, int pageSize);
        News GetNewsByUrl(string urlSlug);
        News CreateOrUpdateNews(News news);
        Tag CreateOrUpdateTag(Tag tags);
    }
}
