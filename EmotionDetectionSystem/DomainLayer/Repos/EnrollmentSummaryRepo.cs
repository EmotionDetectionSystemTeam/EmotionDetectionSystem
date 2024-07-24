using System.Collections.Concurrent;
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
        private ConcurrentDictionary<string, EnrollmentSummary> _enrollmentSummaryByEmail;
        private bool _enableCache = true;
        public bool EnableCache
        {
            get => _enableCache;
            set => _enableCache = value;
        }

        public EnrollmentSummaryRepo(string lessonId)
        {
            _lessonId = lessonId;
            _enrollmentSummaryByEmail = new ConcurrentDictionary<string, EnrollmentSummary>();
            GetAll();
        }

        public void ClearCache()
        {
            _enrollmentSummaryByEmail.Clear();
        }

        public List<EnrollmentSummary> GetAll()
        {
            List<EnrollmentSummary> _enrollmentSummaries = DBHandler.Instance.GetAllEnrollmentSummariesPerLesson(_lessonId);
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
            Dictionary<string, List<string>> studentWiningEmotions = new Dictionary<string, List<string>>();
            foreach (var enrollmentSummary in _enrollmentSummaryByEmail.Values)
            {
                string studentEmail = enrollmentSummary.Student.Email.ToLower();
                List<string> studentEmotions = enrollmentSummary.GetAllWiningEmotionData();

                if (!studentWiningEmotions.ContainsKey(studentEmail))
                {
                    studentWiningEmotions.Add(studentEmail, studentEmotions);
                }
                else
                {
                    studentWiningEmotions[studentEmail].AddRange(studentEmotions);
                }
            }
            return studentWiningEmotions;
        }

        public void Add(EnrollmentSummary item)
        {
            DBHandler.Instance.AddEnrollmentSummary(item);
            CacheEnrollmentSummary(item);
        }

        public void Update(EnrollmentSummary item)
        {
            DBHandler.Instance.UpdateEnrollmentSummary(item);
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
            return _enrollmentSummaryByEmail.ContainsKey(item.Student.Email);
        }

        public void ResetDomainData()
        {
            _enrollmentSummaryByEmail.Clear();
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
            if (!_enrollmentSummaryByEmail.TryGetValue(userEmail.ToLower(), out EnrollmentSummary enrollmentSummary))
            {
                enrollmentSummary = DBHandler.Instance.GetEnrollmentSummaryPerStudentLesson(userEmail.ToLower(), _lessonId);
                if (enrollmentSummary == null)
                {
                    throw new Exception("Student didn't join the lesson.");
                }
                else
                {
                    CacheEnrollmentSummary(enrollmentSummary);
                }
            }
            enrollmentSummary.AddEmotionData(emotionData);
            DBHandler.Instance.UpdateEnrollmentSummary(enrollmentSummary);
        }
        public IEnumerable<EmotionData> GetEmotionDataEntries()
        {
            List<EnrollmentSummary> _enrollmentSummaries = _enrollmentSummaryByEmail.Values.ToList<EnrollmentSummary>();
            List<EmotionData> emotionsData = (new List<EmotionData>()).Concat(_enrollmentSummaries.SelectMany(x => x.GetAllEmotionData())).ToList<EmotionData>();
            foreach(EnrollmentSummary summary in _enrollmentSummaries)
            {
                Update(summary);
            }
            return emotionsData;

        }
        public IEnumerable<EnrollmentSummary> GetEnrollmentSummariesWithData()
        {
            List<EnrollmentSummary> _enrollmentSummaries = _enrollmentSummaryByEmail.Values.ToList<EnrollmentSummary>();
            return _enrollmentSummaries
                .Where(enrollmentSummary => enrollmentSummary.PeekFirstNotSeenEmotionData() != null).ToList();
        }
        public Dictionary<Student, EnrollmentSummary> GetStudentsEmotions()
        {
            List<EnrollmentSummary> _enrollmentSummaries = _enrollmentSummaryByEmail.Values.ToList<EnrollmentSummary>();
            return _enrollmentSummaries.ToDictionary(enrollmentSummary => enrollmentSummary.Student, enrollmentSummary => enrollmentSummary);
        }

        private void CacheEnrollmentSummary(EnrollmentSummary summary)
        {
            if (!EnableCache)
                return;
            string email = summary.Student.Email.ToLower();
            if (!_enrollmentSummaryByEmail.ContainsKey(email))
            {
                _enrollmentSummaryByEmail[email] = summary;
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
    }
}
