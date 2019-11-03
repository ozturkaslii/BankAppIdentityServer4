using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Bank.Console
{
    internal class Discover
    {
        internal async Task<(DiscoveryDocumentResponse, HttpClient)> Get()
        {
            var client = new HttpClient();
            var discover = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (discover.IsError)
                System.Console.WriteLine(discover.Error);

            return (discover, client);
        }

        internal TokenClient GetToken(HttpClient client, DiscoveryDocumentResponse discover)
        {
            var tokenClient = new TokenClient(client, new TokenClientOptions
            {
                Address = discover.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret"
            });

            return tokenClient;
        }
    }
}