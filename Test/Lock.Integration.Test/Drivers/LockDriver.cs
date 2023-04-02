using Lock.Integration.Test.Model;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Lock.Integration.Test.Drivers
{
    public class LockDriver
    {
        public async Task<HttpResponseMessage> LoginAsync(string userName, string password)
        {
            var client = new HttpClient();
            var body = new UserCredentialDto { UserName = userName, Password = password };
            return await client.PostAsync("http://localhost:5010/api/users/login", new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));
        }

        public async Task<HttpResponseMessage> OpenDoorAsync(string comments, string token)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await client.PostAsync("http://localhost:5010/api/doors/1/open", new StringContent(JsonConvert.SerializeObject(comments), Encoding.UTF8, "application/json"));
        }
    }
}
