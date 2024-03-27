using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEDS.Logic.RequestsObj
{
    public class PushEmotionDataRequest : IRequest
    {
        public string SessionId { get; set; }
        public string Email { get; set; }
        public string LessonId { get; set; }
        public ServiceEmotionData EmotionData { get; set; }

        public PushEmotionDataRequest(string sessionId, string email, string lessonId, ServiceEmotionData emotionData)
        {
            SessionId = sessionId;
            Email = email;
            LessonId = lessonId;
            EmotionData = emotionData;
        }
    }
}
