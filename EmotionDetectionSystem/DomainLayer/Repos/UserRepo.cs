using System.Collections.Concurrent;
using EmotionDetectionSystem.DataLayer;
using EmotionDetectionSystem.DomainLayer.objects;
using log4net;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace EmotionDetectionSystem.DomainLayer.Repos;

public class UserRepo : IRepo<User>
{
    private static ConcurrentDictionary<string, User> _userByEmail = new ConcurrentDictionary<string, User>();
    private static readonly ILog _logger = LogManager.GetLogger(typeof(UserRepo));

    public UserRepo()
    {
        _userByEmail = new ConcurrentDictionary<string, User>();
    }

    public List<User> GetAll()
    {
        List<User> users = DBHandler.Instance.GetAllUsers();
        foreach (var user in users)
        {
            _userByEmail.AddOrUpdate(user.Email, user, (key, oldValue) => user);
        }
        return users;
    }

    public User GetById(string id)
    {
        return GetByEmail(id);
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
            _userByEmail.TryAdd(email, user);
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
        _userByEmail.TryAdd(item.Email, item);
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
        return DBHandler.Instance.GetTeacherByEmail(email);
    }
    public void ClearCache()
    {
        _userByEmail.Clear();
    }

}