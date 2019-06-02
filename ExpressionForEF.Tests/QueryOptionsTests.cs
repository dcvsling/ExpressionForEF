using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;
using Xunit;
#if NET
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Objects;
#elif NETSTANDARD
#endif


namespace ExpressionForEF.Tests
{

    public class QueryOptionsTests
    {

        [Fact]
        public void Test1()
        {
            var context = new TestDbContext();

            var queryvalues = TestData.QueryValues;
            var str = JsonConvert.SerializeObject(queryvalues);
            var options = OptionsHelper.CreateOptions<TestMaster>(queryvalues);

            var query = context.TestMasters.ConfigureQuery(options).ToSql();

            var q2 = context.TestMasters
                .Where(t => t.No >= 1 && t.No <= 3)
                .Where(t => t.Name == "A" || t.Name == "B" || t.Name == "D")
                .Where(t => t.ModityDate != DateTimeOffset.MaxValue)
                .Where(t => t.Id != Guid.Empty)
                .OrderByDescending(t => t.No)
                .ThenBy(t => t.ModityDate)
                .Skip(3)
                .Take(3)
                .ToSql();


            Assert.Equal(q2, query);

        }
    }
    public static class IQueryableExtensions
    {
        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(t => t.Name == "_queryCompiler");

        private static readonly FieldInfo QueryModelGeneratorField = QueryCompilerTypeInfo.DeclaredFields.First(t => t.Name == "_queryModelGenerator");

        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(t => t.Name == "_database");

        private static readonly PropertyInfo DatabaseDependenciesField = typeof(Database).GetTypeInfo().DeclaredProperties.Single(t => t.Name == "Dependencies");

        public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            var queryCompiler = (QueryCompiler)QueryCompilerField.GetValue(query.Provider);
            var modelGenerator = (QueryModelGenerator)QueryModelGeneratorField.GetValue(queryCompiler);
            var queryModel = modelGenerator.ParseQuery(query.Expression);
            var database = (IDatabase)DataBaseField.GetValue(queryCompiler);
            var databaseDependencies = (DatabaseDependencies)DatabaseDependenciesField.GetValue(database);
            var queryCompilationContext = databaseDependencies.QueryCompilationContextFactory.Create(false);
            var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
            return modelVisitor.Queries.First().ToString();
        }
    }
}
