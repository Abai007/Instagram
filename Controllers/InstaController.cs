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
            List<IntermediateTable> follows = _db.IntermediateTables.Where(f => f.UserFollowerId == CurrentUser().Result.Id).ToList();
            List<ImageModel> images = new List<ImageModel>();
            CommentLikeViewModel commentLike = new CommentLikeViewModel();
            
            if (follows.Count != 0)
            {
                foreach (var follow in follows)
                {
                    UserObj u = _db.InstaUsers.FirstOrDefault(u => u.Id == follow.UserFollowId);
                    commentLike.imageModels = _db.ImageModels.Where(i => i.UserObjId == u.Id).ToList();
                    if(commentLike.imageModels.Count != 0)
                    {
                        foreach(var im in commentLike.imageModels)
                        {
                            im.Likes = _db.Likes.Where(l => l.ImageModelId == im.Id).ToList();
                            im.Comments = _db.Comments.Where(l => l.ImageModelId == im.Id).ToList();
                            images.Add(im);
                        }
                    }
                }
            }
            List<ImageModel> imageModels = (from model in images
                      orderby model.CreateDate
                      select model).ToList();
            
            return View(imageModels);
        }
        public IActionResult IndexFilter(string query)
        {
            ;
            if (query != null)
            {
                var users = _db.InstaUsers.Where(
                    p => p.Name.Contains(query) ||
                    p.Email.Contains(query) ||
                    p.InfoUser.Contains(query) ||
                    p.Login.Contains(query)).ToList();
                return View(users);
            }
            else
            {
                return View(new List<UserObj>());
            }
            
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
            file.CreateDate = DateTime.Now;
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
                List<IntermediateTable> follows = _db.IntermediateTables.Where(f => f.UserFollowerId == CurrentUser().Result.Id).ToList();
                List<IntermediateTable> followers = _db.IntermediateTables.Where(f => f.UserFollowId == CurrentUser().Result.Id).ToList();
                UserObj user = CurrentUser().Result;
                user.Follows = new List<UserObj>();
                user.Followers = new List<UserObj>();
                user.ImageModels = _db.ImageModels.Where(i => i.UserObjId == user.Id).ToList();
                if (follows.Count != 0)
                {
                    foreach (var follow in follows)
                    {
                        UserObj u = _db.InstaUsers.FirstOrDefault(u => u.Id == follow.UserFollowId);
                        user.Follows.Add(u);
                    }
                    user.FollowCount = user.Follows.Count;
                }
                if(followers.Count != 0)
                {
                    foreach (var follower in followers)
                    {
                        UserObj u = _db.InstaUsers.FirstOrDefault(u => u.Id == follower.UserFollowId);
                        user.Followers.Add(u);
                    }
                    user.FollowerCount = user.Followers.Count;
                }
                return View(user);
            }
            else
            {
                UserObj userObj = _db.InstaUsers.FirstOrDefault(u => u.Id == Id);
                userObj.Follows = new List<UserObj>();
                userObj.Followers = new List<UserObj>();
                userObj.ImageModels = _db.ImageModels.Where(i => i.UserObjId == userObj.Id).ToList();
                List<IntermediateTable> follows = _db.IntermediateTables.Where(f => f.UserFollowerId == Id).ToList();
                List<IntermediateTable> followers = _db.IntermediateTables.Where(f => f.UserFollowId == Id).ToList();
                if (follows.Count != 0)
                {
                    foreach (var follow in follows)
                    {
                        UserObj u = _db.InstaUsers.FirstOrDefault(u => u.Id == follow.UserFollowId);
                        userObj.Follows.Add(u);
                    }
                    userObj.FollowCount = userObj.Follows.Count;
                }
                if (followers.Count != 0)
                {
                    foreach (var follower in followers)
                    {
                        UserObj u = _db.InstaUsers.FirstOrDefault(u => u.Id == follower.UserFollowId);
                        userObj.Followers.Add(u);
                    }
                    userObj.FollowerCount = userObj.Followers.Count;
                }
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
        public IActionResult Following(string Id)
        {
            if(Id != CurrentUser().Result.Id)
            {
                if(_db.IntermediateTables.FirstOrDefault(i => i.UserFollowerId == CurrentUser().Result.Id && i.UserFollowId == Id) == null)
                {
                    IntermediateTable intermediateTable = new IntermediateTable();
                    intermediateTable.UserFollowId = Id;
                    intermediateTable.UserFollowerId = CurrentUser().Result.Id;
                    _db.IntermediateTables.Add(intermediateTable);
                    _db.SaveChanges();
                }
                else
                {
                    IntermediateTable intermediateTable = _db.IntermediateTables.FirstOrDefault
                        (i => i.UserFollowerId == CurrentUser().Result.Id &&
                        i.UserFollowId == Id);
                    _db.IntermediateTables.Remove(intermediateTable);
                    _db.SaveChanges();
                }
            }
            return RedirectToAction("Profile" , "Insta", new { Id = Id});
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
                else
                {
                    Like like = _db.Likes.FirstOrDefault(i => i.UserObjId == user.Id && i.ImageModelId == image.Id);
                    imageModel.Like.LikeBody = 0;
                    _db.Likes.Remove(like);
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

            return RedirectToAction("InfoPost", "Insta");
        }
    }
}