using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Core.Dto;
using Web.Core.Model;
using Web.Core.Util;

namespace Web.Core.Service
{
    public abstract class UserServiceBase : IServiceBase<UserDto, string>
    {
        public virtual void DeleteById(string key, string userSession = null)
        {
            using (var context = new MyContext())
            {
                User user = context.Users.FirstOrDefault(x => x.UserName == key);
                context.Users.Remove(user);

                context.SaveChanges();
            }
        }

        public virtual List<UserDto> GetAll()
        {
            using (var context = new MyContext())
            {
                return context.Users
                    .Select(x => new UserDto()
                    {
                        UserName = x.UserName,
                        FullName = x.FullName,
                        Email = x.Email,
                        Address = x.Address,
                        Avatar = x.Avatar,
                        Dob = x.Dob,
                        Gender = x.Gender,
                        LastLogin = x.LastLogin,
                        Phone = x.Phone
                    })
                    .ToList();
            }
        }

        public virtual UserDto GetById(string key)
        {
            using (var context = new MyContext())
            {
                return context.Users
                   .Where(x => x.UserName == key)
                   .Select(x => new UserDto()
                   {
                       UserName = x.UserName,
                       FullName = x.FullName,
                       Email = x.Email,
                       Address = x.Address,
                       Avatar = x.Avatar,
                       Dob = x.Dob,
                       Gender = x.Gender,
                       LastLogin = x.LastLogin,
                       Phone = x.Phone
                   })
                   .FirstOrDefault();
            }
        }

        public virtual UserDto Insert(UserDto entity)
        {
            using (var context = new MyContext())
            {
                if (context.Users.Any(x => x.UserName == entity.UserName))
                    throw new ArgumentException("Tên tài khoản đã tồn tại");

                User user = new User()
                {
                    UserName = entity.UserName,
                    Phone = entity.Phone,
                    Address = entity.Address,
                    Avatar = entity.Avatar,
                    Dob = entity.Dob,
                    Email = entity.Email,
                    FullName = entity.FullName,
                    Gender = entity.Gender,
                    Password = DataHelper.SHA256Hash(entity.UserName + "_" + entity.Password),
                };

                context.Users.Add(user);

                context.SaveChanges();

                return entity;
            }
        }

        public virtual void Update(string key, UserDto entity)
        {
            using (var context = new MyContext())
            {
                User user = context.Users.FirstOrDefault(x => x.UserName == key);

                user.Phone = entity.Phone;
                user.Address = entity.Address;
                user.Avatar = entity.Avatar;
                user.Dob = entity.Dob;
                user.Email = entity.Email;
                user.FullName = entity.FullName;
                user.Gender = entity.Gender;

                context.SaveChanges();
            }
        }

        public virtual void ResetPassword(string key, string newPassword)
        {
            using (var context = new MyContext())
            {
                User user = context.Users.FirstOrDefault(x => x.UserName == key);

                user.Password = DataHelper.SHA256Hash(user.UserName + "_" + newPassword);
                context.SaveChanges();
            }
        }

        public virtual UserDto CheckLogin(UserDto entity)
        {
            using (var context = new MyContext())
            {
                User user = context.Users
                    .FirstOrDefault(x => x.UserName == entity.UserName);

                if (user == null)
                    throw new ArgumentException("Tài khoản hoặc mật khẩu không đúng");

                string passwordCheck = DataHelper.SHA256Hash(entity.UserName + "_" + entity.Password);

                if (user.Password != passwordCheck)
                    throw new ArgumentException("Tài khoản hoặc mật khẩu không đúng");

                user.LastLogin = DateTime.Now;
                context.SaveChanges();

                return new UserDto()
                {
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Avatar = user.Avatar,
                    Address = user.Address,
                    Phone = user.Phone,
                    Email = user.Email,
                    Dob = user.Dob,
                    Gender = user.Gender,
                    LastLogin = user.LastLogin
                };
            }
        }
    }
}
