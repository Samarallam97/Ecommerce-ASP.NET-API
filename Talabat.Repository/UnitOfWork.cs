
using System.Collections;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.RepositoryInterfaces;
using Talabat.Repository.Data;
using Talabat.Repository.Repositories;

namespace Talabat.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly StoreContext _context;
		private Hashtable _repositories;


		public UnitOfWork(StoreContext context)
        {
			_context=context;
			_repositories = new Hashtable();
		}

		public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity 
		{
			var key = typeof(TEntity).Name;

			if(!_repositories.ContainsKey(key) )
			{
				var repository = new GenericRepository<TEntity>(_context) ;

				_repositories.Add(key, repository);
			}

			return _repositories[key] as IGenericRepository<TEntity>;
		}

		public async Task<int> CompleteAsync()
		{
			return await _context.SaveChangesAsync();
		}

		public async ValueTask DisposeAsync()
		{
			 await _context.DisposeAsync();
		}


	}
}
