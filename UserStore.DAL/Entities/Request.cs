using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStore.DAL.Entities
{
    public class Request
    {
        public int Id { get; set; }

        public string Theme { get; set; }

        public string Message { get; set; }

        public ApplicationUser Author { get; set; }

        public string AttachmentLink { get; set; }

        public DateTime Create { get; set; }

        public bool Scanned { get; set; }

    }
}
