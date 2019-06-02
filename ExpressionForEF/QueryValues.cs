using System.Collections.Generic;

namespace ExpressionForEF
{
    public class QueryValues : Dictionary<string, Range>
    {

        public PagingOptions Paging { get; set; }
    }
}