using System;
using System.Collections.Generic;
using System.Linq;
using Web.Core.Dto;
using Web.Core.Model;

namespace Web.Core.Service
{
    public abstract class ImportHistoryServiceBase : IServiceBase<ImportHistoryDto, int>
    {
        public virtual void DeleteById(int key, string userSession = null)
        {
            throw new NotImplementedException();
        }

        public virtual List<ImportHistoryDto> GetAll()
        {
            using (var context = new MyContext())
            {
                return context.ImportHistories
                    .Select(x => new ImportHistoryDto()
                    {
                        Id = x.Id,
                        ProductID = x.ProductID,
                        ImportDate = x.ImportDate,
                        ImportedBy = x.ImportedBy,
                        Quantity = x.Quantity,
                        Price = x.Price,
                        Note = x.Note,
                    })
                    .ToList();
            }
        }

        public virtual ImportHistoryDto GetById(int key)
        {
            throw new NotImplementedException();
        }

        public virtual ImportHistoryDto GetByAlias(string alias)
        {
            throw new NotImplementedException();
        }

        public virtual ImportHistoryDto Insert(ImportHistoryDto entity)
        {
            using (var context = new MyContext())
            {
                ImportHistory importHistory = new ImportHistory()
                {
                    ImportDate = entity.ImportDate,
                    ImportedBy = entity.ImportedBy,
                    ProductID = entity.ProductID,
                    Quantity = entity.Quantity,
                    Price = entity.Price,
                    Note = entity.Note,
                };

                context.ImportHistories.Add(importHistory);
                context.SaveChanges();

                return entity;
            }
        }

        public virtual void Update(int key, ImportHistoryDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
