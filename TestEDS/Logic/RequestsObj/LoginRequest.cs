using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEDS.Logic.RequestsObj
{

    public class LoginRequest : IRequest
    {
        public string sessionId { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public LoginRequest(string SessionId, string Email, string Password)
        {
            this.sessionId = SessionId;
            this.email = Email;
            this.password = Password;
        }
    }
}
