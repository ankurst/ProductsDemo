using ProductWebAPI.Models;
using System.Collections.Generic;

namespace Products.BusinessLayer
{
    public interface ICategoryDataprovider
    {
        IList<ProductCategoryViewModel> GetCategoriesList();

        bool AddCategory(ProductCategoryViewModel category);
    }
}
