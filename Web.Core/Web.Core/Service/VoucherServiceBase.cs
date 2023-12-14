using System.Collections.Generic;
using System.Linq;
using Web.Core.Dto;
using Web.Core.Model;

namespace Web.Core.Service
{
    public abstract class VoucherServiceBase : IServiceBase<VoucherDto, int>
    {
        public virtual void DeleteById(int key, string userSession = null)
        {
            using (var context = new MyContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    Voucher voucher = context.Vouchers.FirstOrDefault(x => x.Id == key);

                    context.Vouchers.Remove(voucher);

                    context.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        public virtual List<VoucherDto> GetAll()
        {
            using (var context = new MyContext())
            {
                return context.Vouchers
                    .Select(x => new VoucherDto()
                    {
                        Id = x.Id,
                        VoucherCode = x.VoucherCode,
                        Description = x.Description,
                        Type = x.Type,
                        DiscountAmount = x.DiscountAmount,
                        IsActive = x.IsActive,
                    })
                    .ToList();
            }
        }
        public virtual List<VoucherDto> GetAllAvailable()
        {
            using (var context = new MyContext())
            {
                return context.Vouchers
                    .Where(x => x.IsActive > 0)
                    .Select(x => new VoucherDto()

                    {
                        Id = x.Id,
                        VoucherCode = x.VoucherCode,
                        Description = x.Description,
                        Type = x.Type,
                        DiscountAmount = x.DiscountAmount,
                        IsActive = x.IsActive,
                    })
                    .ToList();
            }
        }

        public virtual VoucherDto GetById(int key)
        {
            using (var context = new MyContext())
            {
                return context.Vouchers
                    .Where(x => x.Id == key)
                    .Select(x => new VoucherDto()
                    {
                        Id = x.Id,
                        VoucherCode = x.VoucherCode,
                        Description = x.Description,
                        Type = x.Type,
                        DiscountAmount = x.DiscountAmount,
                        IsActive = x.IsActive,
                    })
                    .FirstOrDefault();
            }
        }


        public virtual VoucherDto Insert(VoucherDto entity)
        {
            using (var context = new MyContext())
            {
                Voucher voucher = new Voucher()
                {
                    Type = entity.Type,
                    Description = entity.Description,
                    VoucherCode = entity.VoucherCode,
                    DiscountAmount = entity.DiscountAmount,
                    IsActive = entity.IsActive
                };
                context.Vouchers.Add(voucher);
                context.SaveChanges();
                context.SaveChanges();

                return entity;
            }
        }

        public virtual void Update(int key, VoucherDto entity)
        {
            using (var context = new MyContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    Voucher voucher = context.Vouchers.FirstOrDefault(x => x.Id == key);
                    voucher.VoucherCode = entity.VoucherCode;
                    voucher.Description = entity.Description;
                    voucher.Type = entity.Type;
                    voucher.DiscountAmount = entity.DiscountAmount;
                    voucher.IsActive = entity.IsActive;
                    context.SaveChanges();
                    transaction.Commit();
                }
            }
        }
    }
}
