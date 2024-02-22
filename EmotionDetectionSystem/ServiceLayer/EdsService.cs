using EmotionDetectionSystem.DomainLayer.Managers;
using EmotionDetectionSystem.DomainLayer.objects;
using EmotionDetectionSystem.Service;
using EmotionDetectionSystem.ServiceLayer;
using log4net;

namespace EmotionDetectionSystem.ServiceLayer;

public class EdsService : IEdsService
{
    private EdsManager _edsManager;
    private ILog _logger = LogManager.GetLogger(typeof(EdsService));
    public Response Register(string email, string firstName, string lastName, string password, string confirmPassword,
        bool isStudent)
    {
        _logger.InfoFormat($"Registering user with email: {email} has been received");
        try
        {
            _edsManager.Register(email, firstName, lastName, password, isStudent);
            return new Response();
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error registering user with email: {email} - {e.Message}");
            return new Response(e.Message);
        }
    }

    public Response<ServiceUser> Login(string sessionId, string email, string password)
    {
        _logger.InfoFormat($"Login request for user with email: {email} has been received");
        try
        {
            var user = _edsManager.Login(sessionId, email, password);
            _logger.InfoFormat(user.Type.Equals("Student")
                                   ? $"Student with email: {email} has been logged in"
                                   : $"Teacher with email: {email} has been logged in");
            return Response<ServiceUser>.FromValue(new ServiceUser(user));
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error logging in user with email: {email} - {e.Message}");
            return Response<ServiceUser>.FromError(e.Message);
        }
    }

    public Response Logout(string sessionId)
    {
        _logger.InfoFormat($"Logout request for session: {sessionId} has been received");
        try
        {
            _edsManager.Logout(sessionId);
            _logger.InfoFormat($"Session: {sessionId} has been logged out");
            return new Response();
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error logging out session: {sessionId} - {e.Message}");
            return new Response(e.Message);
        }
    }

    public Response<string> CreateLesson(string sessionId, string title, string description, string[] tags)
    {
        throw new NotImplementedException();
    }

    public Response EndLesson(string sessionId)
    {
        throw new NotImplementedException();
    }

    public Response<ServiceUser> JoinLesson(string sessionId, string entryCode)
    {
        throw new NotImplementedException();
    }

    public Response<Dictionary<int, ServiceEnrollmentSummary>> ViewStudentsDuringLesson(string sessionId)
    {
        throw new NotImplementedException();
    }

    public Response<List<ServiceLesson>> ViewLessonDashboard(string sessionId)
    {
        throw new NotImplementedException();
    }

    public Response<ServiceUser> ViewStudent(string sessionId)
    {
        throw new NotImplementedException();
    }
}