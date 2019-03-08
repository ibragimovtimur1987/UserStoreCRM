using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using UserStore.BLL.DTO;
using UserStore.BLL.Infrastructure;
using UserStore.DAL.Entities;

namespace UserStore.BLL.Interfaces
{
    public interface IRequestService : IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        void AddRequest(Request Request, string currentUserId, HttpPostedFileBase file);
        void UpdateRequest(Request Request, HttpPostedFileBase file =null);
        Request GetRequest(int? id);
        IEnumerable<Request> GetRequests();
        ApplicationUser GetApplicationUser(string UserId);
        byte[] DowloadFile(string filepath);
        Task SetInitialData(UserDTO adminDto, List<string> roles);
    } 
}
