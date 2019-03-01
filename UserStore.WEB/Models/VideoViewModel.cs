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
        [Display(Name = "Название")]
        [Required]
        public string Title { get; set; }
        [Display(Name = "Описание")]
        [Required]
        public string Note { get; set; }
        [Display(Name = "Режиссёр")]
        [Required]
        public string Producer { get; set; }
        [Display(Name = "Год выпуска")]
        [Required]
        public int? Year { get; set; }
        [Display(Name = "Постер")]
        [Required]
        public byte[] Poster{ get; set; }

        public string ContentPath { get; set; }

        public ApplicationUser Author { get; set; }

        [Display(Name = "Опубликовал")]
        public string AuthorUserName { get; set; }

        public bool IsAuthor { get; set; }

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
            IsAuthor = Author?.Id == currentUserId;
        }

        private void FillFields(Request Request)
        {
            Id = Request.Id;
            Title = Request.Title;
            Note = Request.Note;
            Producer = Request.Producer;
            Year = Request.Year;
            Poster = Request.Poster;
            ContentPath = Request.ContentPath;
            Author = Request?.Author;
            AuthorUserName = Request?.Author?.UserName;
        }

        public Request CreateRequest()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<RequestViewModel, Request>()).CreateMapper();
            return mapper.Map<RequestViewModel, Request>(this);
        }
    }
}