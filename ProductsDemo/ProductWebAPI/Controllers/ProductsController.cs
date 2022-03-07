using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ProductWebAPI.Models;
using Products.BusinessLayer;
using log4net;
using System.Net;
using Products.Common;

namespace ProductWebAPI.Controllers
{
    public class ProductsController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IProductDataprovider _productDataProvider = null;

        public ProductsController() { }

        public ProductsController(IProductDataprovider prodBLObj = null)
        {
            //caller who wants its own implementation can pass it in this constructor (e.g. test proj passing mock)
            //else this constructor will instanciate it
            _productDataProvider = prodBLObj ?? new ProductDataProvider();
        }
        // GET: Products
        [HttpGet]
        public IHttpActionResult Index()
        {
            log.Info("Start Get");
            IList<ProductViewModel> products = null;

            products = _productDataProvider.GetProductsList();

            if (products != null)
            {
                log.Info($"found {products.Count} product(s) from db");
                log.Info("End Get");
                return Ok(products);
            }
            else
                return NotFound();
        }

        // GET: Products/Details/1
        [HttpGet]
        public IHttpActionResult Details(long id)
        {
            ProductViewModel product = null;

            product = _productDataProvider.GetProductDetails(id);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST: Products/Create
        [HttpPost]
        public IHttpActionResult Create(ProductViewModel product)
        {
            log.Info($"Start Create {Utility.ToJson(product)}");

            if (ModelState.IsValid)
            {
                bool result = _productDataProvider.AddProduct(product);

                log.Info("End Create");

                if (result)
                    return Ok();
                else
                    return InternalServerError();
            }
            else
            {
                log.Info($"Invalid ModelState- {ModelState.Values.ToArray()}");
                return BadRequest(ModelState);
            }
        }

        // POST: Products/Edit/5        
        [HttpPost, ActionName("Edit")]
        public IHttpActionResult Edit(ProductViewModel product)
        {
            log.Info($"Start Edit {Utility.ToJson(product)}");

            if (ModelState.IsValid)
            {
                product = _productDataProvider.UpdateProduct(product);

                if (product != null)
                    return Ok(product);
                else
                    return Content(HttpStatusCode.NotFound, "requested product not found");
            }
            else
                return BadRequest(ModelState);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        public IHttpActionResult Delete(long id)
        {
            if (id <= 0)
                return BadRequest("Invalid Product Id");

            bool result = _productDataProvider.DeleteProduct(id);

            if (result)
                return Ok();
            else
                return NotFound();
        }

        
    }
}
