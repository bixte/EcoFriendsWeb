using EcoFriendsWeb.DataModels;
using EcoFriendsWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EcoFriendsWeb.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDataBaseContext DataBase;
        private readonly IWebHostEnvironment Environment;
        public UserManager<User> UserManager { get; }

        public BlogController(AppDataBaseContext db, UserManager<User> userManager, IWebHostEnvironment webHostEnvironment)
        {
            DataBase = db;
            UserManager = userManager;
            Environment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new BlogViewModels()
            {
                Posts = await DataBase.BlogPosts
                .Include(p => p.MainImagesStore)
                .Include(p => p.Tags)
                .ToListAsync()
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ViewPost(int id)
        {
            var post = await DataBase.BlogPosts?.FirstOrDefaultAsync(p => p.Id == id);
            if (post != null)
            {
                await DataBase.Entry(post)
                        .Collection(p => p.Tags)?.LoadAsync();

                await DataBase.Entry(post)
                    .Collection(p => p.MainImagesStore)?.LoadAsync();

                await DataBase.Entry(post)
                    .Collection(p => p.PostParticals)?.LoadAsync();

                await DataBase.Entry(post)
                    .Collection(p => p.Comments).LoadAsync();

                foreach (var comment in post.Comments)
                    await DataBase.Entry(comment).Reference(c => c.User).LoadAsync();


                if (post.PostParticals != null)
                {
                    foreach (var postPartical in post.PostParticals)
                    {
                        await DataBase.Entry(postPartical)?.Collection(pp => pp.SubTexts)?.LoadAsync();
                        await DataBase.Entry(postPartical)?.Collection(pp => pp.ImagesStore)?.LoadAsync();
                    }
                }
                var model = new BlogPostViewModel() { Post = post };
                return View(model);
            }
            else
                return NotFound();

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreatePost(string particalCount = null)
        {
            if (particalCount !=null && int.TryParse(particalCount, out int result))           
                ViewData.Add("particalCount", result);
            
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreatePost(List<IFormFile> mainSlider, List<List<IFormFile>> particalsImages, CreatePostViewModel model)
        {
            string imagesPath = $"{Environment.WebRootPath}/img/blog";

            var mainImagesStore = new List<ImagePicture>();
            foreach (var file in mainSlider)
            {
                var filePath = $"{imagesPath}/{file.FileName}";
                using var stream = System.IO.File.Create($"{filePath}");
                await file.CopyToAsync(stream);
                mainImagesStore.Add(new ImagePicture()
                {
                    Name = file.Name,
                    Path = $"/img/blog/{file.FileName}"
                });
            }

            var postParticals = new List<PostPartical>();
            if (model.PostParticals.Count > 0)
            {
                for (int i = 0; i < model.PostParticals.Count; i++)
                {
                    var partical = new PostPartical
                    {
                        Title = model.PostParticals[i].Title ?? string.Empty,
                        SubTexts = model.PostParticals[i].SubTexts
                    };
                    if (particalsImages[i] != null)
                    {
                        partical.ImagesStore = new List<ImagePicture>();

                        foreach (var file in particalsImages[i])
                        {
                            var filePath = $"{imagesPath}/{file.FileName}";
                            using var stream = System.IO.File.Create($"{filePath}");
                            await file.CopyToAsync(stream);
                            partical.ImagesStore.Add(new ImagePicture()
                            {
                                Name = file.Name,
                                Path = $"/img/blog/{file.FileName}"
                            });
                        }
                    }
                    postParticals.Add(partical);
                }
            }

            var post = new BlogPost();
            if (model.Title != null)
                post.Title = model.Title;
            if (model.Tags != null)
                post.Tags = model.Tags;
            if (model.ShortDesctiption != null)
                post.ShortDesctiption = model.Desctiption;
            if (model.Desctiption != null)
                post.Desctiption = model.Desctiption;

            post.MainImagesStore = mainImagesStore;
            post.PostParticals = postParticals;
            post.TimeCreate = DateTime.Now.ToString("f");
            post.Comments = new List<Comment>();

            await DataBase.BlogPosts.AddAsync(post);
            await DataBase.SaveChangesAsync();
            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await DataBase.BlogPosts.FirstOrDefaultAsync(p => p.Id == id);
            if (post != null)
            {
                await DataBase.Entry(post).Collection(p => p.PostParticals).LoadAsync();
                await DataBase.Entry(post).Collection(p => p.Tags).LoadAsync();
                await DataBase.Entry(post).Collection(p => p.MainImagesStore).LoadAsync();
                foreach (var postPartical in post.PostParticals)
                {
                    await DataBase.Entry(postPartical)?.Collection(pp => pp.ImagesStore).LoadAsync();
                    await DataBase.Entry(postPartical)?.Collection(pp => pp.SubTexts)?.LoadAsync();
                }
                DataBase.BlogPosts.Remove(post);
                await DataBase.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment(int idPost, string userName, string userComment)
        {
            if (!string.IsNullOrWhiteSpace(userComment))
            {
                var user = await UserManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var blogPost = await DataBase.BlogPosts.FirstOrDefaultAsync(e => e.Id == idPost);
                    await DataBase.Entry(blogPost).Collection(ep => ep.Comments).LoadAsync();
                    if (blogPost != null)
                    {
                        blogPost.Comments.Add(new Comment()
                        {
                            DataCreate = DateTime.Now.ToString("g"),
                            Text = userComment,
                            User = user
                        });
                        await DataBase.SaveChangesAsync();
                        return RedirectToAction("ViewPost", new { id = idPost });
                    }
                }
            }
            ModelState.AddModelError("", "Нельзя добавить пустой комментарий");
            return RedirectToAction("ViewPost", new { id = idPost });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteComment(int idPost, int idComment)
        {
            var blogPost = await DataBase.BlogPosts.FirstOrDefaultAsync(e => e.Id == idPost);
            await DataBase.Entry(blogPost).Collection(p => p.Comments).LoadAsync();
            if (blogPost != null)
            {
                blogPost.Comments.Remove(
                    blogPost.Comments.FirstOrDefault(c => c.Id == idComment));
                await DataBase.SaveChangesAsync();
                return RedirectToAction("ViewPost", new { id = blogPost.Id });
            }

            ModelState.AddModelError("", "Ошибка");
            return RedirectToAction("ViewPost", new { id = blogPost.Id });
        }
    }
}

