using Esport.Backend.Authorization;
using Esport.Backend.Entities;
using Esport.Backend.Enums;
using Esport.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Esport.Backend.Controllers
{
    public class NewsController(INewsService newsService) : ControllerBase
    {
        private readonly INewsService newsService = newsService;

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<News>> GetNews(int page, int pageSize)
        {
            var news = newsService.GetNews(page, pageSize);
            return Ok(news);
        }
        [HttpGet("[action]/{urlSlug}")]
        public ActionResult<News> GetNewsByUrl(string urlSlug)
        {
            var game = newsService.GetNewsByUrl(urlSlug);
            return Ok(game);
        }

        [Authorize([UserRole.Admin])]
        [HttpPost("[action]")]
        public ActionResult<News> CreateOrUpdateNews([FromBody]News news)
        {
            var newsResult = newsService.CreateOrUpdateNews(news);
            return Ok(newsResult);
        }

        [Authorize([UserRole.Admin])]
        [HttpPost("[action]")]
        public ActionResult<Tag> CreateOrUpdateTag([FromBody]Tag tag)
        {
            var tags = newsService.CreateOrUpdateTag(tag);
            return Ok(tags);
        }
        [HttpGet("[action]")]
        public ActionResult<IEnumerable<string>> GetAllUrlSlugs()
        {
            return Ok(newsService.GetAllUrlSlugs());
        }
    }
}