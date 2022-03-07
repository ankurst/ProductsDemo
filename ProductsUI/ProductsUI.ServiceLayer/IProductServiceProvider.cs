using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductWebAPI.ViewModel;

namespace ProductsUI.ServiceLayer
{
    public interface IProductServiceProvider
    {
        IList<ProductViewModel> GetProductsList();

        ProductViewModel GetProductDetails(long id);

        bool AddProduct(ProductViewModel product);

        ProductViewModel UpdateProduct(ProductViewModel product);

        IList<ProductCategoryViewModel> GetCategoriesList();

        //bool DeleteProduct(long id);
    }
}
