namespace EmotionDetectionSystem.DomainLayer.objects;

public class EmotionData
{
    DateTime _time;
    float neutral;
    float happy;
    float sad;
    float angry;
    float surprised;
    float disgusted;
    float fearful;
    
    public EmotionData(float neutral, float happy, float sad, float angry, float surprised, float disgusted, float fearful)
    {
        _time          = DateTime.Now;
        this.neutral   = neutral;
        this.happy     = happy;
        this.sad       = sad;
        this.angry     = angry;
        this.surprised = surprised;
        this.disgusted = disgusted;
        this.fearful   = fearful;
    }
    
    public DateTime Time { get => _time; set => _time = value; }
    public float Neutral { get => neutral; set => neutral = value; }
    public float Happy { get => happy; set => happy = value; }
    public float Sad { get => sad; set => sad = value; }
    public float Angry { get => angry; set => angry = value; }
    public float Surprised { get => surprised; set => surprised = value; }
    public float Disgusted { get => disgusted; set => disgusted = value; }
    public float Fearful { get => fearful; set => fearful = value; }
    
    
}