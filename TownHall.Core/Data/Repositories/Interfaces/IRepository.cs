using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TownHall.Core
{
	public interface IRepository<T> where T : class
	{
		T GetById(int id);
		IEnumerable<T> GetAll();
		void Add(T entity);
		void Update(T entity);
		void Remove(T entity);
		void SaveChanges();
	}
}
