using homework_59.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homework_59.ViewModel
{
    public class CommentLikeViewModel
    {
        public ImageModel ImageModel { get; set; }
        public List<ImageModel> imageModels { get; set; }
        public Comment Comment { get; set; }
        public Like Like { get; set; }
    }
}
