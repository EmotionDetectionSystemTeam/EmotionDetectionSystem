using EmotionDetectionSystem.DomainLayer.Managers;
using EmotionDetectionSystem.DomainLayer.objects;
using EmotionDetectionSystem.ServiceLayer.objects;
using EmotionDetectionSystem.ServiceLayer.objects.charts;
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
    public void Dispose()
    {
        _edsManager.StopProcessingTasks();
        _edsManager.ClearAll();
    }

    public Response Register(string email, string firstName, string lastName, string password, string confirmPassword,
                             int    userType)
    {
        _logger.InfoFormat($"Registering user with email: {email} has been received");
        try
        {
            var correlationId = Guid.NewGuid().ToString();
            _edsManager.Register(correlationId, email, firstName, lastName, password, userType);
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
            var correlationId = Guid.NewGuid().ToString();
            var user = _edsManager.Login(correlationId,sessionId, email, password);
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
            var correlationId = Guid.NewGuid().ToString();
            _edsManager.Logout(correlationId,sessionId, email);
            _logger.InfoFormat($"Session: {sessionId} has been logged out");
            return new Response();
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error logging out session: {sessionId} - {e.Message}");
            return new Response(e.Message);
        }
    }

    public Response<SActiveLesson> CreateLesson(string   sessionId, string email, string title, string description,
                                                string[] tags)
    {
        _logger.InfoFormat($"Create lesson request for session: {sessionId} has been received");
        try
        {
            var correlationId = Guid.NewGuid().ToString();
            var lesson = _edsManager.CreateLesson(correlationId,sessionId, email, title, description, tags);
            _logger.InfoFormat($"Lesson with title: {title} has been created");
            return Response<SActiveLesson>.FromValue(new SActiveLesson(lesson));
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error creating lesson with title: {title} - {e.Message}");
            return Response<SActiveLesson>.FromError(e.Message);
        }
    }

    public Response EndLesson(string sessionId, string email)
    {
        _logger.InfoFormat($"End lesson request for session: {sessionId} has been received");
        try
        {
            var correlationId = Guid.NewGuid().ToString();
            _edsManager.EndLesson(correlationId,sessionId, email);
            _logger.InfoFormat($"Lesson with session: {sessionId} has been ended");
            return new Response();
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error ending lesson with session: {sessionId} - {e.Message}");
            return new Response(e.Message);
        }
    }

    public Response<SActiveLesson> JoinLesson(string sessionId, string email, string entryCode)
    {
        _logger.InfoFormat($"Join lesson request for session: {sessionId} has been received");
        try
        {
            var correlationId = Guid.NewGuid().ToString();
            var lesson = _edsManager.JoinLesson(correlationId,sessionId, email, entryCode);
            _logger.InfoFormat($"User with session: {sessionId} has joined the lesson");
            return Response<SActiveLesson>.FromValue(new SActiveLesson(lesson));
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error joining lesson with session: {sessionId} - {e.Message}");
            return Response<SActiveLesson>.FromError(e.Message);
        }
    }

    public Response<List<ServiceEnrollmentSummary>> ViewStudentsDuringLesson(
        string sessionId, string email, string lessonId)
    {
        _logger.InfoFormat($"View students during lesson request for session: {sessionId} has been received");
        try
        {
            var correlationId = Guid.NewGuid().ToString();
            var enrollmentSummaries = _edsManager.ViewStudentsDuringLesson(correlationId,sessionId, email, lessonId);
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
            var correlationId = Guid.NewGuid().ToString();
            var lessons        = _edsManager.ViewTeacherDashboard(correlationId,sessionId, email);
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
            var correlationId = Guid.NewGuid().ToString();
            var student = _edsManager.ViewStudent(correlationId,sessionId, email, studentEmail);
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
            var correlationId = Guid.NewGuid().ToString();
            _edsManager.PushEmotionData(correlationId,sessionId, email, lessonId, emotionData.ToDomainObject());
            _logger.InfoFormat($"Push emotion data request for session: {sessionId} has been completed");
            return new Response();
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error pushing emotion data with session: {sessionId} - {e.Message}");
            return new Response(e.Message);
        }
    }

    public Response EnterAsGuest(string sessionId)
    {
        _logger.InfoFormat($"Enter as guest request for session: {sessionId} has been received");
        return new Response();
    }

    public Response<List<ServiceRealTimeUser>> GetLastEmotionsData(string sessionId, string email, string lessonId)
    {
        _logger.InfoFormat($"Get last emotion data request for session: {sessionId} has been received");
        try
        {
            var correlationId = Guid.NewGuid().ToString();
            var enrollmentSummaries = _edsManager.GetLastEmotionsData(correlationId,sessionId, email, lessonId);

            var realTimeUsers = enrollmentSummaries
                .Select(enrollmentSummary => new ServiceRealTimeUser(
                            enrollmentSummary.Student,
                            enrollmentSummary.GetFirstNotSeenEmotionData().GetWinningEmotion(),
                            enrollmentSummary.GetPreviousEmotionData()
                        )).ToList();

            return Response<List<ServiceRealTimeUser>>.FromValue(realTimeUsers);
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error getting last emotion data with session: {sessionId} - {e.Message}");
            return Response<List<ServiceRealTimeUser>>.FromError(e.Message);
        }
    }

    public Response<SActiveLesson> GetLesson(string sessionId, string email, string lessonId)
    {
        _logger.InfoFormat($"Get lesson request for session: {sessionId} has been received");
        try
        {
            var lesson = _edsManager.GetLesson(sessionId, email, lessonId);
            return Response<SActiveLesson>.FromValue(new SActiveLesson(lesson));
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error getting lesson with session: {sessionId} - {e.Message}");
            return Response<SActiveLesson>.FromError(e.Message);
        }
    }

    public Response<List<LessonOverview>> GetEnrolledLessons(string sessionId, string teacherEmail)
    {
        _logger.InfoFormat($"Get enrolled lesson request for session: {sessionId} has been received");
        try
        {
            var lessons        = _edsManager.GetEnrolledLessons(sessionId, teacherEmail);
            var lessonsService = lessons.Select(lesson => new LessonOverview(lesson)).ToList();
            return Response<List<LessonOverview>>.FromValue(lessonsService);
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error getting enrolled lesson with session: {sessionId} - {e.Message}");
            return Response<List<LessonOverview>>.FromError(e.Message);
        }
    }

    public Response<List<StudentInClassOverview>> GetStudentDataByLesson(
        string sessionId, string teacherEmail, string lessonId)
    {
        _logger.InfoFormat($"Get student data by lesson request for session: {sessionId} has been received");
        try
        {
            var correlationId = Guid.NewGuid().ToString();
            var enrollmentSummaries = _edsManager.GetStudentDataByLesson(correlationId,sessionId, teacherEmail, lessonId);
            var studentInClassOverviews = enrollmentSummaries
                .Select(enrollmentSummary => new StudentInClassOverview(enrollmentSummary)).ToList();
            return Response<List<StudentInClassOverview>>.FromValue(studentInClassOverviews);
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error getting student data by lesson with session: {sessionId} - {e.Message}");
            return Response<List<StudentInClassOverview>>.FromError(e.Message);
        }
    }

    public Response<List<StudentOverview>> GetAllStudentsData(string sessionId, string teacherEmail)
    {
        _logger.InfoFormat($"Get student data request for session: {sessionId} has been received");
        try
        {
            var correlationId = Guid.NewGuid().ToString();
            var enrollmentSummariesDict = _edsManager.GetAllStudentsData(correlationId,sessionId, teacherEmail);
            var response = enrollmentSummariesDict
                .Select(enrollmentSummary => new StudentOverview(enrollmentSummary.Value[0].Student, enrollmentSummary.Value))
                .ToList();
            return Response<List<StudentOverview>>.FromValue(response);
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error getting student data with session: {sessionId} - {e.Message}");
            return Response<List<StudentOverview>>.FromError(e.Message);
        }
    }

    public Response<StudentOverview> GetStudentData(string sessionId, string teacherEmail, string studentEmail)
    {
        _logger.InfoFormat($"Get student data request for session: {sessionId} has been received");
        try
        {
            var correlationId = Guid.NewGuid().ToString();
            var enrollmentSummaries = _edsManager.GetStudentData(correlationId,sessionId, teacherEmail, studentEmail);
            var studentOverview     = new StudentOverview(enrollmentSummaries.Item1, enrollmentSummaries.Item2);
            return Response<StudentOverview>.FromValue(studentOverview);
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error getting student data with session: {sessionId} - {e.Message}");
            return Response<StudentOverview>.FromError(e.Message);
        }
    }

    public Response NotifySurpriseStudent(string sessionId, string teacherEmail, string studentEmail)
    {
        _logger.InfoFormat($"Notify surprise student request for session: {sessionId} has been received");
        try
        {
            var correlationId = Guid.NewGuid().ToString();
            _edsManager.NotifySurpriseStudent(correlationId,sessionId, teacherEmail, studentEmail);
            return new Response();
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error notifying surprise student with session: {sessionId} - {e.Message}");
            return new Response(e.Message);
        }
    }

    public Response LeaveLesson(string sessionId, string email, string lessonId)
    {
        _logger.InfoFormat($"Leave lesson request for session: {sessionId} has been received");
        try
        {
            var correlationId = Guid.NewGuid().ToString();
            _edsManager.LeaveLesson(correlationId,sessionId, email, lessonId);
            return new Response();
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error leaving lesson with session: {sessionId} - {e.Message}");
            return new Response(e.Message);
        }
    }
    
    public Response AddTeacherApproach(string sessionId, string teacherEmail, string lessonId, string studentUsername){
        _logger.InfoFormat($"Add teacher approach request for session: {sessionId} has been received");
        try
        {
            var correlationId = Guid.NewGuid().ToString();
            _edsManager.AddTeacherApproach(correlationId,sessionId, teacherEmail, lessonId, studentUsername);
            return new Response();
        }
        catch (Exception e)
        {
            _logger.ErrorFormat($"Error adding teacher approach with session: {sessionId} - {e.Message}");
            return new Response(e.Message);
        }
    }
}