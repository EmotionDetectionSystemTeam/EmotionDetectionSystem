using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.ServiceLayer.objects
{
    [Serializable]
    public class SEmotionData
    {
        public DateTime Time { get; set; }
        public double Neutral { get; set; }
        public double Happy { get; set; }
        public double Sad { get; set; }
        public double Angry { get; set; }
        public double Surprised { get; set; }
        public double Disgusted { get; set; }
        public double Fearful { get; set; }

        public SEmotionData(double neutral, double happy, double sad, double angry, double surprised,
                          double disgusted,
                          double fearful)
        {
            Time = DateTime.Now;
            Neutral = neutral;
            Happy = happy;
            Sad = sad;
            Angry = angry;
            Surprised = surprised;
            Disgusted = disgusted;
            Fearful = fearful;
        }


        public EmotionData ToDomainObject()
        {
            return new EmotionData(DateTime.Now, Neutral, Happy, Sad, Angry, Surprised, Disgusted, Fearful);
        }

    }
}
