using Aspire.Hosting.ApplicationModel;
using HealthChecks.SqlServer;

namespace AIFunctionCallingRAG.Hosting
{
    public static class SqlResourceHealthCheckExtensions
    {
        public static IResourceBuilder<SqlServerServerResource> WithHealthCheck(this IResourceBuilder<SqlServerServerResource> builder)
        {
            return builder.WithSqlHealthCheck(cs => new SqlServerHealthCheckOptions { ConnectionString = cs });
        }

        private static IResourceBuilder<T> WithSqlHealthCheck<T>(this IResourceBuilder<T> builder, Func<string, SqlServerHealthCheckOptions> healthCheckOptionsFactory)
            where T : IResource
        {
            return builder.WithAnnotation(HealthCheckAnnotation.Create(cs => new SqlServerHealthCheck(healthCheckOptionsFactory(cs))));
        }
    }
}
