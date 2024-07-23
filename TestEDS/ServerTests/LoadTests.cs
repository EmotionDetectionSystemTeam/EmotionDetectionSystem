
using TestEDS.Logic;
using NUnit.Framework;
using TestEDS.Logic.RequestsObj;
using static TestEDS.ServerTests.MultipleStudentsSendsEmotionToServer;
using System.Collections.Concurrent;
using System.Reflection;

namespace TestEDS.ServerTests
{
    [TestFixture]
    public class LoadTests : AbstractTestCase
    {
        public const string NEUTRAL = "Neutral";
        public const string HAPPY = "Happy";
        public const string SAD = "Sad";
        public const string ANGRY = "Angry";
        public const string SURPRISED = "Surprised";
        public const string DISGUSTED = "Disgusted";
        public const string FEARFUL = "Fearful";
        public const string entryCode = "66B3QR58UMJV9Q3";
        public const string sessionId = "2";
        int numberOfStudents = 6;
        [SetUp]
        public new void BeforeTest()
        {
        }
        [Test]
        public void PushDataToExistingLessonCunccurently()
        {
            try
            {
                List<Thread> threads = new List<Thread>();
                List<Task> registrationTasks = new List<Task>();
                var server = new MultipleStudentsSendsEmotionToServer();
                var map = new ConcurrentDictionary<string, ServiceEmotionData>();
                for (int i = 0; i < numberOfStudents; i++)
                {
                    RegisterRequest studentData = new MultipleStudentsSendsEmotionToServer().RegisterAsync(false);
                    var loginRequest = new LoginRequest(studentData.password, studentData.email, studentData.password);
                    new MultipleStudentsSendsEmotionToServer().LogInUser(loginRequest);
                        
                    MultipleStudentsSendsEmotionToServer sender = new MultipleStudentsSendsEmotionToServer();
                    sender.JoinLesson(new JoinLessonRequest(studentData.password, studentData.email, entryCode));
                    Thread.Sleep(300);
                }
                DateTime startTime = DateTime.Now;
                TimeSpan duration = TimeSpan.FromMinutes(1);
                DateTime endTime = startTime.Add(duration);

                while (DateTime.Now < endTime)
                {
                    foreach (var studentData in MultipleStudentsSendsEmotionToServer.studentsData)
                    {
                        // Generate random emotion data
                        Random random = new Random();
                        if (random.NextDouble() > 0.6)
                        {
                            ServiceEmotionData emotionData = new ServiceEmotionData(GetRandomEmotion(), DateTime.Now);

                            new MultipleStudentsSendsEmotionToServer().PushEmotionDataAsync(new PushEmotionDataRequest(studentData.password, studentData.email, sessionId, emotionData));

                            Thread.Sleep(300); // simulate time between requests
                        }
                    }
                    Thread.Sleep(3000);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("not working");
                Assert.True(false);
            }
        }
        private static readonly string[] emotions =
            {
             NEUTRAL,
             HAPPY,
                SAD,
             ANGRY,
             SURPRISED,
             DISGUSTED,
            FEARFUL
         };

        private static readonly Random random = new Random();

        public static string GetRandomEmotion()
        {
            int index = random.Next(emotions.Length);
            return emotions[index];
        }
    }
}
