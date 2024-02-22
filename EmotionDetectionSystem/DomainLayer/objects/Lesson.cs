namespace EmotionDetectionSystem.DomainLayer.objects;

public class Lesson
{
    private string                  _lessonName;
    private string                  _description;
    private Teacher                 _teacher;
    private DateTime                _date;
    private bool                    _isActive;
    private string                  _entryCode;
    private List<EnrollmentSummary> _emotions;
    private List<string>            _tags;

    public Lesson(Teacher teacher, string lessonName, string description, string entryCode, List<string> tags)
    {
        _teacher    = teacher;
        _lessonName = lessonName;
        _description = description;
        _date       = DateTime.Now;
        _isActive   = true;
        _entryCode  = entryCode;
        _tags       = tags;
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

    public List<EnrollmentSummary> Emotions
    {
        get => _emotions;
        set => _emotions = value;
    }

    public List<string> Tags
    {
        get => _tags;
        set => _tags = value;
    }
}