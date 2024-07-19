using EmotionDetectionServer;
using EmotionDetectionSystem.DomainLayer.Events;
using EmotionDetectionSystem.DomainLayer.objects;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents a summary of enrollment details including student, lesson, and emotion data.
/// </summary>
[Table("EnrollmentSummary")]
public class EnrollmentSummary
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }
    private object _lock = new object();
    public EnrollmentSummary() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnrollmentSummary"/> class with emotion data.
    /// </summary>
    /// <param name="student">The student enrolled.</param>
    /// <param name="lesson">The lesson associated with the enrollment.</param>
    /// <param name="emotionData">List of emotion data recorded during enrollment.</param>
    public EnrollmentSummary(Student student, Lesson lesson, List<EmotionData> emotionData)
    {
        Student = student;
        Lesson = lesson;
        EmotionData = emotionData;
        TeacherApproach = new List<string>();
        StudentId = student.Email;
        LessonId = lesson.LessonId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnrollmentSummary"/> class without emotion data.
    /// </summary>
    /// <param name="student">The student enrolled.</param>
    /// <param name="lesson">The lesson associated with the enrollment.</param>
    public EnrollmentSummary(Student student, Lesson lesson)
    {
        Student = student;
        Lesson = lesson;
        EmotionData = new List<EmotionData>();
        TeacherApproach = new List<string>();
        StudentId = student.Email;
        LessonId = lesson.LessonId;
    }

    /// <summary>
    /// Adds emotion data to the enrollment summary.
    /// </summary>
    /// <param name="emotionData">The emotion data to add.</param>
    public void AddEmotionData(EmotionData emotionData)
    {
        lock (_lock)
        {
            EmotionData.Add(emotionData);
        }
    }

    /// <summary>
    /// Marks all emotion data as seen and returns the list of emotion data.
    /// </summary>
    /// <returns>List of emotion data with 'Seen' flag set to true.</returns>
    public List<EmotionData> GetAllEmotionData()
    {
        foreach (var emotionData in EmotionData)
        {
            emotionData.Seen = true;
        }

        return EmotionData;
    }

    /// <summary>
    /// Retrieves all winning emotions from the emotion data.
    /// </summary>
    /// <returns>List of winning emotions.</returns>
    public List<string> GetAllWiningEmotionData()
    {
        return EmotionData.Select(emotionData => emotionData.GetWinningEmotion()).ToList();
    }

    /// <summary>
    /// Gets the first emotion data that has not been marked as seen.
    /// </summary>
    /// <returns>The first not seen emotion data.</returns>
    public EmotionData GetFirstNotSeenEmotionData()
    {
        lock (_lock)
        {
            var emotionData = EmotionData.FindLast(ed => ed.Time == EmotionData.Max(ed => ed.Time));
            if (emotionData == null || emotionData.Seen)
            {
                return new EmotionData();
            }

            emotionData.Seen = true;
            return emotionData!;
        }
    }

    /// <summary>
    /// Peeks at the first emotion data that has not been marked as seen without marking it.
    /// </summary>
    /// <returns>The first not seen emotion data.</returns>
    public EmotionData PeekFirstNotSeenEmotionData()
    {
        lock (_lock)
        {
            var emotionData = EmotionData.FindLast(ed => ed.Time == EmotionData.Max(ed => ed.Time));
            if (emotionData == null || emotionData.Seen)
            {
                return new EmotionData();
            }
            return emotionData;
        }
    }

    /// <summary>
    /// Retrieves all previous winning emotions from the recorded emotion data.
    /// </summary>
    /// <returns>List of previous winning emotions.</returns>
    public List<string> GetPreviousEmotionData()
    {
        var winningEmotions = new List<string>();
        foreach (var emotionData in EmotionData)
        {
            var winningEmotion = emotionData.GetWinningEmotion();
            if (winningEmotion != null && winningEmotion != Emotions.NODATA)
            {
                winningEmotions.Add(winningEmotion);
            }
        }

        return winningEmotions;
    }

    /// <summary>
    /// Adds a teacher approach event message to the teacher approach list.
    /// </summary>
    /// <param name="teacher">The teacher associated with the approach event.</param>
    public void AddTeacherApproach(Teacher teacher)
    {
        TeacherApproach.Add(new TeacherApproachEvent(Student, teacher).GenerateMsg());
    }

    /// <summary>
    /// Gets or sets the student enrolled in the lesson.
    /// </summary>
    public Student Student { get; set; }

    /// <summary>
    /// Gets or sets the lesson associated with the enrollment.
    /// </summary>
    public Lesson Lesson { get; set; }

    /// <summary>
    /// Gets or sets the list of emotion data recorded during the enrollment.
    /// </summary>
    public List<EmotionData> EmotionData { get; set; }

    /// <summary>
    /// Gets or sets the list of teacher approach messages.
    /// </summary>
    public List<string> TeacherApproach { get; set; }
    public List<EmotionData> NegativeEmotionData { get; set; }
    public string StudentId { get; set; }
    public string LessonId { get; set; }
}
