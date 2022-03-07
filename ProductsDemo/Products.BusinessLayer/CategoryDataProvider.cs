using log4net;
using Products.BusinessLayer.Entities;
using ProductWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Products.BusinessLayer
{
    public class CategoryDataProvider : ICategoryDataprovider
    {
        private static readonly log4net.ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public bool AddCategory(ProductCategoryViewModel category)
        {
            try
            {
                using (var db = new EcommerceDemoEntities())
                {
                    db.ProductCategories.Add(new ProductCategory()
                    {
                        CategoryName = category.Name,
                        ProductAttributeLookups = category.Attributes.Select(ca => new ProductAttributeLookup()
                        {
                            AttributeId = ca.AttributeId,
                            AttributeName = ca.Name,
                            ProdCatId = ca.CategoryId
                        }).ToList<ProductAttributeLookup>()
                    });
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                var errMsg = $"Error while deleting Product- {ex.Message}, InnerException- {ex.InnerException?.Message}";
                log.Info(errMsg, ex);
                return false;
            }
        }

        public IList<ProductCategoryViewModel> GetCategoriesList()
        {
            using (var db1 = new EcommerceDemoEntities())
            {
                return db1.ProductCategories.Select(p => new ProductCategoryViewModel()
                {
                    Id = p.ProdCatId,
                    Name = p.CategoryName,
                    Attributes = p.ProductAttributeLookups.Select(ca => new CategoryAttributeViewModel()
                    {
                        CategoryId = ca.ProdCatId,
                        Name = ca.AttributeName,
                        AttributeId = ca.AttributeId
                    }).ToList<CategoryAttributeViewModel>()
                }).ToList<ProductCategoryViewModel>();
            }
        }
    }
}
