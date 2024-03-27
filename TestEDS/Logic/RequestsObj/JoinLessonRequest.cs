using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEDS.Logic.RequestsObj
{
    public class JoinLessonRequest : IRequest
    {
        public string SessionId { get; set; }
        public string Email { get; set; }
        public string EntryCode { get; set; }

        public JoinLessonRequest(string sessionId, string email, string entryCode)
        {
            SessionId = sessionId;
            Email = email;
            EntryCode = entryCode;
        }
    }
}
