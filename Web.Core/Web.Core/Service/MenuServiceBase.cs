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
    public abstract class MenuServiceBase : IServiceBase<MenuDto, int>
    {
        public virtual void DeleteById(int key, string userSession = null)
        {
            using (var context = new MyContext())
            {
                Menu menu = context.Menus.FirstOrDefault(x => x.Id == key);

                context.Menus.Remove(menu);
                context.SaveChanges();
            }
        }

        public virtual List<MenuDto> GetAll()
        {
            using (var context = new MyContext())
            {
                List<MenuDto> menuDtos = context.Menus
                    .OrderBy(x => x.Index)
                    .ToList()
                    .Select(x => new MenuDto()
                    {
                        Active = x.Active,
                        Alias = x.Alias,
                        Id = x.Id,
                        Image = x.Image,
                        Index = x.Index,
                        Name = x.Name,
                        ParentMenu = x.ParentMenu,
                        PMenu = x.PMenu == null ? null : new MenuDto()
                        {
                            Id = x.PMenu.Id,
                            Name = x.PMenu?.Name
                        },
                        ShowHomePage = x.ShowHomePage,
                        ViewType = x.ViewType
                    }).ToList();

                return menuDtos;
            }
        }

        public virtual List<MenuDto> GetAllActive()
        {
            using (var context = new MyContext())
            {
                List<MenuDto> menuDtos = context.Menus
                    .Where(x => x.Active)
                    .OrderBy(x => x.Index)
                    .Select(x => new MenuDto()
                    {
                        Alias = x.Alias,
                        Id = x.Id,
                        Image = x.Image,
                        Index = x.Index,
                        Name = x.Name,
                        ParentMenu = x.ParentMenu,
                        ShowHomePage = x.ShowHomePage,
                        ViewType = x.ViewType,
                    }).ToList();

                List<MenuDto> parentMenu = menuDtos.FindAll(x => x.ParentMenu == null);
                parentMenu.ForEach(x =>
                {
                    x.SubMenus = menuDtos.FindAll(y => y.ParentMenu == x.Id);
                });
                return parentMenu;
            }
        }

        public virtual List<MenuDto> GetParentMenu()
        {
            using (var context = new MyContext())
            {
                return context.Menus
                     .Where(x => x.ParentMenu == null)
                     .OrderBy(x => x.Index)
                     .Select(x => new MenuDto()
                     {
                         Id = x.Id,
                         Name = x.Name,
                     }).ToList();
            }
        }

        public virtual List<MenuDto> GetAllShowHomePage()
        {
            using (var context = new MyContext())
            {
                List<MenuDto> menuDtos = context.Menus
                    .Where(x => x.Active && x.ShowHomePage == true)
                    .OrderBy(x => x.Index)
                    .Select(x => new MenuDto()
                    {
                        Alias = x.Alias,
                        Id = x.Id,
                        Image = x.Image,
                        Name = x.Name,
                        Products = x.Products
                            .OrderBy(y => y.Index)
                            .Select(y => new ProductDto()
                            {
                                Id = y.Id,
                                Alias = y.Alias,
                                Name = y.Name,
                                Price = y.Price,
                                DiscountPrice = y.DiscountPrice,
                                DiscountPercent = y.DiscountPercent,
                                Image = y.Image
                            }).ToList()
                    }).ToList();

                return menuDtos;
            }
        }

        public virtual MenuDto GetById(int key)
        {
            using (var context = new MyContext())
            {
                return context.Menus
                    .Where(x => x.Id == key)
                    .Select(x => new MenuDto()
                    {
                        Active = x.Active,
                        Alias = x.Alias,
                        Id = x.Id,
                        Image = x.Image,
                        Index = x.Index,
                        Name = x.Name,
                        ParentMenu = x.ParentMenu,
                        ShowHomePage = x.ShowHomePage,
                        ViewType = x.ViewType,
                        MetaContentLanguage = x.MetaContentLanguage,
                        MetaContentType = x.MetaContentType,
                        MetaDescription = x.MetaDescription,
                        MetaRevisitAfter = x.MetaRevisitAfter,
                        MetaRobots = x.MetaRobots,
                    }).FirstOrDefault();
            }
        }

        public virtual MenuDto GetByAlias(string alias)
        {
            using (var context = new MyContext())
            {
                return context.Menus
                    .Where(x => x.Alias == alias)
                    .Select(x => new MenuDto()
                    {
                        ParentMenu= x.ParentMenu,
                        Alias = x.Alias,
                        Id = x.Id,
                        Image = x.Image,
                        Name = x.Name,
                        MetaContentLanguage = x.MetaContentLanguage,
                        MetaContentType = x.MetaContentType,
                        MetaDescription = x.MetaDescription,
                        MetaRevisitAfter = x.MetaRevisitAfter,
                        MetaRobots = x.MetaRobots,
                    }).FirstOrDefault();
            }
        }

        public virtual MenuDto Insert(MenuDto entity)
        {
            using (var context = new MyContext())
            {
                Menu menu = new Menu()
                {
                    Active = entity.Active,
                    Alias = "",
                    Image = entity.Image,
                    Index = entity.Index,
                    Name = entity.Name,
                    ParentMenu = entity.ParentMenu,
                    ShowHomePage = entity.ShowHomePage,
                    ViewType = entity.ViewType,
                    MetaContentLanguage = entity.MetaContentLanguage,
                    MetaContentType = entity.MetaContentType,
                    MetaDescription = entity.MetaDescription,
                    MetaRevisitAfter = entity.MetaRevisitAfter,
                    MetaRobots = entity.MetaRobots,
                };

                context.Menus.Add(menu);
                context.SaveChanges();
                menu.Alias = DataHelper.Unsign(menu.Name) + "-" + menu.Id;
                context.SaveChanges();

                return entity;
            }
        }

        public virtual void Update(int key, MenuDto entity)
        {
            using (var context = new MyContext())
            {
                Menu menu = context.Menus.FirstOrDefault(x => x.Id == key);

                menu.ParentMenu = entity.ParentMenu;
                menu.Name = entity.Name;
                menu.Alias = DataHelper.Unsign(entity.Name) + "-" + key;
                menu.Image = entity.Image;
                menu.Index = entity.Index;
                menu.ShowHomePage = entity.ShowHomePage;
                menu.ViewType = entity.ViewType;
                menu.Active = entity.Active;
                menu.MetaContentLanguage = entity.MetaContentLanguage;
                menu.MetaContentType = entity.MetaContentType;
                menu.MetaDescription = entity.MetaDescription;
                menu.MetaRevisitAfter = entity.MetaRevisitAfter;
                menu.MetaRobots = entity.MetaRobots;

                context.SaveChanges();
            }
        }
    }
}