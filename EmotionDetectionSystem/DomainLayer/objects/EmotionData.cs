using EmotionDetectionServer;

namespace EmotionDetectionSystem.DomainLayer.objects
{
    /// <summary>
    /// Represents emotional data captured at a specific time.
    /// </summary>
    public class EmotionData
    {
        private bool _seen;
        
        public EmotionData(DateTime time,  string winningEmotion)
        {
            Time      = time;
            WinningEmotion = winningEmotion;
            _seen     = false;
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
        public string WinningEmotion { get; set; }
        private double Nodata { get; set; }
        
        public bool Seen
        {
            get => _seen;
            set => _seen = value;
        }

        public string GetWinningEmotion()
        {
            return WinningEmotion ?? Emotions.NODATA;
        }
    }
}
