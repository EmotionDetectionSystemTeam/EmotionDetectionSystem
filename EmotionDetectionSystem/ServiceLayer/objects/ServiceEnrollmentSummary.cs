using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.ServiceLayer.objects;

public class ServiceEnrollmentSummary
{
    public ServiceStudent           Student;
    public List<ServiceEmotionData> EmotionData;
    
    public ServiceEnrollmentSummary(ServiceStudent student, List<ServiceEmotionData> emotionData)
    {
        Student     = student;
        EmotionData = emotionData;
    }
    //constructor with EnrolmentSummary as parameter
    public ServiceEnrollmentSummary(EnrollmentSummary enrollmentSummary)
    {
        Student     = new ServiceStudent(enrollmentSummary.Student);
        EmotionData = new List<ServiceEmotionData>();
        foreach (var emotionData in enrollmentSummary.EmotionData)
        {
            EmotionData.Add(new ServiceEmotionData(emotionData.Neutral, emotionData.Happy, emotionData.Sad, emotionData.Angry, emotionData.Surprised, emotionData.Disgusted, emotionData.Fearful));
        }
    }
    
}