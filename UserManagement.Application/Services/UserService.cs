// ===== UserManagement.Application =====
// File: Services/UserService.cs

using UserManagement.Application.DTOs;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Enums;
using System.Text.RegularExpressions;
using System.Reflection;

namespace UserManagement.Application.Services;

public class UserService
{
    private readonly List<User> _users = new();

    public void CreateUser(CreateUserRequest request)
    {
        ValidateLogin(request.Login);
        ValidatePassword(request.Password);
        ValidateName(request.Name);

        if (_users.Any(u => u.Login == request.Login))
            throw new Exception("Login already exists");

        var creator = _users.FirstOrDefault(u => u.Login == request.CreatedBy && u.Admin);
        if (creator == null)
            throw new Exception("Only admin can create users");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Login = request.Login,
            PasswordHash = request.Password,
            Name = request.Name,
            Gender = request.Gender,
            Birthday = request.Birthday,
            Admin = request.Admin,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = request.CreatedBy
        };

        _users.Add(user);
    }

    public void UpdateInfo(string login, string name, Gender gender, DateTime? birthday, string requester)
    {
        var user = GetActiveUser(login);
        var actor = GetActiveUser(requester);
        if (!actor.Admin && actor.Login != user.Login)
            throw new Exception("Permission denied");

        ValidateName(name);
        user.Name = name;
        user.Gender = gender;
        user.Birthday = birthday;
        user.ModifiedOn = DateTime.UtcNow;
        user.ModifiedBy = requester;
    }

    public void UpdatePassword(string login, string newPassword, string requester)
    {
        var user = GetActiveUser(login);
        var actor = GetActiveUser(requester);
        if (!actor.Admin && actor.Login != user.Login)
            throw new Exception("Permission denied");

        ValidatePassword(newPassword);
        user.PasswordHash = newPassword;
        user.ModifiedOn = DateTime.UtcNow;
        user.ModifiedBy = requester;
    }

    public void UpdateLogin(string oldLogin, string newLogin, string requester)
    {
        var user = GetActiveUser(oldLogin);
        var actor = GetActiveUser(requester);
        if (!actor.Admin && actor.Login != user.Login)
            throw new Exception("Permission denied");

        ValidateLogin(newLogin);
        if (_users.Any(u => u.Login == newLogin))
            throw new Exception("Login already in use");

        user.Login = newLogin;
        user.ModifiedOn = DateTime.UtcNow;
        user.ModifiedBy = requester;
    }

    public IEnumerable<User> GetActiveUsers(string requester)
    {
        EnsureAdmin(requester);
        return _users.Where(u => u.RevokedOn == null).OrderBy(u => u.CreatedOn);
    }

    public object GetUser(string login, string requester)
    {
        EnsureAdmin(requester);
        var user = _users.FirstOrDefault(u => u.Login == login);
        if (user == null) throw new Exception("User not found");
        return new
        {
            user.Name,
            user.Gender,
            user.Birthday,
            IsActive = user.RevokedOn == null
        };
    }

    public User Authenticate(string login, string password)
    {
        var user = _users.FirstOrDefault(u => u.Login == login && u.PasswordHash == password);
        if (user == null || user.RevokedOn != null)
            throw new Exception("Invalid credentials or inactive user");
        return user;
    }

    public IEnumerable<User> GetUsersOlderThan(int age, string requester)
    {
        EnsureAdmin(requester);
        var today = DateTime.Today;
        return _users.Where(u => u.Birthday.HasValue && (today.Year - u.Birthday.Value.Year) > age);
    }

    public void DeleteUser(string login, bool soft, string requester)
    {
        EnsureAdmin(requester);
        var user = _users.FirstOrDefault(u => u.Login == login);
        if (user == null) throw new Exception("User not found");

        if (soft)
        {
            user.RevokedOn = DateTime.UtcNow;
            user.RevokedBy = requester;
        }
        else
        {
            _users.Remove(user);
        }
    }

    public void RestoreUser(string login, string requester)
    {
        EnsureAdmin(requester);
        var user = _users.FirstOrDefault(u => u.Login == login);
        if (user == null) throw new Exception("User not found");

        user.RevokedOn = null;
        user.RevokedBy = null;
    }

    private void EnsureAdmin(string login)
    {
        var user = _users.FirstOrDefault(u => u.Login == login && u.Admin && u.RevokedOn == null);
        if (user == null) throw new Exception("Admin access required");
    }

    private User GetActiveUser(string login)
    {
        var user = _users.FirstOrDefault(u => u.Login == login && u.RevokedOn == null);
        if (user == null) throw new Exception("Active user not found");
        return user;
    }

    private void ValidateLogin(string login)
    {
        if (!Regex.IsMatch(login, "^[a-zA-Z0-9]+$"))
            throw new Exception("Login must contain only Latin letters and digits");
    }

    private void ValidatePassword(string password)
    {
        if (!Regex.IsMatch(password, "^[a-zA-Z0-9]+$"))
            throw new Exception("Password must contain only Latin letters and digits");
    }

    private void ValidateName(string name)
    {
        if (!Regex.IsMatch(name, "^[a-zA-Zа-яА-Я]+$"))
            throw new Exception("Name must contain only letters");
    }
}
