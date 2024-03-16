using System.Collections.Concurrent;
using EmotionDetectionSystem.DomainLayer.objects;
using log4net;

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
        _emotionDataTasks         = new ConcurrentQueue<PushEmotionDataTask>();
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

    public void Register(string email, string firstName, string lastName, string password, int userType)
    {
        _userManager.Register(email, firstName, lastName, password, userType);
    }

    public User Login(string sessionId, string email, string password)
    {
        return _userManager.Login(sessionId, email, password);
    }

    public void Logout(string sessionId, string email)
    {
        IsValidSession(sessionId, email);
        _userManager.Logout(sessionId);
    }

    public Lesson CreateLesson(string sessionId, string email, string title, string description, string[] tags)
    {
        IsValidSession(sessionId, email);
        var teacher = _userManager.GetTeacher(email);
        return _lessonManager.CreateLesson(teacher, title, description, tags);
    }

    private void IsValidSession(string sessionId, string email)
    {
        if (!_userManager.IsValidSession(sessionId, email))
        {
            throw new Exception($"Session: {sessionId} is not valid");
        }
    }

    public void EndLesson(string sessionId, string email)
    {
        IsValidSession(sessionId, email);
        _lessonManager.EndLesson(email);
    }

    public Lesson JoinLesson(string sessionId, string email, string entryCode)
    {
        IsValidSession(sessionId, email);
        var user = _userManager.GetUser(email);
        return _lessonManager.JoinLesson(user, entryCode);
    }

    public IEnumerable<EnrollmentSummary> ViewStudentsDuringLesson(
        string sessionId, string email, string lessonId)
    {
        IsValidSession(sessionId, email);
        var user = _userManager.GetUser(email);
        if (user is not Viewer)
        {
            throw new Exception($"User {user.Email} cannot view students data");
        }

        var viewer = (Viewer)user;
        return _lessonManager.ViewStudentsDuringLesson(viewer, lessonId);
    }

    public IEnumerable<Lesson> ViewTeacherDashboard(string sessionId, string email)
    {
        IsValidSession(sessionId, email);
        var user = _userManager.GetUser(email);
        if (user is not Teacher)
        {
            throw new Exception($"User {user.Email} is not a teacher");
        }

        var teacher = (Teacher)user;
        return teacher.Lessons;
    }

    public Student ViewStudent(string sessionId, string email, string studentEmail)
    {
        IsValidSession(sessionId, email);
        var user = _userManager.GetUser(email);
        if (user is not Viewer)
        {
            throw new Exception($"User {user.Email} cannot view students data");
        }

        return _userManager.GetStudent(studentEmail);
    }

    public void PushEmotionData(string sessionId, string email, string lessonId, EmotionData emotionData)
    {
        var pushTaskInfo = new PushEmotionDataTask(sessionId, email, lessonId, emotionData);
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
            throw new Exception($"Error processing emotion data task - {e.Message}");
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

    public IEnumerable<EnrollmentSummary> GetLastEmotionsData(string sessionId, string email, string lessonId)
    {
        IsValidSession(sessionId, email);
        var lesson = _lessonManager.GetLesson(lessonId);
        return lesson.GetEnrollmentSummariesWithData();
    }

    public Lesson GetLesson(string sessionId, string email, string lessonId)
    {
        IsValidSession(sessionId, email);
        return _lessonManager.GetLesson(lessonId);
    }
}