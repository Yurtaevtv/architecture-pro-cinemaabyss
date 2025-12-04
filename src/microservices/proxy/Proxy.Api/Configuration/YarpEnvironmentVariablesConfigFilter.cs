using System.Text.RegularExpressions;
using Yarp.ReverseProxy.Configuration;

namespace Proxy_Api.Configuration
{
    public class YarpEnvironmentVariablesConfigFilter(IConfiguration config) : IProxyConfigFilter
    {
        private static readonly Regex envPattern = new(@"\${([^}]+)}", RegexOptions.Compiled);


        /// <summary>
        /// Configures the cluster by replacing environment variable placeholders in destination addresses.
        /// </summary>
        /// <param name="cluster">The cluster configuration to process.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The processed cluster configuration.</returns>
        public ValueTask<ClusterConfig> ConfigureClusterAsync(ClusterConfig cluster, CancellationToken cancellationToken)
        {
            // Each cluster has a dictionary of destinations, which is read-only, so we'll create a new one with our updates 
            var newDests = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase);

            if (cluster.Destinations != null)
            {
                foreach (var d in cluster.Destinations)
                {
                    var origAddress = d.Value.Address;
                    if (envPattern.IsMatch(origAddress))
                    {
                        // Get the name of the env variable from the destination and lookup value
                        var lookup = envPattern.Matches(origAddress)[0].Groups[1].Value;
                        var newAddress = config[lookup];

                        if (string.IsNullOrWhiteSpace(newAddress))
                        {
                            throw new ArgumentException($"Configuration Filter Error: Substitution for '{lookup}' in cluster '{d.Key}' not found as an environment variable.");
                        }

                        var modifiedDest = d.Value with { Address = newAddress };
                        newDests.Add(d.Key, modifiedDest);
                    }
                    else
                    {
                        newDests.Add(d.Key, d.Value);
                    }
                }
            }

            return new ValueTask<ClusterConfig>(cluster with { Destinations = newDests });
        }

        /// <summary>
        /// Configures the route configuration. No modifications are made in this implementation.
        /// </summary>
        /// <param name="route">The route configuration to process.</param>
        /// <param name="cluster">The associated cluster configuration, if any.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The unmodified route configuration.</returns>
        public ValueTask<RouteConfig> ConfigureRouteAsync(RouteConfig route, ClusterConfig? cluster, CancellationToken cancellationToken)
        {
            // No changes to routes in this example
            return new ValueTask<RouteConfig>(route);
        }
    }
}
