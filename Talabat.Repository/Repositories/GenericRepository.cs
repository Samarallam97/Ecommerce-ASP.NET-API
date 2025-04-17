using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.RepositoryInterfaces;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly StoreContext _context;

		public GenericRepository(StoreContext context)
        {
			_context = context;
		}

        public IReadOnlyList<T> GetAll()
		{
			return _context.Set<T>().ToList();
		}

		public T? GetById(int id)
		{
			return _context.Set<T>().Find(id);
		}


		public IReadOnlyList<T> GetAllWithSpec(ISpecification<T> spec)
		{
			return GetQuery(spec).ToList();
		}

		public T? GetEntityWithSpec(ISpecification<T> spec)
		{
			return GetQuery(spec).FirstOrDefault();
		}

		//////////////////////////////////////////////////////////

		public void Add(T entity)
		{
			 _context.Set<T>().Add(entity);
		}

		public void Update(T entity)
		{
			_context.Update(entity);
		}

		public void Delete(T entity)
		{
			 _context.Remove(entity);
		}

		/////////////////////////////////////////////////////////
		private IQueryable<T> GetQuery(ISpecification<T> spec)
		{
			IQueryable<T> query = _context.Set<T>();

			if (spec.Criteria is not null)
				query = query.Where(spec.Criteria);


			if(spec.OrderBy is not null)
				query = query.OrderBy(spec.OrderBy);
			else if(spec.OrderByDesc is not null) 
				query = query.OrderByDescending(spec.OrderByDesc);

			if (spec.Skip is not null)
				query = query.Skip(spec.Skip.Value);
			if (spec.Take is not null)
				query = query.Take(spec.Take.Value);

			query = spec.Includes.Aggregate(query, (oldQuery, includeStat) => oldQuery.Include(includeStat));
			
			return query;
		}

	}
}
