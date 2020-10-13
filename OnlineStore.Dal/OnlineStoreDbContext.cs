using Microsoft.EntityFrameworkCore;
using OnlineStore.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Dal
{
    public class OnlineStoreDbContext : DbContext
    {
        public OnlineStoreDbContext(DbContextOptions<OnlineStoreDbContext> options)
            : base(options)
        {
        }
        public DbSet<File> Files { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
