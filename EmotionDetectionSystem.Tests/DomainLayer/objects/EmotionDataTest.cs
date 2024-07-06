using System;
using EmotionDetectionServer;
using EmotionDetectionSystem.DomainLayer.objects;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmotionDetectionSystem.Tests.DomainLayer.objects;

[TestClass]
[TestSubject(typeof(EmotionData))]
public class EmotionDataTest
{

    [TestMethod]
    public void GetWinningEmotionTest()
    {
        var emotionData = new EmotionData(DateTime.Now, Emotions.FEARFUL);
        Assert.AreEqual(Emotions.FEARFUL, emotionData.GetWinningEmotion());
    }
    
    [TestMethod]
    public void GetWinningEmotionTest_CheckingNeutralNormalization()
    {
        var emotionData = new EmotionData(DateTime.Now, Emotions.FEARFUL);
        Assert.AreEqual(Emotions.FEARFUL, emotionData.GetWinningEmotion());
    }
    
}