using System;
using System.Collections.Generic;
using System.Linq;
using Web.Core.Dto;
using Web.Core.Model;
using Web.Core.Util;
//notthing

namespace Web.Core.Service
{
    public abstract class ProductServiceBase : IServiceBase<ProductDto, int>
    {
        public virtual void DeleteById(int key, string userSession = null)
        {
            using (var context = new MyContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    Product product = context.Products.FirstOrDefault(x => x.Id == key);

                    context.ProductAttributes.RemoveRange(product.ProductAttributes);
                    context.ProductImages.RemoveRange(product.ProductImages);
                    context.ProductRelateds.RemoveRange(product.ProductRelateds);
                    context.Reviews.RemoveRange(product.Reviews);
                    context.Products.Remove(product);

                    context.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        public virtual List<ProductDto> GetAll()
        {
            using (var context = new MyContext())
            {
                return context.Products
                    .Select(x => new ProductDto()
                    {
                        Id = x.Id,
                        Alias = x.Alias,
                        CategoryId = x.CategoryId,
                        Created = x.Created,
                        Description = x.Description,
                        DiscountPercent = x.DiscountPercent,
                        DiscountPrice = x.DiscountPrice,
                        Selling = x.Selling,
                        Image = x.Image,
                        Index = x.Index,
                        Quantity = x.Quantity,
                        MenuId = x.MenuId,
                        Price = x.Price,
                        Name = x.Name,
                        ShortDescription = x.ShortDescription,
                        Status = x.Status,
                        UserDef1 = x.UserDef1,
                        UserDef2 = x.UserDef2,
                        UserDef3 = x.UserDef3,
                        UserDef4 = x.UserDef4,
                        UserDef5 = x.UserDef5,
                        Tags = x.Tags,
                        Menu = x.Menu == null ? null : new MenuDto()
                        {
                            Name = x.Menu.Name
                        }
                    })
                    .ToList();
            }
        }
        public virtual List<ProductDto> GetAllOrder(string orderBy = "")
        {
            using (var context = new MyContext())
            {
                var query = context.Products
                    .Where(x => x.Status == 10);
                if (!string.IsNullOrWhiteSpace(orderBy))
                {
                    switch (orderBy)
                    {
                        case "name-asc":
                            query = query.OrderBy(x => x.Name);
                            break;
                        case "name-desc":
                            query = query.OrderByDescending(x => x.Name);
                            break;
                        case "price-asc":
                            query = query.OrderBy(x => x.Price);
                            break;
                        case "price-desc":
                            query = query.OrderByDescending(x => x.Price);
                            break;
                        case "price-diff-desc":
                            query = query.OrderByDescending(x => x.Price - x.DiscountPrice);
                            break;
                    }
                }
                return query
                    .Select(x => new ProductDto()
                    {
                        Id = x.Id,
                        Alias = x.Alias,
                        DiscountPercent = x.DiscountPercent,
                        DiscountPrice = x.DiscountPrice,
                        Image = x.Image,
                        Price = x.Price,
                        Name = x.Name,
                        Quantity = x.Quantity,
                        MetaContentLanguage = x.MetaContentLanguage,
                        MetaContentType = x.MetaContentType,
                        MetaDescription = x.MetaDescription,
                        MetaRevisitAfter = x.MetaRevisitAfter,
                        MetaRobots = x.MetaRobots,
                        Rate = x.Rate,  
                        
                    })
                    .ToList();
            }

        }

        public virtual List<ProductDto> GetAllSelling()
        {
            using (var context = new MyContext())
            {
                var query = context.Products
                        .Where(x => x.Selling == true && x.Status == 10 && x.Quantity > 0)
                        .Select(x => new ProductDto()
                        {
                            Id = x.Id,
                            Alias = x.Alias,
                            DiscountPercent = x.DiscountPercent,
                            DiscountPrice = x.DiscountPrice,
                            Quantity = x.Quantity,
                            Image = x.Image,
                            Price = x.Price,
                            Name = x.Name,
                            MetaDescription = x.MetaDescription,
                            Rate = x.Rate   
                        })
                        .ToString(); // Log the generated SQL query
                Console.WriteLine("SQL Query: " + query);

                return context.Products
                    .Where(x => x.Selling == true && x.Status == 10 && x.Quantity > 0)
                    .Select(x => new ProductDto()
                    {
                        Id = x.Id,
                        Alias = x.Alias,
                        DiscountPercent = x.DiscountPercent,
                        DiscountPrice = x.DiscountPrice,
                        Quantity = x.Quantity,
                        Image = x.Image,
                        Price = x.Price,
                        Name = x.Name,
                        MetaDescription = x.MetaDescription,
                        Rate= x.Rate
                    })
                    .ToList();
            }
        }

        public virtual List<ProductDto> GetByMenu(int menuId, string orderBy = "")
        {

            using (var context = new MyContext())
            {

                MenuDto menuDto = new MenuDto();
                menuDto = context.Menus
                    .Where(x => x.Id == menuId)
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
                if (menuDto.ParentMenu != null)
                {
                    var query = context.Products
                    .Where(x => x.Status == 10 && x.MenuId == menuId);

                    if (!string.IsNullOrWhiteSpace(orderBy))
                    {
                        switch (orderBy)
                        {
                            case "name-asc":
                                query = query.OrderBy(x => x.Name);
                                break;
                            case "name-desc":
                                query = query.OrderByDescending(x => x.Name);
                                break;
                            case "price-asc":
                                query = query.OrderBy(x => x.Price);
                                break;
                            case "price-desc":
                                query = query.OrderByDescending(x => x.Price);
                                break;
                        }
                    }
                    return query
                        .Select(x => new ProductDto()
                        {
                            Id = x.Id,
                            Alias = x.Alias,
                            DiscountPercent = x.DiscountPercent,
                            DiscountPrice = x.DiscountPrice,
                            Image = x.Image,
                            Price = x.Price,
                            Name = x.Name,
                            Rate = x.Rate
                        })
                        .ToList();
                }
                else
                {
                    List<int?> listMenuID = new List<int?>();
                    foreach (var menu in context.Menus)
                    {
                        if (menu.ParentMenu == menuDto.Id)
                        {
                            listMenuID.Add(menu.Id);
                        }
                    }
                    var query = context.Products
                    .Where(x => x.Status == 10 && listMenuID.Contains(x.MenuId));
                    if (!string.IsNullOrWhiteSpace(orderBy))
                    {
                        switch (orderBy)
                        {
                            case "name-asc":
                                query = query.OrderBy(x => x.Name);
                                break;
                            case "name-desc":
                                query = query.OrderByDescending(x => x.Name);
                                break;
                            case "price-asc":
                                query = query.OrderBy(x => x.Price);
                                break;
                            case "price-desc":
                                query = query.OrderByDescending(x => x.Price);
                                break;
                        }
                    }
                    return query
                        .Select(x => new ProductDto()
                        {
                            Id = x.Id,
                            Alias = x.Alias,
                            DiscountPercent = x.DiscountPercent,
                            DiscountPrice = x.DiscountPrice,
                            Image = x.Image,
                            Quantity = x.Quantity,
                            Price = x.Price,
                            Name = x.Name,
                            Rate = x.Rate
                        })
                        .ToList();
                }
            }
        }

        public virtual List<ProductDto> Search(string keySearch, string orderBy = "")
        {
            if (string.IsNullOrWhiteSpace(keySearch))
                keySearch = null;

            using (var context = new MyContext())
            {
                var query = context.Products
                    .Where(x => x.Status == 10)
                    .Where(x => keySearch == null || x.Name.Contains(keySearch));

                if (!string.IsNullOrWhiteSpace(orderBy))
                {
                    switch (orderBy)
                    {
                        case "name-asc":
                            query = query.OrderBy(x => x.Name);
                            break;
                        case "name-desc":
                            query = query.OrderByDescending(x => x.Name);
                            break;
                        case "price-asc":
                            query = query.OrderBy(x => x.Price);
                            break;
                        case "price-desc":
                            query = query.OrderByDescending(x => x.Price);
                            break;
                    }
                }

                return query
                    .Select(x => new ProductDto()
                    {
                        Id = x.Id,
                        Alias = x.Alias,
                        DiscountPercent = x.DiscountPercent,
                        DiscountPrice = x.DiscountPrice,
                        Image = x.Image,
                        Quantity = x.Quantity,
                        Price = x.Price,
                        Name = x.Name,
                        Rate = x.Rate
                    })
                    .ToList();
            }
        }

        public virtual ProductDto GetById(int key)
        {
            using (var context = new MyContext())
            {
                var product = context.Products
                    .Where(x => x.Id == key)
                    .Select(x => new ProductDto()
                    {
                        Id = x.Id,
                        Alias = x.Alias,
                        CategoryId = x.CategoryId,
                        Created = x.Created,
                        Description = x.Description,
                        DiscountPercent = x.DiscountPercent,
                        DiscountPrice = x.DiscountPrice,
                        Selling = x.Selling,
                        Image = x.Image,
                        Rate = x.Rate,
                        RateAmount = x.RateAmount,
                        Index = x.Index,
                        Quantity = x.Quantity,
                        MenuId = x.MenuId,
                        Price = x.Price,
                        Name = x.Name,
                        ShortDescription = x.ShortDescription,
                        Status = x.Status,
                        MetaContentLanguage = x.MetaContentLanguage,
                        MetaContentType = x.MetaContentType,
                        MetaDescription = x.MetaDescription,
                        MetaRevisitAfter = x.MetaRevisitAfter,
                        MetaRobots = x.MetaRobots,
                        UserDef1 = x.UserDef1,
                        UserDef2 = x.UserDef2,
                        UserDef3 = x.UserDef3,
                        UserDef4 = x.UserDef4,
                        UserDef5 = x.UserDef5,
                        Tags = x.Tags,
                        ProductAttributes = x.ProductAttributes.Select(y => new ProductAttributeDto()
                        {
                            AttributeId = y.AttributeId,
                            UserDef1 = y.UserDef1,
                            UserDef2 = y.UserDef2,
                            UserDef3 = y.UserDef3,
                            UserDef4 = y.UserDef4,
                            UserDef5 = y.UserDef5,
                            ValueNumber = y.ValueNumber,
                            ValueString = y.ValueString,
                            Index = y.Index,
                        }).ToList(),
                        ProductRelateds = x.ProductRelateds.Select(y => new ProductRelatedDto()
                        {
                            ProductRelatedId = y.ProductRelatedId,
                            Index = y.Index,
                        }).ToList(),
                        ProductImages = x.ProductImages.Select(y => new ProductImageDto()
                        {
                            Image = y.Image,
                            Index = y.Index
                        }).ToList(),
                        Reviews = x.Reviews.Select(y => new ReviewDto()
                        {
                            Id = y.Id,
                            Star = y.Star,
                            Content = y.Content,
                            Active = y.Active,
                            Created = y.Created,
                            CustomerCode = y.CustomerCode,
                            CustomerName = y.CustomerName,
                        }).ToList(),
                    })
                    .FirstOrDefault();
                return product;
            }
        }

        public virtual ProductDto GetByAlias(string alias)
        {
            using (var context = new MyContext())
            {
                return context.Products
                    .Where(x => x.Alias == alias)
                    .Select(x => new ProductDto()
                    {
                        Id = x.Id,
                        Alias = x.Alias,
                        CategoryId = x.CategoryId,
                        Created = x.Created,
                        Description = x.Description,
                        DiscountPercent = x.DiscountPercent,
                        DiscountPrice = x.DiscountPrice,
                        Selling = x.Selling,
                        Image = x.Image,
                        Index = x.Index,
                        MenuId = x.MenuId,
                        Price = x.Price,
                        Name = x.Name,
                        Rate = x.Rate,
                        RateAmount = x.RateAmount,
                        Quantity = x.Quantity,
                        ShortDescription = x.ShortDescription,
                        Status = x.Status,
                        MetaContentLanguage = x.MetaContentLanguage,
                        MetaContentType = x.MetaContentType,
                        MetaDescription = x.MetaDescription,
                        MetaRevisitAfter = x.MetaRevisitAfter,
                        MetaRobots = x.MetaRobots,
                        UserDef1 = x.UserDef1,
                        UserDef2 = x.UserDef2,
                        UserDef3 = x.UserDef3,
                        UserDef4 = x.UserDef4,
                        UserDef5 = x.UserDef5,
                        Tags = x.Tags,
                        ProductAttributes = x.ProductAttributes.Select(y => new ProductAttributeDto()
                        {
                            Attribute = new AttributeDto()
                            {
                                Name = y.Attribute.Name
                            },
                            AttributeId = y.AttributeId,
                            UserDef1 = y.UserDef1,
                            UserDef2 = y.UserDef2,
                            UserDef3 = y.UserDef3,
                            UserDef4 = y.UserDef4,
                            UserDef5 = y.UserDef5,
                            ValueNumber = y.ValueNumber,
                            ValueString = y.ValueString,
                            Index = y.Index,
                        }).ToList(),
                        ProductRelateds = x.ProductRelateds.Select(y => new ProductRelatedDto()
                        {
                            ProductRelatedId = y.ProductRelatedId,
                            Index = y.Index,
                        }).ToList(),
                        ProductImages = x.ProductImages.Select(y => new ProductImageDto()
                        {
                            Image = y.Image,
                            Index = y.Index
                        }).ToList(),
                        Reviews = x.Reviews.Select(y => new ReviewDto()
                        {
                            Id = y.Id,
                            Star = y.Star,
                            Content = y.Content,
                            Active = y.Active,
                            Created = y.Created,
                            CustomerCode = y.CustomerCode,
                            CustomerName = y.CustomerName,
                        }).ToList(),
                    })
                    .FirstOrDefault();
            }
        }

        public virtual ProductDto Insert(ProductDto entity)
        {
            using (var context = new MyContext())
            {
                Product product = new Product()
                {
                    Price = entity.Price,
                    Name = entity.Name,
                    MenuId = entity.MenuId,
                    Index = entity.Index,
                    Image = entity.Image,
                    DiscountPrice = entity.DiscountPrice,
                    DiscountPercent = entity.DiscountPercent,
                    Selling = entity.Selling,
                    Description = entity.Description,
                    Quantity = entity.Quantity,
                    Created = DateTime.Now,
                    CategoryId = entity.CategoryId,
                    Alias = "",
                    ShortDescription = entity.ShortDescription,
                    Status = entity.Status,
                    MetaContentLanguage = entity.MetaContentLanguage,
                    MetaContentType = entity.MetaContentType,
                    MetaDescription = entity.MetaDescription,
                    MetaRevisitAfter = entity.MetaRevisitAfter,
                    MetaRobots = entity.MetaRobots,
                    UserDef1 = entity.UserDef1,
                    Tags = entity.Tags,
                    UserDef2 = entity.UserDef2,
                    UserDef3 = entity.UserDef3,
                    UserDef4 = entity.UserDef4,
                    UserDef5 = entity.UserDef5
                };

                if (entity.ProductImages != null && entity.ProductImages.Count > 0)
                {
                    product.ProductImages = entity.ProductImages.Select(x => new ProductImage()
                    {
                        Image = x.Image,
                    }).ToList();
                }
                if (entity.ProductAttributes != null && entity.ProductAttributes.Count > 0)
                {
                    product.ProductAttributes = entity.ProductAttributes.Select(x => new ProductAttribute()
                    {
                        AttributeId = x.AttributeId,
                        UserDef1 = x.UserDef1,
                        UserDef2 = x.UserDef2,
                        UserDef3 = x.UserDef3,
                        UserDef4 = x.UserDef4,
                        UserDef5 = x.UserDef5,
                        ValueNumber = x.ValueNumber,
                        ValueString = x.ValueString,
                    }).ToList();
                }
                if (entity.ProductRelateds != null && entity.ProductRelateds.Count > 0)
                {
                    product.ProductRelateds = entity.ProductRelateds.Select(x => new ProductRelated()
                    {
                        ProductRelatedId = x.ProductRelatedId,
                    }).ToList();
                }

                context.Products.Add(product);
                context.SaveChanges();
                product.Alias = DataHelper.Unsign(product.Name) + "-" + product.Id;
                context.SaveChanges();

                return entity;
            }
        }

        public virtual void Update(int key, ProductDto entity)
        {
            using (var context = new MyContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    Product product = context.Products.FirstOrDefault(x => x.Id == key);

                    product.MenuId = entity.MenuId;
                    product.CategoryId = entity.CategoryId;
                    product.Name = entity.Name;
                    product.Alias = DataHelper.Unsign(entity.Name) + "-" + key;
                    product.Image = entity.Image;
                    product.Index = entity.Index;
                    product.Status = entity.Status;
                    product.Price = entity.Price;
                    product.Rate = entity.Rate;
                    product.RateAmount = entity.RateAmount;
                    product.DiscountPrice = entity.DiscountPrice;
                    product.Quantity = entity.Quantity;
                    product.DiscountPercent = entity.DiscountPercent;
                    product.Selling = entity.Selling;
                    product.Tags = entity.Tags;
                    product.ShortDescription = entity.ShortDescription;
                    product.Description = entity.Description;
                    product.MetaContentLanguage = entity.MetaContentLanguage;
                    product.MetaContentType = entity.MetaContentType;
                    product.MetaDescription = entity.MetaDescription;
                    product.MetaRevisitAfter = entity.MetaRevisitAfter;
                    product.MetaRobots = entity.MetaRobots;
                    product.UserDef1 = entity.UserDef1;
                    product.UserDef2 = entity.UserDef2;
                    product.UserDef3 = entity.UserDef3;
                    product.UserDef4 = entity.UserDef4;
                    product.UserDef5 = entity.UserDef5;

                    context.ProductAttributes.RemoveRange(product.ProductAttributes);
                    context.ProductImages.RemoveRange(product.ProductImages);
                    context.ProductRelateds.RemoveRange(product.ProductRelateds);

                    if (entity.ProductAttributes != null && entity.ProductAttributes.Count > 0)
                    {
                        context.ProductAttributes.AddRange(entity.ProductAttributes.Select(x => new ProductAttribute()
                        {
                            AttributeId = x.AttributeId,
                            ProductId = key,
                            Index = x.Index,
                            UserDef1 = x.UserDef1,
                            UserDef2 = x.UserDef2,
                            UserDef3 = x.UserDef3,
                            UserDef4 = x.UserDef4,
                            UserDef5 = x.UserDef5,
                            ValueNumber = x.ValueNumber,
                            ValueString = x.ValueString,
                        }));
                    }

                    if (entity.ProductImages != null && entity.ProductImages.Count > 0)
                    {
                        context.ProductImages.AddRange(entity.ProductImages.Select(x => new ProductImage()
                        {
                            ProductId = key,
                            Image = x.Image,
                            Index = x.Index
                        }));
                    }

                    if (entity.ProductRelateds != null && entity.ProductRelateds.Count > 0)
                    {
                        context.ProductRelateds.AddRange(entity.ProductRelateds.Select(x => new ProductRelated()
                        {
                            ProductId = key,
                            ProductRelatedId = x.ProductRelatedId,
                            Index = x.Index
                        }));
                    }

                    context.SaveChanges();
                    transaction.Commit();
                }
            }
        }
    }
}
