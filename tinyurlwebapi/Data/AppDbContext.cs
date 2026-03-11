namespace tinyurlwebapi.Data
{
    using Microsoft.EntityFrameworkCore;
    using tinyurlwebapi.Model;

    namespace TinyUrlWebApi.Data
    {
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            public DbSet<UrlsDto> Urls { get; set; }
        }
    }

}
