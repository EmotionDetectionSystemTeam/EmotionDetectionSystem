using System.Collections.Concurrent;
using EmotionDetectionSystem.DomainLayer.Events;
using EmotionDetectionSystem.DomainLayer.objects;
using log4net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

namespace EmotionDetectionSystem.DomainLayer.Managers;

public class EdsManager
{
    private readonly        UserManager                          _userManager;
    private readonly        LessonManager                        _lessonManager;
    private readonly        ConcurrentQueue<PushEmotionDataTask> _emotionDataTasks;
    private readonly        CancellationTokenSource              _cancellationTokenSource;
    private readonly        AutoResetEvent                       _taskEvent;
    private                 bool                                 _isProcessingTasks;
    private static readonly ILog                                 Log = LogManager.GetLogger(typeof(EdsManager));

    public EdsManager()
    {
        _userManager             = new UserManager();
        _lessonManager           = new LessonManager();
        _emotionDataTasks        = new ConcurrentQueue<PushEmotionDataTask>();
        _cancellationTokenSource = new CancellationTokenSource();
        _taskEvent               = new AutoResetEvent(false);
        _isProcessingTasks       = true;

        // Start the background worker thread
        ThreadPool.QueueUserWorkItem(ProcessQueue);
    }

    public void StopProcessingTasks()
    {
        _isProcessingTasks = false;
        _cancellationTokenSource.Cancel();
        _taskEvent.Set(); // Signal the event to unblock the background thread
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="correlationId">The correlation ID for logging.</param>
    /// <param name="email">The email address of the user.</param>
    /// <param name="firstName">The first name of the user.</param>
    /// <param name="lastName">The last name of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <param name="userType">The type of the user.</param>
    public void Register(string correlationId, string email, string firstName, string lastName, string password,
                         int    userType)
    {
        Log.Info($"[{correlationId}] Registering user: {email}");
        _userManager.Register(correlationId, email, firstName, lastName, password, userType);
    }

    /// <summary>
    /// Logs in a user.
    /// </summary>
    /// <param name="correlationId">The correlation ID for logging.</param>
    /// <param name="sessionId">The session ID of the user.</param>
    /// <param name="email">The email address of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <returns>The logged-in user object.</returns>
    public User Login(string correlationId, string sessionId, string email, string password)
    {
        Log.Info($"[{correlationId}] Logging in user: {email}");
        return _userManager.Login(correlationId, sessionId, email, password);
    }

    /// <summary>
    /// Logs out a user.
    /// </summary>
    /// <param name="correlationId">The correlation ID for logging.</param>
    /// <param name="sessionId">The session ID of the user.</param>
    /// <param name="email">The email address of the user.</param>
    public void Logout(string correlationId, string sessionId, string email)
    {
        Log.Info($"[{correlationId}] Logging out user: {email} with session ID: {sessionId}");
        IsValidSession(sessionId, email);
        _userManager.Logout(correlationId, sessionId);
    }

    /// <summary>
    /// Creates a new lesson.
    /// </summary>
    /// <param name="correlationId">The correlation ID for logging.</param>
    /// <param name="sessionId">The session ID of the user.</param>
    /// <param name="email">The email address of the user.</param>
    /// <param name="title">The title of the lesson.</param>
    /// <param name="description">The description of the lesson.</param>
    /// <param name="tags">The tags associated with the lesson.</param>
    /// <returns>The created lesson.</returns>
    public Lesson CreateLesson(string   correlationId, string sessionId, string email, string title, string description,
                               string[] tags)
    {
        Log.Info($"[{correlationId}] Creating lesson titled: {title} by user: {email}");

        IsValidSession(sessionId, email);
        var teacher = _userManager.GetTeacher(email);
        return _lessonManager.CreateLesson(correlationId, teacher, title, description, tags);
    }

    private void IsValidSession(string sessionId, string email)
    {
        if (!_userManager.IsValidSession(sessionId, email))
        {
            throw new Exception($"Session: {sessionId} is not valid");
        }
    }

    /// <summary>
    /// Ends a lesson for the specified user.
    /// </summary>
    /// <param name="correlationId">The correlation ID for logging.</param>
    /// <param name="sessionId">The session ID of the user.</param>
    /// <param name="email">The email address of the user.</param>
    public void EndLesson(string correlationId, string sessionId, string email)
    {
        Log.Info($"[{correlationId}] Ending lesson for user: {email}");

        IsValidSession(sessionId, email);
        _lessonManager.EndLesson(correlationId, email);
    }

    /// <summary>
    /// Allows a user to join a lesson using the provided entry code.
    /// </summary>
    /// <param name="correlationId">The correlation ID for logging.</param>
    /// <param name="sessionId">The session ID of the user.</param>
    /// <param name="email">The email address of the user.</param>
    /// <param name="entryCode">The entry code for the lesson.</param>
    /// <returns>The joined lesson.</returns>
    public Lesson JoinLesson(string correlationId, string sessionId, string email, string entryCode)
    {
        Log.Info($"[{correlationId}] User {email} is joining lesson with entry code: {entryCode}");

        IsValidSession(sessionId, email);
        var user = _userManager.GetUser(email);
        return _lessonManager.JoinLesson(correlationId, user, entryCode);
    }

    /// <summary>
    /// Retrieves a summary of students enrolled during a lesson for a viewer user.
    /// </summary>
    /// <param name="correlationId">The correlation ID for logging.</param>
    /// <param name="sessionId">The session ID of the user.</param>
    /// <param name="email">The email address of the user.</param>
    /// <param name="lessonId">The ID of the lesson.</param>
    /// <returns>An IEnumerable of EnrollmentSummary objects.</returns>
    public IEnumerable<EnrollmentSummary> ViewStudentsDuringLesson(string correlationId, string sessionId, string email,
                                                                   string lessonId)
    {
        Log.Info($"[{correlationId}] User {email} is viewing students during lesson with ID: {lessonId}");

        IsValidSession(sessionId, email);
        var user = _userManager.GetUser(email);
        if (!(user is Viewer))
        {
            Log.Warn($"[{correlationId}] User {user.Email} is not authorized to view students data");
            throw new Exception($"User {user.Email} cannot view students data");
        }

        var viewer = (Viewer)user;
        return _lessonManager.ViewStudentsDuringLesson(correlationId, viewer, lessonId);
    }

    /// <summary>
    /// Retrieves lessons for a teacher's dashboard.
    /// </summary>
    /// <param name="correlationId">The correlation ID for logging.</param>
    /// <param name="sessionId">The session ID of the user.</param>
    /// <param name="email">The email of the teacher.</param>
    /// <returns>An IEnumerable of Lesson objects.</returns>
    public IEnumerable<Lesson> ViewTeacherDashboard(string correlationId, string sessionId, string email)
    {
        IsValidSession(sessionId, email);
        var user = _userManager.GetUser(email);
        if (user is not Teacher)
        {
            Log.Warn($"[{correlationId}] User {user.Email} is not a teacher. Unable to retrieve teacher dashboard.");
            throw new Exception($"User {user.Email} is not a teacher");
        }

        var teacher = (Teacher)user;
        Log.Info($"[{correlationId}] Retrieving dashboard for teacher {teacher.Email}");

        return teacher.Lessons;
    }

    /// <summary>
    /// Retrieves student details for a viewer.
    /// </summary>
    /// <param name="correlationId">The correlation ID for logging.</param>
    /// <param name="sessionId">The session ID of the viewer.</param>
    /// <param name="email">The email of the viewer.</param>
    /// <param name="studentEmail">The email of the student to retrieve.</param>
    /// <returns>A Student object representing the requested student.</returns>
    public Student ViewStudent(string correlationId, string sessionId, string email, string studentEmail)
    {
        IsValidSession(sessionId, email);
        var user = _userManager.GetUser(email);
        if (user is not Viewer)
        {
            Log.Warn($"[{correlationId}] User {user.Email} cannot view students data");
            throw new Exception($"User {user.Email} cannot view students data");
        }

        Log.Info($"[{correlationId}] Retrieving student details for viewer {user.Email}");
        return _userManager.GetStudent(studentEmail);
    }

    /// <summary>
    /// Pushes emotion data to be processed asynchronously.
    /// </summary>
    /// <param name="correlationId">The correlation ID for logging.</param>
    /// <param name="sessionId">The session ID of the user pushing the data.</param>
    /// <param name="email">The email of the user pushing the data.</param>
    /// <param name="lessonId">The ID of the lesson associated with the emotion data.</param>
    /// <param name="emotionData">The emotion data to push.</param>
    public void PushEmotionData(string      correlationId, string sessionId, string email, string lessonId,
                                EmotionData emotionData)
    {
        var pushTaskInfo = new PushEmotionDataTask(correlationId, sessionId, email, lessonId, emotionData);
        _emotionDataTasks.Enqueue(pushTaskInfo);
        _taskEvent.Set(); // Signal the event to unblock the background thread
    }

    private void ProcessQueue(object state)
    {
        while (_isProcessingTasks)
        {
            if (WaitHandle.WaitAny(new[] { _taskEvent, _cancellationTokenSource.Token.WaitHandle }) == 1)
            {
                break;
            }

            while (_emotionDataTasks.TryDequeue(out var taskInfo))
            {
                ProcessTask(taskInfo);
            }
        }
    }

    private void ProcessTask(PushEmotionDataTask emotionDataTask)
    {
        try
        {
            IsValidSession(emotionDataTask.SessionId, emotionDataTask.Email);
            var user = _userManager.GetUser(emotionDataTask.Email);
            if (user is not Student)
            {
                throw new Exception($"User {user.Email} is not a student");
            }

            var lesson = _lessonManager.GetLesson(emotionDataTask.LessonId);
            lesson.PushEmotionData(user.Email, emotionDataTask.EmotionData);
        }
        catch (Exception e)
        {
            Log.Error($"[{emotionDataTask.CorrelationId}] Error processing emotion data task - {e.Message}");
        }
    }

    public bool IsLoggedIn(string sessionId, string email)
    {
        try
        {
            IsValidSession(sessionId, email);
            return true;
        }
        catch (Exception _)
        {
            return false;
        }
    }

    public User GetUser(string email)
    {
        return _userManager.GetUser(email);
    }

    public bool IsProcessingTasks => _emotionDataTasks.Count > 0;

    /// <summary>
    /// Retrieves the last emotions data for a lesson.
    /// </summary>
    /// <param name="correlationId">Correlation ID for tracing the request.</param>
    /// <param name="sessionId">Session ID of the user.</param>
    /// <param name="email">Email of the user.</param>
    /// <param name="lessonId">ID of the lesson to retrieve data from.</param>
    /// <returns>List of enrollment summaries with emotion data.</returns>
    public IEnumerable<EnrollmentSummary> GetLastEmotionsData(string correlationId, string sessionId, string email, string lessonId)
    {
        IsValidSession(sessionId, email);
        var lesson = _lessonManager.GetLesson(lessonId);
        return lesson.GetEnrollmentSummariesWithData(correlationId);
    }


    /// <summary>
    /// Retrieves a lesson based on the provided session and user credentials.
    /// </summary>
    /// <param name="sessionId">Session ID of the user.</param>
    /// <param name="email">Email of the user.</param>
    /// <param name="lessonId">ID of the lesson to retrieve.</param>
    /// <returns>The Lesson object corresponding to the provided lesson ID.</returns>
    public Lesson GetLesson(string sessionId, string email, string lessonId)
    {
        IsValidSession(sessionId, email);
        return _lessonManager.GetLesson(lessonId);
    }


    public IEnumerable<Lesson> GetEnrolledLessons(string sessionId, string teacherEmail)
    {
        IsValidSession(sessionId, teacherEmail);
        var teacher = _userManager.GetTeacher(teacherEmail);
        return teacher.Lessons;
    }

    /// <summary>
    /// Retrieves enrollment summaries with student data for a specific lesson.
    /// </summary>
    /// <param name="correlationId">The correlation ID for tracking logs related to this operation.</param>
    /// <param name="sessionId">The session ID of the logged-in teacher.</param>
    /// <param name="teacherEmail">The email address of the teacher requesting student data.</param>
    /// <param name="lessonId">The ID of the lesson for which student data is requested.</param>
    /// <returns>A list of enrollment summaries containing student data for the specified lesson.</returns>
    /// <exception cref="Exception">Thrown when the lesson with the specified <paramref name="lessonId"/> is not found.</exception>
    public List<EnrollmentSummary> GetStudentDataByLesson(string correlationId, string sessionId, string teacherEmail, string lessonId)
    {
        IsValidSession(sessionId, teacherEmail);
    
        var teacher = _userManager.GetTeacher(teacherEmail);
        var lesson  = teacher.Lessons.FirstOrDefault(l => l.LessonId == lessonId);
    
        if (lesson == null)
        {
            throw new Exception($"Lesson with id {lessonId} not found");
        }

        var enrollmentSummaries = lesson.GetEnrollmentSummariesWithData(correlationId).ToList();

        Log.Info($"[{correlationId}] Retrieved {enrollmentSummaries.Count} enrollment data for lesson {lessonId}");

        return enrollmentSummaries;
    }


    /// <summary>
    /// Retrieves all student enrollment data grouped by student for a teacher across all his lessons.
    /// </summary>
    /// <param name="correlationId">The correlation ID for tracking logs related to this operation.</param>
    /// <param name="sessionId">The session ID of the logged-in teacher.</param>
    /// <param name="teacherEmail">The email address of the teacher requesting student data.</param>
    /// <returns>A dictionary where each key is a student and the value is a list of enrollment summaries for that student across all lessons.</returns>
    /// <exception cref="Exception">Thrown when there is an issue with session validation or if the teacher's lessons cannot be retrieved.</exception>
    public Dictionary<Student, List<EnrollmentSummary>> GetAllStudentsData(string correlationId, string sessionId, string teacherEmail)
    {
        IsValidSession(sessionId, teacherEmail);
    
        var teacher = _userManager.GetTeacher(teacherEmail);
        var lessons = teacher.Lessons;
    
        var studentData = new Dictionary<Student, List<EnrollmentSummary>>();

        foreach (var lesson in lessons)
        {
            var enrollmentSummaries = lesson.GetEnrollmentSummariesWithData(correlationId).ToList();
        
            foreach (var enrollmentSummary in enrollmentSummaries)
            {
                if (!studentData.TryGetValue(enrollmentSummary.Student, out var value))
                {
                    value                                  = new List<EnrollmentSummary>();
                    studentData[enrollmentSummary.Student] = value;
                }

                value.Add(enrollmentSummary);
            }
        }

        Log.InfoFormat(correlationId, $"Retrieved enrollment data for {studentData.Count} students");

        return studentData;
    }


    /// <summary>
    /// Notifies a student about a surprise event initiated by the teacher.
    /// </summary>
    /// <param name="correlationId">The correlation ID for tracking logs related to this operation.</param>
    /// <param name="sessionId">The session ID of the logged-in student.</param>
    /// <param name="teacherEmail">The email address of the teacher initiating the surprise event.</param>
    /// <param name="studentEmail">The email address of the student to notify.</param>
    /// <exception cref="Exception">Thrown when there is an issue with session validation or if the teacher or student cannot be retrieved.</exception>
    public void NotifySurpriseStudent(string correlationId, string sessionId, string teacherEmail, string studentEmail)
    {
        IsValidSession(sessionId, studentEmail);
    
        var teacher = _userManager.GetTeacher(teacherEmail);
        var student = _userManager.GetStudent(studentEmail);
    
        var surprisedEventMsg = new SurprisedEvent("Surprised", student).GenerateMsg();
        teacher.Notify(surprisedEventMsg);
    
        Log.InfoFormat(correlationId, $"Notified student {studentEmail} about surprise event");

    }


    /// <summary>
    /// Allows a student to leave a lesson they are currently enrolled in.
    /// </summary>
    /// <param name="correlationId">The correlation ID for tracking logs related to this operation.</param>
    /// <param name="sessionId">The session ID of the logged-in student.</param>
    /// <param name="email">The email address of the student leaving the lesson.</param>
    /// <param name="lessonId">The ID of the lesson from which the student wants to leave.</param>
    /// <exception cref="Exception">Thrown when there is an issue with session validation, if the user is not a student, or if the lesson cannot be found.</exception>
    public void LeaveLesson(string correlationId, string sessionId, string email, string lessonId)
    {
        IsValidSession(sessionId, email);
    
        var user = _userManager.GetUser(email);
        if (user is not Student)
        {
            throw new Exception($"User {user.Email} is not a student");
        }
    
        var lesson = _lessonManager.GetLesson(lessonId);
        lesson.Leave(user);
    
        Log.InfoFormat(correlationId, $"Student {email} left lesson {lessonId}");
    }


    /// <summary>
    /// Retrieves the data related to a specific student, including their details and enrollment summaries in lessons.
    /// </summary>
    /// <param name="correlationId">The correlation ID for tracking logs related to this operation.</param>
    /// <param name="sessionId">The session ID of the teacher initiating the request.</param>
    /// <param name="teacherEmail">The email address of the teacher initiating the request.</param>
    /// <param name="studentEmail">The email address of the student whose data is being retrieved.</param>
    /// <returns>A tuple containing the student details and a list of enrollment summaries in lessons.</returns>
    /// <exception cref="Exception">Thrown when there is an issue with session validation or if the teacher does not exist.</exception>
    public (Student student, List<EnrollmentSummary> enrollments) GetStudentData(string correlationId, string sessionId, string teacherEmail, string studentEmail)
    {
        IsValidSession(sessionId, teacherEmail);
    
        var teacher     = _userManager.GetTeacher(teacherEmail);
        var enrollments = _lessonManager.GetLessonByStudentEmail(correlationId,teacher, studentEmail);
    
        var student = _userManager.GetStudent(studentEmail);
        return (student, enrollments);
    }


    /// <summary>
    /// Adds a teacher's approach or feedback to a student during a lesson.
    /// </summary>
    /// <param name="correlationId">The correlation ID for tracking logs related to this operation.</param>
    /// <param name="sessionId">The session ID of the teacher initiating the request.</param>
    /// <param name="teacherEmail">The email address of the teacher adding the approach.</param>
    /// <param name="lessonId">The ID of the lesson where the approach is being added.</param>
    /// <param name="studentUsername">The username of the student receiving the approach.</param>
    /// <exception cref="Exception">Thrown when there is an issue with session validation, if the teacher or student does not exist, or if the lesson is not found.</exception>
    public void AddTeacherApproach(string correlationId, string sessionId, string teacherEmail, string lessonId, string studentUsername)
    {
        IsValidSession(sessionId, teacherEmail);
    
        var teacher = _userManager.GetTeacher(teacherEmail);
        var lesson  = _lessonManager.GetLesson(lessonId);
        var student = _userManager.GetStudent(studentUsername);
    
        lesson.AddTeacherApproach(teacher, student);
    }

}