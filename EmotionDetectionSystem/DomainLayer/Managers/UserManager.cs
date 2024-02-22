using System.Collections.Concurrent;
using EmotionDetectionSystem.DomainLayer.objects;
using Microsoft.Extensions.Logging;

namespace EmotionDetectionSystem.DomainLayer.Managers;

public class UserManager
{
    private static ConcurrentDictionary<string, User> _userBySession;
    private static ConcurrentDictionary<string, User> _userByEmail;
    private Security _paswwordSecurity;
    
    public UserManager()
    {
        _userBySession = new ConcurrentDictionary<string, User>();
        _userByEmail = new ConcurrentDictionary<string, User>();
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
        User user = isStudent ? new Student(email, firstName, lastName, _paswwordSecurity.HashPassword(password)) 
            : new Teacher(email, firstName, lastName, password);
        _userByEmail.TryAdd(email, user);
    }

    public string Login(string sessionId, string email, string password)
    {
        email = email.ToLower();
        if (isLoggedIn(sessionId))
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
        {
            throw new Exception("Password is incorrect");
        }
        
    }
    
    private bool isLoggedIn(string sessionId)
    {
        return _userBySession.ContainsKey(sessionId);
    }

    public void Logout(string sessionId)
    {
        throw new NotImplementedException();
    }
    
}