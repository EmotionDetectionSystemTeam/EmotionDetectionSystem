using System;
using System.Collections.Generic;
using System.Threading;
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
        _teacher           = new Teacher("testTeacher", "testTeacher", "testTeacher", "testTeacher");
        _entryCode         = "test";
        _student           = new Student("testStudent", "testStudent", "testStudent", "testStudent");
        _lesson            = new Lesson("1", _teacher, "testLesson", "", _entryCode, new List<string>());
        _enrollmentSummary = new EnrollmentSummary(_student, _lesson);
    }

    [TestMethod]
    public void AddEmotionData_Success()
    {
        EmotionData emotionData = new EmotionData(DateTime.Now, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
        _enrollmentSummary.AddEmotionData(emotionData);
        Assert.AreEqual(emotionData, _enrollmentSummary.EmotionData[0]);
    }

    [TestMethod]
    public void GetFirstNotSeenEmotionData_Success()
    {
        var firstEmotionData  = new EmotionData(DateTime.Now, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
        var secondEmotionData = new EmotionData(DateTime.Now, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
        _enrollmentSummary.AddEmotionData(firstEmotionData);
        _enrollmentSummary.AddEmotionData(secondEmotionData);
        Assert.AreEqual(firstEmotionData,  _enrollmentSummary.GetFirstNotSeenEmotionData());
        Assert.AreEqual(secondEmotionData, _enrollmentSummary.GetFirstNotSeenEmotionData());
        Assert.AreEqual(0,                 _enrollmentSummary.NotSeenEmotionDataQueue.Count);
        Assert.AreEqual(2,                 _enrollmentSummary.EmotionData.Count);
    }

    [TestMethod]
    public void GetFirstNotSeenEmotionData_EmptyQueueAndOneSeenEmotionData_Success()
    {
        var firstEmotionData = new EmotionData(DateTime.Now, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
        _enrollmentSummary.AddEmotionData(firstEmotionData);
        Assert.AreEqual(firstEmotionData, _enrollmentSummary.GetFirstNotSeenEmotionData());
        Assert.AreEqual(firstEmotionData, _enrollmentSummary.GetFirstNotSeenEmotionData());
        Assert.AreEqual(0,                _enrollmentSummary.NotSeenEmotionDataQueue.Count);
        Assert.AreEqual(1,                _enrollmentSummary.EmotionData.Count);
    }

    [TestMethod]
    public void GetLatestSeenEmotionData_EmptyQueueAndManySeenEmotionData_Success()
    {
        var firstEmotionData  = new EmotionData(DateTime.Now, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
        Thread.Sleep(1000);
        var fourthEmotionData = new EmotionData(DateTime.Now, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
        Thread.Sleep(1000);
        var thirdEmotionData  = new EmotionData(DateTime.Now, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
        Thread.Sleep(1000);
        var secondEmotionData = new EmotionData(DateTime.Now, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
        _enrollmentSummary.AddEmotionData(firstEmotionData);
        _enrollmentSummary.AddEmotionData(secondEmotionData);
        _enrollmentSummary.AddEmotionData(thirdEmotionData);
        _enrollmentSummary.AddEmotionData(fourthEmotionData);
        _enrollmentSummary.GetFirstNotSeenEmotionData();
        _enrollmentSummary.GetFirstNotSeenEmotionData();
        _enrollmentSummary.GetFirstNotSeenEmotionData();
        _enrollmentSummary.GetFirstNotSeenEmotionData();
        Assert.AreEqual(0, _enrollmentSummary.NotSeenEmotionDataQueue.Count);
        var emotionsData = _enrollmentSummary.GetFirstNotSeenEmotionData();
        Assert.AreEqual(secondEmotionData, emotionsData);
    }
}