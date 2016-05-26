# RigoFunc.DAL
This repo contains the data access layer library that inner implementation use repository and unit of work pattern.
The *highlight* feature is *auto recording data change history*

# How to use

## Install RigoFunc.DAL
To install *RigoFunc.DAL*, run the following command in the Package Manager Console
```
PM> Install-Package RigoFunc.DAL
```

## Enable auto history functionality

```C#
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
```

To enable auto history functionality, need to two steps

1. ```using RigoFunc.DAL.Extensions;``` in your DbContext.
2. Override the *OnModelCreating* method, as following:
```C#
protected override void OnModelCreating(ModelBuilder modelBuilder) {
    base.OnModelCreating(modelBuilder);

    modelBuilder.EnableAutoHistory();
}
```

## Configure Asp.Net Core services

To use *unit of work* provided by *RigoFunc.DAL* library, coding as

```C#
public void ConfigureServices(IServiceCollection services) {
    services.AddDbContext<BloggingContext>(options => options.UseSqlServer(Configuration["Data:Default:ConnectionString"]));
    // why this? 
    services.AddScoped<DbContext, BloggingContext>();

    // Add unit of work
    services.AddScoped<IUnitOfWork, UnitOfWork>();

    services.AddMvc();
}
```

## Configure connectionstring using appsettings.json
```Json
{
  "Data": {
    "Default": {
      "ConnectionString": "Server=(localdb)\\mssqllocaldb;Database=rigofunc;Trusted_Connection=True;"
    }
  },
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  }
}
```

## Impls the API

```C#
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using RigoFunc.DAL;
using RigoFunc.Api.Models;

namespace RigoFunc.Api.Controllers {
    [Route("api/[controller]")]
    public class BlogsController : Controller {
        private readonly IUnitOfWork _unitOfWork;

        public BlogsController(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IEnumerable<Blog> Get() {
            var context = _unitOfWork.Context<BloggingContext>();

            return context.Blogs.ToArray();
        }
        [HttpPost]
        public async Task Post([FromBody]Blog blog) {
            var repo = _unitOfWork.Repository<Blog>();

            await repo.InsertAsync(blog);

            await _unitOfWork.SaveChangesAsync(true);
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]Blog blog) {
            var repo = _unitOfWork.Repository<Blog>();

            var willUpdate = repo.Where(b => b.Id == id).Include(b => b.Posts).FirstOrDefault();

            willUpdate.Url = blog.Url;

            foreach (var item in blog.Posts) {
                willUpdate.Posts.Add(item);
            }

            await _unitOfWork.SaveChangesAsync(true);
        }
    }
}
```