using System.Collections.Concurrent;
using EmotionDetectionSystem.DomainLayer.objects;
using EmotionDetectionSystem.RepoLayer;
using log4net;
using Microsoft.Extensions.Logging;

namespace EmotionDetectionSystem.DomainLayer.Managers;

public class UserManager
{
    private                 UserRepo                           _userRepo;
    private static          ConcurrentDictionary<string, User> _userBySession;
    private                 Security                           _paswwordSecurity;
    private static readonly ILog                               _logger = LogManager.GetLogger(typeof(UserManager));

    public UserManager()
    {
        _userRepo         = new UserRepo();
        _paswwordSecurity = new Security();
        _userBySession    = new ConcurrentDictionary<string, User>();
    }

    public void Register(string email, string firstName, string lastName, string password, bool isStudent)
    {
        email = email.ToLower();
        if (_userRepo.GetByEmail(email) != null)
        {
            throw new Exception("Email is already in use");
        }

        if (!_paswwordSecurity.IsValidPassword(password))
        {
            throw new Exception("Password is not valid");
        }

        User user = isStudent
            ? new Student(email, firstName, lastName, _paswwordSecurity.HashPassword(password))
            : new Teacher(email, firstName, lastName, password);
        _userRepo.Add(user);
    }

    public User Login(string sessionId, string email, string password)
    {
        email = email.ToLower();
        if (IsLoggedIn(sessionId, email))
        {
            throw new Exception("User is already logged in");
        }

        if (!_userRepo.ContainsEmail(email))
        {
            throw new Exception("User does not exist");
        }

        User user = _userRepo.GetByEmail(email);
        if (!_paswwordSecurity.VerifyPassword(password, user.Password))
        {
            throw new Exception("Password is incorrect");
        }

        _userBySession.TryAdd(sessionId, user);
        return user;
    }

    private bool IsLoggedIn(string sessionId, string email)
    {
        var user = _userBySession.Values.FirstOrDefault((user) => user.Email.Equals(email));
        return _userBySession.ContainsKey(sessionId) || user != null;
    }

    public bool IsValidSession(string sessionId, string email)
    {
        if (!_userBySession.ContainsKey(sessionId))
        {
            _logger.ErrorFormat($"Session: {sessionId} is not valid");
            return false;
        }

        if (!_userBySession[sessionId].Email.Equals(email))
        {
            _logger.ErrorFormat($"Session: {sessionId} is not valid for user with email: {email}");
            return false;
        }

        return true;
    }

    public void Logout(string sessionId)
    {
        if (!_userBySession.ContainsKey(sessionId))
        {
            throw new Exception("Session does not exist");
        }

        _userBySession.TryRemove(sessionId, out User user);
        if (user != null)
        {
            _logger.InfoFormat($"User with email: {user.Email} and sessionId {sessionId} has been logged out");
        }
    }

    public Teacher GetTeacher(string email)
    {
        var user = _userRepo.GetByEmail(email);
        if (user is Teacher teacher)
        {
            return teacher;
        }
        throw new Exception("User is not a teacher");
    }
}