using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homework_59.Models
{
    public class InstaContext : IdentityDbContext<UserObj>
    {
        public DbSet<UserObj> InstaUsers { get; set; }
        public DbSet<ImageModel> ImageModels { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<IntermediateTable> IntermediateTables { get; set; }
        public InstaContext(DbContextOptions<InstaContext> options) : base(options)
        {
        }
    }
}
