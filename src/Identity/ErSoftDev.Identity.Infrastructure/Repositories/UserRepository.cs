using System.Linq.Expressions;
using Common.Contracts;
using ErSoftDev.DomainSeedWork;
using ErSoftDev.Identity.Domain.AggregatesModel.UserAggregate;
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

        public async Task<User?> GetUserByUsernameAndPassword(string username, string password, CancellationToken cancellationToken)
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
    }
}
