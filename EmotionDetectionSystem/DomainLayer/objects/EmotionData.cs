using EmotionDetectionServer;

namespace EmotionDetectionSystem.DomainLayer.objects
{
    /// <summary>
    /// Represents emotional data captured at a specific time.
    /// </summary>
    public class EmotionData
    {
        private bool _seen;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmotionData"/> class with specific emotional values.
        /// </summary>
        /// <param name="time">The timestamp when the emotional data was recorded.</param>
        /// <param name="neutral">The neutral emotion value.</param>
        /// <param name="happy">The happy emotion value.</param>
        /// <param name="sad">The sad emotion value.</param>
        /// <param name="angry">The angry emotion value.</param>
        /// <param name="surprised">The surprised emotion value.</param>
        /// <param name="disgusted">The disgusted emotion value.</param>
        /// <param name="fearful">The fearful emotion value.</param>
        public EmotionData(DateTime time, double neutral, double happy, double sad, double angry, double surprised,
                           double disgusted, double fearful)
        {
            Time = time;
            Neutral = neutral;
            Happy = happy;
            Sad = sad;
            Angry = angry;
            Surprised = surprised;
            Disgusted = disgusted;
            Fearful = fearful;
            _seen = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmotionData"/> class with default values.
        /// </summary>
        public EmotionData()
        {
            Nodata = 1;
        }

        /// <summary>
        /// Gets or sets the timestamp when the emotional data was recorded.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Gets or sets the neutral emotion value.
        /// </summary>
        public double Neutral { get; set; }

        /// <summary>
        /// Gets or sets the happy emotion value.
        /// </summary>
        public double Happy { get; set; }

        /// <summary>
        /// Gets or sets the sad emotion value.
        /// </summary>
        public double Sad { get; set; }

        /// <summary>
        /// Gets or sets the angry emotion value.
        /// </summary>
        public double Angry { get; set; }

        /// <summary>
        /// Gets or sets the surprised emotion value.
        /// </summary>
        public double Surprised { get; set; }

        /// <summary>
        /// Gets or sets the disgusted emotion value.
        /// </summary>
        public double Disgusted { get; set; }

        /// <summary>
        /// Gets or sets the fearful emotion value.
        /// </summary>
        public double Fearful { get; set; }

        private double Nodata { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this emotion data has been seen.
        /// </summary>
        public bool Seen
        {
            get => _seen;
            set => _seen = value;
        }

        /// <summary>
        /// Determines and returns the winning emotion based on weighted values.
        /// </summary>
        /// <returns>The winning emotion.</returns>
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
                { Emotions.NODATA, 1 }
            };

            var emotions = new Dictionary<string, double>
            {
                { Emotions.NEUTRAL, Neutral * weights[Emotions.NEUTRAL] },
                { Emotions.HAPPY, Happy * weights[Emotions.HAPPY] },
                { Emotions.SAD, Sad * weights[Emotions.SAD] },
                { Emotions.ANGRY, Angry * weights[Emotions.ANGRY] },
                { Emotions.SURPRISED, Surprised * weights[Emotions.SURPRISED] },
                { Emotions.DISGUSTED, Disgusted * weights[Emotions.DISGUSTED] },
                { Emotions.FEARFUL, Fearful * weights[Emotions.FEARFUL] },
                { Emotions.NODATA, Nodata * weights[Emotions.NODATA] }
            };

            var max = emotions.MaxBy(kvp => kvp.Value).Key;
            return max;
        }
    }
}
