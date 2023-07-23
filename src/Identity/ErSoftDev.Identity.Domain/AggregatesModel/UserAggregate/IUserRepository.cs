using ErSoftDev.DomainSeedWork;

namespace ErSoftDev.Identity.Domain.AggregatesModel.UserAggregate
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByUsernameAndPassword(string username, string password, CancellationToken cancellationToken);

        Task<User?> GetUser(long id);
    }
}
