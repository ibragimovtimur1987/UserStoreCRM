using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.EF;
using UserStore.DAL.Entities;
using UserStore.Data.Interfaces;

namespace UserStore.DAL.Repositories
{
    public class RequestRepository : IRepository<Request,int>
    {
        private ApplicationContext db;

        public RequestRepository(ApplicationContext context)
        {
            this.db = context;
        }

        public IQueryable<Request> GetAll()
        {
            return db.Requests.AsNoTracking().Include("Author").AsQueryable();
        }

        public Request Get(int id)
        {
            return db.Requests.Include("Author").FirstOrDefault(x=>x.Id == id);
        }

        public void Create(Request Request)
        {
            db.Requests.Add(Request);
        }

        public void Update(Request Request)
        {
            db.Requests.Attach(Request);
            db.Entry(Request).Property(x=>x.Scanned).IsModified = true;
        }

        public IQueryable<Request> Find(Func<Request, Boolean> predicate)
        {
            return db.Requests.AsNoTracking().Where(predicate).AsQueryable();
        }
        public Request FindLastOrDefault(Func<Request, Boolean> predicate)
        {
            return db.Requests.AsNoTracking().Include("Author").LastOrDefault(predicate);
        }
        public void Delete(int id)
        {
            Request Request = db.Requests.Find(id);
            if (Request != null)
                db.Requests.Remove(Request);
        }
    }
}
