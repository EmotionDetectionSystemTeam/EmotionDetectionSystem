using TestEDS.Logic.RequestsObj;
using System.Text;

namespace TestEDS.Logic
{
    public class EDS
    {
        private string _serverUri;
        private HttpClient _client;
        private const string registerEnd = "api/eds/register";
        public EDS(string serverUri,HttpClient client)
        {
            _serverUri = serverUri;
            _client = client;
        }
        public void Register(string email, string firstName, string lastName, string password, string confirmPassword, int isStudent)
        {
            RegisterRequest request = new RegisterRequest(email, firstName, lastName, password, confirmPassword, isStudent);
            string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            HttpContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            Task<HttpResponseMessage> responseTask = _client.PostAsync(_serverUri + registerEnd, httpContent);
            HttpResponseMessage response = responseTask.Result; // Blocking call

            if (response.IsSuccessStatusCode)
            {
                string responseContent = response.Content.ReadAsStringAsync().Result; // Blocking call
                Console.WriteLine($"Registration successful. Server response: {responseContent}");
            }
            else
            {
                Console.WriteLine($"Registration failed. Server returned status code: {response.StatusCode}");
            }
        }

    }
}
