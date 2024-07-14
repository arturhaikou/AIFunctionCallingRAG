using Aspire.Hosting.ApplicationModel;
using HealthChecks.Uris;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIFunctionCallingRAG.Hosting
{
    public static class HttpEndpointHealthCheckExtensions
    {
        public static IResourceBuilder<T> WithHealthCheck<T>(
            this IResourceBuilder<T> builder,
            string? endpointName = null,
            string path = "health",
            Action<UriHealthCheckOptions>? configure = null)
            where T : IResourceWithEndpoints
        {
            return builder.WithAnnotation(new HealthCheckAnnotation(async (resource, ct) =>
            {
                if (resource is not IResourceWithEndpoints resourceWithEndpoints)
                {
                    return null;
                }

                var endpoint = endpointName is null
                 ? resourceWithEndpoints.GetEndpoints().FirstOrDefault(e => e.Scheme is "http" or "https")
                 : resourceWithEndpoints.GetEndpoint(endpointName);

                var url = endpoint?.Url;

                if (url is null)
                {
                    return null;
                }

                var options = new UriHealthCheckOptions();

                options.AddUri(new(new(url), path));

                configure?.Invoke(options);

                var client = new HttpClient();
                return new UriHealthCheck(options, () => client);
            }));
        }
    }
}
