using EmotionDetectionSystem.DomainLayer.objects;
using EmotionDetectionSystem.Service;
using EmotionDetectionSystem.ServiceLayer;
using log4net;

namespace EmotionDetectionSystem.DomainLayer.Managers;

public class EdsManager
{
    private UserManager _userManager;
    private LessonManager _lessonManager;
    private static readonly ILog Log = LogManager.GetLogger(typeof(EdsManager));
    public EdsManager(UserManager userManager, LessonManager lessonManager)
    {
        _userManager = userManager;
        _lessonManager = lessonManager;
    }
    
    public void Register(string email, string firstName, string lastName, string password, bool isStudent)
    {
        throw new NotImplementedException();
    }

    public string Login(string sessionId, string email, string password)
    {
        throw new NotImplementedException();
    }
    public void Logout(string sessionId)
    {
        throw new NotImplementedException();
    }
    
    public Lesson CreateLesson(string sessionId, string title, string description, string[] tags)
    {
        throw new NotImplementedException();
    }

    public void EndLesson(string sessionId)
    {
        throw new NotImplementedException();
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