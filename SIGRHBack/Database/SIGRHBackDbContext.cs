using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SIGRHBack.Models;

namespace SIGRHBack.Database
{
    public class SIGRHBackDbContext : IdentityDbContext<AppUser>
    {
        public SIGRHBackDbContext(DbContextOptions options) : base(options)
        {
        }

        // définition des tables
        public DbSet<TokenInfo> TokenInfo { get; set; }
    }
}
