

namespace TestEDS.Logic.RequestsObj
{
    public class RegisterRequest : IRequest
    {

        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public int isStudent { get; set; }

        public RegisterRequest(string email, string firstName, string lastName, string password, string confirmPassword, int isStudent)
        {
            this.email = email;
            this.firstName = firstName;
            this.lastName = lastName;
            this.password = password;
            this.confirmPassword = confirmPassword;
            this.isStudent = isStudent;
        }
    }
}