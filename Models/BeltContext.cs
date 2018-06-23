using Microsoft.EntityFrameworkCore;

namespace Belt.Models
{
    public class BeltContext : DbContext
    {
        public BeltContext(DbContextOptions<BeltContext> options) : base(options){}

        public DbSet<User_Reg> users {get;set;}
        public DbSet<Activities> activities {get;set;}
        public DbSet<Attendee> attendess {get;set;}

    }
}