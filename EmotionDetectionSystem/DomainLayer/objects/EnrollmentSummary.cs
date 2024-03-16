using System.Collections.Concurrent;

namespace EmotionDetectionSystem.DomainLayer.objects;

public class EnrollmentSummary
{
    private Student                      _student;
    private Lesson                       _lesson;
    private List<EmotionData>            _emotionData;
    private object                       _lock = new object();
    private Queue<EmotionData> _notSeenEmotionDataQueue;

    public EnrollmentSummary(Student student, Lesson lesson, List<EmotionData> emotionData)
    {
        _student     = student;
        _lesson      = lesson;
        _emotionData = emotionData;
        _notSeenEmotionDataQueue = new Queue<EmotionData>();
    }

    public EnrollmentSummary(Student student, Lesson lesson)
    {
        _student                 = student;
        _lesson                  = lesson;
        _emotionData             = new List<EmotionData>();
        _notSeenEmotionDataQueue = new Queue<EmotionData>();
    }
    
    public void AddEmotionData(EmotionData emotionData)
    {
        lock (_lock)
        {
            _notSeenEmotionDataQueue.Enqueue(emotionData);
        }
    }
    
    public List<EmotionData> GetAllEmotionData()
    {
        lock (_lock)
        {
            while (_notSeenEmotionDataQueue.TryDequeue(out var emotionData))
            {
                _emotionData.Add(emotionData);
            }
        }

        return _emotionData;
    }
    public List<string> getAllWiningEmotionData()
    {
        List<string> WinningEmotions = new List<String>();
        foreach(EmotionData emotionData in _emotionData)
        {
            WinningEmotions.Add(emotionData.GetWinningEmotion());
        }
        return WinningEmotions;

    }

    public Student Student
    {
        get { return _student; }
        set { _student = value; }
    }

    public Lesson Lesson
    {
        get { return _lesson; }
        set { _lesson = value; }
    }
    
    public Queue<EmotionData> NotSeenEmotionDataQueue
    {
        get => _notSeenEmotionDataQueue;
        set { _notSeenEmotionDataQueue = value; }
    }

    public List<EmotionData> EmotionData
    {
        get
        {
            lock (_lock)
            {
                while (_notSeenEmotionDataQueue.TryDequeue(out var emotionData))
                {
                    _emotionData.Add(emotionData);
                }
            }

            return _emotionData;
        }
        set { _emotionData = value; }
    }
    
    public EmotionData GetFirstNotSeenEmotionData()
    {
        lock (_lock)
        {
            EmotionData emotionData;
            if (_notSeenEmotionDataQueue.Count > 0)
            {
                emotionData = _notSeenEmotionDataQueue.Dequeue();
                _emotionData.Add(emotionData);
            }
            else
            {
                emotionData = _emotionData.FindLast(ed => ed.Time == _emotionData.Max(ed => ed.Time));
            }
            return emotionData!;
        }
    }
    
    public EmotionData PeekFirstNotSeenEmotionData()
    {
        lock (_lock)
        {
            EmotionData emotionData;
            if (_notSeenEmotionDataQueue.Count > 0)
            {
                emotionData = _notSeenEmotionDataQueue.Peek();
            }
            else
            {
                emotionData = _emotionData.FindLast(ed => ed.Time == _emotionData.Max(ed => ed.Time));
            }
            return emotionData!;
        }
    }

    public List<string> getPreviousEmotionData()
    {
        var winningEmotions = new List<string>();
        foreach(var emotionData in _emotionData)
        {
            var winningEmotion = emotionData.GetWinningEmotion();
            if (winningEmotion != null)
            {
                winningEmotions.Add(winningEmotion);
            }
        }
        return winningEmotions;
    }
}