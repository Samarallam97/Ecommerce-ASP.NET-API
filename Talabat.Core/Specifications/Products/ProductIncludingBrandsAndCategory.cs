using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;


namespace Talabat.Core.Specifications.Products
{
    public class ProductIncludingBrandsAndCategory : Specification<Product>
    {
        public ProductIncludingBrandsAndCategory(ProductParams productParams)
            : base(p =>
                 (string.IsNullOrEmpty(productParams.Search) || p.Name.ToLower().Contains(productParams.Search)) &&
                 (!productParams.BrandId.HasValue || p.BrandId == productParams.BrandId) &&
                 (!productParams.CategoryId.HasValue || p.CategoryId == productParams.CategoryId)
                 )
        {
            AddIncludes();
            ApplySorting(productParams.Sort);
            ApplyPagination(productParams.PageSize, productParams.PageIndex);

        }

        public ProductIncludingBrandsAndCategory(Expression<Func<Product, bool>> criteria) : base(criteria)
        {
            AddIncludes();
        }

        ////////////////////////////////////////////////////
        private void AddIncludes()
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Category);
        }
        private void ApplySorting(string? sort)
        {
            if (sort is not null)
            {
                switch (sort)
                {
                    case "name":
                        OrderBy = p => p.Name;
                        break;
                    case "priceAsc":
                        OrderBy = p => p.Price;
                        break;
                    case "priceDesc":
                        OrderByDesc = p => p.Price;
                        break;
                    default:
                        OrderBy = p => p.Name;
                        break;
                }
            }
            else
            {
                OrderBy = p => p.Name;
            }
        }

        private void ApplyPagination(int? pageSize, int? pageIndex)
        {
            Skip = (pageIndex - 1) * pageSize;
            Take = pageSize;
        }

    }
}
