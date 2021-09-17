using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homework_59.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string TextBody { get; set; }
        public string CreatorName { get; set; }
        public int ImageModelId { get; set; }
        public ImageModel ImageModel { get; set; }
        public string UserObjId { get; set; }
        public UserObj UserObj { get; set; }
    }
}
