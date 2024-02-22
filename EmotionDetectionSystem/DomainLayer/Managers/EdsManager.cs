using EmotionDetectionSystem.DomainLayer.objects;
using EmotionDetectionSystem.Service;
using EmotionDetectionSystem.ServiceLayer;
using log4net;

namespace EmotionDetectionSystem.DomainLayer.Managers;

public class EdsManager
{
    private                 UserManager   _userManager;
    private                 LessonManager _lessonManager;
    private static readonly ILog          Log = LogManager.GetLogger(typeof(EdsManager));

    public EdsManager()
    {
        _userManager   = new UserManager();
        _lessonManager = new LessonManager();
    }

    public void Register(string email, string firstName, string lastName, string password, bool isStudent)
    {
        _userManager.Register(email, firstName, lastName, password, isStudent);
    }

    public User Login(string sessionId, string email, string password)
    {
        return _userManager.Login(sessionId, email, password);
    }

    public void Logout(string sessionId,string email)
    {
        IsValidSession(sessionId, email); 
        _userManager.Logout(sessionId);
    }

    public Lesson CreateLesson(string sessionId, string email, string title, string description, string[] tags)
    {
        IsValidSession(sessionId, email); 
        Teacher teacher = _userManager.GetTeacher(email);
        return _lessonManager.CreateLesson(teacher, title, description, tags);
    }
    
    private bool IsValidSession(string sessionId, string email)
    {
        if (!_userManager.IsValidSession(sessionId, email))
        {
            throw new Exception("Session is not valid");
        }
        return true;
    }

    public void EndLesson(string sessionId, string email)
    {
        IsValidSession(sessionId, email); 
        _lessonManager.EndLesson(email);
    }

    public User JoinLesson(string sessionId, string entryCode)
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, EnrollmentSummary> ViewStudentsDuringLesson(string sessionId)
    {
        throw new NotImplementedException();
    }

    public List<Lesson> ViewTeacherDashboard(string sessionId)
    {
        throw new NotImplementedException();
    }

    public Response<ServiceUser> ViewStudent(string sessionId)
    {
        throw new NotImplementedException();
    }
}