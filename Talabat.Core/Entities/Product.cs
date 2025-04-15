using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
	public class Product : BaseEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public string PictureUrl { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
		public int BrandId { get; set; }
		public virtual Brand Brand { get; set; }

    }
}
