using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace homework_59.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        [Required]
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string UserObjId { get; set; }
        public UserObj UserObj { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Like> Likes { get; set; }
    }
}
