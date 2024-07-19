using EmotionDetectionSystem.DomainLayer.Events;
using EmotionDetectionSystem.DomainLayer.Repos;
using log4net;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmotionDetectionSystem.DomainLayer.objects
{
    /// <summary>
    /// Represents a lesson in the Emotion Detection System.
    /// </summary>
    [Table("Lesson")]
    public class Lesson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        private string _lessonId;
        private string _lessonName;
        private string _description;
        private Teacher _teacher;
        private List<Viewer> _viewers;
        private DateTime _date;
        private bool _isActive;
        private string _entryCode;
        private List<string> _tags;
        private EnrollmentSummaryRepo _enrollmentSummaryRepo;
        private static readonly ILog Log = LogManager.GetLogger(typeof(Lesson));

        public Lesson() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Lesson"/> class.
        /// </summary>
        /// <param name="lessonId">The lesson identifier.</param>
        /// <param name="teacher">The teacher for the lesson.</param>
        /// <param name="lessonName">The name of the lesson.</param>
        /// <param name="description">The description of the lesson.</param>
        /// <param name="entryCode">The entry code for accessing the lesson.</param>
        /// <param name="tags">The tags associated with the lesson.</param>
        public Lesson(string lessonId, Teacher teacher, string lessonName, string description, string entryCode, List<string> tags)
        {
            _lessonId = lessonId;
            _teacher = teacher;
            _lessonName = lessonName;
            _description = description;
            _date = DateTime.Now;
            _isActive = true;
            _entryCode = entryCode;
            _tags = tags;
            _enrollmentSummaryRepo = new EnrollmentSummaryRepo(lessonId);
            _viewers = new List<Viewer>();
        }

        /// <summary>
        /// Gets or sets the name of the lesson.
        /// </summary>
        public string LessonName
        {
            get => _lessonName;
            set => _lessonName = value;
        }

        /// <summary>
        /// Gets or sets the teacher of the lesson.
        /// </summary>
        public Teacher Teacher
        {
            get => _teacher;
            set => _teacher = value;
        }

        /// <summary>
        /// Gets or sets the date of the lesson.
        /// </summary>
        public DateTime Date
        {
            get => _date;
            set => _date = value;
        }

        /// <summary>
        /// Gets or sets whether the lesson is active.
        /// </summary>
        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }

        /// <summary>
        /// Gets or sets the entry code for accessing the lesson.
        /// </summary>
        public string EntryCode
        {
            get => _entryCode;
            set => _entryCode = value;
        }

        /// <summary>
        /// Gets or sets the tags associated with the lesson.
        /// </summary>
        public List<string> Tags
        {
            get => _tags;
            set => _tags = value;
        }

        /// <summary>
        /// Gets or sets the list of viewers enrolled in the lesson.
        /// </summary>
        public List<Viewer> Viewers
        {
            get => _viewers ??= new List<Viewer>();
            set => _viewers = value;
        }

        /// <summary>
        /// Gets or sets the description of the lesson.
        /// </summary>
        public string Description
        {
            get => _description;
            set => _description = value;
        }

        /// <summary>
        /// Gets or sets the identifier of the lesson.
        /// </summary>
        public string LessonId
        {
            get => _lessonId;
            set => _lessonId = value;
        }
        public EnrollmentSummaryRepo EnrollmentSummaryRepo
        {
            get => _enrollmentSummaryRepo ??= new EnrollmentSummaryRepo(_lessonId);
            set => _enrollmentSummaryRepo = value;
        }

        /// <summary>
        /// Ends the lesson and notifies all enrolled students.
        /// </summary>
        public void EndLesson()
        {
            IsActive = false;
            foreach (var student in EnrollmentSummaryRepo.GetAll().Select(enrollment => enrollment.Student))
            {
                student.Notify(new LessonEndedEvent(this).GenerateMsg());
            }
        }

        /// <summary>
        /// Checks if a student is enrolled in the lesson.
        /// </summary>
        /// <param name="student">The student to check.</param>
        /// <returns><c>true</c> if the student is enrolled; otherwise, <c>false</c>.</returns>
        public bool ContainStudent(Student student)
        {
            return EnrollmentSummaryRepo.ContainStudent(student);
        }

        /// <summary>
        /// Adds a student to the lesson if not already enrolled.
        /// </summary>
        /// <param name="student">The student to add.</param>
        public void AddStudent(Student student)
        {
            if (ContainStudent(student))
            {
                return;
            }
            var enrollmentSummary = new EnrollmentSummary(student, this);
            EnrollmentSummaryRepo.Add(enrollmentSummary);
            _teacher.Notify(new StudentJoinLessonEvent(student).GenerateMsg());
        }

        /// <summary>
        /// Checks if a viewer is allowed to view student data.
        /// </summary>
        /// <param name="viewer">The viewer to check.</param>
        /// <returns><c>true</c> if the viewer is allowed; otherwise, <c>false</c>.</returns>
        public bool IsAllowedToViewStudentsData(Viewer viewer)
        {
            return Viewers.Contains(viewer) || viewer == _teacher;
        }

        /// <summary>
        /// Retrieves all enrollment summaries for the lesson.
        /// </summary>
        /// <returns>The list of enrollment summaries.</returns>
        public List<EnrollmentSummary> GetEnrollmentSummaries()
        {
            return EnrollmentSummaryRepo.GetAll();
        }

        /// <summary>
        /// Pushes emotion data for a user to the lesson's repository.
        /// </summary>
        /// <param name="userEmail">The email of the user.</param>
        /// <param name="emotionData">The emotion data to push.</param>
        public void PushEmotionData(string userEmail, EmotionData emotionData)
        {
            if (_isActive)
            {
                EnrollmentSummaryRepo.PutEmotionData(userEmail, emotionData);   
            }
        }

        /// <summary>
        /// Adds a viewer to the lesson if not already added.
        /// </summary>
        /// <param name="viewer">The viewer to add.</param>
        public void AddViewer(Viewer viewer)
        {
            if (!Viewers.Contains(viewer) || viewer != _teacher)
            {
                Viewers.Add(viewer);
            }
        }

        /// <summary>
        /// Retrieves all emotion data entries associated with the lesson.
        /// </summary>
        /// <returns>The list of emotion data entries.</returns>
        public IEnumerable<EmotionData> GetEmotionDataEntries()
        {
            var entries = new List<EmotionData>();
            entries.AddRange(EnrollmentSummaryRepo.GetEmotionDataEntries());
            return entries;
        }

        /// <summary>
        /// Retrieves enrollment summaries with associated data for the lesson.
        /// </summary>
        /// <param name="correlationId">The correlation ID for logging purposes.</param>
        /// <returns>The list of enrollment summaries with associated data.</returns>
        public IEnumerable<EnrollmentSummary> GetEnrollmentSummariesWithData(string correlationId)
        {
            Log.Info($"[{correlationId}] Getting enrollment summaries with data for lesson {_lessonId}");
            return EnrollmentSummaryRepo.GetEnrollmentSummariesWithData();
        }

        /// <summary>
        /// Retrieves winning emotions for students in the lesson.
        /// </summary>
        /// <returns>The dictionary mapping student IDs to winning emotions.</returns>
        public Dictionary<string, List<string>> GetStudentWiningEmotions()
        {
            return EnrollmentSummaryRepo.GetStudentWiningEmotions();
        }

        /// <summary>
        /// Retrieves emotions data for students in the lesson.
        /// </summary>
        /// <returns>The dictionary mapping students to their enrollment summaries containing emotions data.</returns>
        public Dictionary<Student, EnrollmentSummary> GetStudentsEmotions()
        {
            return EnrollmentSummaryRepo.GetStudentsEmotions();
        }

        /// <summary>
        /// Notifies the teacher that a student has left the lesson.
        /// </summary>
        /// <param name="student">The student who left.</param>
        public void Leave(Student student)
        {
            _teacher.Notify(new StudentLeftLessonEvent(student).GenerateMsg());
        }

        /// <summary>
        /// Removes a viewer from the lesson.
        /// </summary>
        /// <param name="viewer">The viewer to remove.</param>
        public void Leave(Viewer viewer)
        {
            Viewers.Remove(viewer);
        }

        /// <summary>
        /// Notifies all students that the lesson teacher has left.
        /// </summary>
        /// <param name="teacher">The teacher who left.</param>
        public void Leave(Teacher teacher)
        {
            foreach (var student in EnrollmentSummaryRepo.GetAll().Select(enrollment => enrollment.Student))
            {
                student.Notify(new TeacherLeftLesson(teacher).GenerateMsg());
            }
        }

        /// <summary>
        /// Retrieves the enrollment summary for a student by email.
        /// </summary>
        /// <param name="studentEmail">The email of the student.</param>
        /// <returns>The enrollment summary for the student.</returns>
        public EnrollmentSummary GetEnrollmentSummaryByEmail(string studentEmail)
        {
            return EnrollmentSummaryRepo.GetById(studentEmail);
        }

        /// <summary>
        /// Adds a teacher's approach to a student's enrollment summary.
        /// </summary>
        /// <param name="teacher">The teacher who approached.</param>
        /// <param name="student">The student who received the approach.</param>
        public void AddTeacherApproach(Teacher teacher, Student student)
        {
            var enrollmentSummary = GetEnrollmentSummaryByEmail(student.Email);
            enrollmentSummary.AddTeacherApproach(teacher);
        }
        
        public void Leave(User user)
        {
            user.Leave(this);
        }
    }
}
