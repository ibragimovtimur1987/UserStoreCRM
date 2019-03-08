using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using UserStore.DAL.Entities;

namespace UserStore.DAL.EF
{
    public class ApplicationContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(string conectionString) : base(conectionString) { }
        static ApplicationContext()
        {
            Database.SetInitializer<ApplicationContext>(new StoreDbInitializer());
        }
        public DbSet<Request> Requests { get; set; }
        public class StoreDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationContext>
        {
            protected override void Seed(ApplicationContext db)
            {
            }
        }
    }
}
