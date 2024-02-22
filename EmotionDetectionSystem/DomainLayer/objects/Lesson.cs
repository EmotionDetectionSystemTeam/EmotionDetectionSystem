namespace EmotionDetectionSystem.DomainLayer.objects;

public class Lesson
{
    private string                  _lessonName;
    private Teacher                 _teacher;
    private DateTime                _date;
    private bool                    _isActive;
    private string                  _entryCode;
    private List<EnrollmentSummary> _emotions;
    private List<string>            _tags;
    
    public Lesson(string lessonName, Teacher teacher, DateTime date, bool isActive, string entryCode, List<EnrollmentSummary> emotions, List<string> tags)
    {
        _lessonName = lessonName;
        _teacher    = teacher;
        _date       = date;
        _isActive   = isActive;
        _entryCode  = entryCode;
        _emotions   = emotions;
        _tags   = tags;
    }
    
    public string LessonName { get => _lessonName;   set => _lessonName = value; }
    public Teacher Teacher   { get => _teacher;   set => _teacher = value; }
    public DateTime       Date      { get => _date;      set => _date = value; }
    public bool           IsActive  { get => _isActive;  set => _isActive = value; }
    public string         EntryCode { get => _entryCode; set => _entryCode = value; }
    public List<EnrollmentSummary> Emotions { get => _emotions; set => _emotions = value; }
    public List<string> Tags { get => _tags; set => _tags = value; }
}