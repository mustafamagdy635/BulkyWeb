using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BulkyBook.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext db;
        internal DbSet<T> DbSet;
        public Repository(ApplicationDbContext db)
        {
            this.db = db;
            this.DbSet = db.Set<T>();
            db.Products.Include(u => u.category).Include(d => d.CategoryId);

        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null ,bool tracked =false)
        {
            IQueryable<T> Query;
            if (tracked)
            {
                Query = DbSet;
            }
            else
            {
               Query = DbSet.AsNoTracking();
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    Query = Query.Include(prop);
                }
            }
            Query = Query.Where(filter);
            return Query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter,string ? includeProperties =null)
        {
            IQueryable<T> Query = DbSet;
            if (filter != null)
            {
                Query = Query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var prop in includeProperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    Query = Query.Include(prop);
                }
            }
            return Query.ToList();

        }

        public void Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        public void RemoveRnge(IEnumerable<T> entity)
        {
            DbSet.RemoveRange(entity);
        }
    }
}
