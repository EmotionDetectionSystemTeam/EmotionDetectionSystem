using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.ServiceLayer;

public class ServiceEmotionData
{
    public DateTime Time;
    public double    Neutral;
    public double    Happy;
    public double    Sad;
    public double    Angry;
    public double    Surprised;
    public double    Disgusted;
    public double    Fearful;

    public ServiceEmotionData(double neutral, double happy, double sad, double angry, double surprised, double disgusted,
                              double fearful)
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