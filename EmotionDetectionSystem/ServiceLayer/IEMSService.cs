using System;
using EmotionDetectionSystem.ServiceLayer;

namespace EmotionDetectionSystem.Service
{
	public interface EmptyInterface
	{
		Response Register(string email, string firstName, string lastName, string password, string confirmPassword,
			bool isStudent);
		Response<ServiceUser> Login(string email, string password);
		Response Logout(string sessionId);
		
	}
}

