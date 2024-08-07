﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEDS.Logic
{
    [Serializable]
    public class ServiceRealTimeUser
    {
        protected ServiceRealTimeUser(string email, string firstName, string lastName, string winingEmotion)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            WiningEmotion = winingEmotion;
        }

        public ServiceRealTimeUser(User user, string winingEmotion, List<string> previousEmotions)
        {
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            WiningEmotion = winingEmotion;
            PreviousEmotions = previousEmotions;
        }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WiningEmotion { get; set; }
        public List<string> PreviousEmotions { get; set; }
    }

}
