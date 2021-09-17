using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homework_59.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int LikeBody { get; set; }
        public int ImageModelId { get; set; }
        public ImageModel ImageModel { get; set; }
        public string UserObjId { get; set; }
        public UserObj UserObj { get; set; }
    }
}
