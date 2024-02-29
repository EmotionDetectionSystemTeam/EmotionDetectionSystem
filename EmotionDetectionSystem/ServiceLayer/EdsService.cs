using EmotionDetectionSystem.DomainLayer.Managers;
using EmotionDetectionSystem.ServiceLayer.Responses;
using log4net;

namespace EmotionDetectionSystem.ServiceLayer;

public class EdsService : IEdsService
{
    private          EdsManager _edsManager;
    private readonly ILog       _logger = LogManager.GetLogger(typeof(EdsService));

    public EdsService()
    {
        _edsManager = new EdsManager();
    }

    public Response Register(string email, string firstName, string lastName, string password, string confirmPassword,
                             int    userType)
    {
        _logger.InfoFormat($"Registering user with email: {email} has been received");
        try
        {
            _edsManager.Register(email, firstName, lastName, password, userType);
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

    public Response Logout(string sessionId, string email)
    {
        _logger.InfoFormat($"Logout request for session: {sessionId} has been received");
        try
        {
            _edsManager.Logout(sessionId, email);
            _logger.InfoFormat($"Session: {sessionId} has been logged out");
            return new Response();
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error logging out session: {sessionId} - {e.Message}");
            return new Response(e.Message);
        }
    }

    public Response<string> CreateLesson(string   sessionId, string email, string title, string description,
                                         string[] tags)
    {
        _logger.InfoFormat($"Create lesson request for session: {sessionId} has been received");
        try
        {
            var lesson = _edsManager.CreateLesson(sessionId, email, title, description, tags);
            _logger.InfoFormat($"Lesson with title: {title} has been created");
            return Response<string>.FromValue(lesson.EntryCode);
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error creating lesson with title: {title} - {e.Message}");
            return Response<string>.FromError(e.Message);
        }
    }

    public Response EndLesson(string sessionId, string email)
    {
        _logger.InfoFormat($"End lesson request for session: {sessionId} has been received");
        try
        {
            _edsManager.EndLesson(sessionId, email);
            _logger.InfoFormat($"Lesson with session: {sessionId} has been ended");
            return new Response();
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error ending lesson with session: {sessionId} - {e.Message}");
            return new Response(e.Message);
        }
    }

    public Response<ServiceLesson> JoinLesson(string sessionId, string email, string entryCode)
    {
        _logger.InfoFormat($"Join lesson request for session: {sessionId} has been received");
        try
        {
            var lesson = _edsManager.JoinLesson(sessionId, email, entryCode);
            _logger.InfoFormat($"User with session: {sessionId} has joined the lesson");
            return Response<ServiceLesson>.FromValue(new ServiceLesson(lesson));
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error joining lesson with session: {sessionId} - {e.Message}");
            return Response<ServiceLesson>.FromError(e.Message);
        }
    }

    public Response<List<ServiceEnrollmentSummary>> ViewStudentsDuringLesson(
        string sessionId, string email, string lessonId)
    {
        _logger.InfoFormat($"View students during lesson request for session: {sessionId} has been received");
        try
        {
            var enrollmentSummaries = _edsManager.ViewStudentsDuringLesson(sessionId, email, lessonId);
            var enrollmentSummariesService = enrollmentSummaries
                .Select(enrollmentSummary => new ServiceEnrollmentSummary(enrollmentSummary)).ToList();
            _logger.InfoFormat($"View students during lesson request for session: {sessionId} has been completed");
            return Response<List<ServiceEnrollmentSummary>>.FromValue(enrollmentSummariesService);
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error viewing students during lesson with session: {sessionId} - {e.Message}");
            return Response<List<ServiceEnrollmentSummary>>.FromError(e.Message);
        }
    }

    public Response<List<ServiceLesson>> ViewLessonDashboard(string sessionId, string email)
    {
        _logger.InfoFormat($"View lesson dashboard request for session: {sessionId} has been received");
        try
        {
            var lessons = _edsManager.ViewTeacherDashboard(sessionId, email);
            var lessonsService = lessons.Select(lesson => new ServiceLesson(lesson)).ToList();
            _logger.InfoFormat($"View lesson dashboard request for session: {sessionId} has been completed");
            return Response<List<ServiceLesson>>.FromValue(lessonsService);
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error viewing lesson dashboard with session: {sessionId} - {e.Message}");
            return Response<List<ServiceLesson>>.FromError(e.Message);
        }
    }
    
    public Response<ServiceUser> ViewStudent(string sessionId, string email, string studentEmail)
    {
        _logger.InfoFormat($"View student request for session: {sessionId} has been received");
        try
        {
            var student = _edsManager.ViewStudent(sessionId, email, studentEmail);
            _logger.InfoFormat($"View student request for session: {sessionId} has been completed");
            return Response<ServiceUser>.FromValue(new ServiceUser(student));
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error viewing student with session: {sessionId} - {e.Message}");
            return Response<ServiceUser>.FromError(e.Message);
        }
    }
    
    public Response PushEmotionData(string sessionId, string email, string lessonId, ServiceEmotionData emotionData)
    {
        _logger.InfoFormat($"Push emotion data request for session: {sessionId} has been received");
        try
        {
            _edsManager.PushEmotionData(sessionId, email, lessonId, emotionData.ToDomainObject());
            _logger.InfoFormat($"Push emotion data request for session: {sessionId} has been completed");
            return new Response();
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error pushing emotion data with session: {sessionId} - {e.Message}");
            return new Response(e.Message);
        }
    }
}