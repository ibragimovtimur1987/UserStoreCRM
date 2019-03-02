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

namespace UserStore.Web.Controllers
{
    [Authorize]
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
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            string currentUser = User.Identity.GetUserId();
            List<RequestViewModel> RequestViews = RequestService.GetRequests().Select(x=>new RequestViewModel(x, currentUser)).ToList();
            return View(RequestViews.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ContentResult Index(RequestViewModel request)
        {
            return null;
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(VideoViewModel videoViewModel)
        //{
        //    Video video = videoViewModel.GetVideo();
        //    videoService.UpdateVideo(video);
        //    return RedirectToAction("Index");
        //}
        // Добавление
        public ActionResult Create()
        {
            return PartialView("Create");
        }    
        // Добавление
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RequestViewModel RequestViewModel, HttpPostedFileBase file)
        {
            Request Request = RequestViewModel.CreateRequest();
            RequestService.AddRequest(Request, User.Identity.GetUserId(), file);
            return RedirectToAction("Index");
        }

    }
}