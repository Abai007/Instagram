using homework_59.Models;
using homework_59.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace homework_59.Controllers
{
    public class InstaController : Controller
    {
        private readonly UserManager<UserObj> _userManager;

        private readonly SignInManager<UserObj> _signInManager;

        private IWebHostEnvironment _appEnvironment;

        private InstaContext _db;
        public InstaController(UserManager<UserObj> userManager, SignInManager<UserObj> signInManager, InstaContext db, IWebHostEnvironment appEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
            _appEnvironment = appEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult IndexFilter(string query)
        {
            var users = _db.InstaUsers.ToList();
            if (query != null)
            {
                users.AddRange(users.Where(
                    p => p.Name.Contains(query) ||
                    p.Email.Contains(query) ||
                    p.InfoUser.Contains(query) ||
                    p.Login.Contains(query)).ToList());
            }
            return View(users);
        }
        private ImageModel ReadIForm(IFormFile ImgFile)
        {
            string path = "/Avatars/" + ImgFile.FileName;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                ImgFile.CopyTo(fileStream);
            }
            ImageModel file = new ImageModel { Name = ImgFile.FileName, Path = path, };
            return file;
        }
        private async Task<UserObj> CurrentUser()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }
        public IActionResult AddImg()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddImg(ImageModel img, IFormFile ImgFile)
        {
            ImageModel file = ReadIForm(ImgFile);
            file.Description = img.Description;
            UserObj userObj = CurrentUser().Result;
            file.UserObjId = userObj.Id;
            file.UserObj = userObj;
            _db.ImageModels.Add(file);
            _db.SaveChanges();
            return RedirectToAction("Profile", "Insta");
        }
        public IActionResult Profile(string Id)
        {
            if (Id == null)
            {
                UserObj user = CurrentUser().Result;
                user.ImageModels = _db.ImageModels.Where(i => i.UserObjId == user.Id).ToList();
                return View(user);
            }
            else
            {
                UserObj userObj = _db.InstaUsers.FirstOrDefault(u => u.Id == Id);
                userObj.ImageModels = _db.ImageModels.Where(i => i.UserObjId == userObj.Id).ToList();
                return View(userObj);
            }

        }
        public IActionResult InfoPost(int Id)
        {
            CommentLikeViewModel commentLike = new CommentLikeViewModel();
            ImageModel image = _db.ImageModels.FirstOrDefault(i => i.Id == Id);
            image.UserObj = _db.InstaUsers.FirstOrDefault(u => u.Id == image.UserObjId);
            image.Comments = _db.Comments.Where(c => c.ImageModelId == image.Id).ToList();
            image.Likes = _db.Likes.Where(l => l.ImageModelId == image.Id).ToList();
            commentLike.ImageModel = image;
            return View(commentLike);
        }
        [HttpPost]
        public IActionResult InfoPost(CommentLikeViewModel imageModel)
        {
            ImageModel image = new ImageModel();
            if (imageModel.Comment != null)
                image = _db.ImageModels.FirstOrDefault(i => i.Id == imageModel.Comment.ImageModelId);
            if (imageModel.Like != null)
                image = _db.ImageModels.FirstOrDefault(i => i.Id == imageModel.Like.ImageModelId);
            UserObj user = CurrentUser().Result;
            var likes = _db.Likes.Where(l => l.UserObjId == user.Id && l.ImageModelId == image.Id).ToList();
            if (imageModel.Like != null)
            {
                if (likes.Count < 1)
                {
                    imageModel.Like.UserObjId = user.Id;
                    imageModel.Like.ImageModelId = image.Id;
                    imageModel.Like.LikeBody = 1;
                    _db.Likes.Add(imageModel.Like);
                    _db.SaveChanges();
                }
            }
            if (imageModel.Comment != null)
            {
                imageModel.Comment.UserObjId = user.Id;
                imageModel.Comment.UserObj = user;
                imageModel.Comment.ImageModelId = image.Id;
                imageModel.Comment.CreatorName = CurrentUser().Result.Login;
                if (imageModel.Comment.TextBody != null)
                    _db.Comments.Add(imageModel.Comment);
                _db.SaveChanges();
            }

            return RedirectToAction("InfoPost");
        }
    }
}
