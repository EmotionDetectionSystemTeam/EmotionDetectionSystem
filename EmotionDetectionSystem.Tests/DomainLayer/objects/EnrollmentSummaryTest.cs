using System;
using System.Collections.Generic;
using System.Threading;
using EmotionDetectionServer;
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
        EmotionData emotionData = new EmotionData(DateTime.Now, Emotions.GenerateRandomEmotion());
        _enrollmentSummary.AddEmotionData(emotionData);
        Assert.AreEqual(emotionData, _enrollmentSummary.EmotionData[0]);
    }

    [TestMethod]
    public void GetFirstNotSeenEmotionData_Success()
    {
        var firstEmotionData  = new EmotionData(DateTime.Now, Emotions.GenerateRandomEmotion());
        var secondEmotionData = new EmotionData(DateTime.Now, Emotions.GenerateRandomEmotion());
        _enrollmentSummary.AddEmotionData(firstEmotionData);
        _enrollmentSummary.AddEmotionData(secondEmotionData);
        Assert.AreEqual(secondEmotionData,  _enrollmentSummary.GetFirstNotSeenEmotionData());
        Assert.AreEqual(Emotions.NODATA, _enrollmentSummary.GetFirstNotSeenEmotionData().GetWinningEmotion());
        Assert.AreEqual(2,                 _enrollmentSummary.EmotionData.Count);
    }

    [TestMethod]
    public void GetFirstNotSeenEmotionData_EmptyQueueAndOneSeenEmotionData_Success()
    {
        var firstEmotionData = new EmotionData(DateTime.Now, Emotions.GenerateRandomEmotion());
        _enrollmentSummary.AddEmotionData(firstEmotionData);
        Assert.AreEqual(firstEmotionData, _enrollmentSummary.GetFirstNotSeenEmotionData());
        Assert.AreEqual(Emotions.NODATA, _enrollmentSummary.GetFirstNotSeenEmotionData().GetWinningEmotion());
        Assert.AreEqual(1,                _enrollmentSummary.EmotionData.Count);
    }

    [TestMethod]
    public void GetLatestSeenEmotionData_EmptyQueueAndManySeenEmotionData_Success()
    {
        var firstEmotionData = new EmotionData(DateTime.Now, Emotions.GenerateRandomEmotion());
        Thread.Sleep(1000);
        var fourthEmotionData = new EmotionData(DateTime.Now, Emotions.GenerateRandomEmotion());
        Thread.Sleep(1000);
        var thirdEmotionData = new EmotionData(DateTime.Now, Emotions.GenerateRandomEmotion());
        Thread.Sleep(1000);
        var secondEmotionData = new EmotionData(DateTime.Now, Emotions.GenerateRandomEmotion());
        _enrollmentSummary.AddEmotionData(firstEmotionData);
        _enrollmentSummary.AddEmotionData(secondEmotionData);
        _enrollmentSummary.AddEmotionData(thirdEmotionData);
        _enrollmentSummary.AddEmotionData(fourthEmotionData);
        var emotionsData = _enrollmentSummary.GetFirstNotSeenEmotionData();
        var emotionsData1 = _enrollmentSummary.GetFirstNotSeenEmotionData();
        Assert.AreEqual(secondEmotionData, emotionsData);
        Assert.AreEqual(emotionsData1.GetWinningEmotion(),  Emotions.NODATA);
    }
}