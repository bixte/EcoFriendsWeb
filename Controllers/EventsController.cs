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
using System.Threading.Tasks;

namespace EcoFriendsWeb.Controllers
{
    public class EventsController : Controller
    {
        private readonly AppDataBaseContext DataBase;

        private readonly UserManager<User> UserManager;

        private readonly IWebHostEnvironment Environment;

        public EventsController(AppDataBaseContext db, UserManager<User> userManager, IWebHostEnvironment environment)
        {
            DataBase = db;
            UserManager = userManager;
            Environment = environment;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var events = DataBase.EventPosts;
            await events.Include(e => e.ImagesStore).LoadAsync();
            var model = new EventsViewModel()
            {
                EventPosts = events
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ViewEvent(int id)
        {
            var eventPost = await DataBase.EventPosts.FindAsync(id);
            if (eventPost != null)
            {
                await DataBase.Entry(eventPost).Collection(e => e.ImagesStore).LoadAsync();
                await DataBase.Entry(eventPost).Collection(e => e.Comments).LoadAsync();

                foreach (var comment in eventPost.Comments)
                    await DataBase.Entry(comment).Reference(c => c.User).LoadAsync();

                var model = new EventPostViewModel
                {
                    EventPost = eventPost
                };
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EventUserAdd(int idEvent, string userName)
        {
            var eventPost = await DataBase.EventPosts.FindAsync(idEvent);
            await DataBase.Entry(eventPost).Collection(e => e.Users).LoadAsync();
            var user = await UserManager.FindByNameAsync(userName);
            if (eventPost.Users.Contains(user))
            {
                eventPost.Users.Add(user);
                await DataBase.SaveChangesAsync();
            }
            return RedirectToAction("ViewEvent", new { id = idEvent });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEventPost(int id)
        {
            var eventPost = await DataBase.EventPosts.FirstOrDefaultAsync(e => e.Id == id);
            if (eventPost != null)
            {
                await DataBase.Entry(eventPost).Collection(e => e.Comments)?.LoadAsync();
                await DataBase.Entry(eventPost).Collection(e => e.Tags).LoadAsync();
                DataBase.EventPosts.Remove(eventPost);
                await DataBase.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> CreateComment(int idEvent, string userName, string userComment)
        {
            if (!string.IsNullOrWhiteSpace(userComment))
            {
                var user = await UserManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var eventPost = await DataBase.EventPosts.FirstOrDefaultAsync(e => e.Id == idEvent);
                    await DataBase.Entry(eventPost).Collection(ep => ep.Comments).LoadAsync();
                    if (eventPost != null)
                    {
                        eventPost.Comments.Add(new Comment()
                        {
                            DataCreate = DateTime.Now.ToString("g"),
                            Text = userComment,
                            User = user
                        });
                        await DataBase.SaveChangesAsync();
                        return RedirectToAction("ViewEvent", new { id = idEvent });
                    }
                }
            }
            ModelState.AddModelError("", "Нельзя добавить пустой комментарий");
            return RedirectToAction("ViewEvent", new { id = idEvent });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteComment(int idEvent, int idComment)
        {
            var eventPost = await DataBase.EventPosts.FirstOrDefaultAsync(e => e.Id == idEvent);
            await DataBase.Entry(eventPost).Collection(p => p.Comments).LoadAsync();
            if (eventPost != null)
            {
                eventPost.Comments.Remove(
                    eventPost.Comments.Find(c => c.Id == idComment));
                await DataBase.SaveChangesAsync();
                return RedirectToAction("ViewEvent", new { id = eventPost.Id });
            }

            ModelState.AddModelError("", "Ошибка");
            return RedirectToAction("ViewEvent", new { id = eventPost.Id });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateEvent() => View();

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateEvent(CreateEventViewModel model, IFormFileCollection mainSlider)
        {
            if (ModelState.IsValid)
            {
                model.ImagesStore = new List<ImagePicture>();
                foreach (var slide in mainSlider)
                {
                    using (var stream = System.IO.File.Create($"{Environment.WebRootPath}/img/events/{slide.FileName}"))
                    {
                        await slide.CopyToAsync(stream);
                    }
                    model.ImagesStore.Add(new ImagePicture
                    {
                        Name = slide.Name,
                        Path = $"/img/events/{slide.FileName}"
                    });
                }
                model.TimeCreate = DateTime.Now.ToString("f");
                var eventPost = new EventPost
                {
                    Title = model.Title,
                    ShortDescription = model.ShortDescription,
                    Desctiption = model.Desctiption,
                    ImagesStore = model.ImagesStore,
                    Location = model.Location,
                    ContactAdress = model.ContactAdress,
                    ContactPhone = model.ContactPhone,
                    Tags = model.Tags,
                    ContactEmail = model.ContactEmail,
                    EventData = model.EventData,
                    TimeCreate = model.TimeCreate,
                    Text = model.Text,
                    Text2 = model.Text2,
                };
                await DataBase.EventPosts.AddAsync(eventPost);
                await DataBase.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View();

        }
    }
}
