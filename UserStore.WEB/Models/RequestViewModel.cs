using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UserStore.DAL.Entities;

namespace UserStore.Models
{
    public class RequestViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Тема")]
        [Required]
        public string Theme { get; set; }
        [Display(Name = "Сообщение")]
        [Required]
        public string Message { get; set; }
        [Display(Name = "Имя клиента")]
        [Required]
        public string UserName { get; set; }
        [Display(Name = "Почта клиента")]
        [Required]
        public string Email { get; set; }
        [Display(Name = "Прикрепленный файл")]
        [Required]
        public string AttachmentLink { get; set; }
        [Display(Name = "Время создания")]
        [Required]
        public DateTime Create { get; set; }
        [Display(Name = "Просмотрено")]
        public bool Scanned { get; set; }
        public RequestViewModel()
        {

        }
        public RequestViewModel(Request Request)
        {
            FillFields(Request);
        }

        public RequestViewModel(Request Request,string currentUserId)
        {
            FillFields(Request);
        }

        private void FillFields(Request Request)
        {
            Id = Request.Id;
            Theme = Request.Theme;
            Message = Request.Message;
            if (Request.Author != null)
            {
                Email = Request.Author.Email;
                UserName = Request.Author.UserName;
            }
            AttachmentLink = Request.AttachmentLink;
            Create = Request.Create;
            Scanned = Request.Scanned;
           
        }

        public Request CreateRequest()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<RequestViewModel, Request>()).CreateMapper();
            return mapper.Map<RequestViewModel, Request>(this);
        }
    }
}