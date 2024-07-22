using EmotionDetectionSystem.DomainLayer.objects;
using EmotionDetectionSystem.ServiceLayer.objects.charts;

namespace EmotionDetectionSystem.ServiceLayer.objects;

public class ServiceRealTimeUser
{
    public ServiceRealTimeUser(User user, string winingEmotion, List<EmotionData> previousEmotions)
    {
        Email         = user.Email;
        FirstName     = user.FirstName;
        LastName      = user.LastName;
        WiningEmotion = winingEmotion;
        PreviousEmotions = previousEmotions
            .Select(emotion => new EmotionDataChart(emotion)).ToList();
    }

    public string                   Email            { get; set; }
    public string                   FirstName        { get; set; }
    public string                   LastName         { get; set; }
    public string                   WiningEmotion    { get; set; }
    public List<EmotionDataChart> PreviousEmotions { get; set; }
}