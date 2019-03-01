using UserStore.BLL.Interfaces;
using UserStore.DAL.Repositories;

namespace UserStore.BLL.Services
{
    public class ServiceCreator : IServiceCreator
    {
        public IRequestService CreateUserService(string connection)
        {
            return new RequestService(new IdentityUnitOfWork(connection));
        }
    }
}
