using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace TownHall
{
    // need this class to implement Repository design pattern
	public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly TownHallContext _context;

        public Repository(TownHallContext context)
        {
            _context = context;
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity); 
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity); 
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
