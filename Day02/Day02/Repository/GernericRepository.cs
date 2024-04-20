using Day02.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Day02.Repository
{
    public class GernericRepository<T> where T : class
    {
        private readonly ITIContext context;

        public GernericRepository(ITIContext context)
        {
            this.context = context;
        }

        public List<T> GetAll(string[] includes = null)
        {
            if(includes == null)
                return context.Set<T>().ToList();

            var q = context.Set<T>().AsQueryable();

            foreach (var item in includes)
            {
                q = q.Include(item);
            }

            return q.ToList() ;
        }

        public T GetById(int id)
        {
            return context.Set<T>().Find(id);
        }

        public List<T> GetByName(Func<T,bool> filter)
        {
            return context.Set<T>().Where(filter).ToList();
        }

        public void add(T obj)
        {
            context.Set<T>().Add(obj);
        }

        public void Update(T obj)
        {
            context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void delete(int id)
        {
            T obj = context.Set<T>().Find(id);
            context.Set<T>().Remove(obj);
        }

        public void save()
        {
            context.SaveChanges();
        }
    }
}
