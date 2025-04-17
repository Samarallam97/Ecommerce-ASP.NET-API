using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.RepositoryInterfaces
{
	public interface IGenericRepository<T> where T : BaseEntity
	{
		IReadOnlyList<T> GetAll();
		T? GetById(int id);

		IReadOnlyList<T> GetAllWithSpec(ISpecification<T> spec);
		T? GetEntityWithSpec(ISpecification<T> spec);

		void Add(T entity);
		void Update(T entity);
		void Delete(T entity);
	}
}
