using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Core.Dto;

namespace Web.Core.Service
{
    public abstract class EmailTemplateServiceBase : IServiceBase<EmailTemplateDto, int>
    {
        public virtual void DeleteById(int key, string userSession = null)
        {
            throw new NotImplementedException();
        }

        public virtual List<EmailTemplateDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public virtual EmailTemplateDto GetById(int key)
        {
            throw new NotImplementedException();
        }

        public virtual EmailTemplateDto Insert(EmailTemplateDto entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(int key, EmailTemplateDto entity)
        {
            throw new NotImplementedException();
        }
    }
}