using NUnit.Framework;
using System.Net.Http.Json;
using System.Text.Json;
using TestEDS.Logic.RequestsObj;
using TestEDS.Logic;
using NLog;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;
using static TestEDS.ServerTests.MultipleStudentsSendsEmotionToServer;
using Newtonsoft.Json;

namespace TestEDS.ServerTests
{

    public class MultipleStudentsSendsEmotionToServer
    {
        private const string BaseUrl = "https://localhost:7069"; // Replace with your API base URL
        private static Logger log = LogManager.GetCurrentClassLogger();
        private const string registerEnd = "/api/eds/register";
        private const string loginEnd = "/api/eds/login";
        private const string lessonEnd = "/api/eds/create-lesson";
        private const string joinLessonEnd = "/api/eds/join-lesson";
        private const string pushEnd = "/api/eds/push-emotion-data";
        private const string lastEmotionDataEnd = "/api/eds/get-last-emotions-data";
        public static ConcurrentBag<RegisterRequest> studentsData = new ConcurrentBag<RegisterRequest>();
        public static ConcurrentBag<RegisterRequest> teachersData = new ConcurrentBag<RegisterRequest>();


        public RegisterRequest RegisterAsync(bool isTeacher)
        {
            using (HttpClient client = new HttpClient())
            {
                // Generate random registration data for each student
                var registrationData = GenerateRandomRegistrationData();
                if (isTeacher)
                {
                    registrationData.isStudent = 1;
                }
                
                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(registrationData);
                HttpContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                Task<HttpResponseMessage> registrationResponse = client.PostAsync(BaseUrl + registerEnd, httpContent);
                HttpResponseMessage response = registrationResponse.Result;
                
                Assert.True(response.IsSuccessStatusCode);
                Console.WriteLine(response.ToString() + "\n" + System.Text.Json.JsonSerializer.Deserialize<RootObject>(response.Content.ReadAsStringAsync().Result).ToString() + "\n");
                // Extract the session ID from the registration response
                if (isTeacher)
                {
                    teachersData.Add(registrationData);
                }
                else
                {
                    studentsData.Add(registrationData);
                }
                return registrationData;

            }
        }

        public void LogInUser(LoginRequest loginRequest)
        {
            using (HttpClient client = new HttpClient())
            {
                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(loginRequest);
                HttpContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                Task<HttpResponseMessage> loginResponse = client.PostAsync(BaseUrl + loginEnd, httpContent);
                HttpResponseMessage response = loginResponse.Result;
             
                Assert.True(response.IsSuccessStatusCode);
                Console.WriteLine(response.ToString() + "\n" + System.Text.Json.JsonSerializer.Deserialize<RootObject>(response.Content.ReadAsStringAsync().Result).ToString() + "\n");
                // Additional assertions or logging can be added as needed
            }
        }
        public Lesson CreateLesson(CreateLessonRequest lessonRequest)
        {
            using (HttpClient client = new HttpClient())
            {
                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(lessonRequest);
                HttpContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                Task<HttpResponseMessage> lessonResponse = client.PostAsync(BaseUrl + lessonEnd, httpContent);
                HttpResponseMessage response = lessonResponse.Result;
                Console.WriteLine(response.ToString() + "\n" + response.RequestMessage.ToString());
                Assert.True(response.IsSuccessStatusCode);
                var responseContent = System.Text.Json.JsonSerializer.Deserialize<LessonObj>(response.Content.ReadAsStringAsync().Result);
                Console.WriteLine(responseContent.value.ToString() + "\n");
                return responseContent.value;
                // Additional assertions or logging can be added as needed
                //return response.Content.
            }
        }
        public void JoinLesson(JoinLessonRequest lessonRequest)
        {
            using (HttpClient client = new HttpClient())
            {
                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(lessonRequest);
                HttpContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                Task<HttpResponseMessage> joinResponse = client.PostAsync(BaseUrl + joinLessonEnd, httpContent);
                HttpResponseMessage response = joinResponse.Result;
                
                Assert.True(response.IsSuccessStatusCode);
                Console.WriteLine(response.ToString() + "\n" + (response.Content.ReadAsStringAsync().Result).ToString() + "\n");                // Additional assertions or logging can be added as needed
            }
        }
        public List<User> ValidateEmotionDataHasArrivedToTeacher(GetLastEmotionsDataRequest emotionsRequest)
        {
            using (HttpClient client = new HttpClient())
            {
                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(emotionsRequest);
                HttpContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                Task<HttpResponseMessage> emotionDataResponse = client.PostAsync(BaseUrl + lastEmotionDataEnd, httpContent);
                HttpResponseMessage response = emotionDataResponse.Result;

                Assert.True(response.IsSuccessStatusCode);
                var responseContent = Newtonsoft.Json.JsonConvert.DeserializeObject<UserData>(response.Content.ReadAsStringAsync().Result);
                Console.WriteLine(response.ToString() + "\n" + (response.Content.ReadAsStringAsync().Result).ToString() + "\n"); 
                return responseContent.Users;
            }
        }

        public async Task<HttpResponseMessage> RegisterAsync(HttpClient client, RegisterRequest registrationData)
        {

            return await client.PostAsJsonAsync($"{BaseUrl}/api/eds/register", registrationData);
        }

        public void PushEmotionDataAsync(PushEmotionDataRequest request)
        {
            using (HttpClient client = new HttpClient())
            {
                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                HttpContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                Task<HttpResponseMessage> joinResponse = client.PostAsync(BaseUrl + pushEnd, httpContent);
                HttpResponseMessage response = joinResponse.Result;

                Assert.True(response.IsSuccessStatusCode);
                Console.WriteLine(response.ToString() + "\n" + (response.Content.ReadAsStringAsync().Result).ToString() + "\n");
            }
        }

        public RegisterRequest GenerateRandomRegistrationData()
        {
            string password = GenerateRandomPassword();
            // Replace this with your actual data generation logic
            return new RegisterRequest
            (
                GenerateRandomEmail(),
                GenerateRandomName(),
                GenerateRandomName(),
                password,
                password,
                 0
            );
        }

        public string GenerateRandomEmail()
        {
            // Replace this with your actual email generation logic
            return $"user{Guid.NewGuid().ToString().Substring(0, 8)}@example.com";
        }

        public string GenerateRandomName()
        {
            // Replace this with your actual name generation logic
            string[] names = { "Alice", "Bob", "Charlie", "David", "Eva", "Frank", "Grace", "Harry" };
            return names[new Random().Next(names.Length)];
        }

        public string GenerateRandomPassword()
        {
            // Replace this with your actual password generation logic
            return new PasswordGenerator().GenerateRandomPassword();
        }

        public bool GenerateRandomBool()
        {
            // Replace this with your actual boolean generation logic
            return new Random().Next(2) == 0;
        }

        public string GenerateRandomEmotionData()
        {
            // Replace this with your actual emotion data generation logic
            var emotionDataObject = new
            {
                emotion = GenerateRandomEmotion(),
                intensity = GenerateRandomIntensity()
            };
            return System.Text.Json.JsonSerializer.Serialize(emotionDataObject);
        }

        public string GenerateRandomEmotion()
        {
            // Replace this with your actual emotion generation logic
            string[] emotions = { "happy", "sad", "angry", "surprised", "calm" };
            return emotions[new Random().Next(emotions.Length)];
        }

        public double GenerateRandomIntensity()
        {
            // Replace this with your actual intensity generation logic
            return new Random().NextDouble();
        }

        // ... (Other methods remain unchanged)

        public class RegistrationData
        {
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
            public bool IsStudent { get; set; }
            public string SessionId { get; set; }
        }

        // Queue to store registered students for sending emotion data
        public class RootObject
        {
            public string value { get; set; }
            public string errorMessage { get; set; }
        }
        public class joinObject
        {
            public string value { get; set; }
            public string errorMessage { get; set; }
        }
        public class LessonObj
        {
            public Lesson value { get; set; }
            public string errorMessage { get; set; }
        }
        public class EmotionDataObj
        {
            public List<ServiceRealTimeUser> value { get; set; }
            public string errorMessage { get; set; }
        }
        public class Lesson
        {
            public string lessonId { get; set; }
            public string lessonName { get; set; }
            public string teacher { get; set; }
            public DateTime date { get; set; }
            public bool isActive { get; set; }
            public string entryCode { get; set; }
            public int studentsQuantity { get; set; }
            public List<object> studentsEmotions { get; set; } // You might want to replace `object` with the actual type of studentsEmotions
        }

    }



}
