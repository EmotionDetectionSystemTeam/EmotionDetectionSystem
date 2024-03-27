using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEDS.Logic.RequestsObj
{
    public class CreateLessonRequest : IRequest
    {
        public string SessionId { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] Tags { get; set; }

        public CreateLessonRequest(string sessionId, string email, string title, string description, string[] tags)
        {
            SessionId = sessionId;
            Email = email;
            Title = title;
            Description = description;
            Tags = tags;
        }
    }
}
