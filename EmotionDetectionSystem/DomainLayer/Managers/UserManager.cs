using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using EmotionDetectionSystem.DomainLayer.objects;
using EmotionDetectionSystem.DomainLayer.Repos;
using log4net;

namespace EmotionDetectionSystem.DomainLayer.Managers;

public class UserManager
{
    private readonly        UserRepo                           _userRepo;
    private static          ConcurrentDictionary<string, User> _userBySession = null!;
    private readonly        Security                           _passwordSecurity;
    private static readonly ILog                               Log = LogManager.GetLogger(typeof(UserManager));
    public UserManager()
    {
        _userRepo         = new UserRepo();
        _passwordSecurity = new Security();
        _userBySession    = new ConcurrentDictionary<string, User>();
    }

    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="correlationId">The correlation ID for logging.</param>
    /// <param name="email">The email address of the user.</param>
    /// <param name="firstName">The first name of the user.</param>
    /// <param name="lastName">The last name of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <param name="userType">The type of the user, represented as an integer.</param>
    public void Register(string correlationId, string email, string firstName, string lastName, string password,
                         int    userType)
    {
        Log.Info($"[{correlationId}] Validating input for user: {email}");
        ValidateInput(email, firstName, lastName, password);

        email = email.ToLower();
        var type = (UserType)userType;

        if (type is UserType.Teacher or UserType.Admin)
        {
            Log.Info($"[{correlationId}] Validating teacher or admin: {email}");
            ValidateTeacherOrAdmin(email, password);
        }

        var user = CreateUser(email, firstName, lastName, password, type);
        _userRepo.Add(user);
        Log.Info($"[{correlationId}] User registered: {email}");
    }

    private void ValidateInput(string email, string firstName, string lastName, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
            string.IsNullOrEmpty(password))
        {
            throw new Exception("One or more fields are empty");
        }
    }

    private void ValidateTeacherOrAdmin(string email, string password)
    {
        if (!IsValidEmail(email))
        {
            throw new Exception("Email is not valid");
        }

        if (_userRepo.ContainsEmail(email))
        {
            throw new Exception("Email is already in use");
        }

        if (!_passwordSecurity.IsValidPassword(password))
        {
            throw new Exception("Password is not valid");
        }
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

    /// <summary>
    /// Logs in a user in the system.
    /// </summary>
    /// <param name="correlationId">The correlation ID for logging.</param>
    /// <param name="sessionId">The session ID of the user.</param>
    /// <param name="email">The email address of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <returns>The logged-in user object.</returns>
    public User Login(string correlationId, string sessionId, string email, string password)
    {
        Log.Info($"[{correlationId}] Attempting to log in user: {email}");

        email = email.ToLower();
        if (IsLoggedIn(sessionId, email))
        {
            Log.Warn($"[{correlationId}] User {email} is already logged in with session {sessionId}");
            RemoveUserFromSession(correlationId,sessionId, email);
        }

        if (!_userRepo.ContainsEmail(email))
        {
            Log.Error($"[{correlationId}] User {email} does not exist");
            throw new Exception("User does not exist");
        }

        var user = _userRepo.GetByEmail(email);
        if (!_passwordSecurity.VerifyPassword(password, user.Password))
        {
            Log.Error($"[{correlationId}] Incorrect password or username for user {email}");
            throw new Exception("Password or username are incorrect");
        }

        _userBySession?.TryAdd(sessionId, user);
        Log.Info($"[{correlationId}] User {email} logged in successfully with session {sessionId}");
        return user;
    }

    private void RemoveUserFromSession(string correlationId, string sessionId, string email)
    {
        _userBySession.TryRemove(sessionId, out var user);
        var canChangeSession = user != null && user.Email.Equals(email) || user == null;
        if (canChangeSession) return;
        var msg = $"User {email} is not logged in with session {sessionId}";
        Log.Warn($"[{correlationId}] {msg}");
        throw new Exception(msg);
    }

    private bool IsLoggedIn(string sessionId, string email)
    {
        var user = _userBySession.Values.FirstOrDefault((user) => user.Email.Equals(email));
        return _userBySession.ContainsKey(sessionId) || user != null;
    }

    public bool IsValidSession(string sessionId, string email)
    {
        if (!_userBySession.TryGetValue(sessionId, out var user))
        {
            Log.ErrorFormat($"Session: {sessionId} is not valid");
            return false;
        }

        if (user.Email.Equals(email.ToLower())) return true;
        Log.ErrorFormat($"Session: {sessionId} is not valid for user with email: {email}");
        return false;
    }

    /// <summary>
    /// Logs out a user from the system.
    /// </summary>
    /// <param name="correlationId">The correlation ID for logging.</param>
    /// <param name="sessionId">The session ID of the user.</param>
    public void Logout(string correlationId, string sessionId)
    {
        if (!_userBySession.ContainsKey(sessionId))
        {
            Log.Warn($"[{correlationId}] Session {sessionId} does not exist.");
            throw new Exception("Session does not exist");
        }

        _userBySession.TryRemove(sessionId, out var user);
        if (user != null)
        {
            Log.Info($"[{correlationId}] User with email: {user.Email} and sessionId {sessionId} has been logged out");
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

    public Student GetStudent(string studentEmail)
    {
        var student = _userRepo.GetByEmail(studentEmail);
        return student as Student ?? throw new Exception($"There is no student with email: {studentEmail}");
    }

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        const string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        return Regex.IsMatch(email, pattern);
    }
}