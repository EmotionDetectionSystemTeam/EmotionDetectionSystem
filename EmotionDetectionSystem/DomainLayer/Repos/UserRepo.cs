using System.Collections.Concurrent;
using EmotionDetectionSystem.DomainLayer.objects;
using log4net;

namespace EmotionDetectionSystem.DomainLayer.Repos;

public class UserRepo : IRepo<User>
{
    private static          ConcurrentDictionary<string, User> _userByEmail;
    private static readonly ILog                               _logger = LogManager.GetLogger(typeof(UserRepo));
    
    public UserRepo()
    {
        _userByEmail = new ConcurrentDictionary<string, User>();
    }

    public List<User> GetAll()
    {
        return _userByEmail.Values.ToList();
    }

    public User GetById(string email)
    {
        if (!_userByEmail.ContainsKey(email))
        {
            _logger.ErrorFormat($"User with email: {email} does not exist");
            return null;
        }

        return _userByEmail[email];
    }

    public User GetByEmail(string email)
    {
        if (_userByEmail.TryGetValue(email, out var value)) return value;
        var errorMsg = $"User with email: {email} does not exist";
        _logger.ErrorFormat(errorMsg);
        throw new Exception(errorMsg);

    }

    public void Add(User item)
    {
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
        return _userByEmail.ContainsKey(email);
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
}