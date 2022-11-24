using Microsoft.EntityFrameworkCore;
using powerbi_embedded_api.Entities;

namespace powerbi_embedded_api.DA
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _config;
        public DataContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        public DbSet<AccessRequest>? AccessRequests { get; set; }
    }
}