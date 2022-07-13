using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Tests.Integration
{
    internal sealed class TestUserRepisotory: IUserRepository
    {
        private readonly List<User> _users = new();
        
        public async Task<User> GetByIdAsync(UserId id)
        {
            await Task.CompletedTask;
            return _users.SingleOrDefault(u => u.Id == id);
        }

        public async Task<User> GetByEmailAsync(Email email)
        {
            await Task.CompletedTask;
            return _users.SingleOrDefault(u => u.Email== email);
        }

        public async Task<User> GetByUsernameAsync(Username username)
        {
            await Task.CompletedTask;
            return _users.SingleOrDefault(u => u.Username == username);
        }

        public async Task AddAsync(User user)
        {
            _users.Add(user);
            await Task.CompletedTask;
        }
    }
}
