using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.ServiceLayer.objects
{
    [Serializable]
    public class ServiceEmotionData
    {
        public DateTime Time { get; set; }
        public string WinningEmotion { get; set; }
        
        public ServiceEmotionData(string winningEmotion, DateTime time)
        {
            Time      = time;
            WinningEmotion = winningEmotion;
        }

        public EmotionData ToDomainObject()
        {
            return new EmotionData(Time, WinningEmotion);
        }
    }
}






