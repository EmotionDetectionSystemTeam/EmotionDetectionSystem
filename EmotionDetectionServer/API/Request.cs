using EmotionDetectionServer.API;
using EmotionDetectionSystem.ServiceLayer.objects;
using EmotionDetectionSystem.ServiceLayer.objects.charts;
using EmotionDetectionSystem.ServiceLayer.Responses;

namespace EmotionDetectionServer.API
{
    public class EnterAsGuestRequest : IRequest
    {

        public string sessionId { get; set; }


        public EnterAsGuestRequest(string SessionID)
        {
            this.sessionId = SessionID;

        }
    }
    public class RegisterRequest : IRequest
    {

        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public int isStudent { get; set; }

        public RegisterRequest(string email, string firstName, string lastName, string password, string confirmPassword, int isStudent)
        {
            this.email = email;
            this.firstName = firstName;
            this.lastName = lastName;
            this.password = password;
            this.confirmPassword = confirmPassword;
            this.isStudent = isStudent;
        }


    }
    public class LoginRequest : IRequest
    {
        public string sessionId { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public LoginRequest(string SessionId, string Email, string Password)
        {
            this.sessionId = SessionId;
            this.email = Email;
            this.password = Password;
        }
    }

    public class CreateLessonRequest : IRequest
    {
        public string SessionId { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] Tags { get; set; }

        public CreateLessonRequest(string sessionId, string email, string title, string description, string[] tags)
        {
            SessionId = sessionId;
            Email = email;
            Title = title;
            Description = description;
            Tags = tags;
        }
    }

    public class JoinLessonRequest : IRequest
    {
        public string SessionId { get; set; }
        public string Email { get; set; }
        public string EntryCode { get; set; }

        public JoinLessonRequest(string sessionId, string email, string entryCode)
        {
            SessionId = sessionId;
            Email = email;
            EntryCode = entryCode;
        }
    }

    public class LogoutRequest : IRequest
    {
        public string SessionId { get; set; }
        public string Email { get; set; }
        public LogoutRequest(string sessionId, string email)
        {
            SessionId = sessionId;
            Email = email;
        }
    }

    public class EndLessonRequest : IRequest
    {
        public string SessionId { get; set; }
        public string Email { get; set; }

        public EndLessonRequest(string sessionId, string email)
        {
            SessionId = sessionId;
            Email = email;
        }
    }

    public class PushEmotionDataRequest : IRequest
    {
        public string SessionId { get; set; }
        public string Email { get; set; }
        public string LessonId { get; set; }
        public ServiceEmotionData EmotionData { get; set; }

        public PushEmotionDataRequest(string sessionId, string email, string lessonId, ServiceEmotionData emotionData)
        {
            SessionId = sessionId;
            Email = email;
            LessonId = lessonId;
            EmotionData = emotionData;
        }
    }

    public class GetLastEmotionsDataRequest : IRequest
    {
        public string SessionId { get; set; }
        public string Email { get; set; }
        public string LessonId { get; set; }

        public GetLastEmotionsDataRequest(string sessionId, string email, string lessonId)
        {
            SessionId = sessionId;
            Email = email;
            LessonId = lessonId;
        }
    }

    public class GetLessonRequest : IRequest
    {
        public string SessionId { get; set; }
        public string Email { get; set; }
        public string LessonId { get; set; }

        public GetLessonRequest(string sessionId, string email, string lessonId)
        {
            SessionId = sessionId;
            Email = email;
            LessonId = lessonId;
        }
    }

    public class EmotionNotificationRequest : IRequest
    {
        public string SessionId { get; set; }
        public string TeacherEmail { get; set; }
        public string StudentEmail { get; set; }

        public EmotionNotificationRequest(string sessionId, string teacherEmail, string studentEmail)
        {
            SessionId = sessionId;
            TeacherEmail = teacherEmail;
            StudentEmail = studentEmail;
        }
    }

    public class GetEnrolledLessonsRequest : IRequest
    {
        public string SessionId { get; set; }
        public string TeacherEmail { get; set; }

        public GetEnrolledLessonsRequest(string sessionId, string teacherEmail)
        {
            SessionId = sessionId;
            TeacherEmail = teacherEmail;
        }
    }

    public class GetStudentDataByLessonRequest : IRequest
    {
        public string SessionId { get; set; }
        public string Email { get; set; }
        public string LessonId { get; set; }

        public GetStudentDataByLessonRequest(string sessionId, string email, string lessonId)
        {
            SessionId = sessionId;
            Email = email;
            LessonId = lessonId;
        }
    }

    public class GetAllStudentDataRequest : IRequest
    {
        public string SessionId { get; set; }
        public string Email { get; set; }

        public GetAllStudentDataRequest(string sessionId, string email)
        {
            SessionId = sessionId;
            Email = email;
        }
    }

    public class GetStudentDataRequest : IRequest
    {
        public string SessionId { get; set; }
        public string TeacherEmail { get; set; }
        public string StudentEmail { get; set; }


        public GetStudentDataRequest(string sessionId, string teacherEmail, string studentEmail)
        {
            SessionId = sessionId;
            StudentEmail = studentEmail;
            TeacherEmail = teacherEmail;

        }
    }

    public class LeaveLessonRequest : IRequest
    {
        public string SessionId { get; set; }
        public string Email { get; set; }
        public string LessonId { get; set; }

        public LeaveLessonRequest(string sessionId, string email, string lessonId)
        {
            SessionId = sessionId;
            Email = email;
            LessonId = lessonId;
        }


    }
    public class AddTeacherApproachRequest : IRequest
    {
        public string SessionId { get; set; }
        public string TeacherEmail { get; set; }
        public string LessonId { get; set; }
        public string StudentEmail { get; set; }

        public AddTeacherApproachRequest(string sessionId, string teacherEmail, string lessonId, string studentEmail)
        {
            SessionId = sessionId;
            TeacherEmail = teacherEmail;
            LessonId = lessonId;
            StudentEmail = studentEmail;
        }


    }

}

