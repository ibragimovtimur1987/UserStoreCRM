namespace UserStore.BLL.Interfaces
{
    public interface IServiceCreator
    {
        IRequestService CreateUserService(string connection);
    }
}
