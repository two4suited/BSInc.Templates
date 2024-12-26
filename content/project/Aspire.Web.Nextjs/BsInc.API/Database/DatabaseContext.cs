using Microsoft.EntityFrameworkCore;

namespace AspireTemplate.ApiService.Database
{
    public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
    {
        
    }
}