using ProductWebAPI.Models;
using System.Collections.Generic;

namespace Products.BusinessLayer
{
    public interface IProductDataprovider
    {
        IList<ProductViewModel> GetProductsList();

        ProductViewModel GetProductDetails(long id);

        bool AddProduct(ProductViewModel product);

        ProductViewModel UpdateProduct(ProductViewModel product);

        bool DeleteProduct(long id);
    }
}
