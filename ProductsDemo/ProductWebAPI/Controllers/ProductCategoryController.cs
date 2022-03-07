using System.Collections.Generic;
using System.Web.Http;
using Products.BusinessLayer;
using ProductWebAPI.Models;

namespace ProductWebAPI.Controllers
{
    public class ProductCategoryController : ApiController
    {
        private readonly ICategoryDataprovider _categoryDataProvider = null;

        public ProductCategoryController() { }
        public ProductCategoryController(ICategoryDataprovider categoryBLObj = null)
        {
            //caller who wants its own implementation can pass it in this constructor (e.g. test proj passing mock)
            //else this constructor will instanciate it
            _categoryDataProvider = categoryBLObj ?? new CategoryDataProvider();
        }
        // GET: ProductCategory
        public IHttpActionResult GetAllCategoriesWithAttributes()
        {
            IList<ProductCategoryViewModel> categories = null;

            categories = _categoryDataProvider.GetCategoriesList();

            if (categories != null)
                return Ok(categories);
            else
                return NotFound();
        }


        // POST: ProductCategory/Create
        [HttpPost]
        public IHttpActionResult Create(ProductCategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                bool result = _categoryDataProvider.AddCategory(category);

                if (result)
                    return Ok();
                else
                    return InternalServerError();
            }
            else
                return BadRequest(ModelState);
        }

    }
}