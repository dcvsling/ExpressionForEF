using System;
using System.Collections.Generic;
#if NET
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Objects;
#elif NETSTANDARD
using Microsoft.EntityFrameworkCore;
#endif


namespace ExpressionForEF.Tests
{
    public class TestMaster
    {
        public Guid Id { get; set; }
        public int No { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTimeOffset ModityDate { get; set; }
        public List<TestDetail> Defails { get; set; }
    }
}
