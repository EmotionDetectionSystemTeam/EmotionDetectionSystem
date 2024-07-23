using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.DataLayer
{
    public interface IDBHandler
    {
        void CleanDB();
        bool UserExistsByEmail(string email);
        User GetUserByEmail(string email);
        void AddUser(User newUser);
        void RemoveUser(string userEmail);
        List<Lesson> GetAllLessons();
        Lesson GetByEntryCode(string entryCode);
        Lesson GetLessonById(string id);
        void AddLesson(Lesson item);
        List<Lesson> GetByTeacherEmail(string email);
        void CanConnectToDatabase();
        User GetUserById(string id);
        List<User> GetAllUsers();
        Teacher GetTeacherByEmail(string email);
        List<EnrollmentSummary> GetAllEnrollmentSummariesPerLesson(string lessonId);
        EnrollmentSummary GetEnrollmentSummaryByEmail(string lessonId, string email);
        Dictionary<string, List<string>> GetStudentWiningEmotions(string lessonId);
        void AddEnrollmentSummary(EnrollmentSummary item);
        bool EnrollmentSummaryContainStudent(Student student, string lessonId);
        EnrollmentSummary GetEnrollmentSummaryPerStudentLesson(string userEmail, string lessonId);
        void PutEmotionData(string userEmail, string lessonId, EmotionData emotionData);
        IEnumerable<EmotionData> GetEmotionDataEntries(string lessonId);
        IEnumerable<EnrollmentSummary> GetEnrollmentSummariesWithData(string lessonId);
        Dictionary<Student, EnrollmentSummary> GetStudentsEmotions(string lessonId);
        void UpdateLesson(Lesson item);
    }
}
