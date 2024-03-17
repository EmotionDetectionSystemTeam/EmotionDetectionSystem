using EmotionDetectionSystem.DomainLayer.Repos;

namespace EmotionDetectionSystem.DomainLayer.objects;

public class Lesson
{
    private string                _lessonId;
    private string                _lessonName;
    private string                _description;
    private Teacher               _teacher;
    private List<Viewer>          _viewers;
    private DateTime              _date;
    private bool                  _isActive;
    private string                _entryCode;
    private List<string>          _tags;
    private EnrollmentSummaryRepo _enrollmentSummaryRepo;

    public Lesson(string       lessonId, Teacher teacher, string lessonName, string description, string entryCode,
                  List<string> tags)
    {
        _lessonId              = lessonId;
        _teacher               = teacher;
        _lessonName            = lessonName;
        _description           = description;
        _date                  = DateTime.Now;
        _isActive              = true;
        _entryCode             = entryCode;
        _tags                  = tags;
        _enrollmentSummaryRepo = new EnrollmentSummaryRepo(lessonId);
        _viewers               = new List<Viewer>();
    }

    public string LessonName
    {
        get => _lessonName;
        set => _lessonName = value;
    }

    public Teacher Teacher
    {
        get => _teacher;
        set => _teacher = value;
    }

    public DateTime Date
    {
        get => _date;
        set => _date = value;
    }

    public bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    public string EntryCode
    {
        get => _entryCode;
        set => _entryCode = value;
    }

    public List<string> Tags
    {
        get => _tags;
        set => _tags = value;
    }

    public List<Viewer> Viewers
    {
        get => _viewers;
        set => _viewers = value;
    }

    public string Description
    {
        get => _description;
        set => _description = value;
    }

    public string LessonId
    {
        get => _lessonId;
        set => _lessonId = value;
    }

    public void EndLesson()
    {
        _isActive = false;
    }

    public bool ContainStudent(Student student)
    {
        return _enrollmentSummaryRepo.ContainStudent(student);
    }

    public void AddStudent(Student student)
    {
        var enrollmentSummary = new EnrollmentSummary(student, this);
        _enrollmentSummaryRepo.Add(enrollmentSummary);
    }

    public bool IsAllowedToViewStudentsData(Viewer viewer)
    {
        return _viewers.Contains(viewer) || viewer == _teacher;
    }

    public List<EnrollmentSummary> GetEnrollmentSummaries()
    {
        return _enrollmentSummaryRepo.GetAll();
    }

    public void PushEmotionData(string userEmail, EmotionData emotionData)
    {
        _enrollmentSummaryRepo.PutEmotionData(userEmail, emotionData);
    }

    public void AddViewer(Viewer viewer)
    {
        if (!_viewers.Contains(viewer) || viewer != _teacher)
        {
            _viewers.Add(viewer);
        }
    }

    public IEnumerable<EmotionData> GetEmotionDataEntries()
    {
        var entries = new List<EmotionData>();
        entries.AddRange(_enrollmentSummaryRepo.GetEmotionDataEntries());
        return entries;
    }

    public IEnumerable<EnrollmentSummary> GetEnrollmentSummariesWithData()
    {
        return _enrollmentSummaryRepo.GetEnrollmentSummariesWithData();
    }
    public Dictionary<string, List<string>> GetStudentWiningEmotions() { 
        return _enrollmentSummaryRepo.GetStudentWiningEmotions();
    }
    public Dictionary<Student,EnrollmentSummary> GetStudentsEmotions() {
        return _enrollmentSummaryRepo.GetStudentsEmotions();
    }
}