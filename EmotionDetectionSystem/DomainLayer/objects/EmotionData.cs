using EmotionDetectionServer;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmotionDetectionSystem.DomainLayer.objects
{
    /// <summary>
    /// Represents emotional data captured at a specific time.
    /// </summary>
    [Table("EmotionData")]
    public class EmotionData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
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
            WinningEmotion = Emotions.NODATA;
        }

        /// <summary>
        /// Gets or sets the timestamp when the emotional data was recorded.
        /// </summary>
        public DateTime Time { get; set; }
        public string WinningEmotion { get; set; }
        public bool Seen
        {
            get => _seen;
            set => _seen = value;
        }

        public string GetWinningEmotion()
        {
            return WinningEmotion;
        }
    }
}
