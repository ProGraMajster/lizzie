using System;
using System.Net.Http;
using lizzie.Runtime;

namespace lizzie.Std
{
    /// <summary>
    /// HTTP operations exposed to scripts.
    /// </summary>
    public static class Http
    {
        private static readonly HttpClient _client = new();

        /// <summary>
        /// Performs an HTTP GET request to the specified URL.
        /// </summary>
        public static string get(string url, IResourceLimiter lim, INetworkPolicy policy)
        {
            lim.Demand(Capability.Network);
            var uri = new Uri(url);
            if (!policy.IsOriginAllowed(uri))
                throw new UnauthorizedAccessException($"Origin '{uri}' is not allowed.");
            return _client.GetStringAsync(uri).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Performs an HTTP POST request to the specified URL with the provided body.
        /// </summary>
        public static string post(string url, string body, IResourceLimiter lim, INetworkPolicy policy)
        {
            lim.Demand(Capability.Network);
            var uri = new Uri(url);
            if (!policy.IsOriginAllowed(uri))
                throw new UnauthorizedAccessException($"Origin '{uri}' is not allowed.");
            var response = _client.PostAsync(uri, new StringContent(body)).GetAwaiter().GetResult();
            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Performs an HTTP PUT request to the specified URL with the provided body.
        /// </summary>
        public static string put(string url, string body, IResourceLimiter lim, INetworkPolicy policy)
        {
            lim.Demand(Capability.Network);
            var uri = new Uri(url);
            if (!policy.IsOriginAllowed(uri))
                throw new UnauthorizedAccessException($"Origin '{uri}' is not allowed.");
            var response = _client.PutAsync(uri, new StringContent(body)).GetAwaiter().GetResult();
            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Performs an HTTP DELETE request to the specified URL.
        /// </summary>
        public static string delete(string url, IResourceLimiter lim, INetworkPolicy policy)
        {
            lim.Demand(Capability.Network);
            var uri = new Uri(url);
            if (!policy.IsOriginAllowed(uri))
                throw new UnauthorizedAccessException($"Origin '{uri}' is not allowed.");
            var response = _client.DeleteAsync(uri).GetAwaiter().GetResult();
            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }
    }
}
