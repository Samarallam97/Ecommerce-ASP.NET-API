using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
	public class Specification<T> : ISpecification<T> where T : BaseEntity
	{
		public Expression<Func<T, bool>> Criteria { get; set; }
		public List<Expression<Func<T, object>>> Includes { get; set ; } = new ();
		public Expression<Func<T, object>> OrderBy { get ; set ; }
		public Expression<Func<T, object>> OrderByDesc { get ; set ; }
		public int? Skip { get ; set ; }
		public int? Take { get ; set ; }

		public Specification()
        {
            
        }
		public Specification(Expression<Func<T, bool>> criteria)
		{
			Criteria = criteria;
		}
	}
}
