using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using UserStore.Models;
using UserStore.BLL.DTO;
using System.Security.Claims;
using UserStore.BLL.Interfaces;
using UserStore.BLL.Infrastructure;

namespace UserStore.Controllers
{
    public class AccountController : Controller
    {
        private IRequestService RequestService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IRequestService>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return CheckRole();
            }
            else
                return View();
        }

        private ActionResult CheckRole()
        {
            int? par = 1;
            if (HttpContext.User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Request", par);
            }
            else if (HttpContext.User.IsInRole("user"))
            {
                return RedirectToAction("Create", "Request");
            }
            else
            {
                return new HttpUnauthorizedResult();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
          //  await SetInitialDataAsync();

            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO { UserName = model.UserName, Password = model.Password};
                ClaimsIdentity claim = await RequestService.Authenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return CheckRole();
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            await SetInitialDataAsync();

            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    UserName= model.UserName,
                    Role = "user"
                };
                OperationDetails operationDetails = await RequestService.Create(userDto);
                if (operationDetails.Succedeed)
                {
                        int? par = 1;
                    return View("SuccessRegister");
                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }
        private async Task SetInitialDataAsync()
        {
            await RequestService.SetInitialData(new UserDTO
            {
                Email = "someemail@mail.ru",
                UserName = "someemail@mail.ru",
                Password = "ad57D_ewr45",
                Name = "Семен Семенович Павлов",
                Address = "ул. Спортивная, д.31, кв.73",
                Role = "admin",
            }, new List<string> { "user", "admin" });
        }
    }
}