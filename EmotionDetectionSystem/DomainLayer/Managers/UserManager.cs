using System.Collections.Concurrent;
using EmotionDetectionSystem.DomainLayer.objects;
using log4net;
using Microsoft.Extensions.Logging;

namespace EmotionDetectionSystem.DomainLayer.Managers;

public class UserManager
{
    private static ConcurrentDictionary<string, User> _userBySession;
    private static ConcurrentDictionary<string, User> _userByEmail;
    private        Security                           _paswwordSecurity;
    private static readonly ILog _logger = LogManager.GetLogger(typeof(UserManager));

    public UserManager()
    {
        _userBySession    = new ConcurrentDictionary<string, User>();
        _userByEmail      = new ConcurrentDictionary<string, User>();
        _paswwordSecurity = new Security();
    }

    public void Register(string email, string firstName, string lastName, string password, bool isStudent)
    {
        email = email.ToLower();
        if (_userByEmail.ContainsKey(email))
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
        _userByEmail.TryAdd(email, user);
    }

    public User Login(string sessionId, string email, string password)
    {
        email = email.ToLower();
        if (isLoggedIn(sessionId, email))
        {
            throw new Exception("User is already logged in");
        }

        if (!_userByEmail.ContainsKey(email))
        {
            throw new Exception("User does not exist");
        }

        User user = _userByEmail[email];
        if (!_paswwordSecurity.VerifyPassword(password, user.Password))
        {
            throw new Exception("Password is incorrect");
        }
        _userBySession.TryAdd(sessionId, user);
        return user;
    }

    private bool isLoggedIn(string sessionId, string email)
    {
        var user = _userBySession.Values.FirstOrDefault((user) => user.Email.Equals(email));
        return _userBySession.ContainsKey(sessionId) || user != null;
    }

    public void Logout(string sessionId)
    {
        if (!_userBySession.ContainsKey(sessionId))
        {
            throw new Exception("Session does not exist");
        }
        _userBySession.TryRemove(sessionId, out User user);
        if (user!=null)
        {
            _logger.InfoFormat($"User with email: {user.Email} and sessionId {sessionId} has been logged out");
        }
    }
}