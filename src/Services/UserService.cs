using SmartRide.src.DataStructures;

namespace SmartRide.src.Services;
public class UserService
{
    private readonly SmartRideDbContext _dbContext;
    private readonly HashMap<string, UserDto> _userMap;

    public UserService(SmartRideDbContext context)
    {
        _dbContext = context;
        _userMap = new HashMap<string, UserDto>(100);

        // Load data from the database into the HashMap
        LoadUsersFromDb();
    }

    private void LoadUsersFromDb()
    {
        var users = _dbContext.Users.ToList();
        foreach (var user in users)
        {
            _userMap.Put(user.Email, new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.Phonenumber
            });
        }
    }

    public void AddUser(UserDto user)
    {
        if (_userMap.ContainsKey(user.Email))
            throw new ArgumentException("Email has already used");

        var u = new User()
        {
            Name = user.Name,
            Email = user.Email,
            Phonenumber = user.PhoneNumber
        };
        _dbContext.Users.Add(u);
        _dbContext.SaveChanges();

        // getting the id
        user.Id = u.Id;
        Console.WriteLine($"User Id=> {user.Id}");
        _userMap.Put(u.Email, user);
    }

    public UserDto? GetUser(string email)
    {
        return _userMap.Get(email);
    }

    public void RemoveUser(string email)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Email == email) 
            ?? throw new Exception("User not found.");
        _dbContext.Users.Remove(user);
        _dbContext.SaveChanges();

        _userMap.Remove(email);
    }
}

