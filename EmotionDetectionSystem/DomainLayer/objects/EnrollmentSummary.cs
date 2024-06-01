using System.Collections.Concurrent;

namespace EmotionDetectionSystem.DomainLayer.objects;

public class EnrollmentSummary
{
    private object _lock = new object();

    public EnrollmentSummary(Student student, Lesson lesson, List<EmotionData> emotionData)
    {
        Student     = student;
        Lesson      = lesson;
        EmotionData = emotionData;
    }

    public EnrollmentSummary(Student student, Lesson lesson)
    {
        Student     = student;
        Lesson      = lesson;
        EmotionData = new List<EmotionData>();
    }

    public void AddEmotionData(EmotionData emotionData)
    {
        lock (_lock)
        {
            EmotionData.Add(emotionData);
        }
    }

    public List<EmotionData> GetAllEmotionData()
    {
        return EmotionData;
    }

    public List<string> GetAllWiningEmotionData()
    {
        return EmotionData.Select(emotionData => emotionData.GetWinningEmotion()).ToList();
    }

    public Student Student { get; set; }

    public Lesson Lesson { get; set; }

    public List<EmotionData> EmotionData { get; set; }

    public EmotionData GetFirstNotSeenEmotionData()
    {
        lock (_lock)
        {
            var emotionData = EmotionData.FindLast(ed => ed.Time == EmotionData.Max(data => data.Time));
            return emotionData!;
        }
    }

    public List<string> getPreviousEmotionData()
    {
        return EmotionData.Select(emotionData => emotionData.GetWinningEmotion()).OfType<string>().ToList();
    }
}