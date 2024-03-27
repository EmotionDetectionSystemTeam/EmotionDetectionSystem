using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEDS.Logic.RequestsObj
{
    public class GetLastEmotionsDataRequest : IRequest
    {
        public string SessionId { get; set; }
        public string Email { get; set; }
        public string LessonId { get; set; }

        public GetLastEmotionsDataRequest(string sessionId, string email, string lessonId)
        {
            SessionId = sessionId;
            Email = email;
            LessonId = lessonId;
        }
    }
}
