using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using RigoFunc.DAL.Extensions;

namespace RigoFunc.Api.Models {
    public class Blog {
        public int Id { get; set; }
        public string Url { get; set; }
        public List<Post> Posts { get; set; }
    }

    public class Post {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
    }

    public class BloggingContext : DbContext {
        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options) { }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.EnableAutoHistory();
        }
    }
}
