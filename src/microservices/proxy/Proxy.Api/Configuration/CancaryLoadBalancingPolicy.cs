using Yarp.ReverseProxy.LoadBalancing;
using Yarp.ReverseProxy.Model;

namespace Proxy_Api.Configuration
{
    public class CancaryLoadBalancingPolicy(IConfiguration configuration) : ILoadBalancingPolicy
    {
        private static Random _rnd = new();
        public string Name => "Cancary";

        public DestinationState? PickDestination(HttpContext context, ClusterState cluster, IReadOnlyList<DestinationState> availableDestinations)
        {
            if (availableDestinations.Count != 2)
            {
                throw new ApplicationException("Invalid configuration. Cancary excepect two destinations");
            }

            if (availableDestinations.All(d => d.Model.Config.Metadata?.TryGetValue("SwitchOn", out string? v) != true))
            {
                throw new ApplicationException("Invalid configuration. Cancary mode expect switchOn metadata");
            }

            DestinationState cancaryDest = availableDestinations
                    .First(d => d.Model.Config.Metadata!.ContainsKey("SwitchOn"));

            if (cancaryDest.Model.Config.Metadata!["SwitchOn"] == "true")
            {
                if (!cancaryDest.Model.Config.Metadata!.ContainsKey("Chance") ||
                    !(int.TryParse(cancaryDest.Model.Config.Metadata!["Chance"], out int chance) &&
                    chance is > 0 and <= 100))
                {
                    throw new ApplicationException("Invalid configuration. Cancary chance value is not valid");
                }

                return _rnd.Next(100) < chance ? cancaryDest : availableDestinations.Except([cancaryDest]).First();
            }

            return cancaryDest;
        }

    }

}
