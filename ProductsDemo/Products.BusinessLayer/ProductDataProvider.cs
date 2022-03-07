using ProductWebAPI.Models;
using Products.BusinessLayer.Entities;
using System.Data.Entity;
using log4net;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Products.BusinessLayer
{
    public class ProductDataProvider : IProductDataprovider
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool AddProduct(ProductViewModel product)
        {
            using (var db = new EcommerceDemoEntities())
            {
                //transaction implemented as calling SaveChanges twice
                using (DbContextTransaction productTxn = db.Database.BeginTransaction())
                {
                    try
                    {
                        var dbProduct = new Product()
                        {
                            ProdName = product.Name,
                            ProdDescription = product.Description,
                            ProdCatId = product.Category.Id
                        };
                        db.Products.Add(dbProduct);

                        log.Info($"Product being added {product.Name}, {product.Description}, Category id {product.Category.Id}");

                        db.SaveChanges();

                        //below code is for adding ProductAttribute also, to correctly add product in DB

                        var lastAddedProductId = dbProduct.ProductId;

                        foreach (var pa in product.Attributes)
                        {
                            db.ProductAttributes.Add(new ProductAttribute()
                            {
                                ProductId = lastAddedProductId,
                                AttributeId = pa.Id,
                                AttributeValue = pa.Value
                            });
                        }

                        log.Info($"Product Attributes being added for category with id {product.Category.Id}");

                        db.SaveChanges();

                        productTxn.Commit();

                    }
                    catch (Exception ex)
                    {
                        productTxn.Rollback();
                        var errMsg = $"Error while adding Product- {ex.Message}, InnerException- {ex.InnerException?.Message}";
                        log.Info(errMsg, ex);
                        throw ex;
                    }
                    return true;
                }
            }
        }

        public bool DeleteProduct(long id)
        {
            using (var db = new EcommerceDemoEntities())
            {
                var productInDb = db.Products.Where(p => p.ProductId == id).FirstOrDefault<Product>();
                if (productInDb != null)
                {
                    db.Entry(productInDb).State = EntityState.Deleted;

                    db.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        public ProductViewModel GetProductDetails(long id)
        {
            using (var db = new EcommerceDemoEntities())
            {
                return db.Products.Where(p => p.ProductId == id).Select(p => new ProductViewModel()
                {
                    Id = p.ProductId,
                    Name = p.ProdName,
                    Description = p.ProdDescription,
                    Category = new ProductCategoryViewModel()
                    {
                        Id = p.ProductCategory.ProdCatId,
                        Name = p.ProductCategory.CategoryName,
                        Attributes = p.ProductCategory.ProductAttributeLookups.Select(ca => new CategoryAttributeViewModel()
                        {
                            CategoryId = ca.ProdCatId,
                            Name = ca.AttributeName,
                            AttributeId = ca.AttributeId
                        }).ToList<CategoryAttributeViewModel>()
                    },
                    Attributes = p.ProductAttributes.Select(a => new ProductAttributeViewModel()
                    {
                        Id = a.AttributeId,
                        Value = a.AttributeValue
                    }).ToList<ProductAttributeViewModel>()

                }).FirstOrDefault<ProductViewModel>();
            }
        }

        public IList<ProductViewModel> GetProductsList()
        {
            using (var db = new EcommerceDemoEntities())
            {
                return db.Products.Select(p => new ProductViewModel()
                {
                    Id = p.ProductId,
                    Name = p.ProdName,
                    Description = p.ProdDescription,
                    Category = new ProductCategoryViewModel()
                    {
                        Id = p.ProductCategory.ProdCatId,
                        Name = p.ProductCategory.CategoryName,
                        Attributes = p.ProductCategory.ProductAttributeLookups.Select(ca => new CategoryAttributeViewModel()
                        {
                            CategoryId = ca.ProdCatId,
                            Name = ca.AttributeName,
                            AttributeId = ca.AttributeId
                        }).ToList<CategoryAttributeViewModel>()
                    },
                    Attributes = p.ProductAttributes.Select(a => new ProductAttributeViewModel()
                    {
                        Id = a.AttributeId,
                        Value = a.AttributeValue
                    }).ToList<ProductAttributeViewModel>()

                }).ToList<ProductViewModel>();
            }
        }

        public ProductViewModel UpdateProduct(ProductViewModel product)
        {
            using (var db = new EcommerceDemoEntities())
            {
                var productInDb = db.Products.Where(p => p.ProductId == product.Id).FirstOrDefault<Product>();
                if (productInDb != null)
                {
                    productInDb.ProdName = product.Name;
                    productInDb.ProdDescription = product.Description;
                    //not modifying category as product category cannot be changed

                    foreach (var (pa, p) in
                    from pa in productInDb.ProductAttributes
                    from p in product.Attributes
                    where pa.AttributeId == p.Id
                    select (pa, p))
                    {
                        pa.AttributeValue = p.Value;
                    }

                    log.Info($"Product with id {product.Id}, Category id {productInDb.ProductCategory.ProdCatId} being updated ");

                    db.SaveChanges();

                    log.Info("End Create");

                    return product;
                }
                else
                    return null;
            }
        }
    }
}