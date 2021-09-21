using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homework_59.Models
{
    public class UserObj : IdentityUser
    {
        public string Login { get; set; }
        public string Avatar { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string InfoUser { get; set; }
        public string Gender { get; set; }
        public int FollowerCount { get; set; }
        public int FollowCount { get; set; }
        public List<ImageModel> ImageModels { get; set; }
        public List<UserObj> Followers { get; set; }
        public List<UserObj> Follows { get; set; }
    }
}
