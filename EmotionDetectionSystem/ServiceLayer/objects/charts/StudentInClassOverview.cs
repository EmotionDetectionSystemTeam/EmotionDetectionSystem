using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.ServiceLayer.objects.charts;

public class StudentInClassOverview
{
    public string                 ClassName { get; set; }
    public string                 Email     { get; set; }
    public string                 FirstName { get; set; }
    public string                 LastName  { get; set; }
    public List<EmotionDataChart> Emotions  { get; set; }

    public StudentInClassOverview(EnrollmentSummary enrollmentSummary)
    {
        ClassName = enrollmentSummary.Lesson.LessonName;
        Email     = enrollmentSummary.Student.Email;
        FirstName = enrollmentSummary.Student.FirstName;
        LastName  = enrollmentSummary.Student.LastName;
        Emotions  = new List<EmotionDataChart>();
        foreach (var emotionData in enrollmentSummary.GetAllEmotionData())
        {
            Emotions.Add(new EmotionDataChart(emotionData));
        }
    }
}

public class EmotionDataChart
{
    public string   Emotion { get; set; }
    public DateTime Date    { get; set; }

    public EmotionDataChart(EmotionData emotionData)
    {
        Emotion = emotionData.GetWinningEmotion();
        Date    = emotionData.Time;
    }
}