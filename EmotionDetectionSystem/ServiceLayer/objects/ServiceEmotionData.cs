using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.ServiceLayer;

public class ServiceEmotionData
{
    public DateTime Time;
    public float    Neutral;
    public float    Happy;
    public float    Sad;
    public float    Angry;
    public float    Surprised;
    public float    Disgusted;
    public float    Fearful;

    public ServiceEmotionData(float neutral, float happy, float sad, float angry, float surprised, float disgusted,
                              float fearful)
    {
        Time      = DateTime.Now;
        Neutral   = neutral;
        Happy     = happy;
        Sad       = sad;
        Angry     = angry;
        Surprised = surprised;
        Disgusted = disgusted;
        Fearful   = fearful;
    }

    //constructor with DomainLayer.EmotionData
    public ServiceEmotionData(EmotionData emotionData)
    {
        Time      = emotionData.Time;
        Neutral   = emotionData.Neutral;
        Happy     = emotionData.Happy;
        Sad       = emotionData.Sad;
        Angry     = emotionData.Angry;
        Surprised = emotionData.Surprised;
        Disgusted = emotionData.Disgusted;
        Fearful   = emotionData.Fearful;
    }

    public EmotionData ToDomainObject()
    {
        return new EmotionData(Time, Neutral, Happy, Sad, Angry, Surprised, Disgusted, Fearful);
    }
}