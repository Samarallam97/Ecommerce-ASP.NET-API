using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
	public interface ISpecification<T> where T : BaseEntity
	{
        public Expression<Func<T ,bool >> Criteria { get; set; } // p => P.Id == id
		public List<Expression<Func<T, object>>> Includes { get; set; } // p => p.Brands 
		public Expression<Func<T, object>> OrderBy { get; set; } // p => p.name
		public Expression<Func<T, object>> OrderByDesc { get; set; } // p => p.name
		public int? Skip { get; set; }
		public int? Take { get; set; }





	}
}
