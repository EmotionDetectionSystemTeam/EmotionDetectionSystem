using System;
using System.Collections.Generic;
using EmotionDetectionSystem.DomainLayer.objects;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmotionDetectionSystem.Tests.DomainLayer.objects;

[TestClass]
[TestSubject(typeof(Lesson))]
public class LessonTest
{

    private Student           _student;
    private Lesson            _lesson;
    private Teacher           _teacher;
    private string            _entryCode;
    
    [TestInitialize]
    public void SetUp()
    {
        _teacher           = new Teacher("testTeacher", "testTeacher", "testTeacher", "testTeacher");
        _entryCode         = "test";
        _student           = new Student("testStudent", "testStudent", "testStudent", "testStudent");
        _lesson            = new Lesson("1", _teacher, "testLesson", "", _entryCode, new List<string>());
    }
    
    [TestMethod]
    public void EndLesson_Success()
    {
        _lesson.EndLesson();
        Assert.IsFalse(_lesson.IsActive);
    }
    
    [TestMethod]
    public void ContainStudent_Success()
    {
        _lesson.AddStudent(_student);
        Assert.IsTrue(_lesson.ContainStudent(_student));
    }
    
    [TestMethod]
    public void ContainStudent_Fail()
    {
        Assert.IsFalse(_lesson.ContainStudent(_student));
    }
    
    [TestMethod]
    public void AddViewer_Success()
    {
        var teacher1 = new Teacher("testTeacher1", "testTeacher1", "testTeacher1", "testTeacher1");
        _lesson.AddViewer(teacher1);
        Assert.IsTrue(_lesson.Viewers.Contains(teacher1));
    }
    
    [TestMethod]
    public void IsAllowedToViewStudentData_ExistingViewer_Success()
    {
        var teacher1 = new Teacher("testTeacher1", "testTeacher1", "testTeacher1", "testTeacher1");
        _lesson.AddViewer(teacher1);
        Assert.IsTrue(_lesson.IsAllowedToViewStudentsData(teacher1));
    }
    
    [TestMethod]
    public void IsAllowedToViewStudentData_Fail()
    {
        var teacher1 = new Teacher("testTeacher1", "testTeacher1", "testTeacher1", "testTeacher1");
        Assert.IsFalse(_lesson.IsAllowedToViewStudentsData(teacher1));
    }
    
    [TestMethod]
    public void IsAllowedToViewStudentData_Teacher_Success()
    {
        Assert.IsTrue(_lesson.IsAllowedToViewStudentsData(_teacher));
    }
    
    [TestMethod]
    public void GetEnrollmentSummary_Success()
    {
        _lesson.AddStudent(_student);
        Assert.AreEqual(1, _lesson.GetEnrollmentSummaries().Count);
        Assert.AreEqual(_student, _lesson.GetEnrollmentSummaries()[0].Student);
    }
    
    [TestMethod]
    public void PutEmotionData_Success()
    {
        var emotionData = new EmotionData(DateTime.Now,1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
        _lesson.AddStudent(_student);
        _lesson.PushEmotionData(_student.Email, emotionData);
        Assert.AreEqual(emotionData, _lesson.GetEnrollmentSummaries()[0].EmotionData[0]);
    }
    
    [TestMethod]
    public void PushEmotionData_Fail()
    {
        var emotionData = new EmotionData(DateTime.Now,1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
        Assert.ThrowsException<Exception>(() => _lesson.PushEmotionData(_student.Email, emotionData));
    }
    
}