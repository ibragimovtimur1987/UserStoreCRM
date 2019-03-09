using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.Entities;

namespace UserStore.BLL.Interfaces
{
    public interface IEmailService
    {
       Task SendEmailAsync(Request Request);
    }
}
