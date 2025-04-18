namespace Talabat.Core.Specifications.Products
{
    public class ProductParams
    {
        public string? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }

        private string? search;
        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }


        private const int MAX_PAGE_SIZE = 5;

        private int pageSize = MAX_PAGE_SIZE;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value; }
        }
        public int PageIndex { get; set; } = 1;

    }
}
