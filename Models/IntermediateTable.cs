using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homework_59.Models
{
    public class IntermediateTable
    {
        public int Id { get; set; }
        public string UserFollowerId { get; set; }
        public UserObj UserObjFollower { get; set; }
        public string UserFollowId { get; set; }
        public UserObj UserObjFollow { get; set; }
    }
}
