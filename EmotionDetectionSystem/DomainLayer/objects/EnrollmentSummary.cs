using EmotionDetectionServer;
using EmotionDetectionSystem.DomainLayer.objects;

public class EnrollmentSummary
{
    private object _lock = new object();

    public EnrollmentSummary(Student student, Lesson lesson, List<EmotionData> emotionData)
    {
        Student                 = student;
        Lesson                  = lesson;
        EmotionData             = emotionData;
    }

    public EnrollmentSummary(Student student, Lesson lesson)
    {
        Student                 = student;
        Lesson                  = lesson;
        EmotionData             = new List<EmotionData>();
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
        foreach (var emotionData in EmotionData)
        {
            emotionData.Seen = true;
        }

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
            var emotionData = EmotionData.FindLast(ed => ed.Time == EmotionData.Max(ed => ed.Time));
            if (emotionData == null || emotionData.Seen)
            {
                return new EmotionData();
            }

            emotionData.Seen = true;
            return emotionData!;
        }
    }

    public EmotionData PeekFirstNotSeenEmotionData()
    {
        lock (_lock)
        {
            var emotionData = EmotionData.FindLast(ed => ed.Time == EmotionData.Max(ed => ed.Time));
            if (emotionData == null || emotionData.Seen)
            {
                return new EmotionData();
            }
            return emotionData;
        }
    }

    public List<string> GetPreviousEmotionData()
    {
        var winningEmotions = new List<string>();
        foreach (var emotionData in EmotionData)
        {
            var winningEmotion = emotionData.GetWinningEmotion();
            if (winningEmotion != null && winningEmotion != Emotions.NODATA)
            {
                winningEmotions.Add(winningEmotion);
            }
        }

        return winningEmotions;
    }
}