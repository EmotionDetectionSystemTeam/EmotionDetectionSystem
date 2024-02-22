using System;
using EmotionDetectionSystem.ServiceLayer;

namespace EmotionDetectionSystem.Service
{
	public interface IEdsService
	{
		Response Register(string email, string firstName, string lastName, string password, string confirmPassword, bool isStudent);
		Response<ServiceUser> Login(string sessionId, string email, string password);
		Response Logout(string sessionId);
		Response<string> CreateLesson(string sessionId, string title, string description, string[] tags);
		Response EndLesson(string sessionId);
		Response<ServiceUser> JoinLesson(string sessionId, string entryCode);
		Response<Dictionary<int,ServiceEnrollmentSummary>> ViewStudentsDuringLesson(string sessionId);
		Response<List<ServiceLesson>> ViewLessonDashboard(string sessionId);
		Response<ServiceUser> ViewStudent(string sessionId);
		
	}

}

