using EmotionDetectionServer;

namespace EmotionDetectionSystem.DomainLayer.objects;

public class EmotionData
{
    private bool     _seen;

    public EmotionData(DateTime time,      double neutral, double happy, double sad, double angry, double surprised,
                       double   disgusted, double fearful)
    {
        Time      = time;
        Neutral   = neutral;
        Happy     = happy;
        Sad       = sad;
        Angry     = angry;
        Surprised = surprised;
        Disgusted = disgusted;
        Fearful   = fearful;
        _seen      = false;
    }

    public EmotionData()
    {
        Nodata = 1;
    }

    public DateTime Time { get; set; }

    public double Neutral { get; set; }

    public double Happy { get; set; }

    public double Sad { get; set; }

    public double Angry { get; set; }

    public double Surprised { get; set; }

    public double Disgusted { get; set; }

    public double Fearful { get; set; }

    private double Nodata { get; set; }

    public bool Seen
    {
        get => _seen;
        set => _seen = value;
    }

    public string GetWinningEmotion()
    {
        var weights = new Dictionary<string, double>
        {
            { Emotions.NEUTRAL, 0.4 },
            { Emotions.HAPPY, 1 },
            { Emotions.SAD, 1 },
            { Emotions.ANGRY, 1 },
            { Emotions.SURPRISED, 1 },
            { Emotions.DISGUSTED, 1 },
            { Emotions.FEARFUL, 1 },
            { Emotions.NODATA, 1}
        };

        var emotions = new Dictionary<string, double>
        {
            { Emotions.NEUTRAL, Neutral     * weights[Emotions.NEUTRAL] },
            { Emotions.HAPPY, Happy         * weights[Emotions.HAPPY] },
            { Emotions.SAD, Sad             * weights[Emotions.SAD] },
            { Emotions.ANGRY, Angry         * weights[Emotions.ANGRY] },
            { Emotions.SURPRISED, Surprised * weights[Emotions.SURPRISED] },
            { Emotions.DISGUSTED, Disgusted * weights[Emotions.DISGUSTED] },
            { Emotions.FEARFUL, Fearful     * weights[Emotions.FEARFUL] },
            { Emotions.NODATA, Nodata       * weights[Emotions.NODATA]}
        };

        var max = emotions.MaxBy(kvp => kvp.Value).Key;
        return max;
    }
    
}