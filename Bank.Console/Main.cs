using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bank.Console
{
    public static class Main
    {


        internal static async Task MainAsync()
        {
            const string PostUrl = "http://localhost:57266/api/customers";
            const string GetUrl = "http://localhost:57266/api/customers";

            var result = new Discover();
            var discover = (await result.Get()).Item1;
            var client = (await result.Get()).Item2;

            //Get Bearer token
            var tokenClient = result.GetToken(client, discover);

            var tokenResponse = await tokenClient.RequestClientCredentialsTokenAsync("bankapi");

            if (tokenResponse.IsError)
                System.Console.WriteLine(tokenResponse.Error);

            System.Console.WriteLine(tokenResponse.Json);
            System.Console.WriteLine("\n\n");

            //consume Customer API
            client.SetBearerToken(tokenResponse.AccessToken);

            var customerContent = new StringContent(
                JsonConvert.SerializeObject(new
                {
                    Id = 1,
                    Name = "Asd",
                    Surname = "Öztürk"
                }), Encoding.UTF8, "application/json");


            var createCustomerResponse = await client.PostAsync(PostUrl, customerContent);

            if (!createCustomerResponse.IsSuccessStatusCode)
                System.Console.WriteLine(createCustomerResponse.StatusCode);

            var getCustomerResponse = await client.GetAsync(GetUrl);

            if (!getCustomerResponse.IsSuccessStatusCode)
                System.Console.WriteLine(getCustomerResponse.StatusCode);
            else
            {
                var content = await getCustomerResponse.Content.ReadAsStringAsync();
                System.Console.WriteLine(JArray.Parse(content));
            }

            System.Console.Read();
        }
    }
}