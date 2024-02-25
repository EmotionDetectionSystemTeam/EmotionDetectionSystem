using System.Collections.Concurrent;
using EmotionDetectionSystem.DomainLayer.objects;
using EmotionDetectionSystem.RepoLayer;
using log4net;
using Microsoft.Extensions.Logging;

namespace EmotionDetectionSystem.DomainLayer.Managers;

public class UserManager
{
    private readonly        UserRepo                           _userRepo;
    private static          ConcurrentDictionary<string, User> _userBySession = null!;
    private readonly        Security                           _passwordSecurity;
    private static readonly ILog                               Logger = LogManager.GetLogger(typeof(UserManager));

    public UserManager()
    {
        _userRepo         = new UserRepo();
        _passwordSecurity = new Security();
        _userBySession    = new ConcurrentDictionary<string, User>();
    }

    public void Register(string email, string firstName, string lastName, string password, int userType)
    {
        email = email.ToLower();
        if (_userRepo.ContainsEmail(email))
        {
            throw new Exception("Email is already in use");
        }

        if (!_passwordSecurity.IsValidPassword(password))
        {
            throw new Exception("Password is not valid");
        }

        var type = (UserType)userType;
        var user = CreateUser(email, firstName, lastName, password, type);
        _userRepo.Add(user);
    }

    private User CreateUser(string email, string firstName, string lastName, string password, UserType userType)
    {
        var encryptedPassword = _passwordSecurity.HashPassword(password);
        return userType switch
        {
            UserType.Student => new Student(email, firstName, lastName, encryptedPassword),
            UserType.Teacher => new Teacher(email, firstName, lastName, encryptedPassword),
            UserType.Admin   => new Admin(email, firstName, lastName, encryptedPassword),
            _                => throw new Exception("User type is not valid")
        };
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

        var user = _userRepo.GetByEmail(email);
        if (!_passwordSecurity.VerifyPassword(password, user.Password))
        {
            throw new Exception("Password is incorrect");
        }

        _userBySession?.TryAdd(sessionId, user);
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
            Logger.ErrorFormat($"Session: {sessionId} is not valid");
            return false;
        }

        if (_userBySession[sessionId].Email.Equals(email)) return true;
        Logger.ErrorFormat($"Session: {sessionId} is not valid for user with email: {email}");
        return false;

    }

    public void Logout(string sessionId)
    {
        if (!_userBySession.ContainsKey(sessionId))
        {
            throw new Exception("Session does not exist");
        }

        _userBySession.TryRemove(sessionId, out var user);
        if (user != null)
        {
            Logger.InfoFormat($"User with email: {user.Email} and sessionId {sessionId} has been logged out");
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

    public User GetUser(string email)
    {
        return _userRepo.GetByEmail(email);
    }
}