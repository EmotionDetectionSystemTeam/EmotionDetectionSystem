namespace EmotionDetectionSystem.DomainLayer.objects;

public class EnrollmentSummary
{
    private Student           _student;
    private Lesson            _lesson;
    private List<EmotionData> _emotionData;

    public EnrollmentSummary(Student student, Lesson lesson, List<EmotionData> emotionData)
    {
        _student     = student;
        _lesson      = lesson;
        _emotionData = emotionData;
    }

    public EnrollmentSummary(Student student, Lesson lesson)
    {
        _student     = student;
        _lesson      = lesson;
        _emotionData = new List<EmotionData>();
    }

    public Student Student
    {
        get { return _student; }
        set { _student = value; }
    }

    public Lesson Lesson
    {
        get { return _lesson; }
        set { _lesson = value; }
    }

    public List<EmotionData> EmotionData
    {
        get { return _emotionData; }
        set { _emotionData = value; }
    }
}