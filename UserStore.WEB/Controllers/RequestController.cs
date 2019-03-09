using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserStore.BLL.Interfaces;
using PagedList;
using PagedList.Mvc;
using UserStore.DAL.Entities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.IO;
using UserStore.Models;
using AutoMapper;
using System.Threading.Tasks;

namespace UserStore.Web.Controllers
{
    
    public class RequestController : Controller
    {
        // GET: Request
        private IRequestService RequestService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IRequestService>();
            }
        }
        [Authorize(Roles = "admin")]
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            string currentUser = User.Identity.GetUserId();
            List<RequestViewModel> RequestViews = RequestService.GetRequests().Select(x=>new RequestViewModel(x, currentUser)).ToList();
            return View(RequestViews.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ContentResult Index(RequestViewModel requestViewModel)
        {
            Request request = requestViewModel.CreateRequest();
            RequestService.UpdateRequest(request);
            return null;
        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Download(string filepath)
        {           
            return File(
                RequestService.DownloadFile(filepath), System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(filepath));
        }

        [Authorize]
        // Добавление
        public ActionResult Create()
        {
            string currentUserId = User.Identity.GetUserId();
            Request request = RequestService.GetMyRequestToday(currentUserId);
            if (request!= null)
            {
                DateTime sendDate = request.Create.AddDays(1);
                ViewBag.SendDate = sendDate.ToString("F");
                return PartialView("Message");
            }
            else
            {
                return PartialView("Create");
            }
        }    
        // Добавление
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(RequestViewModel RequestViewModel, HttpPostedFileBase file)
        {
            Request Request = RequestViewModel.CreateRequest();
            RequestService.AddRequest(Request, User.Identity.GetUserId(), file);
            //await RequestService.SendEmailAsync(Request);
            return PartialView("Success");
        }
    }
}