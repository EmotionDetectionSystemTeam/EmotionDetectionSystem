using System.Text;
using EmotionDetectionSystem.DomainLayer.objects;
using EmotionDetectionSystem.DomainLayer.Repos;
using EmotionDetectionSystem.ServiceLayer.objects;
using EmotionDetectionSystem.ServiceLayer.Responses;
using log4net;

namespace EmotionDetectionSystem.DomainLayer.Managers;

public class LessonManager
{
    private                 LessonRepo _lessonRepo;
    private                 long       _lessonIdFactory = 1;
    private static readonly ILog       Log              = LogManager.GetLogger(typeof(LessonManager));

    public LessonManager()
    {
        _lessonRepo = new LessonRepo();
    }

    /// <summary>
    /// Creates a new lesson.
    /// </summary>
    /// <param name="correlationId">The correlation ID for logging.</param>
    /// <param name="teacher">The teacher creating the lesson.</param>
    /// <param name="title">The title of the lesson.</param>
    /// <param name="description">The description of the lesson.</param>
    /// <param name="tags">The tags associated with the lesson.</param>
    /// <returns>The created lesson.</returns>
    public Lesson CreateLesson(string correlationId, Teacher teacher, string title, string description, string[] tags)
    {
        Log.Info($"[{correlationId}] Creating lesson titled: {title} by teacher: {teacher.Email}");

        if (HasActiveLesson(correlationId, teacher.Email))
        {
            throw new Exception("Teacher already has an active lesson");
        }

        var newLesson = new Lesson(_lessonIdFactory++.ToString(), teacher, title, description, GenerateEntryCode(),
                                   tags.ToList());
        _lessonRepo.Add(newLesson);
        teacher.AddLesson(newLesson);
        return newLesson;
    }

    private bool HasActiveLesson(string correlationId, string email)
    {
        var lessons            = _lessonRepo.GetByTeacherEmail(email);
        var activeLessonExists = lessons.Any(l => l.IsActive);
        if (activeLessonExists)
        {
            Log.Warn($"[{correlationId}] Teacher with email {email} already has an active lesson");
        }
        return activeLessonExists;
    }


    /// <summary>
    /// Ends the active lesson for the specified teacher.
    /// </summary>
    /// <param name="correlationId">The correlation ID for logging.</param>
    /// <param name="email">The email address of the teacher.</param>
    public void EndLesson(string correlationId, string email)
    {
        Log.Info($"[{correlationId}] Ending active lesson for teacher: {email}");

        var lessons = _lessonRepo.GetByTeacherEmail(email);
        var lesson  = lessons.FirstOrDefault(l => l.IsActive);
        if (lesson == null)
        {
            Log.Warn($"[{correlationId}] No active lesson found for teacher: {email}");
            throw new Exception("No active lesson found");
        }

        lesson.EndLesson();
        _lessonRepo.Update(lesson);
    }

    /// <summary>
    /// Allows a user to join a lesson using the provided entry code.
    /// </summary>
    /// <param name="correlationId">The correlation ID for logging.</param>
    /// <param name="user">The user who wants to join the lesson.</param>
    /// <param name="entryCode">The entry code for the lesson.</param>
    /// <returns>The joined lesson.</returns>
    public Lesson JoinLesson(string correlationId, User user, string entryCode)
    {
        Log.Info($"[{correlationId}] User {user.Email} is joining lesson with entry code: {entryCode}");

        var lessons = _lessonRepo.GetByEntryCode(entryCode);
        var lesson  = lessons.FirstOrDefault(l => l.IsActive);
        if (lesson == null)
        {
            Log.Warn($"[{correlationId}] Invalid entry code: {entryCode}");
            throw new Exception("Invalid entry code");
        }

        user.JoinLesson(lesson);
        return lesson;
    }

    /// <summary>
    /// Retrieves enrollment summaries for students during a lesson for a viewer.
    /// </summary>
    /// <param name="correlationId">The correlation ID for logging.</param>
    /// <param name="viewer">The Viewer object requesting student data.</param>
    /// <param name="lessonId">The ID of the lesson.</param>
    /// <returns>A List of EnrollmentSummary objects.</returns>
    public List<EnrollmentSummary> ViewStudentsDuringLesson(string correlationId, Viewer viewer, string lessonId)
    {
        Log.Info($"[{correlationId}] Viewer {viewer.Email} is viewing students during lesson with ID: {lessonId}");

        var lesson = _lessonRepo.GetById(lessonId);
        if (!lesson.IsAllowedToViewStudentsData(viewer))
        {
            Log.Warn($"[{correlationId}] Viewer {viewer.Email} is not allowed to view students data for lesson {lessonId}");
            throw new Exception("Viewer is not allowed to view students data");
        }

        return lesson.GetEnrollmentSummaries();
    }


    public Response<ServiceUser> ViewStudent(string sessionId)
    {
        throw new NotImplementedException();
    }

    private static string GenerateEntryCode(int length = 15)
    {
        var random = new Random();

        const string allowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        var code = new StringBuilder();

        for (var i = 0; i < length; i++)
        {
            var index = random.Next(0, allowedChars.Length);
            code.Append(allowedChars[index]);
        }

        return code.ToString();
    }

    public Lesson GetLesson(string lessonId)
    {
        return _lessonRepo.GetById(lessonId);
    }

    public void AddViewer(string lessonId, Viewer viewer)
    {
        var lesson = _lessonRepo.GetById(lessonId);
        lesson.AddViewer(viewer);
        _lessonRepo.Update(lesson);
    }

    public List<EnrollmentSummary> GetLessonByStudentEmail(string correlationId, Teacher teacher, string studentEmail)
    {
        Log.Info($"[{correlationId}] Getting lessons for student with email: {studentEmail}");
        var studentLessons = new List<EnrollmentSummary>();
        foreach (var lesson in teacher.Lessons)
        {
            var enrollmentSummary = lesson.GetEnrollmentSummaries().Find(x => x.Student.Email.Equals(studentEmail));
            if (enrollmentSummary != null)
            {
                studentLessons.Add(enrollmentSummary);
            }
        }
        Log.Info($"[{correlationId}] Found {studentLessons.Count} lessons for student with email: {studentEmail}");

        return studentLessons;
    }
}