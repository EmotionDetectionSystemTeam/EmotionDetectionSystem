using System;
using System.Collections.Generic;
using EmotionDetectionSystem.DomainLayer.objects;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmotionDetectionSystem.Tests.DomainLayer.objects;

[TestClass]
[TestSubject(typeof(EnrollmentSummary))]
public class EnrollmentSummaryTest
{

    private EnrollmentSummary _enrollmentSummary;
    private Student           _student;
    private Lesson            _lesson;
    private Teacher           _teacher;
    private string            _entryCode;
    
    [TestInitialize]
    public void SetUp()
    {
        _teacher = new Teacher("testTeacher", "testTeacher", "testTeacher", "testTeacher");
        _entryCode = "test";
        _student = new Student("testStudent", "testStudent", "testStudent", "testStudent");
        _lesson = new Lesson("1",_teacher,"testLesson","",_entryCode,new List<string>());
        _enrollmentSummary = new EnrollmentSummary(_student, _lesson);
    }
    [TestMethod]
    public void AddEmotionData_Success()
    {
        EmotionData emotionData = new EmotionData(DateTime.Now, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
        _enrollmentSummary.AddEmotionData(emotionData);
        Assert.AreEqual(emotionData, _enrollmentSummary.EmotionData[0]);
    }
}