using System.Collections.Concurrent;
using System.Security.Cryptography;
using EmotionDetectionSystem.DataLayer;
using EmotionDetectionSystem.DomainLayer.objects;
using log4net;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace EmotionDetectionSystem.DomainLayer.Repos;

public class UserRepo : IRepo<User>
{
    private static ConcurrentDictionary<string, User> _userByEmail = new ConcurrentDictionary<string, User>();
    private static readonly ILog _logger = LogManager.GetLogger(typeof(UserRepo));
    private bool _enableCache = true;
    public bool EnableCache
    {
        get => _enableCache;
        set => _enableCache = value;
    }
    public UserRepo()
    {
        _userByEmail = new ConcurrentDictionary<string, User>();
    }
    public void CacheUsers(List<User> users)
    {
        if (!EnableCache)
            return;
        foreach (var user in users)
        {
            _userByEmail.AddOrUpdate(user.Email, user, (key, oldValue) => user);
        }
    }
    public void CacheUser(User user)
    {
        if (!EnableCache)
            return;
       _userByEmail.AddOrUpdate(user.Email, user, (key, oldValue) => user);
    }
    public List<User> GetAll()
    {
        List<User> users = DBHandler.Instance.GetAllUsers();
        CacheUsers(users);
        return users;
    }

    public User GetById(string id)
    {
        User user = GetByEmail(id);
        CacheUser(user);
        return user;
    }

    public User GetByEmail(string email)
    {
        if (_userByEmail.TryGetValue(email, out var user))
        {
            return user;
        }

        user = DBHandler.Instance.GetUserByEmail(email);
        if (user != null)
        {
            CacheUser(user);
        }
        else
        {
            _logger.Error($"User with email: {email} does not exist");
        }
        return user;
    }

    public void Add(User item)
    {
        if (_userByEmail.ContainsKey(item.Email))
        {
            var errorMsg = $"User with email: {item.Email} already exists";
            _logger.Error(errorMsg);
            throw new Exception(errorMsg);
        }

        DBHandler.Instance.AddUser(item);
        CacheUser(item);
    }

    public void Update(User item)
    {
        throw new NotImplementedException();
    }

    public void Delete(string id)
    {
        throw new NotImplementedException();
    }

    public bool ContainsID(string id)
    {
        throw new NotImplementedException();
    }
    public bool ContainsEmail(string email)
    {
        try
        {
            User user = DBHandler.Instance.GetUserByEmail(email);
            if (user != null && user.Email == email)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public bool ContainsValue(User item)
    {
        throw new NotImplementedException();
    }

    public void ResetDomainData()
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public Teacher GetTeacherByEmail(string email)
    {
        string lowerEmail = email.ToLower();
        if (_userByEmail.TryGetValue(lowerEmail, out var user) && user.Type.Equals("Teacher"))
            return (Teacher)user;
        Teacher teacher = DBHandler.Instance.GetTeacherByEmail(lowerEmail);
        CacheUser(teacher);
        return teacher;
    }
    public void ClearCache()
    {
        _userByEmail.Clear();
    }

}