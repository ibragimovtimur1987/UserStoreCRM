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
using System.Net.Mail;
using System.Net;
using System.Diagnostics;
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
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.UserName };
                await Database.UserManager.CreateAsync(user, userDto.Password);
                // добавляем роль
                await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);
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
            ApplicationUser user = await Database.UserManager.FindAsync(userDto.UserName, userDto.Password);
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
        public void UpdateRequest(Request Request, HttpPostedFileBase file = null)
        {
            if (Request == null)
                throw new Exception("Заявка не найдена");
            Database.Requests.Update(Request);
            Database.Save();
        }
        public void AddRequest(Request Request,string currentUserId, HttpPostedFileBase file)
        {
            Request.Author = GetApplicationUser(currentUserId);
            if (file != null)
            {
                string pathToRoot = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\uploads\\";
                string pathFolder = "";
                string filename = Path.GetFileName(file.FileName);
                string pathToFileOutRoot = Path.Combine(pathFolder, filename);
                string pathToFile = Path.Combine(pathToRoot, pathToFileOutRoot);
                if (pathToFile != null) file.SaveAs(pathToFile);
                Request.AttachmentLink = pathToFileOutRoot;
            }
            Request.Create = DateTime.Now;
            Database.Requests.Create(Request);
            Database.Save();
        }
        public Request GetRequest(int? id)
        {
            if (id == null)
                throw new Exception("Заявка не найдена");
            return Database.Requests.Get(id.Value);
        }
        public void Dispose()
        {
            Database.Dispose();
        }
        public Request GetMyRequestToday(string userId)
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);
            return Database.Requests.FindLastOrDefault(x => x.Author != null && x.Author.Id == userId && x.Create > yesterday);
        }
        public byte[] DownloadFile(string filepath)
        {
            string fullName = Path.Combine(GetBaseDir(), filepath);
            byte[] fileBytes = GetFile(fullName);
            return fileBytes;
        }

        private string GetBaseDir()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\uploads\";
            return path;
        }

        private byte[] GetFile(string s)
        {

            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
        // начальная инициализация бд
        public async Task SetInitialData(UserDTO adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }

            await Create(adminDto);
        }
        public async Task SendEmailAsync(Request Request, HttpPostedFileBase file)
        {
            try
            {          
                MailAddress from = new MailAddress(Request.Author.Email, Request.Author.UserName);
                MailAddress to = new MailAddress(Constants.AdminData.Email);
                MailMessage mail = new MailMessage(from, to);
                mail.Subject = Request.Theme;
                mail.Body = Request.Message;
                SmtpClient smtp = new SmtpClient(Constants.SMTPSetting.Host, Constants.SMTPSetting.Port);
                mail.Attachments.Add(new Attachment(file.InputStream, file.ContentType));
                //smtp.Credentials = new NetworkCredential("somemail@gmail.com", "mypassword");
                await smtp.SendMailAsync(mail);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                // You must close or flush the trace to empty the output buffer.  
                Debug.Flush();
            }
        }
    } 
}
