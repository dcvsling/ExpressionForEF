using Microsoft.EntityFrameworkCore;
// #if NET
// using System.Data.Entity;
// using System.Data.Entity.Core.Objects;
// using System.Data.Objects;
// #elif NETCORE
// using Microsoft.EntityFrameworkCore;
// #endif


namespace ExpressionForEF.Tests
{
    public class TestDbContext : DbContext
    {
        public TestDbContext()
            : base(new DbContextOptionsBuilder()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableDetailedErrors(true)
                .EnableSensitiveDataLogging(true)
                .UseSqlServer(@"Data Source=(localdb)\ProjectsV13;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
                .Options)
        {

        }
        public virtual DbSet<TestMaster> TestMasters { get; set; }
        public virtual DbSet<TestDetail> TestDetails { get; set; }
    }
}
