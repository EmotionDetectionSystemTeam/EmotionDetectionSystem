using System.Collections.Generic;
using System.Linq;
using EmotionDetectionSystem.DataLayer;
using EmotionDetectionSystem.DomainLayer.objects;
using EmotionDetectionSystem.ServiceLayer.objects;

namespace EmotionDetectionSystem.DomainLayer.Repos
{
    public class EnrollmentSummaryRepo : IRepo<EnrollmentSummary>
    {
        private readonly string _lessonId;
        private List<EnrollmentSummary> _enrollmentSummaries;
        private Dictionary<string, EnrollmentSummary> _enrollmentSummaryByEmail;
        private Dictionary<string, List<string>> _studentWinningEmotions;
        private Dictionary<string, EmotionData> _emotionDataByEmail;
        private bool _enableCache = true;
        public bool EnableCache
        {
            get => _enableCache;
            set => _enableCache = value;
        }

        public EnrollmentSummaryRepo(string lessonId)
        {
            _lessonId = lessonId;
            _enrollmentSummaries = new List<EnrollmentSummary>();
            _enrollmentSummaryByEmail = new Dictionary<string, EnrollmentSummary>();
            _studentWinningEmotions = new Dictionary<string, List<string>>();
            _emotionDataByEmail = new Dictionary<string, EmotionData>();
        }
        
        public void ClearCache()
        {
            _enrollmentSummaries.Clear();
            _enrollmentSummaryByEmail.Clear();
            _studentWinningEmotions.Clear();
            _emotionDataByEmail.Clear();
        }

        public List<EnrollmentSummary> GetAll()
        {
            _enrollmentSummaries = DBHandler.Instance.GetAllEnrollmentSummariesPerLesson(_lessonId);
            CacheEnrollmentSummaries(_enrollmentSummaries);
            return _enrollmentSummaries;
        }

        public EnrollmentSummary GetById(string email)
        {
            string lower_email = email.ToLower();
            if (_enrollmentSummaryByEmail.TryGetValue(lower_email, out var enrollmentSummary))
            {
                return enrollmentSummary;
            }

            enrollmentSummary = DBHandler.Instance.GetEnrollmentSummaryByEmail(_lessonId, lower_email);
            if (enrollmentSummary != null)
            {
                CacheEnrollmentSummary(enrollmentSummary);
            }
            return enrollmentSummary;
        }

        public Dictionary<string, List<string>> GetStudentWiningEmotions()
        {
            if (_studentWinningEmotions.Count == 0)
            {
                _studentWinningEmotions = DBHandler.Instance.GetStudentWiningEmotions(_lessonId);
            }
            return _studentWinningEmotions;
        }

        public void Add(EnrollmentSummary item)
        {
            DBHandler.Instance.AddEnrollmentSummary(item);
            CacheEnrollmentSummary(item);
        }

        public void Update(EnrollmentSummary item)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public bool ContainsID(string id)
        {
            return _enrollmentSummaryByEmail.ContainsKey(id);
        }

        public bool ContainsValue(EnrollmentSummary item)
        {
            return _enrollmentSummaryByEmail.ContainsValue(item);
        }

        public void ResetDomainData()
        {
            _enrollmentSummaries.Clear();
            _enrollmentSummaryByEmail.Clear();
            _studentWinningEmotions.Clear();
            _emotionDataByEmail.Clear();
        }

        public void Clear()
        {
            ResetDomainData();
        }

        public bool ContainStudent(Student student)
        {
            return (GetById(student.Email) != null);
        }

        public void PutEmotionData(string userEmail, EmotionData emotionData)
        {
            DBHandler.Instance.PutEmotionData(userEmail, _lessonId, emotionData);
            CacheEmotionData(userEmail, emotionData);
        }

        public IEnumerable<EmotionData> GetEmotionDataEntries()
        {
            return DBHandler.Instance.GetEmotionDataEntries(_lessonId);

        }

        public IEnumerable<EnrollmentSummary> GetEnrollmentSummariesWithData()
        {
            return DBHandler.Instance.GetEnrollmentSummariesWithData(_lessonId);
        }

        public Dictionary<Student, EnrollmentSummary> GetStudentsEmotions()
        {
            return DBHandler.Instance.GetStudentsEmotions(_lessonId);
        }

        private void CacheEnrollmentSummary(EnrollmentSummary summary)
        {
            if (!EnableCache)
                return;
            if (!_enrollmentSummaryByEmail.ContainsKey(summary.Student.Email))
            {
                _enrollmentSummaryByEmail[summary.Student.Email] = summary;
            }
        }

        private void CacheEnrollmentSummaries(IEnumerable<EnrollmentSummary> summaries)
        {
            if (!EnableCache)
                return;
            if (summaries == null) {  return; }
            foreach (var summary in summaries)
            {
                CacheEnrollmentSummary(summary);
            }
        }

        private void CacheEmotionData(string email, EmotionData data)
        {
            if (!EnableCache)
                return;
            if (!_emotionDataByEmail.ContainsKey(email))
            {
                _emotionDataByEmail[email] = data;
            }
        }
    }
}
