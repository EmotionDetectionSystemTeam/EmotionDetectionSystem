namespace EmotionDetectionSystem.DomainLayer.objects;

public class EmotionData
{
    private DateTime _time;
    private double    _neutral;
    private double    _happy;
    private double    _sad;
    private double    _angry;
    private double    _surprised;
    private double    _disgusted;
    private double    _fearful;
    
    public EmotionData(DateTime time, double neutral, double happy, double sad, double angry, double surprised, double disgusted, double fearful)
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
    public double Neutral { get => _neutral; set => _neutral = value; }
    public double Happy { get => _happy; set => _happy = value; }
    public double Sad { get => _sad; set => _sad = value; }
    public double Angry { get => _angry; set => _angry = value; }
    public double Surprised { get => _surprised; set => _surprised = value; }
    public double Disgusted { get => _disgusted; set => _disgusted = value; }
    public double Fearful { get => _fearful; set => _fearful = value; }
}