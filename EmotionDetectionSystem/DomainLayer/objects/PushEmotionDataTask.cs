namespace EmotionDetectionSystem.DomainLayer.objects;

public class PushEmotionDataTask
{
    public string      SessionId    { get; set; }
    public string      Email       { get; set; }
    public string      LessonId    { get; set; }
    public EmotionData EmotionData { get; set; }
    public PushEmotionDataTask(string sessionId, string email, string lessonId, EmotionData emotionData)
    {
        SessionId    = sessionId;
        Email       = email;
        LessonId    = lessonId;
        EmotionData = emotionData;
    }
}