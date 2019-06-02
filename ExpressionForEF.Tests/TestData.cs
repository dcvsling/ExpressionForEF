using System;
#if NET
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Objects;
#elif NETSTANDARD
#endif


namespace ExpressionForEF.Tests
{
    public static class TestData
    {
        public static QueryValues QueryValues
           => new QueryValues
           {
               [nameof(TestMaster.No)] = (1d, 3d),
               [nameof(TestMaster.Name)] = new string[] { "A", "B", "D" },
               [nameof(TestMaster.ModityDate)] = (DateTimeOffset.MaxValue, default(DateTimeOffset?)),
               [nameof(TestMaster.Id)] = (Guid.Empty, default),
               Paging = new PagingOptions
               {
                   CountPerPage = 3,
                   PageNum = 2,
                   Orders = {
                        new OrderOptions
                        {
                            OrderColumn = nameof(TestMaster.No),
                            IsDescending = true
                        },
                        new OrderOptions
                        {
                            OrderColumn = nameof(TestMaster.ModityDate),
                        }
                    }
               }
           };
    }
}
