namespace EmotionDetectionSystem.DomainLayer.objects;

public class EmotionData
{
    private DateTime _time;
    private float    _neutral;
    private float    _happy;
    private float    _sad;
    private float    _angry;
    private float    _surprised;
    private float    _disgusted;
    private float    _fearful;
    
    public EmotionData(DateTime time, float neutral, float happy, float sad, float angry, float surprised, float disgusted, float fearful)
    {
        _time          = time;
        _neutral   = neutral;
        _happy     = happy;
        _sad       = sad;
        _angry     = angry;
        _surprised = surprised;
        _disgusted = disgusted;
        _fearful   = fearful;
    }
    
    public DateTime Time { get => _time; set => _time = value; }
    public float Neutral { get => _neutral; set => _neutral = value; }
    public float Happy { get => _happy; set => _happy = value; }
    public float Sad { get => _sad; set => _sad = value; }
    public float Angry { get => _angry; set => _angry = value; }
    public float Surprised { get => _surprised; set => _surprised = value; }
    public float Disgusted { get => _disgusted; set => _disgusted = value; }
    public float Fearful { get => _fearful; set => _fearful = value; }
}