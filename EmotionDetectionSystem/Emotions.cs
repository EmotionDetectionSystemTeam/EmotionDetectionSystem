namespace EmotionDetectionServer;
//class that contains all the emotions names as constants 
public class Emotions
{
    public const string NEUTRAL   = "Neutral";
    public const string HAPPY     = "Happy";
    public const string SAD       = "Sad";
    public const string ANGRY     = "Angry";
    public const string SURPRISED = "Surprised";
    public const string DISGUSTED = "Disgusted";
    public const string FEARFUL   = "Fearful";
    public const string NODATA    = "No Data";
    
    public static String CastEmotion(string emotion)
    {
        switch (emotion.ToLower())
        {
            case "neutral":
                return NEUTRAL;
            case "happy":
                return HAPPY;
            case "sad":
                return SAD;
            case "angry":
                return ANGRY;
            case "surprised":
                return SURPRISED;
            case "disgusted":
                return DISGUSTED;
            case "fearful":
                return FEARFUL;
            case "nodata":
                return NODATA;
            default:
                return NEUTRAL;
        }
    }
    
    public static string GenerateRandomEmotion()
    {
        var random = new Random();
        var emotions = new List<string> {NEUTRAL, HAPPY, SAD, ANGRY, SURPRISED, DISGUSTED, FEARFUL};
        return emotions[random.Next(emotions.Count)];
    }
}