using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;

namespace Shopping.Data
{
    public class DBContext : DbContext
    {
        public DBContext (DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public DbSet<Shopping.Models.Member> Member { get; set; } = default!;
        public DbSet<Shopping.Models.Product> Product { get; set; } = default!;
        public DbSet<Shopping.Models.ProductCategory> ProductCategory { get; set; } = default!;
        public DbSet<Shopping.Models.Order> Order { get; set; } = default!;
        public DbSet<Shopping.Models.OrderDetail> OrderDetail { get; set; } = default!;
        public DbSet<Shopping.Models.Banner> Banners { get; set; } = default!;
        public DbSet<Shopping.Models.FAQ> Faqs { get; set; } = default!;

    }
}
