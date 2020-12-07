using webapi.data;
using webapi.model;

namespace webapi.Repository
{
    public class UserRepository : EFRepository<User, ApplicationDbContext>
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}