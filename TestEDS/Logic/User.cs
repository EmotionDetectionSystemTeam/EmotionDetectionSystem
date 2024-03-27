using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEDS.Logic
{
    public class UserData
    {
        [JsonProperty("value")]
        public List<User> Users { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
    }

    public class User
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("winingEmotion")] // Typo in JSON: It should be "winningEmotion"
        public string WinningEmotion { get; set; }

        [JsonProperty("previousEmotions")]
        public List<string> PreviousEmotions { get; set; }
    }
}
