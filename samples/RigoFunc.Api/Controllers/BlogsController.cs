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
