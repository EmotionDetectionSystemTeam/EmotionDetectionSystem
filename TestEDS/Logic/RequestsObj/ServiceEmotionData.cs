using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEDS.Logic.RequestsObj
{
    [Serializable]
    public class ServiceEmotionData
    {
        public DateTime Time { get; set; }
        public double Neutral { get; set; }
        public double Happy { get; set; }
        public double Sad { get; set; }
        public double Angry { get; set; }
        public double Surprised { get; set; }
        public double Disgusted { get; set; }
        public double Fearful { get; set; }

        public ServiceEmotionData(double neutral, double happy, double sad, double angry, double surprised,
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
        public string GetHighestEmotion()
        {
            double[] emotions = { Neutral, Happy, Sad, Angry, Surprised, Disgusted, Fearful };
            string[] emotionLabels = { "Neutral", "Happy", "Sad", "Angry", "Surprised", "Disgusted", "Fearful" };

            double maxEmotion = emotions[0];
            int maxIndex = 0;

            for (int i = 1; i < emotions.Length; i++)
            {
                if (emotions[i] > maxEmotion)
                {
                    maxEmotion = emotions[i];
                    maxIndex = i;
                }
            }

            return emotionLabels[maxIndex];
        }
    }

}
