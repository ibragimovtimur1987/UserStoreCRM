using UserStore.BLL.DTO;
using UserStore.BLL.Infrastructure;
using UserStore.DAL.Entities;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using UserStore.BLL.Interfaces;
using UserStore.DAL.Interfaces;
using System.Collections.Generic;
using System;
using System.Web;
using System.IO;

namespace UserStore.BLL.Services
{
    public class RequestService : IRequestService
    {
        IUnitOfWork Database { get; set; }

        public RequestService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email };
                await Database.UserManager.CreateAsync(user, userDto.Password);           
                await Database.SaveAsync();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }
        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            // находим пользователя
            ApplicationUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);
            // авторизуем его и возвращаем объект ClaimsIdentity
            if(user!=null)
                claim= await Database.UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }
        public ApplicationUser GetApplicationUser(string UserId)
        {
            // находим пользователя
            return Database.UserManager.FindById(UserId);
        }
        public IEnumerable<Request> GetRequests()
        {
            return Database.Requests.GetAll();
        }
        public void UpdateRequest(Request Request, HttpPostedFileBase file)
        {
            if (Request == null)
                throw new Exception("Видео не найдено");
            if (file != null)
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "UploadedFiles/";
                string filename = Path.GetFileName(file.FileName);
                string pathToFile = Path.Combine(path, filename);
                if (filename != null) file.SaveAs(pathToFile);
                Request.AttachmentLink = pathToFile;
            }
            Database.Requests.Update(Request);
            Database.Save();
        }
        public void AddRequest(Request Request,string currentUserId, HttpPostedFileBase file)
        {
            Request.Author = GetApplicationUser(currentUserId);
            if (file != null)
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data/uploads/";
                string filename = Path.GetFileName(file.FileName);
                string pathToFile = Path.Combine(path, filename);
                if (filename != null) file.SaveAs(pathToFile);
                Request.AttachmentLink = pathToFile;
            }
            Request.Create = DateTime.Now;
            Database.Requests.Create(Request);
            Database.Save();
        }
        public Request GetRequest(int? id)
        {
            if (id == null)
                throw new Exception("Видео не найдено");
            return Database.Requests.Get(id.Value);
        }
        public void Dispose()
        {
            Database.Dispose();
        }
    } 
}
