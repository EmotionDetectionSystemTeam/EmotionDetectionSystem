using EmotionDetectionSystem.DomainLayer.objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using EmotionDetectionSystem.DomainLayer.Repos;

namespace EmotionDetectionSystem.DataLayer
{
    public class DBHandler : IDBHandler
    {

        #region properties
        private static readonly object databaseLock = new object();

        private readonly string DbErrorMessage = "Unfortunatly connecting to the db faild, please try again in a few minutes";

        private bool testMode = false;

        public bool TestMode { get => testMode; set => testMode = value; }

        private static IDBHandler instance = null;
        #endregion

        #region constructor
        public static IDBHandler Instance
        {
            get
            {
                lock (databaseLock)
                {
                    if (instance == null)
                    {
                        instance = new DBHandler();
                    }
                    return instance;
                }
            }
            set { instance = value; }
        }

        public DBHandler()
        {

        }
        #endregion

        #region Methods

        #region Clean DB
        public void CleanDB()
        {
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            db.Lessons.RemoveRange(db.Lessons);
                            db.Users.RemoveRange(db.Users);
                            db.EnrollmentSummaries.RemoveRange(db.EnrollmentSummaries);
                            db.EmotionData.RemoveRange(db.EmotionData);
                            db.SaveChanges(true);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("setting up database for tests failed!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(DbErrorMessage);
                }
            }
        }
        #endregion

        #region Members managment

        public bool UserExistsByEmail(string email)
        {
            bool result = false;

            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            result = db.Users.FirstOrDefault(m => m.Email.ToLower().Equals(email.ToLower())) != null;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("failed tofind member by email");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(DbErrorMessage);
                }

                return result;
            }
        }

        public User GetUserByEmail(string email)
        {
            User result;
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            result = db.Users.FirstOrDefault(m => m.Email.ToLower().Equals(email.ToLower()));
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("failed to get member by email. user isn't exist.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(DbErrorMessage);
                }
            }
            return result;
        }

        public void AddUser(User newUser)
        {
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            db.Users.Add(newUser);

                            db.SaveChanges(true);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("failed to interact with users table");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(DbErrorMessage);
                }
            }
        }

        public void RemoveUser(string userEmail)
        {
            if (!testMode)
            {
                lock (this)
                {
                    try
                    {
                        using (var db = DatabaseContextFactory.ConnectToDatabase())
                        {
                            try
                            {
                                var memberFound = db.Users.FirstOrDefault(m => m.Email.Equals(userEmail));

                                if (memberFound != null)
                                {
                                    db.Users.Remove(memberFound);
                                    db.SaveChanges(true);
                                }
                            }
                            catch (Exception ex)
                            {
                                //throw new Exception("failed to interact with stores table");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(DbErrorMessage);
                    }
                }
            }
        }

        public List<Lesson> GetAllLessons()
        {
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            List<Lesson> lessons = db.Lessons.Include(l => l.Teacher).ToList<Lesson>();
                            foreach (var lesson in lessons)
                            {
                                lesson.EnrollmentSummaryRepo = new EnrollmentSummaryRepo(lesson.LessonId);

                            }
                            return lessons;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("failed to interact with lessons table");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(DbErrorMessage);
                }
            }
        }

        public Lesson GetByEntryCode(string entryCode)
        {
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            return db.Lessons.Include(l => l.Teacher).FirstOrDefault(m => m.EntryCode.ToLower().Equals(entryCode.ToLower()));
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("failed to fatch lesson by entry code.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(DbErrorMessage);
                }
            }
        }

        public Lesson GetLessonById(string id)
        {
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            Lesson lesson = db.Lessons.Include(l => l.Teacher).FirstOrDefault(m => m.LessonId.Equals(id));
                            if (lesson == null)
                                throw new Exception($"failed to find a lesson by the id {id}.");
                            lesson.EnrollmentSummaryRepo = new EnrollmentSummaryRepo(id);
                            lesson.Viewers = new List<Viewer>();
                            return lesson;

                        }
                        catch (Exception ex)
                        {
                            throw new Exception("failed to fatch lesson by id.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(DbErrorMessage);
                }
            }
        }

        public void AddLesson(Lesson item)
        {
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            // Retrieve the existing teacher from the database
                            Teacher teacher = db.Users.OfType<Teacher>()
                                                      .Include(t => t.Lessons)
                                                      .FirstOrDefault(m => m.Email == item.Teacher.Email);

                            if (teacher != null)
                            {
                                // Check if the lesson is already being tracked by the context
                                var existingLesson = db.Lessons.Local.FirstOrDefault(l => l.Id == item.Id);
                                if (existingLesson != null)
                                {
                                    // Detach the existing lesson to avoid conflicts
                                    db.Entry(existingLesson).State = EntityState.Detached;
                                }

                                // Attach the new lesson to the context
                                db.Entry(item).State = EntityState.Added;

                                // Add the lesson to the teacher's lessons
                                teacher.Lessons.Add(item);

                                db.SaveChanges();
                            }
                            else
                            {
                                throw new Exception("Teacher not found.");
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Failed to add a new lesson: " + ex.Message, ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error in AddLesson method: " + ex.Message, ex);
                }
            }
        }
        public List<Lesson> GetByTeacherEmail(string email)
        {
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            return db.Lessons.Include(l => l.Teacher).Where(l => l.Teacher.Email.ToLower().Equals(email)).ToList();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("failed to fetch lessons by teacher email.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(DbErrorMessage);
                }
            }
        }
        #endregion

        public void CanConnectToDatabase()
        {
            if (!testMode)
            {
                lock (this)
                {
                    try
                    {
                        using (var db = DatabaseContextFactory.ConnectToDatabase())
                        {
                            db.Database.GetDbConnection().Open();
                            db.Database.GetDbConnection().Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(DbErrorMessage + " or check your internet Connection");
                    }
                }
            }
        }

        public User GetUserById(string id)
        {
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            return db.Users.FirstOrDefault(m => m.Email.Equals(id));
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("failed to fatch lesson by id.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(DbErrorMessage);
                }
            }
        }

        public List<User> GetAllUsers()
        {
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            return db.Users.ToList<User>();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("failed to interact with users table");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(DbErrorMessage);
                }
            }
        }

        public Teacher GetTeacherByEmail(string email)
        {
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            // Retrieve the existing teacher from the database
                            Teacher teacher = db.Users.OfType<Teacher>().Include(t => t.Lessons).FirstOrDefault(m => m.Email == email);

                            if (teacher != null)
                            {

                                // Add the lesson to the teacher's lessons
                                //teacher.Lessons = db.Lessons.Where(l => l.Teacher.Email == email).ToList<Lesson>();
                                return teacher;
                            }
                            else
                            {
                                throw new Exception("Teacher not found.");
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Failed to add a new lesson: " + ex.Message, ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error in AddLesson method: " + ex.Message, ex);
                }
            }
        }

        public List<EnrollmentSummary> GetAllEnrollmentSummariesPerLesson(string lessonId)
        {
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            return db.EnrollmentSummaries.Include(e => e.Student).Include(e => e.Lesson).Include(e => e.EmotionData).Where(e => e.Lesson.LessonId.Equals(lessonId)).ToList<EnrollmentSummary>();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("failed to interact with enrollment summaries table");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(DbErrorMessage);
                }
            }
        }

        public EnrollmentSummary GetEnrollmentSummaryByEmail(string lessonId, string email)
        {
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            return db.EnrollmentSummaries.Include(e => e.Student).Include(e => e.Lesson).Include(e => e.EmotionData).FirstOrDefault(e => e.Student.Email.Equals(email) && e.LessonId.Equals(lessonId));
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("failed to fatch enrollment summary by student email.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(DbErrorMessage);
                }
            }
        }
        public Dictionary<string, List<string>> GetStudentWiningEmotions(string lessonId)
        {
            Dictionary<string, List<string>> studentWiningEmotions = new Dictionary<string, List<string>>();
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            foreach (var enrollmentSummary in GetAllEnrollmentSummariesPerLesson(lessonId))
                            {
                                string studentEmail = enrollmentSummary.Student.Email;
                                List<string> studentEmotions = enrollmentSummary.GetAllWiningEmotionData();

                                if (!studentWiningEmotions.ContainsKey(studentEmail))
                                {
                                    studentWiningEmotions.Add(studentEmail, studentEmotions);
                                }
                                else
                                {
                                    studentWiningEmotions[studentEmail].AddRange(studentEmotions);
                                }
                            }
                            return studentWiningEmotions;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("failed to interact with enrollment summaries table and retrive student wining emotions.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(DbErrorMessage);
                }
            }

        }
        public void AddEnrollmentSummary(EnrollmentSummary item)
        {
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            // Attach the existing Lesson to the context to prevent it from being added as a new entity
                            var existingLesson = db.Lessons.FirstOrDefault(s => s.LessonId == item.Lesson.LessonId);
                            var existingStudent = db.Users.OfType<Student>().FirstOrDefault(s => s.Email == item.Student.Email);
                            if (existingLesson != null && existingStudent != null)
                            {
                                db.Entry(existingLesson).State = EntityState.Unchanged;
                                item.Lesson = existingLesson;
                                db.Entry(existingStudent).State = EntityState.Unchanged;
                                item.Student = existingStudent;
                            }
                            else
                            {
                                throw new Exception("The lesson or student associated with the enrollment summary does not exist in the database.");
                            }

                            db.EnrollmentSummaries.Add(item);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("failed to interact with enrollment summaries table and add a new item.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(DbErrorMessage + ex);
                }
            }
        }

        public bool EnrollmentSummaryContainStudent(Student student, string lessonId)
        {
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            List<EnrollmentSummary> enrollmentSummaries = db.EnrollmentSummaries.Where(e => e.LessonId.Equals(lessonId) && e.Student.Email.Equals(student.Email)).ToList<EnrollmentSummary>();
                            return !enrollmentSummaries.IsNullOrEmpty();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("failed to interact with enrollment summaries table and check if enrollment summary contain student.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(DbErrorMessage);
                }
            }
        }
        public EnrollmentSummary GetEnrollmentSummaryPerStudentLesson(string userEmail, string lessonId)
        {
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            return db.EnrollmentSummaries.Include(e => e.Student).Include(e => e.Lesson).Include(e => e.EmotionData).FirstOrDefault(e => e.Student.Email.Equals(userEmail) && e.LessonId.Equals(lessonId));

                        }
                        catch (Exception ex)
                        {
                            throw new Exception("failed to interact with enrollment summaries table and fetch enrollment summary.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(DbErrorMessage);
                }
            }
        }

        public void PutEmotionData(string userEmail, string lessonId, EmotionData emotionData)
        {
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            EnrollmentSummary enrollmentSummary = GetEnrollmentSummaryPerStudentLesson(userEmail, lessonId);
                            if (enrollmentSummary == null)
                            {
                                throw new Exception($" Student with email {userEmail} not found in lesson");
                            }
                            else
                            {
                                enrollmentSummary.AddEmotionData(emotionData);
                                db.EnrollmentSummaries.Update(enrollmentSummary);
                                db.SaveChanges(true);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("failed to interact with enrollment summaries table and add a new item.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(DbErrorMessage);
                }
            }
        }
        public IEnumerable<EmotionData> GetEmotionDataEntries(string lessonId)
        {
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            var emotionsData = new List<EmotionData>();
                            List<EnrollmentSummary> enrollmentSummaries = GetAllEnrollmentSummariesPerLesson(lessonId);
                            return emotionsData.Concat(enrollmentSummaries.SelectMany(x => x.GetAllEmotionData()));
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("failed to interact with enrollment summaries table and fetch emotion data.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(DbErrorMessage);
                }
            }
        }
        public IEnumerable<EnrollmentSummary> GetEnrollmentSummariesWithData(string lessonId)
        {
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            List<EnrollmentSummary> enrollmentSummaries = GetAllEnrollmentSummariesPerLesson(lessonId);
                            return enrollmentSummaries.Where(enrollmentSummary => enrollmentSummary.PeekFirstNotSeenEmotionData() != null).ToList();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("failed to interact with enrollment summaries table and fetch GetEnrollmentSummariesWithData.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(DbErrorMessage);
                }
            }
        }

        public Dictionary<Student, EnrollmentSummary> GetStudentsEmotions(string lessonId)
        {
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            List<EnrollmentSummary> enrollmentSummaries = GetAllEnrollmentSummariesPerLesson(lessonId);
                            return enrollmentSummaries.ToDictionary(enrollmentSummary => enrollmentSummary.Student, enrollmentSummary => enrollmentSummary);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("failed to interact with enrollment summaries table and fetch the studnets' emotions.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(DbErrorMessage);
                }
            }
        }

        public void UpdateLesson(Lesson item)
        {
            lock (this)
            {
                try
                {
                    using (var db = DatabaseContextFactory.ConnectToDatabase())
                    {
                        try
                        {
                            Lesson lesson = GetLessonById(item.LessonId);
                            if (lesson == null)
                                throw new Exception($"failed to update lesson with lesson id: {item.LessonId}.");
                            db.Lessons.Update(item);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("failed to interact with enrollment summaries table and fetch the studnets' emotions." + ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(DbErrorMessage);
                }
            }
        }
        #endregion
    }
}
