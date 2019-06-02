using System.Collections.Generic;

namespace ExpressionForEF
{
    public class PagingOptions
    {
        public List<OrderOptions> Orders { get; } = new List<OrderOptions>();
        public int CountPerPage { get; set; }
        public int PageNum { get; set; }
    }
}