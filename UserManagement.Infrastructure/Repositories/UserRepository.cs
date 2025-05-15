using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Repositories;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Data;

namespace UserManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public User? GetByLogin(string login)
        {
            return _context.Users.FirstOrDefault(u => u.Login == login);
        }

        public User? GetByLoginAndPasswordHash(string login, string passwordHash)
        {
            return _context.Users
                .FirstOrDefault(u => u.Login == login && u.PasswordHash == passwordHash && u.RevokedOn == null);
        }

        public IEnumerable<User> GetAllActive()
        {
            return _context.Users
                .Where(u => u.RevokedOn == null)
                .OrderBy(u => u.CreatedOn)
                .ToList();
        }

        public IEnumerable<User> GetUsersOlderThan(int age)
        {
            var dateThreshold = DateTime.Today.AddYears(-age);
            return _context.Users
                .Where(u => u.Birthday.HasValue && u.Birthday.Value <= dateThreshold && u.RevokedOn == null)
                .ToList();
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
