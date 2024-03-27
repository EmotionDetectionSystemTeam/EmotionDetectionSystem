
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
        int numberOfStudents = 60;
        [SetUp]
        public new void BeforeTest()
        {
        }
        [Test]
        public void RegisterStudentsAndSendEmotionDataTest()
        {
            try
            {
                List<Thread> threads = new List<Thread>();
                List<Task> registrationTasks = new List<Task>();
                MultipleStudentsSendsEmotionToServer server = new MultipleStudentsSendsEmotionToServer();
                // Register all students concurrently
                for (int i = 0; i < numberOfStudents; i++)
                {
                    Thread thread = new Thread(() => new MultipleStudentsSendsEmotionToServer().RegisterAsync(false));
                    threads.Add(thread);
                    thread.Start();
                }
                foreach (var thread in threads) { thread.Join(); }
                threads = new List<Thread>();
                // Log in all registered users concurrently
                foreach (var userData in MultipleStudentsSendsEmotionToServer.studentsData)
                {
                    var registrationData = userData;
                    var loginRequest = new LoginRequest(registrationData.password, registrationData.email, registrationData.password);
                    Thread thread = new Thread(() => new MultipleStudentsSendsEmotionToServer().LogInUser(loginRequest));
                    threads.Add(thread);
                    thread.Start();
                }

                foreach (var thread in threads) { thread.Join(); }

                //register and login teacher
                server.RegisterAsync(true);
                RegisterRequest teacherData = MultipleStudentsSendsEmotionToServer.teachersData.First();
                var teacherLoginRequest = new LoginRequest(teacherData.password, teacherData.email, teacherData.password);
                server.LogInUser(teacherLoginRequest);
                Lesson lesson = server.CreateLesson(new CreateLessonRequest(teacherData.password, teacherData.email, "blabla", "blablabla", new string[] { }));
                threads = new List<Thread>();
                foreach (var student in MultipleStudentsSendsEmotionToServer.studentsData)
                {
                    Thread thread = new Thread(() =>
                    {
                        // Create an instance of MultipleStudentsSendsEmotionToServer outside the loop
                        MultipleStudentsSendsEmotionToServer sender = new MultipleStudentsSendsEmotionToServer();
                        sender.JoinLesson(new JoinLessonRequest(student.password, student.email, lesson.entryCode));
                        // Set the start time for the 2-minute duration
                        DateTime startTime = DateTime.Now;
                        DateTime endTime = startTime.AddMinutes(0.5);

                        while (DateTime.Now < endTime)
                        {

                            // Generate random emotion data
                            Random random = new Random();
                            ServiceEmotionData emotionData = new ServiceEmotionData(
                                random.NextDouble(), random.NextDouble(), random.NextDouble(),
                                random.NextDouble(), random.NextDouble(), random.NextDouble(),
                                random.NextDouble());

                            // Push emotion data to the server
                            sender.PushEmotionDataAsync(new PushEmotionDataRequest(student.password, student.email, lesson.lessonId, emotionData));

                            // Sleep for 30 seconds before sending the next emotion data
                            Thread.Sleep(15 * 1000); // Sleep for 30 seconds (time is in milliseconds)
                        }
                    });
                    threads.Add(thread);
                    thread.Start();
                }
                foreach (var thread in threads) { thread.Join(); }
            }catch(Exception e)
            {
                Console.WriteLine("not working");
            }

        }
        [Test]
        public void RegisterStudentsCunccurently()
        {
            List<Thread> threads = new List<Thread>();
            List<Task> registrationTasks = new List<Task>();
            MultipleStudentsSendsEmotionToServer server = new MultipleStudentsSendsEmotionToServer();
            // Register all students concurrently
            for (int i = 0; i < numberOfStudents; i++)
            {
                Thread thread = new Thread(() => new MultipleStudentsSendsEmotionToServer().RegisterAsync(false));
                threads.Add(thread);
                thread.Start();
            }
            foreach (var thread in threads) { thread.Join(); }
        }
        [Test]
        public void RegisterStudentsAndThenLoginCunccurently()
        {
            List<Thread> threads = new List<Thread>();
            List<Task> registrationTasks = new List<Task>();
            MultipleStudentsSendsEmotionToServer server = new MultipleStudentsSendsEmotionToServer();
            // Register all students concurrently
            for (int i = 0; i < numberOfStudents; i++)
            {
                Thread thread = new Thread(() => new MultipleStudentsSendsEmotionToServer().RegisterAsync(false));
                threads.Add(thread);
                thread.Start();
            }
            foreach (var thread in threads) { thread.Join(); }
            threads = new List<Thread>();
            // Log in all registered users concurrently
            foreach (var userData in MultipleStudentsSendsEmotionToServer.studentsData)
            {
                var registrationData = userData;
                var loginRequest = new LoginRequest(registrationData.password, registrationData.email, registrationData.password);
                Thread thread = new Thread(() => new MultipleStudentsSendsEmotionToServer().LogInUser(loginRequest));
                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads) { thread.Join(); }
        }
        [Test]
        public void RegisterStudentsAndLoginCunccurently()
        {
            List<Thread> threads = new List<Thread>();
            List<Task> registrationTasks = new List<Task>();
            MultipleStudentsSendsEmotionToServer server = new MultipleStudentsSendsEmotionToServer();
            // Register all students concurrently
            for (int i = 0; i < numberOfStudents; i++)
            {
                Thread thread = new Thread(() => {
                    RegisterRequest studentData = new MultipleStudentsSendsEmotionToServer().RegisterAsync(false);
                    var loginRequest = new LoginRequest(studentData.password, studentData.email, studentData.password);
                    Thread thread = new Thread(() => new MultipleStudentsSendsEmotionToServer().LogInUser(loginRequest));
                });
                threads.Add(thread);
                thread.Start();
            }
            foreach (var thread in threads) { thread.Join(); }

        }
        [Test]
        public void RegisterStudentsAndLoginAndStartPushDataCunccurently()
        {
            try
            {
                List<Thread> threads = new List<Thread>();
                List<Task> registrationTasks = new List<Task>();
                var server = new MultipleStudentsSendsEmotionToServer();
                
                RegisterRequest teacherData = server.RegisterAsync(true);
                var teacherLoginRequest = new LoginRequest(teacherData.password, teacherData.email, teacherData.password);
                server.LogInUser(teacherLoginRequest);
                Lesson lesson = server.CreateLesson(new CreateLessonRequest(teacherData.password, teacherData.email, "blabla", "blablabla", new string[] { }));
                for (int i = 0; i < numberOfStudents; i++)
                {
                    Thread thread = new Thread(() =>
                    {
                        RegisterRequest studentData = new MultipleStudentsSendsEmotionToServer().RegisterAsync(false);
                        var loginRequest = new LoginRequest(studentData.password, studentData.email, studentData.password);
                        new MultipleStudentsSendsEmotionToServer().LogInUser(loginRequest);
                        MultipleStudentsSendsEmotionToServer sender = new MultipleStudentsSendsEmotionToServer();
                        sender.JoinLesson(new JoinLessonRequest(studentData.password, studentData.email, lesson.entryCode));
                        // Set the start time for the 2-minute duration
                        DateTime startTime = DateTime.Now;
                        DateTime endTime = startTime.AddMinutes(0.5);

                        while (DateTime.Now < endTime)
                        {

                            // Generate random emotion data
                            Random random = new Random();
                            ServiceEmotionData emotionData = new ServiceEmotionData(
                                random.NextDouble(), random.NextDouble(), random.NextDouble(),
                                random.NextDouble(), random.NextDouble(), random.NextDouble(),
                                random.NextDouble());

                            // Push emotion data to the server
                            sender.PushEmotionDataAsync(new PushEmotionDataRequest(studentData.password, studentData.email, lesson.lessonId, emotionData));

                            // Sleep for 30 seconds before sending the next emotion data
                            Thread.Sleep(15 * 1000); // Sleep for 30 seconds (time is in milliseconds)
                        }
                    });
                    threads.Add(thread);
                    thread.Start();
                }
                foreach (var thread in threads) { thread.Join(); }
            }
            catch(Exception e)
            {
                Console.WriteLine("not working");
            }
        }
        [Test]
        public void RegisterStudentsAndLoginAndStartPushDataTeacherVerifyDataCunccurently()
        {
            try
            {
                List<Thread> threads = new List<Thread>();
                List<Task> registrationTasks = new List<Task>();
                var server = new MultipleStudentsSendsEmotionToServer();
                var map = new ConcurrentDictionary<string, ServiceEmotionData>();
                RegisterRequest teacherData = server.RegisterAsync(true);
                var teacherLoginRequest = new LoginRequest(teacherData.password, teacherData.email, teacherData.password);
                server.LogInUser(teacherLoginRequest);
                Lesson lesson = server.CreateLesson(new CreateLessonRequest(teacherData.password, teacherData.email, "blabla", "blablabla", new string[] { }));
                for (int i = 0; i < numberOfStudents; i++)
                {
                    Thread thread = new Thread(() =>
                    {
                        RegisterRequest studentData = new MultipleStudentsSendsEmotionToServer().RegisterAsync(false);
                        var loginRequest = new LoginRequest(studentData.password, studentData.email, studentData.password);
                        new MultipleStudentsSendsEmotionToServer().LogInUser(loginRequest);
                        MultipleStudentsSendsEmotionToServer sender = new MultipleStudentsSendsEmotionToServer();
                        sender.JoinLesson(new JoinLessonRequest(studentData.password, studentData.email, lesson.entryCode));

                        // Generate random emotion data
                        Random random = new Random();
                        ServiceEmotionData emotionData = new ServiceEmotionData(
                            random.NextDouble(), random.NextDouble(), random.NextDouble(),
                            random.NextDouble(), random.NextDouble(), random.NextDouble(),
                            random.NextDouble());

                        // Push emotion data to the server
                        sender.PushEmotionDataAsync(new PushEmotionDataRequest(studentData.password, studentData.email, lesson.lessonId, emotionData));

                        // Sleep for 30 seconds before sending the next emotion data
                        Thread.Sleep(3 * 1000); // Sleep for 30 seconds (time is in milliseconds)
                        map[studentData.email] = emotionData;
                    });
                    threads.Add(thread);
                    thread.Start();
                }
                foreach (var thread in threads) { thread.Join(); }
                Thread.Sleep(3 * 1000);
                GetLastEmotionsDataRequest getLastEmotionsDataRequest = new GetLastEmotionsDataRequest(teacherData.password, teacherData.email, lesson.lessonId);
                List<User> emotions = server.ValidateEmotionDataHasArrivedToTeacher(getLastEmotionsDataRequest);
                foreach (var kvp in map)
                {
                    string email = kvp.Key;
                    ServiceEmotionData emotion = kvp.Value;

                    bool found = false;
                    foreach (var realTimeUser in emotions)
                    {
                        if (realTimeUser.Email == email) //&& realTimeUser.WinningEmotion == emotion.GetHighestEmotion())
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        // Emotion not found in the list
                        Assert.True(false);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("not working");
                Assert.True(false);
            }
        }
        [Test]
        public void RegisterStudentsAndLoginAndStartPushDataTeacherVerifyFor5TimesDataCunccurently()
        {
            try
            {
                List<Thread> threads = new List<Thread>();
                List<Task> registrationTasks = new List<Task>();
                var server = new MultipleStudentsSendsEmotionToServer();
                var map = new ConcurrentDictionary<string, ServiceEmotionData>();
                RegisterRequest teacherData = server.RegisterAsync(true);
                var teacherLoginRequest = new LoginRequest(teacherData.password, teacherData.email, teacherData.password);
                server.LogInUser(teacherLoginRequest);
                Lesson lesson = server.CreateLesson(new CreateLessonRequest(teacherData.password, teacherData.email, "blabla", "blablabla", new string[] { }));
                for (int i = 0; i < numberOfStudents; i++)
                {
                    Thread thread = new Thread(() =>
                    {
                        RegisterRequest studentData = new MultipleStudentsSendsEmotionToServer().RegisterAsync(false);
                        var loginRequest = new LoginRequest(studentData.password, studentData.email, studentData.password);
                        new MultipleStudentsSendsEmotionToServer().LogInUser(loginRequest);
                        MultipleStudentsSendsEmotionToServer sender = new MultipleStudentsSendsEmotionToServer();
                        sender.JoinLesson(new JoinLessonRequest(studentData.password, studentData.email, lesson.entryCode));
                    });
                    threads.Add(thread);
                    thread.Start();
                }
                foreach (var thread in threads) { thread.Join(); }
                for(int i = 0; i < 5; i++)
                {
                    foreach (var studentData in MultipleStudentsSendsEmotionToServer.studentsData)
                    {
                        // Generate random emotion data
                        Random random = new Random();
                        ServiceEmotionData emotionData = new ServiceEmotionData(
                            random.NextDouble(), random.NextDouble(), random.NextDouble(),
                            random.NextDouble(), random.NextDouble(), random.NextDouble(),
                            random.NextDouble());

                        new MultipleStudentsSendsEmotionToServer().PushEmotionDataAsync(new PushEmotionDataRequest(studentData.password, studentData.email, lesson.lessonId, emotionData));

                        Thread.Sleep(300); // simulate time between requests
                        map[studentData.email] = emotionData;
                    }
                    Thread.Sleep(10 * 1000);
                    GetLastEmotionsDataRequest getLastEmotionsDataRequest = new GetLastEmotionsDataRequest(teacherData.password, teacherData.email, lesson.lessonId);
                    List<User> emotions = server.ValidateEmotionDataHasArrivedToTeacher(getLastEmotionsDataRequest);
                    foreach (var kvp in map)
                    {
                        string email = kvp.Key;
                        ServiceEmotionData emotion = kvp.Value;

                        bool found = false;
                        foreach (var realTimeUser in emotions)
                        {
                            if (realTimeUser.Email == email) //&& realTimeUser.WinningEmotion == emotion.GetHighestEmotion())
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            // Emotion not found in the list
                            Assert.True(false);
                        }
                    }
                }

                
            }
            catch (Exception e)
            {
                Console.WriteLine("not working");
                Assert.True(false);
            }
        }
    }
}
