﻿using System.Linq.Expressions;
using ErSoftDev.DomainSeedWork;
using ErSoftDev.Framework.Configuration;
using ErSoftDev.Identity.Domain.AggregatesModel.UserAggregate;
using IdGen;
using Microsoft.EntityFrameworkCore;

namespace ErSoftDev.Identity.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository, ITransientDependency
    {
        private readonly IdentityDbContext _identityDbContext;
        public IUnitOfWork UnitOfWork => _identityDbContext;

        public UserRepository(IdentityDbContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }

        public async Task Add(User user)
        {
            await _identityDbContext.Users.AddAsync(user);
        }

        public async Task<User?> Get(long id, CancellationToken cancellationToken)
        {
            return await _identityDbContext.Users.FirstOrDefaultAsync(user => user.Id == id,
                cancellationToken: cancellationToken);
        }

        public async Task<List<User>> GetList(Expression<Func<User, bool>> predicate)
        {
            return await _identityDbContext.Users.Where(predicate).ToListAsync();
        }

        public async Task<User?> GetByUsernameAndPassword(string username, string password, CancellationToken cancellationToken)
        {
            return await _identityDbContext.Users.Include(user => user.UserRefreshTokens).FirstOrDefaultAsync(
                user => user.Username == username && user.Password == password, cancellationToken);
        }

        public async Task<User?> GetUser(long id)
        {
            return await _identityDbContext.Users
                .Include(user => user.UserLogins)
                .Include(user => user.UserRefreshTokens)
                .Include(user => user.UserRoles)
                .FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<User?> GetByRefreshToken(string refreshToken)
        {
            var refreshTokenResponse = await _identityDbContext.Users
                .SelectMany(user => user.UserRefreshTokens.Where(token => token.Token == refreshToken)).ToListAsync();
            if (refreshTokenResponse.Count <= 0)
                return null;

            return await _identityDbContext.Users
                .Include(user => user.UserLogins)
                .Include(user => user.UserRefreshTokens)
                .FirstOrDefaultAsync(user => user.Id == refreshTokenResponse.First().UserId);
        }
    }
}
