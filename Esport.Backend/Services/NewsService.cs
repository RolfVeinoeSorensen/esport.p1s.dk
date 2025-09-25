using Esport.Backend.Entities;
using Esport.Backend.Data;

namespace Esport.Backend.Services
{
    public class NewsService(
        DataContext context) : INewsService
    {

        private readonly DataContext db = context;

        public IEnumerable<News> GetNews(int page, int pageSize)
        {
            return db.News
                .OrderByDescending(n => n.UpdatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        public News GetNewsByUrl(string urlSlug)
        {
            var news = db.News.FirstOrDefault(n => n.UrlSlug == urlSlug);
            if (news == null) throw new KeyNotFoundException("News not found");
            return news;
        }
        public News CreateOrUpdateNews(News news)
        {
            var existingNews = db.News.Find(news.Id);
            if (existingNews == null)
            {
                news.CreatedAt = DateTime.UtcNow;
                news.UpdatedAt = DateTime.UtcNow;
                db.News.Add(news);
            }
            else
            {
                existingNews.Title = news.Title;
                existingNews.Content = news.Content;
                existingNews.UrlSlug = news.UrlSlug;
                existingNews.UpdatedAt = DateTime.UtcNow;
                db.News.Update(existingNews);
            }
            db.SaveChanges();
            return news;
        }

        public Tag CreateOrUpdateTag(Tag tag)
        {
            var existingTag = db.Tags.Find(tag.Id);
            if (existingTag == null)
            {
                db.Tags.Add(tag);
            }
            else
            {
                existingTag.Name = tag.Name;
                db.Tags.Update(existingTag);
            }
            db.SaveChanges();
            return tag;
        }
    }
}