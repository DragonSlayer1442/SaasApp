using DocSaaSApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DocSaaSApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserFile> UserFiles { get; set; }
    }
}
